using SuperSocket;
using SuperSocket.Command;
using SuperSocket.ProtoBase;
using System.Text;

namespace Server
{
    public class PNG : IAsyncCommand<StringPackageInfo>
    {
        public ValueTask ExecuteAsync(IAppSession session, StringPackageInfo package)
        {

            RabbitMQHandler.Publish(package);

            session.SendAsync(Encoding.UTF8.GetBytes($"#ACK|{package.Body}"));

            Console.ForegroundColor = ConsoleColor.Blue;

            Console.WriteLine($"Recieved Message PNG: {package.Body}");
            Console.ResetColor();

            return ValueTask.CompletedTask;
        }

    }
}
