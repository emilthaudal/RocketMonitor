using Microsoft.AspNetCore.Mvc;
using RocketMonitor.Domain.Interface;
using RocketMonitor.Domain.Model;
using RocketMonitor.Service.Interface;

namespace RocketMonitor.API.Controllers;

[ApiController]
[Route("[controller]/[action]")]
public class DashboardController : ControllerBase
{
    private readonly IQueryController _queryController;

    public DashboardController(IQueryController queryController)
    {
        _queryController = queryController;
    }

    [HttpGet]
    public IAsyncEnumerable<Rocket> GetRockets()
    {
        return _queryController.GetRockets().OrderBy(r => r.LaunchSpeed);
    }

    [HttpGet]
    public IAsyncEnumerable<object> GetRocketMessages([FromQuery] Guid channel)
    {
        return _queryController.GetRocketMessages(channel);
    }

    [HttpGet]
    public async Task<ActionResult> GetRocket([FromQuery] Guid channel)
    {
        return Ok(await _queryController.GetRocket(channel));
    }
}