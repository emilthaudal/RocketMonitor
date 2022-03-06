using Microsoft.AspNetCore.Mvc;
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

    /// <summary>
    ///     Returns current state for all rockets ordered by launch time
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    public IAsyncEnumerable<Rocket> GetRockets()
    {
        return _queryController.GetRockets().OrderBy(r => r.LaunchTime);
    }

    /// <summary>
    ///     Returns all messages received for a specific Rocket ordered by Message number
    /// </summary>
    /// <param name="channel">Channel for the specific Rocket</param>
    /// <returns></returns>
    [HttpGet]
    public IAsyncEnumerable<object> GetRocketMessages([FromQuery] Guid channel)
    {
        return _queryController.GetRocketMessages(channel);
    }

    /// <summary>
    ///     Gets the current state of a specific rocket
    /// </summary>
    /// <param name="channel"></param>
    /// <returns></returns>
    [HttpGet]
    public async Task<ActionResult> GetRocket([FromQuery] Guid channel)
    {
        return Ok(await _queryController.GetRocket(channel));
    }
}