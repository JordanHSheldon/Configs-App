namespace EsportsProfileWebApi.Web.Repository;

using EsportsProfileWebApi.Web.Orchestrators.Models.Profile;
using EsportsProfileWebApi.Web.Repository.Entities;
using System.Collections.Generic;

public interface IProfileRepository
{
    Task<ProfileEntity?> GetProfileByUsername(GetProfileByNameRequestModel dataRequest);

    Task<ProfileEntity> GetProfileData(GetProfileRequestModel request);

    Task<UpdateProfileResponseModel> UpdateData(UpdateProfileRequestModel request);

    Task<List<ProfileEntity>> GetPaginatedUsersAsync(GetPaginatedUsersRequestModel req);

    Task<List<PeripheralEntity>> GetPeripheralsAsync();

    Task<UpdateProfileResponseModel> UpdateUserPeripherals(UpdateProfileRequest request);
}