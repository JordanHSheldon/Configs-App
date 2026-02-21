namespace EsportsProfileWebApi.Web.Repository;

using System.Collections.Generic;
using Orchestrators.Models.Data;
using Dapper;
using System.Data;
using Npgsql;

public class DataRepository(ILogger<DataRepository> logger) : IDataRepository
{
    private readonly string _connectionString = Environment.GetEnvironmentVariable("connection_string")
        ?? throw new NotImplementedException();
    
    private readonly ILogger<DataRepository> _logger = logger;

    public async Task<UpdateDataResponseModel> UpdateData(UpdateDataRequestModel request)
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

        return new UpdateDataResponseModel { IsSuccessful = true };
    }

    public async Task<ProfileEntity?> GetUserDataByUsername(GetDataRequestModel request)
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
    
    public async Task<DataEntity> GetProfileData(GetProfileRequestModel request)
    {
        await using var connection = new NpgsqlConnection(_connectionString);

        DynamicParameters parameters = new ();
        parameters.Add("@user_id", request.Id, dbType: DbType.Int16);
        
        await connection.OpenAsync();

        string sql = "SELECT * FROM getProfile(@user_id);";
        DataEntity profile = await connection.QuerySingleAsync<DataEntity>(sql, parameters);

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

    public async Task<List<DataEntity>> GetPaginatedUsersAsync(GetPaginatedUsersRequestModel req)
    {
        await using var connection = new NpgsqlConnection(_connectionString);
        await connection.OpenAsync();
        var sql = $"SELECT * FROM get_user_profiles();";

        IEnumerable<DataEntity> users = await connection.QueryAsync<DataEntity>(sql);

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

    public async Task<UpdateDataResponseModel> UpdateUserPeripherals(UpdateProfileRequest request)
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

        return new UpdateDataResponseModel { IsSuccessful = true };
    }
}