
using GrainInterfacesNet7;
using MongoDB.Driver;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace OrleansClientNet7
{
  public class Worker
  {
    private readonly IModel _channel;
    private readonly IMongoCollection<AnchorMongoNew> _anchors;
    private readonly IClusterClient _orleansClient;
    public Worker(IClusterClient orleansClient)
    {
      var client = new MongoClient("mongodb://127.0.0.1:27017");
      var database = client.GetDatabase("Nearloc");
      _anchors = database.GetCollection<AnchorMongoNew>("Anchor");

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

      //_orleansClient = ConnectClient();
      _orleansClient = orleansClient;

    }

    public void Subscribe()
    {
      var queueName = $"queue.all.package";
      //_channel.QueueDeclare(queueName, true, false, false);
      //_channel.QueueBind(queueName, "amq.topic", "testkey");

      _channel.BasicQos(prefetchSize: 0, prefetchCount: 1, global: false);

      var consumer = new EventingBasicConsumer(_channel);

      consumer.Received += MessageReceived;

      _channel.BasicConsume(queueName, false, consumer);
      Console.WriteLine("Waiting for message...");
    }

    int i = 0;
    void MessageReceived(object? sender, BasicDeliverEventArgs ea)
    {

      var body = ea.Body.ToArray();
      var message = Encoding.UTF8.GetString(body);

      Console.WriteLine($"Recieved Message {i}: {message}");
      i++;

      var parameters = message.Split('|');
      var anchorKey = parameters[1];

      var anchor = CheckAnchor(anchorKey);
      if (anchor == null)
      {
        Console.ForegroundColor = ConsoleColor.Blue;
        Console.WriteLine($"Anchor Bulunamadı");
        Console.ResetColor();

        _channel.BasicAck(ea.DeliveryTag, false);

        return;
      }

      var anchorGrain = _orleansClient.GetGrain<IAnchorGrain>(anchorKey);
      anchorGrain.WakeUpNeo().Wait();

      _channel.BasicAck(ea.DeliveryTag, false);



      Thread.Sleep(1000);

    }

    AnchorMongoNew CheckAnchor(string anchorKey)
    {
      var builder = Builders<AnchorMongoNew>.Filter;
      var filter = builder.Eq(a => a.AnchorKey, anchorKey);
      var checkAnchor = _anchors.Find(filter).FirstOrDefaultAsync().Result;
      return checkAnchor;
    }

   

    //static IClusterClient ConnectClient()
    //{
    //  var client = new ClientBuilder()
    //      .UseLocalhostClustering()
    //      .Configure<ClusterOptions>(options =>
    //      {
    //        options.ClusterId = "deneme_orleans_cluster";
    //        options.ServiceId = "deneme_orleans_cluster";
    //      })
    //      .Build();

    //  client.Connect().Wait();
    //  Console.WriteLine("Client successfully connected to silo host \n");
    //  return client;
    //}


  }
}
