using RocketMonitor.Domain.Interface;
using RocketMonitor.Domain.Model;

namespace RocketMonitor.Service.Interface;

public interface IQueryController
{
    public Task<Rocket?> GetRocket(Guid channel);
    public IAsyncEnumerable<IRocketMessage> GetRocketMessages(Guid channel);
    public IAsyncEnumerable<Rocket> GetRockets();
}