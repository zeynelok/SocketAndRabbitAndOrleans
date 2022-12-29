using Orleans;

namespace GrainInterfacesNet7
{
  public interface IAnchorGrain : IGrainWithStringKey
  {
    public Task WakeUpNeo();
  }
}