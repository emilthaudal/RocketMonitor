using RocketMonitor.Domain.Json;
using RocketMonitor.Domain.Model;

namespace RocketMonitor.Domain.Interface;

public interface IRocketMessage
{
    public Metadata Metadata { get; set; }

    /// <summary>
    ///     Adds the message to an existing Rocket to update the current state
    /// </summary>
    /// <param name="rocket">The rocket to update with message</param>
    /// <returns>The updated Rocket state object</returns>
    public Rocket Fold(Rocket rocket);
}