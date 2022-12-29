
using System.Net.Sockets;
using System.Net;
using System.Text;

IPHostEntry ipHostInfo = await Dns.GetHostEntryAsync("127.0.0.1");
IPAddress ipAddress = ipHostInfo.AddressList[0];
IPEndPoint ipEndPoint = new(ipAddress, 6002);

using Socket client = new(
    ipEndPoint.AddressFamily,
    SocketType.Stream,
    ProtocolType.Tcp);

await client.ConnectAsync(ipEndPoint);

while (true)
{
    // Send message.

    var messageLGN = $"LGN|3100630018504B34|3100630018504B34|410";
    var messageBytesLGN = Encoding.UTF8.GetBytes(messageLGN + "\r\n");

    _ = await client.SendAsync(messageBytesLGN, SocketFlags.None);
    Console.WriteLine($"Socket client sent message: \"{messageLGN}\" ");


    // Receive ack.
    var buffer = new byte[1024];
    var received = await client.ReceiveAsync(buffer, SocketFlags.None);
    var response = Encoding.UTF8.GetString(buffer, 0, received);
    var key = response.Substring(0, 4).ToUpper();
    var data = response.Substring(5).ToUpper();
    if (key == "#ACK")
    {

        Console.WriteLine($"Socket client received acknowledgment: \"{data}\" \n");
    }

    Thread.Sleep(5000);
}
