namespace RocketMonitor.Domain.Json;

public class RocketMessage
{
    public string? Type { get; set; }
    public double LaunchSpeed { get; set; }
    public string? Mission { get; set; }
    public double By { get; set; }
    public string? Reason { get; set; }
    public string? NewMission { get; set; }
}