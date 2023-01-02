

using SuperSocket;
using SuperSocket.Command;
using SuperSocket.ProtoBase;
using System.Text;

namespace Server
{
    public class LGN : IAsyncCommand<CustomSocketSession, StringPackageInfo>
    {
        public ValueTask ExecuteAsync(CustomSocketSession session, StringPackageInfo package)
        {
            session.ConnectedAnchorKey = package.Parameters[1];

            RabbitMQHandler.Publish(package);

            //session.SendAsync(Encoding.UTF8.GetBytes($"#ACK|{package.Body}"));

            Console.WriteLine($"Recieved Message LGN: {package.Body}");

            return ValueTask.CompletedTask;
        }

    }
}
