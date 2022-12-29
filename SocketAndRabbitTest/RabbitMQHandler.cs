using RabbitMQ.Client;
using SuperSocket.ProtoBase;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace Server
{
    public static class RabbitMQHandler
    {

        private static IModel _channel;
        static RabbitMQHandler()
        {
            var connectionFactory = new ConnectionFactory();
            IList<AmqpTcpEndpoint> hosts = new List<AmqpTcpEndpoint>()
            {
            new AmqpTcpEndpoint{HostName="127.0.0.1",Port=5671},
            new AmqpTcpEndpoint{HostName="127.0.0.1",Port=5672},
            new AmqpTcpEndpoint{HostName="127.0.0.1",Port=5673},

            };

            var connection = connectionFactory.CreateConnection(hosts);
            _channel = connection.CreateModel();
            var queueName = $"queue.all.package";
            _channel.QueueDeclare(queueName, true, false, false, new Dictionary<string, object>
                  {
                  { "x-queue-type", "quorum" }
                 });
            _channel.QueueBind(queueName, "amq.topic", "ft.#");
        }

        public static void Publish(StringPackageInfo package)
        {
            var anchorKey = package.Parameters[1];
            var body = Encoding.UTF8.GetBytes(package.Body);

            if (_channel.IsOpen)
            {
                //_channel.QueueDeclare(queueName, true, false, false);

                _channel.BasicPublish("amq.topic", $"ft.{anchorKey}", null, body);

            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;

                Console.WriteLine($"Gönderilemeyen mesaj: {package.Body}");
                Console.ResetColor();
            }
        }


    }
}
