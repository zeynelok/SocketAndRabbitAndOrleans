
using System.Net;
using System.Net.Sockets;
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

    var message = $"PNG|53001F000A504B41|40|10|alert-type";
    var messageBytes = Encoding.UTF8.GetBytes(message + "\r\n");

    _ = await client.SendAsync(messageBytes, SocketFlags.None);
    Console.WriteLine($"Socket client sent message: \"{message}\" ");


    // Receive ack.
    var buffer = new byte[1024];
    var received = await client.ReceiveAsync(buffer, SocketFlags.None);
    
    var response = Encoding.UTF8.GetString(buffer, 0, received);
    var key = response.Substring(0, 4).ToUpper();
    var data = response.Substring(5).ToUpper();
    if (key == "#ACK")
    {
        Console.WriteLine($"Socket client received acknowledgment: \"{data} \" \n");

    }
 

    Thread.Sleep(5000);
}

