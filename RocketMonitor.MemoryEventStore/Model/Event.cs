using RocketMonitor.Infrastructure.Model;

namespace RocketMonitor.MemoryEventStore.Model;

public class Event : IEvent
{
    public string Content { get; set; }
    public string streamId { get; set; }
    public IMetadata Metadata { get; set; }
}