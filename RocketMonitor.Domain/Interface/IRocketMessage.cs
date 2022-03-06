using RocketMonitor.Domain.Json;
using RocketMonitor.Domain.Model;

namespace RocketMonitor.Domain.Interface;

public interface IRocketMessage
{
    public Metadata Metadata { get; set; }
    public Rocket Fold(Rocket rocket);
}