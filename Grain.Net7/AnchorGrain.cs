
using GrainInterfacesNet7;
using Orleans;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace GrainsNet7
{
  public class AnchorGrain : Grain, IAnchorGrain
  {
    private readonly IRabbitMqConnectionManager _rabbitMqConnectionManager;

    private readonly IModel _rabbitChannel;
    private int i = 0;
    public AnchorGrain(IRabbitMqConnectionManager connectionManager)
    {
      _rabbitMqConnectionManager = connectionManager;

      _rabbitChannel = _rabbitMqConnectionManager.Connection.CreateModel();
    }


    public override Task OnActivateAsync(CancellationToken cancellationToken)
    {
      var id = this.GetPrimaryKeyString();


      _rabbitChannel.BasicQos(prefetchSize: 0, prefetchCount: 1, global: false);

      var consumer = new AsyncEventingBasicConsumer(_rabbitChannel); //https://stackoverflow.com/questions/50836811/rabbitmq-handle-received-message-in-async-way/50837172
      consumer.Shutdown += Consumer_Shutdown;
      consumer.Received += MessageReceived;

      var queueName = $"queue.{id.ToString().ToUpperInvariant()}";
      //_rabbitChannel.QueueDeclare(queueName, true, false, false);
      _rabbitChannel.QueueDeclare(queueName, true, false, false, new Dictionary<string, object>
                  {
                  { "x-queue-type", "quorum" }
                 });

      _rabbitChannel.QueueBind(queueName, "amq.topic", $"ft.{id}");
      _rabbitChannel.BasicConsume(queueName, false, consumer);
      return Task.CompletedTask;
    }

    private Task MessageReceived(object sender, BasicDeliverEventArgs @event)
    {
      var id = this.GetPrimaryKeyString();

      var rawPacket = @event.Body.ToArray();
      var message = Encoding.UTF8.GetString(rawPacket);

      Console.WriteLine($"{id} Grain tarafından alınan mesaj {i}: {message}");
      i++;
      _rabbitChannel.BasicAck(@event.DeliveryTag, false);

      return Task.CompletedTask;
    }
    private Task Consumer_Shutdown(object sender, ShutdownEventArgs @event)
    {
      var id = this.GetPrimaryKey();
      Console.ForegroundColor = ConsoleColor.Red;
      Console.WriteLine($"{id} Consumer_Shutdown");
      Console.ResetColor();
      return Task.CompletedTask;

    }

    public Task WakeUpNeo()
    {
      return Task.CompletedTask;
    }
  }
}