namespace RocketMonitor.Domain.Model;

public class Rocket
{
    public Guid Channel { get; set; }
    public string Type { get; set; }
    public double LaunchSpeed { get; set; }
    public string Mission { get; set; }
    public string? ExplodedReason { get; set; }
    public DateTime? ExplodedTime { get; set; }
    public DateTime LaunchTime { get; set; }
}