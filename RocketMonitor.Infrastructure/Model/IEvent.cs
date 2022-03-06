namespace RocketMonitor.Infrastructure.Model;

public interface IEvent
{
    public string Content { get; set; }
    public string streamId { get; set; }
    public IMetadata Metadata { get; set; }
}