using EsportsProfileWebApi.Web.Orchestrators;

namespace EsportsProfileWebApi.Web.Controllers;

using Microsoft.AspNetCore.Mvc;

[Route("api/[controller]")]
[ApiController]
public class StatsController(IStatsOrchestrator statsOrchestrator) : ControllerBase
{
    private readonly IStatsOrchestrator _statsOrchestrator = statsOrchestrator;
    
    [HttpPost]
    public async Task<IActionResult> UploadConfig()
    {
        var userStats = _statsOrchestrator.GetStatsBySteamId("76561198234450920");
        return Ok(new { message = userStats});
    }
}