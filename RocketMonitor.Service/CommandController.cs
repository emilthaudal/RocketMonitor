using System.Text.Json;
using Microsoft.Extensions.Configuration;
using RocketMonitor.Domain.Interface;
using RocketMonitor.Infrastructure.Interface;
using RocketMonitor.Infrastructure.Model;
using RocketMonitor.MemoryEventStore.Model;
using RocketMonitor.Service.Interface;

namespace RocketMonitor.Service;

public class CommandController : ICommandController
{
    private readonly IEventStore _eventStore;
    private readonly string _streamPrefix;

    public CommandController(IEventStore eventStore, RocketConfiguration configuration)
    {
        _eventStore = eventStore;
        _streamPrefix = configuration.Stream;
    }

    public async Task AppendMessageToStream(IRocketMessage rocketMessage)
    {
        var streamId = $"{_streamPrefix}{rocketMessage.Metadata.Channel:N}";
        IEvent e = new Event();
        e.Content = JsonSerializer.Serialize<object>(rocketMessage);
        e.streamId = streamId;
        e.Metadata = new Metadata
        {
            EventNumber = rocketMessage.Metadata.MessageNumber, EventTime = DateTime.Now,
            EventType = rocketMessage.Metadata.MessageType
        };
        var result = await _eventStore.AppendStream(streamId, e);
    }
}