namespace EsportsProfileWebApi.Web.Repository;

using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using Orchestrators.Models.Data;
using Entities.Data;
using Dapper;
using System.Data;
using Npgsql;

public class DataRepository(IConfiguration configuration) : IDataRepository
{
    private readonly string _connectionString = configuration.GetConnectionString("DefaultConnection")
        ?? throw new NotImplementedException();

    public async Task<UpdateDataResponseModel> UpdateData(UpdateDataRequestModel request)
    {
        await using var connection = new NpgsqlConnection(_connectionString);
        await connection.OpenAsync();

        if(Int32.TryParse(request.Id, out int userId) == false)
        {
            throw new ArgumentException("Invalid user ID format.");
        }

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
        
        if(Int32.TryParse(request.Id, out int userId) == false)
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

    public async Task<List<DataEntity>> GetPaginatedUsersAsync(GetPaginatedUsersRequestModel req)
    {
        await using var connection = new NpgsqlConnection(_connectionString);
        await connection.OpenAsync();
 
        var sql = $"[dbo].[get_paginated_users]";
        var users = await connection.QueryAsync<DataEntity>(sql);

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