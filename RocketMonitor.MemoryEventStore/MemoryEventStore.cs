using System.Collections.Concurrent;
using RocketMonitor.Infrastructure.Enum;
using RocketMonitor.Infrastructure.Interface;
using RocketMonitor.Infrastructure.Model;

namespace RocketMonitor.MemoryEventStore;

public class MemoryEventStore : IEventStore
{
    private readonly ConcurrentDictionary<string, List<IEvent>> _streams = new();

    public async Task<int> AppendStream(string stream, IEvent appendEvent, StreamExists exists = StreamExists.Any)
    {
        var containsKey = _streams.ContainsKey(stream);
        switch (containsKey)
        {
            case true when exists == StreamExists.StreamDoesntExist:
                throw new InvalidOperationException("Stream already exists, when specified not to");
            case false when exists == StreamExists.StreamExists:
                throw new InvalidOperationException("Stream doesn't exist, when expected to");
        }

        if (_streams.TryGetValue(stream, out var events))
        {
            if (events.Any(e => e.Metadata.EventNumber == appendEvent.Metadata.EventNumber)) return events.Count;
            events.Add(appendEvent);
            return events.Count;
        }

        if (_streams.TryAdd(stream, new List<IEvent> {appendEvent})) return 1;

        throw new InvalidOperationException("Failed to append stream with event");
    }

    public async IAsyncEnumerable<IEvent> ReadStream(string stream, bool forwards = true)
    {
        if (!_streams.TryGetValue(stream, out var events)) yield break;
        foreach (var @event in events) yield return @event;
    }

    public async IAsyncEnumerable<string> GetAllStreams()
    {
        foreach (var stream in _streams) yield return stream.Key;
    }
}