using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GrainInterfaces
{
  public interface IRabbitMqConnectionManager
  {
    public IConnection Connection { get; }

  }
}
