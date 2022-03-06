namespace RocketMonitor.Domain.Json;

public class InputMessage
{
    public Metadata Metadata { get; set; }
    public RocketMessage Message { get; set; }
}