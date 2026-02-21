namespace EsportsProfileWebApi.Web.Repository;

using System.Collections.Generic;
using Dapper;
using System.Data;
using Npgsql;

public class PeripheralRepository(ILogger<PeripheralRepository> logger) : IPeripheralRepository
{
    private readonly string _connectionString = Environment.GetEnvironmentVariable("connection_string")
        ?? throw new NotImplementedException();
    
    private readonly ILogger<PeripheralRepository> _logger = logger;

    public async Task<List<PeripheralEntity>> GetPeripheralsAsync()
    {
        await using var connection = new NpgsqlConnection(_connectionString);
        await connection.OpenAsync();
        
        string sql = "SELECT * FROM get_peripherals();";
        var peripherals = await connection.QueryAsync<PeripheralEntity>(sql);

        await connection.CloseAsync();

        return [..peripherals];
    }
}