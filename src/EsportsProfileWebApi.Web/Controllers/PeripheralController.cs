namespace EsportsProfileWebApi.Web.Controllers;

using AutoMapper;
using Orchestrators;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using EsportsProfileWebApi.Web.Controllers.DTOs.Profile;

[Route("api/[controller]")]
[ApiController]
public class PeripheralController(
    IPeripheralOrchestrator peripheralOrchestrator,
    ILogger<PeripheralController> logger,
    IMapper mapper) : Controller
{
    private readonly ILogger<PeripheralController> _logger = logger;

    private readonly IPeripheralOrchestrator _peripheralOrchestrator = peripheralOrchestrator
        ?? throw new NotImplementedException();
    private readonly IMapper _mapper = mapper
        ?? throw new NotImplementedException();

    [HttpPost]
    [Route("GetPeripherals")]
    public async Task<List<PeripheralDto>> GetPeripherals()
    {
        var result = await _peripheralOrchestrator.GetPeripheralsAsync();
        return _mapper.Map<List<PeripheralDto>>(result.OrderBy(x => x.Name));
    }

    [Authorize]
    [HttpPost]
    [Route("UpdatePeripherals")]
    public async Task<bool> UpdatePeripherals()
    {
        return false;
    }
}