using RocketMonitor.Domain.Interface;

namespace RocketMonitor.Service.Interface;

public interface ICommandController
{
    /// <summary>
    ///     Writes a rocket message to the corresponding stream based on its Channel
    /// </summary>
    /// <param name="rocketMessage">The message to write</param>
    /// <returns>The number of events in the stream after posting the event</returns>
    public Task AppendMessageToStream(IRocketMessage rocketMessage);
}