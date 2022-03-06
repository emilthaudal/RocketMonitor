using RocketMonitor.Infrastructure.Model;

namespace RocketMonitor.MemoryEventStore.Model;

public class EventMetadata : IMetadata
{
    public DateTime EventTime { get; set; }
    public string EventType { get; set; }
    public int EventNumber { get; set; }
}