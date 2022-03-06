namespace RocketMonitor.Infrastructure.Model;

public interface IMetadata
{
    public DateTime EventTime { get; set; }
    public string EventType { get; set; }
    public int EventNumber { get; set; }
}