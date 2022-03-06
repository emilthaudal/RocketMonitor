using RocketMonitor.Domain.Interface;
using RocketMonitor.Domain.Json;
using RocketMonitor.Domain.Model;

namespace RocketMonitor.Domain.Message;

public class RocketSpeedDecreased : IRocketMessage
{
    /// <summary>
    /// Empty constructor for deserialization
    /// </summary>
    public RocketSpeedDecreased()
    {
        
    }
    public RocketSpeedDecreased(InputMessage message)
    {
        By = message.Message.By;
        Metadata = message.Metadata;
    }

    public double By { get; set; }
    public Metadata Metadata { get; set; }

    public Rocket Fold(Rocket rocket)
    {
        rocket.LaunchSpeed -= By;
        return rocket;
    }
}