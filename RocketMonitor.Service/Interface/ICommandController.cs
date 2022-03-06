using RocketMonitor.Domain.Interface;

namespace RocketMonitor.Service.Interface;

public interface ICommandController
{
    public Task AppendMessageToStream(IRocketMessage rocketMessage);
}