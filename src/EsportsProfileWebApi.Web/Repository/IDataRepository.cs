namespace EsportsProfileWebApi.Web.Repository;

using EsportsProfileWebApi.Web.Orchestrators.Models.Data;
using System.Collections.Generic;

public interface IDataRepository
{
    Task<ProfileEntity?> GetUserDataByUsername(GetDataRequestModel dataRequest);

    Task<DataEntity> GetProfileData(GetProfileRequestModel request);

    Task<UpdateDataResponseModel> UpdateData(UpdateDataRequestModel request);

    Task<List<DataEntity>> GetPaginatedUsersAsync(GetPaginatedUsersRequestModel req);

    Task<List<PeripheralEntity>> GetPeripheralsAsync();

    Task<UpdateDataResponseModel> UpdateUserPeripherals(UpdateProfileRequest request);
}