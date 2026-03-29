namespace EsportsProfileWebApi.Web.Controllers;

using AutoMapper;
using Orchestrators;
using Orchestrators.Models.Profile;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using EsportsProfileWebApi.Web.Controllers.DTOs.Profile;

[Route("api/[controller]")]
[ApiController]
public class ProfileController(
    IProfileOrchestrator ProfileOrchestrator,
    ILogger<UserController> logger,
    IMapper mapper) : Controller
{
    private readonly ILogger<UserController> _logger = logger;

    private readonly IProfileOrchestrator _ProfileOrchestrator = ProfileOrchestrator
        ?? throw new NotImplementedException();

    private readonly IMapper _mapper = mapper
        ?? throw new NotImplementedException();

    [HttpPost]
    [Route("GetPaginatedProfiles")]
    public async Task<IEnumerable<GetPaginatedUsersResponseDto>> GetPaginatedProfiles(
        GetPaginatedUsersRequestDTO req)
    {
        var request = _mapper.Map<GetPaginatedUsersRequestModel>(req);
        var result = await _ProfileOrchestrator.GetPaginatedUsersAsync(request);
        
        return _mapper.Map<List<GetPaginatedUsersResponseDto>>(result);
    }

    [Authorize]
    [HttpPost]
    [Route("GetProfile")]
    public async Task<IActionResult> GetProfile()
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value 
                ?? User.FindFirst("user")?.Value;

        if (string.IsNullOrEmpty(userId))
            return Unauthorized();

        var request = new GetProfileRequestModel
        {
            Id = int.Parse(userId)
        };

        var result = await _ProfileOrchestrator.GetProfileData(request);

        return Ok(_mapper.Map<GetProfileResponseDTO>(result));
    }

    [HttpPost]
    [Route("GetProfileByUserName")]
    public async Task<GetProfileResponseDTO?> GetProfileByUsername(GetProfileByNameRequestDTO getProfileRequestDto)
    {
        var request = _mapper.Map<GetProfileByNameRequestModel>(getProfileRequestDto);
        var profile = await _ProfileOrchestrator.GetProfileByUsername(request);
        var result = _mapper.Map<GetProfileResponseDTO?>(profile);

        return result;
    }

    [Authorize]
    [HttpPost]
    [Route("UpdateProfile")]
    public async Task<UpdateProfileResponseDTO?> UpdateProfile(UpdateProfileRequestDTO request)
    {
        var req = _mapper.Map<UpdateProfileRequest>(request);
        req.UserId = int.Parse(HttpContext?.User?.Identity?.Name ?? throw new UnauthorizedAccessException()); 
        var result = await _ProfileOrchestrator.UpdateUserPeripherals(req);
        
        return _mapper.Map<UpdateProfileResponseDTO>(result);
    }
}