using RocketMonitor.Infrastructure.Enum;
using RocketMonitor.Infrastructure.Model;

namespace RocketMonitor.Infrastructure.Interface;

public interface IEventStore
{
    /// <summary>
    ///     Appends an event to specified stream
    /// </summary>
    /// <param name="stream">The stream to write a new event to. E.g. rocket-{channel}</param>
    /// <param name="appendEvent">The event content to add to the stream</param>
    /// <param name="exists"></param>
    /// <returns>Number of new event in stream</returns>
    public Task<int> AppendStream(string stream, IEvent appendEvent, StreamExists exists = StreamExists.Any);

    /// <summary>
    ///     Returns an async enumerable to iterate all events in a specific stream
    /// </summary>
    /// <param name="stream">Name of the stream to iterate</param>
    /// <param name="forwards">Boolean specifying whether oldest events should be read first</param>
    /// <returns>An async enumerable of the events in stream</returns>
    public IAsyncEnumerable<IEvent> ReadStream(string stream, bool forwards = true);

    /// <summary>
    ///     Gets all streams existing in Store
    /// </summary>
    /// <returns>Async enumerable of all stream-ids currently in store</returns>
    public IAsyncEnumerable<string> GetAllStreams();
}