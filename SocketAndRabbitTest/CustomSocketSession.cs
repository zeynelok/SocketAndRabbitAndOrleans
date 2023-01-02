using SuperSocket.Server;

namespace Server
{
    public class CustomSocketSession : AppSession
    {
        public string ConnectedAnchorKey { get; set; }

        public ValueTask SendAsync(ReadOnlyMemory<byte> data)
        {
            return this.Channel.SendAsync(data);
        }
    }
}
