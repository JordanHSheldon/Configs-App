namespace EsportsProfileWebApi.Web.Orchestrators;

using AutoMapper;
using EsportsProfileWebApi.Web.Repository;
using EsportsProfileWebApi.Web.Orchestrators.Models.Profile;
using EsportsProfileWebApi.Web.Clients;

public class ProfileOrchestrator(
    IProfileRepository dataRepository,
    IMapper mapper,
    IStatsClient statsClient) : IProfileOrchestrator
{
    private readonly IProfileRepository profileRepository = dataRepository 
        ?? throw new NotImplementedException();
    private readonly IMapper _mapper = mapper 
        ?? throw new NotImplementedException();

    public async Task<GetProfileResponseModel?> GetProfileByUsername(GetProfileByNameRequestModel dataRequest)
    {
        var result = await profileRepository.GetProfileByUsername(dataRequest);

        var temp = _mapper.Map<GetProfileResponseModel?>(result);
        if(temp is not null && result?.SteamId?.Length > 0)
        {
            temp.Stats = await statsClient.GetStatsBySteamId(result?.SteamId ?? "");
        }

        return temp;
    }

    public async Task<GetProfileResponseModel> GetProfileData(GetProfileRequestModel dataRequest)
    {
        var result = await profileRepository.GetProfileData(dataRequest);
        return _mapper.Map<GetProfileResponseModel>(result);
    }

    public async Task<UpdateProfileResponseModel> UpdateData(UpdateProfileRequestModel request)
    {
        var result = await profileRepository.UpdateData(request);
        return _mapper.Map<UpdateProfileResponseModel>(result);
    }

    public async Task<List<GetPaginatedUsersResponseModel>> GetPaginatedUsersAsync(GetPaginatedUsersRequestModel req)
    {
        var result = await profileRepository.GetPaginatedUsersAsync(req);
        return _mapper.Map<List<GetPaginatedUsersResponseModel>>(result);
    }

    public async Task<UpdateProfileResponseModel> UpdateUserPeripherals(UpdateProfileRequest request)
    {
        return await profileRepository.UpdateUserPeripherals(request);
    }
}