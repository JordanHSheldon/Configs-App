namespace EsportsProfileWebApi.Web.Repository;

using System.Collections.Generic;
using Orchestrators.Models.Profile;
using Dapper;
using System.Data;
using Npgsql;
using EsportsProfileWebApi.Web.Repository.Entities;

public class ProfileRepository(ILogger<ProfileRepository> logger) : IProfileRepository
{
    private readonly string _connectionString = Environment.GetEnvironmentVariable("connection_string")
        ?? throw new NotImplementedException();
    
    private readonly ILogger<ProfileRepository> _logger = logger;

    public async Task<UpdateProfileResponseModel> UpdateData(UpdateProfileRequestModel request)
    {
        await using var connection = new NpgsqlConnection(_connectionString);
        await connection.OpenAsync();

        DynamicParameters parameters = new ();
        parameters.Add("@p_user_id",request.Id, dbType: DbType.Int16);
        parameters.Add("@p_mouse", request.MouseId, dbType: DbType.Int16);
        parameters.Add("@p_mouse_pad", request.MousepadId, dbType: DbType.Int16);
        parameters.Add("@p_key_board", request.KeyboardId ,dbType: DbType.Int16);

        await connection.ExecuteAsync("UpdateUserDataById", parameters, commandType: CommandType.StoredProcedure);
        
        await connection.CloseAsync();

        return new UpdateProfileResponseModel { IsSuccessful = true };
    }

    public async Task<ProfileEntity?> GetProfileByUsername(GetProfileByNameRequestModel request)
    {
        await using var connection = new NpgsqlConnection(_connectionString);
        await connection.OpenAsync();

        DynamicParameters parameters = new ();
        parameters.Add("@username", request.Username, dbType: DbType.String);

        string sql = "SELECT * FROM getProfilebyusername(@username);";
        var profile = await connection.QueryAsync<ProfileEntity>(sql, parameters);

        await connection.CloseAsync();

        return profile?.FirstOrDefault();
    }
    
    public async Task<ProfileEntity> GetProfileData(GetProfileRequestModel request)
    {
        await using var connection = new NpgsqlConnection(_connectionString);

        DynamicParameters parameters = new ();
        parameters.Add("@user_id", request.Id, dbType: DbType.Int16);
        
        await connection.OpenAsync();

        string sql = "SELECT * FROM getProfile(@user_id);";
        ProfileEntity profile = await connection.QuerySingleAsync<ProfileEntity>(sql, parameters);

        await connection.CloseAsync();

        return profile;
    }

    public async Task<ProfileEntity> GetProfile(GetProfileRequestModel request)
    {
        await using var connection = new NpgsqlConnection(_connectionString);

        DynamicParameters parameters = new ();
        parameters.Add("@user_id", request.Id, dbType: DbType.Int16);
        
        await connection.OpenAsync();

        string sql = "SELECT * FROM getProfile(@user_id);";
        ProfileEntity profile = await connection.QuerySingleAsync<ProfileEntity>(sql, parameters);

        await connection.CloseAsync();

        return profile;
    }

    public async Task<List<ProfileEntity>> GetPaginatedUsersAsync(GetPaginatedUsersRequestModel req)
    {
        await using var connection = new NpgsqlConnection(_connectionString);
        await connection.OpenAsync();
        var sql = $"SELECT * FROM get_user_profiles();";

        var users = await connection.QueryAsync<ProfileEntity>(sql);

        await connection.CloseAsync();
        
        return [..users];
    }

    public async Task<List<PeripheralEntity>> GetPeripheralsAsync()
    {
        await using var connection = new NpgsqlConnection(_connectionString);
        await connection.OpenAsync();
        
        string sql = "SELECT * FROM get_peripherals();";
        var peripherals = await connection.QueryAsync<PeripheralEntity>(sql);

        await connection.CloseAsync();

        return [..peripherals];
    }

    public async Task<UpdateProfileResponseModel> UpdateUserPeripherals(UpdateProfileRequest request)
    {
        await using var connection = new NpgsqlConnection(_connectionString);
        await connection.OpenAsync();

        string newPeripheralsCSV = $"{request.KeyboardId},{request.MouseId},{request.MousepadId}";

        DynamicParameters parameters = new ();
        parameters.Add("@user_id", request.UserId, dbType: DbType.Int16);
        parameters.Add("@newPeripherals", newPeripheralsCSV, dbType: DbType.String);

        var sql = "SELECT public.updateProfile(@user_id, @newPeripherals)";

        var result = await connection.ExecuteAsync(sql,parameters);

        await connection.CloseAsync();

        return new UpdateProfileResponseModel { IsSuccessful = true };
    }
}