namespace EsportsProfileWebApi.Web.Orchestrators;

using EsportsProfileWebApi.Web.Orchestrators.Models.Data;

public interface IDataOrchestrator
{
    Task<GetDataResponseModel?> GetUserDataByUsername(GetDataRequestModel dataRequest);

    Task<GetDataResponseModel> GetProfileData(GetProfileRequestModel dataRequest);

    Task<UpdateDataResponseModel> UpdateData(UpdateDataRequestModel request);

    Task<List<GetPaginatedUsersResponseModel>> GetPaginatedUsersAsync(GetPaginatedUsersRequestModel req);

    Task<List<PeripheralModel>> GetPeripheralsAsync();
    
    Task<UpdateDataResponseModel> UpdateUserPeripherals(UpdateProfileRequest request);
}