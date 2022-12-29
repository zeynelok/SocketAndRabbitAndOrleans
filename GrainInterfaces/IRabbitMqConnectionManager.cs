using RabbitMQ.Client;

namespace GrainInterfacesNet7
{
  public interface IRabbitMqConnectionManager
  {
    public IConnection Connection { get; }

  }
}
