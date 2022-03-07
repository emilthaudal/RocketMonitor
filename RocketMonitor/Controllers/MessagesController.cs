using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using RocketMonitor.Domain.Interface;
using RocketMonitor.Domain.Json;
using RocketMonitor.Domain.Message;
using RocketMonitor.Service.Interface;

namespace RocketMonitor.Controllers;

[ApiController]
public class MessagesController : ControllerBase
{
    private readonly ICommandController _commandController;
    private readonly JsonSerializerOptions _jsonSerializerOptions;
    private readonly ILogger<MessagesController> _logger;

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
        _logger.LogInformation("Processing message Channel {Channel}, MessageType: {MessageType}",
            message.Metadata.Channel, message.Metadata.MessageType);
        IRocketMessage specificMessage = message.Metadata.MessageType switch
        {
            nameof(RocketLaunched) => new RocketLaunched(message),
            nameof(RocketSpeedIncreased) => new RocketSpeedIncreased(message),
            nameof(RocketSpeedDecreased) => new RocketSpeedDecreased(message),
            nameof(RocketMissionChanged) => new RocketMissionChanged(message),
            nameof(RocketExploded) => new RocketExploded(message),
            _ => throw new InvalidOperationException("Unsupported Message Type")
        };

        await _commandController.AppendMessageToStream(specificMessage);
        _logger.LogInformation("Processed message OK. Channel {Channel}, MessageType: {MessageType}",
            specificMessage.Metadata.Channel, specificMessage.Metadata.MessageType);
        return Ok();
    }
}