namespace EsportsProfileWebApi.Web.Controllers;

using AutoMapper;
using DTOs.Data;
using Orchestrators;
using Orchestrators.Models.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

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
    public async Task<GetDataResponseDTO?> GetProfileData()
    {
        try {
            var request = new GetProfileRequestModel
            {
                Id = HttpContext.User.Identity?.Name
            };

            var result = await _dataOrchestrator.GetProfileData(request);
            return _mapper.Map<GetDataResponseDTO>(result);
        }
        catch(Exception e)
        {
            _logger.LogError(e.Message);
        }

        return null;
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
    [Route("UpdateUserPeripherals")]
    public async Task<UpdateDataResponseDTO?> UpdateData(UpdateUserPeripheralsRequestDto request)
    {
        var userId = HttpContext.Request.Cookies["user_id"];
        if (userId == null)
            return null;
            
        var req = mapper.Map<UpdateUserPeripheralsRequest>(request);
        var id = HttpContext?.User?.Claims?.First(c => c.Type == "Id")?.Value;
        req.UserId = int.Parse(userId);
        var result = await _dataOrchestrator.UpdateUserPeripherals(req);
        return _mapper.Map<UpdateDataResponseDTO>(result);
    }

    [HttpPost]
    [Route("GetPeripherals")]
    public async Task<List<PeripheralDto>> GetPeripherals()
    {
        var result = await _dataOrchestrator.GetPeripheralsAsync();
        return _mapper.Map<List<PeripheralDto>>(result);
    }
}