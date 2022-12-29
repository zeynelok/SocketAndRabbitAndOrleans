using GrainInterfaces;
using Microsoft.AspNetCore.Connections;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Silo
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
