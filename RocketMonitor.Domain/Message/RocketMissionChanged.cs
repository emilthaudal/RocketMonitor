using RocketMonitor.Domain.Interface;
using RocketMonitor.Domain.Json;
using RocketMonitor.Domain.Model;

namespace RocketMonitor.Domain.Message;

public class RocketMissionChanged : IRocketMessage
{
    /// <summary>
    ///     Empty constructor for deserialization
    /// </summary>
    public RocketMissionChanged()
    {
    }

    public RocketMissionChanged(InputMessage message)
    {
        NewMission = message.Message.NewMission ?? throw new ArgumentNullException(nameof(message));
        Metadata = message.Metadata;
    }

    public string NewMission { get; set; }
    public Metadata Metadata { get; set; }

    public Rocket Fold(Rocket rocket)
    {
        if (rocket.Channel == default || rocket.LaunchTime == default)
            throw new InvalidOperationException("Rocket missing launch information");
        rocket.Mission = NewMission;
        return rocket;
    }
}