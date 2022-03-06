using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using RocketMonitor.Domain.Interface;
using RocketMonitor.Domain.Json;
using RocketMonitor.Domain.Message;
using RocketMonitor.Service.Interface;

namespace RocketMonitor.API.Controllers;

[ApiController]
public class MessagesController : ControllerBase
{
    private readonly ICommandController _commandController;
    private readonly ILogger<MessagesController> _logger;
    private readonly JsonSerializerOptions _jsonSerializerOptions;

    public MessagesController(ILogger<MessagesController> logger, ICommandController commandController)
    {
        _logger = logger;
        _commandController = commandController;
        _jsonSerializerOptions = new JsonSerializerOptions(JsonSerializerDefaults.Web);
    }

    [HttpPost]
    [Route("messages")]
    public async Task<ActionResult> SendMessage()
    {
        var message = await JsonSerializer.DeserializeAsync<InputMessage>(Request.Body, _jsonSerializerOptions);
        if (message == null) throw new ArgumentNullException(nameof(message));
        _logger.LogInformation("Processing message {@Message}", message);
        IRocketMessage specificMessage;
        switch (message.Metadata.MessageType)
        {
            case nameof(RocketLaunched):
                specificMessage = new RocketLaunched(message);
                break;
            case nameof(RocketSpeedIncreased):
                specificMessage = new RocketSpeedIncreased(message);
                break;
            case nameof(RocketSpeedDecreased):
                specificMessage = new RocketSpeedDecreased(message);
                break;
            case nameof(RocketMissionChanged):
                specificMessage = new RocketMissionChanged(message);
                break;
            case nameof(RocketExploded):
                specificMessage = new RocketExploded(message);
                break;
            default: throw new InvalidOperationException("Unsupported Message Type");
        }

        await _commandController.AppendMessageToStream(specificMessage);
        _logger.LogInformation("Processed message OK {@Message}", message);
        return Ok();
    }
}