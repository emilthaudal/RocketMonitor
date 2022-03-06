using RocketMonitor.Domain.Interface;
using RocketMonitor.Domain.Json;
using RocketMonitor.Domain.Model;

namespace RocketMonitor.Domain.Message;

public class RocketExploded : IRocketMessage
{
    /// <summary>
    ///     Empty constructor for deserialization
    /// </summary>
    public RocketExploded()
    {
    }

    public RocketExploded(InputMessage message)
    {
        Reason = message.Message.Reason ?? throw new ArgumentNullException(nameof(message));
        Metadata = message.Metadata;
    }

    public string Reason { get; set; }
    public Metadata Metadata { get; set; }

    public Rocket Fold(Rocket rocket)
    {
        if (rocket.Channel == default || rocket.LaunchTime == default)
            throw new InvalidOperationException("Rocket missing launch information");
        rocket.ExplodedReason = Reason;
        rocket.ExplodedTime = Metadata.MessageTime;
        return rocket;
    }
}