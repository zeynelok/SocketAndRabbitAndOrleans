using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using SuperSocket;
using SuperSocket.SessionContainer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
  public class deneme : IHostedService
  {
    private readonly ISessionContainer _sessionContainer;
    private readonly ILogger<deneme> _logger;
    private readonly IModel _channel;

    public deneme(ISessionContainer sessionContainer, ILogger<deneme> logger)
    {
      _sessionContainer = sessionContainer;
      _logger = logger;
      var connectionFactory = new ConnectionFactory();
      IList<AmqpTcpEndpoint> hosts = new List<AmqpTcpEndpoint>()
        {
           //new AmqpTcpEndpoint{HostName="127.0.0.1",Port=5672},
            new AmqpTcpEndpoint{HostName="127.0.0.1",Port=5671},
            new AmqpTcpEndpoint{HostName="127.0.0.1",Port=5673},

        };

      var connection = connectionFactory.CreateConnection(hosts);
      Console.WriteLine("Rabbit Connection Created");

      _channel = connection.CreateModel();

    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
      _logger.LogInformation("servis başlatıldı");
      Subscribe();
      return Task.CompletedTask;
    }

    public void Subscribe()
    {
      var queueName = $"queue.all.return.package";
            _channel.QueueDeclare(queueName, true, false, false, new Dictionary<string, object>
                  {
                  { "x-queue-type", "quorum" }
                 });
            //_channel.QueueDeclare(queueName, true, false, false);
      _channel.QueueBind(queueName, "amq.topic", "fg.return.all");

      _channel.BasicQos(prefetchSize: 0, prefetchCount: 1, global: false);

      var consumer = new EventingBasicConsumer(_channel);

      consumer.Received += MessageReceivedAsync;

      _channel.BasicConsume(queueName, false, consumer);
      Console.WriteLine("Waiting for message...");
    }

     private async void  MessageReceivedAsync(object? sender, BasicDeliverEventArgs ea)
    {

      var body = ea.Body.ToArray();
      var message = Encoding.UTF8.GetString(body);

      Console.WriteLine($"Recieved Message: {message}");

      var parameters = message.Split('|');
      var anchorKey = parameters[1];

      var sessionList = _sessionContainer.GetSessions<CustomSocketSession>();

      var session=sessionList.FirstOrDefault(s=>s.ConnectedAnchorKey==anchorKey);
     
     

      if (session==null)
      {
        _channel.BasicAck(ea.DeliveryTag, false);
        return;
      }
      var bytesMessage = Encoding.UTF8.GetBytes(message);
      await session.SendAsync(bytesMessage);


      Thread.Sleep(1000);
      _channel.BasicAck(ea.DeliveryTag, false);

    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
      _logger.LogWarning("servis durduruldu");
      return Task.CompletedTask;
    }
  }
}
