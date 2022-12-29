using Orleans;

namespace GrainInterfaces
{
  public interface IAnchorGrain : IGrainWithStringKey
  {
    public Task WakeUpNeo();
  }
}
