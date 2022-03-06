namespace RocketMonitor.Domain.Json;

public class Metadata
{
    public Guid Channel { get; set; }
    public int MessageNumber { get; set; }
    public string MessageType { get; set; }
    public DateTime MessageTime { get; set; }
}