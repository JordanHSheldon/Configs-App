namespace EsportsProfileWebApi.Web.Repository;

using Microsoft.Extensions.Configuration;
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

        if(!int.TryParse(request.Id, out int userId))
            throw new ArgumentException("Invalid user ID format.");

        DynamicParameters parameters = new ();
        parameters.Add("@p_user_id",userId, dbType: DbType.String);
        parameters.Add("@p_mouse", request.Mouse, dbType: DbType.String);
        parameters.Add("@p_mouse_pad", request.MousePad, dbType: DbType.String);
        parameters.Add("@p_head_set", request.HeadSet, dbType: DbType.String);
        parameters.Add("@p_monitor", request.Monitor, dbType: DbType.String);
        parameters.Add("@p_key_board", request.KeyBoard ,dbType: DbType.String);

        await connection.ExecuteAsync("UpdateUserDataById", parameters, commandType: CommandType.StoredProcedure);
        
        await connection.CloseAsync();
        return new UpdateDataResponseModel { IsSuccessful = true };
    }

    public async Task<DataEntity> GetUserData(GetDataRequestModel request)
    {
        await using var connection = new NpgsqlConnection(_connectionString);
        await connection.OpenAsync();

        DynamicParameters parameters = new ();
        parameters.Add("@username", request.Username, dbType: DbType.String);

        string sql = "SELECT * FROM getProfilebyusername(@username);";
        DataEntity profile = await connection.QuerySingleAsync<DataEntity>(sql, parameters);

        await connection.CloseAsync();

        return profile ?? new DataEntity();
    }
    
    public async Task<DataEntity> GetProfileData(GetProfileRequestModel request)
    {
        await using var connection = new NpgsqlConnection(_connectionString);
        _logger.LogInformation($"DATA LOGGED :{request?.Id}");

        if(Int32.TryParse(request?.Id, out int userId) == false)
        {
            throw new ArgumentException("Invalid user ID format.");
        }

        DynamicParameters parameters = new ();
        parameters.Add("@user_id", userId, dbType: DbType.Int16);
        
        await connection.OpenAsync();

        string sql = "SELECT * FROM getProfile(@user_id);";
        DataEntity profile = await connection.QuerySingleAsync<DataEntity>(sql, parameters);

        await connection.CloseAsync();

        return profile;
    }

    public async Task<ProfileEntity> GetProfile(GetProfileRequestModel request)
    {
        await using var connection = new NpgsqlConnection(_connectionString);

        if(int.TryParse(request?.Id, out int userId) == false)
            throw new ArgumentException("Invalid user ID format.");

        DynamicParameters parameters = new ();
        parameters.Add("@user_id", userId, dbType: DbType.Int16);
        
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
        try{
            var sql = $"SELECT * FROM get_user_profiles();";

            IEnumerable<DataEntity> users = await connection.QueryAsync<DataEntity>(sql);

            await connection.CloseAsync();
            
            return [..users];
        }
        catch(Exception ex)
        {
            Console.WriteLine(ex.Message);
        }

        return [];
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

    public async Task<UpdateDataResponseModel> UpdateUserPeripherals(UpdateUserPeripheralsRequest request)
    {
        await using var connection = new NpgsqlConnection(_connectionString);
        await connection.OpenAsync();

        var sql = @"INSERT INTO peripherals (peripheral_user_id, peripheral_picklist_peripherals_id) VALUES (@peripheral_user_id, @peripheral_picklist_peripherals_id);";

        var rows = request.PeripheralIds.AsEnumerable().Select(r => new
        {
            user_id = request.UserId,
            picklist_peripheral_id = r
        });

        await connection.ExecuteAsync(sql, rows);

        await connection.CloseAsync();

        return new UpdateDataResponseModel { IsSuccessful = true };
    }
}