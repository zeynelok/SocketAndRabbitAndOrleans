

using SuperSocket;
using SuperSocket.Command;
using SuperSocket.ProtoBase;
using System.Text;

namespace Server
{
    public class LGN : IAsyncCommand<StringPackageInfo>
    {
        public ValueTask ExecuteAsync(IAppSession session, StringPackageInfo package)
        {
           
            RabbitMQHandler.Publish(package);

            session.SendAsync(Encoding.UTF8.GetBytes($"#ACK|{package.Body}"));

            Console.WriteLine($"Recieved Message LGN: {package.Body}");
         
            return ValueTask.CompletedTask;
        }
    }
}
