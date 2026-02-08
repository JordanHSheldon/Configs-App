namespace EsportsProfileWebApi.Web.Controllers;

using AutoMapper;
using DTOs.Data;
using Orchestrators;
using Orchestrators.Models.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

[Route("api/[controller]")]
[ApiController]
public class ProfileController(
    IDataOrchestrator dataOrchestrator,
    ILogger<UserController> logger,
    IMapper mapper) : Controller
{
    private readonly ILogger<UserController> _logger = logger;

    private readonly IDataOrchestrator _dataOrchestrator = dataOrchestrator
        ?? throw new NotImplementedException();
    private readonly IMapper _mapper = mapper
        ?? throw new NotImplementedException();

    [HttpPost]
    [Route("GetUserProfiles")]
    public async Task<IEnumerable<GetPaginatedUsersResponseDto>> GetPaginatedUsersAsync(
        GetPaginatedUsersRequestDTO req)
    {
        var request = _mapper.Map<GetPaginatedUsersRequestModel>(req);
        var result = await _dataOrchestrator.GetPaginatedUsersAsync(request);
        return _mapper.Map<List<GetPaginatedUsersResponseDto>>(result);
    }

    [Authorize]
    [HttpPost]
    [Route("GetUserProfile")]
    public async Task<IActionResult> GetProfileData()
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value 
                ?? User.FindFirst("user")?.Value;

        if (string.IsNullOrEmpty(userId))
            return Unauthorized();

        var request = new GetProfileRequestModel
        {
            Id = int.Parse(userId)
        };

        var result = await _dataOrchestrator.GetProfileData(request);

        return Ok(_mapper.Map<GetDataResponseDTO>(result));
    }

    [HttpPost]
    [Route("GetProfileByUserName")]
    public async Task<GetDataResponseDTO> GetProfileByUsername(GetDataRequestDTO getDataRequestDto)
    {
        var request = _mapper.Map<GetDataRequestModel>(getDataRequestDto);
        var result = await _dataOrchestrator.GetData(request);
        return _mapper.Map<GetDataResponseDTO>(result);
    }

    [HttpPost]
    [Route("UpdateProfile")]
    public async Task<UpdateDataResponseDTO?> UpdateProfile(UpdateProfileRequestDTO request)
    {
        var req = mapper.Map<UpdateProfileRequest>(request);
        req.UserId = int.Parse(HttpContext?.User?.Identity?.Name ?? throw new UnauthorizedAccessException()); 
        var result = await _dataOrchestrator.UpdateUserPeripherals(req);
        return _mapper.Map<UpdateDataResponseDTO>(result);
    }

    [HttpPost]
    [Route("GetPeripherals")]
    public async Task<List<PeripheralDto>> GetPeripherals()
    {
        var result = await _dataOrchestrator.GetPeripheralsAsync();
        return _mapper.Map<List<PeripheralDto>>(result.OrderBy(x => x.Id));
    }
}