using SuperSocket;
using SuperSocket.Command;
using SuperSocket.ProtoBase;
using System.Text;

namespace Server
{
    public class PNG : IAsyncCommand<CustomSocketSession, StringPackageInfo>
    {

        public async ValueTask ExecuteAsync(CustomSocketSession session, StringPackageInfo package)
        {
            session.ConnectedAnchorKey = package.Parameters[1];
            RabbitMQHandler.Publish(package);
            await session.SendAsync(Encoding.UTF8.GetBytes($"#ACK|{package.Body}"));
            
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine($"Recieved Message PNG: {package.Body}");
            Console.ResetColor();

        }

        //public ValueTask ExecuteAsync(IAppSession session, StringPackageInfo package)
        //{

        //    RabbitMQHandler.Publish(package);

        //    session.SendAsync(Encoding.UTF8.GetBytes($"#ACK|{package.Body}"));

        //    Console.ForegroundColor = ConsoleColor.Blue;

        //    Console.WriteLine($"Recieved Message PNG: {package.Body}");
        //    Console.ResetColor();

        //    return ValueTask.CompletedTask;
        //}

      
    }
}
