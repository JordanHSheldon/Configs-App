namespace EsportsProfileWebApi.Web.Orchestrators;

using EsportsProfileWebApi.Web.Orchestrators.Models.Profile;

public interface IProfileOrchestrator
{
    Task<GetProfileResponseModel?> GetProfileByUsername(GetProfileByNameRequestModel dataRequest);

    Task<GetProfileResponseModel> GetProfileData(GetProfileRequestModel dataRequest);

    Task<List<GetPaginatedUsersResponseModel>> GetPaginatedUsersAsync(GetPaginatedUsersRequestModel req);
    
    Task<UpdateProfileResponseModel> UpdateUserPeripherals(UpdateProfileRequest request);
}