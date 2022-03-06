using RocketMonitor.Domain.Interface;
using RocketMonitor.Domain.Model;

namespace RocketMonitor.Service.Interface;

public interface IQueryController
{
    /// <summary>
    ///     Gets the current state of a specific Rocket based on its Channel
    /// </summary>
    /// <param name="channel">The Channel of the Rocket</param>
    /// <returns>A Rocket object containing the current state, or null if no valid Rocket is found by Channel ID</returns>
    public Task<Rocket?> GetRocket(Guid channel);

    /// <summary>
    ///     Gets all messages saved by a specific Channel ID
    /// </summary>
    /// <param name="channel">The Channel of the Rocket</param>
    /// <returns>An async enumerable of all messages for the Rocket ordered by Message number</returns>
    public IAsyncEnumerable<IRocketMessage> GetRocketMessages(Guid channel);

    /// <summary>
    ///     Gets current state for all Rockets in store
    /// </summary>
    /// <returns>Async enumerable of Rocket objects for all rockets in Store</returns>
    public IAsyncEnumerable<Rocket> GetRockets();
}