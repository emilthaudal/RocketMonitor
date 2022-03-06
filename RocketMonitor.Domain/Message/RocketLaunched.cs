using RocketMonitor.Domain.Interface;
using RocketMonitor.Domain.Json;
using RocketMonitor.Domain.Model;

namespace RocketMonitor.Domain.Message;

public class RocketLaunched : IRocketMessage
{
    /// <summary>
    ///     Empty constructor for deserialization
    /// </summary>
    public RocketLaunched()
    {
    }

    public RocketLaunched(InputMessage message)
    {
        Type = message.Message.Type ?? throw new ArgumentNullException(nameof(message));
        Mission = message.Message.Mission ?? throw new ArgumentNullException(nameof(message));
        LaunchSpeed = message.Message.LaunchSpeed;
        Metadata = message.Metadata;
    }

    public string Type { get; set; }
    public double LaunchSpeed { get; set; }
    public string Mission { get; set; }
    public Metadata Metadata { get; set; }

    public Rocket Fold(Rocket rocket)
    {
        rocket.Type = Type;
        rocket.LaunchSpeed = LaunchSpeed;
        rocket.Mission = Mission;
        rocket.LaunchTime = Metadata.MessageTime;
        rocket.Channel = Metadata.Channel;
        rocket.ExplodedTime = null;
        return rocket;
    }
}