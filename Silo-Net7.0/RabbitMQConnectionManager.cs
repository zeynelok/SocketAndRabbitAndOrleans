using GrainInterfacesNet7;
using RabbitMQ.Client;

namespace Silo_Net7
{
  public class RabbitMQConnectionManager : IRabbitMqConnectionManager
  {
    public IConnection Connection { get; private set; }

    public RabbitMQConnectionManager()
    {

      var connectionFactory = new ConnectionFactory()
      {
        HostName = "127.0.0.1",
        //UserName = rabbitOptions.Value.UserName,
        //Password = rabbitOptions.Value.Password,
        Port = 5672,
        DispatchConsumersAsync = true
      };

      Connection = connectionFactory.CreateConnection();

    }

    ~RabbitMQConnectionManager()
    {
      if (Connection != null)
      {
        if (Connection.IsOpen)
        {
          Connection.Close();
        }

        Connection.Dispose();
        Connection = null;
      }
    }
  }
}
