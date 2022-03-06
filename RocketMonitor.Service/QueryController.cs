using System.Text.Json;
using Microsoft.Extensions.Configuration;
using RocketMonitor.Domain.Interface;
using RocketMonitor.Domain.Message;
using RocketMonitor.Domain.Model;
using RocketMonitor.Infrastructure.Interface;
using RocketMonitor.Service.Interface;

namespace RocketMonitor.Service;

public class QueryController : IQueryController
{
    private readonly IEventStore _eventStore;
    private readonly string _streamPrefix;

    public QueryController(IEventStore eventStore, RocketConfiguration configuration)
    {
        _eventStore = eventStore;
        _streamPrefix = configuration.Stream;
    }

    public async Task<Rocket?> GetRocket(Guid channel)
    {
        // We iterate through event ordered by EventNumber to account for messages received in wrong order when folding object.
        var events = _eventStore.ReadStream($"{_streamPrefix}{channel:N}").OrderBy(e => e.Metadata.EventNumber);
        // If we don't find a RocketLaunched message we don't return the Rocket.
        if (!await events.AnyAsync(e => e.Metadata.EventType.Equals(nameof(RocketLaunched))))
        {
            return null;
        }
        var rocket = new Rocket();
        rocket = await GetRocketMessages(channel).AggregateAsync(rocket, (current, m) => m.Fold(current));

        return rocket;
    }

    public async IAsyncEnumerable<IRocketMessage> GetRocketMessages(Guid channel)
    {
        var events = _eventStore.ReadStream($"{_streamPrefix}{channel:N}").OrderBy(e => e.Metadata.EventNumber);
        // If we don't find a RocketLaunched message we don't return the Rocket.
        if (!await events.AnyAsync(e => e.Metadata.EventType.Equals(nameof(RocketLaunched))))
        {
            yield break;
        }

        await foreach (var e in events)
        {
            IRocketMessage? message;
            switch (e.Metadata.EventType)
            {
                case nameof(RocketLaunched):
                    message = JsonSerializer.Deserialize<RocketLaunched>(e.Content);
                    if (message == null)
                        throw new InvalidOperationException($"Could not serialize {nameof(RocketLaunched)}");

                    yield return message;
                    break;
                case nameof(RocketSpeedIncreased):
                    message = JsonSerializer.Deserialize<RocketSpeedIncreased>(e.Content);
                    if (message == null)
                        throw new InvalidOperationException($"Could not serialize {nameof(RocketSpeedIncreased)}");

                    yield return message;
                    break;
                case nameof(RocketSpeedDecreased):
                    message = JsonSerializer.Deserialize<RocketSpeedDecreased>(e.Content);
                    if (message == null)
                        throw new InvalidOperationException($"Could not serialize {nameof(RocketSpeedDecreased)}");

                    yield return message;
                    break;
                case nameof(RocketMissionChanged):
                    message = JsonSerializer.Deserialize<RocketMissionChanged>(e.Content);
                    if (message == null)
                        throw new InvalidOperationException($"Could not serialize {nameof(RocketMissionChanged)}");

                    yield return message;
                    break;
                case nameof(RocketExploded):
                    message = JsonSerializer.Deserialize<RocketExploded>(e.Content);
                    if (message == null)
                        throw new InvalidOperationException($"Could not serialize {nameof(RocketExploded)}");

                    yield return message;
                    break;
                default: throw new InvalidOperationException($"Unknown message type {e.Metadata.EventType}");
            }
        }
    }

    public async IAsyncEnumerable<Rocket> GetRockets()
    {
        var streams = _eventStore.GetAllStreams();
        await foreach (var stream in streams)
        {
            var stringId = stream.Split('-').Last();
            if (!Guid.TryParse(stringId, out var channel)) continue;

            var rocket = await GetRocket(channel);
            if (rocket == null)
            {
                continue;
            }
            yield return rocket;
        }
    }
}