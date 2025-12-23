namespace EsportsProfileWebApi.Web.Repository;

using Dapper;
using System.Data;
using EsportsProfileWebApi.Web.Orchestrators;
using Npgsql;

public class UserRepository() : IUserRepository
{
    private readonly string _connectionString = Environment.GetEnvironmentVariable("connection_string")
        ?? throw new NotImplementedException();

    public async Task<int> DiscordLogin(DiscordUserData discordUserData)
    {
        await using var connection = new NpgsqlConnection(_connectionString);
        await connection.OpenAsync();

        DynamicParameters parameters = new();
        parameters.Add("@p_email", discordUserData.email, DbType.String);
        parameters.Add("@p_username", discordUserData.username, DbType.String);
        parameters.Add("@p_discord_id", discordUserData.id, DbType.String);
        parameters.Add("@p_avatar", discordUserData.avatar, DbType.String);

        string sql = "select * from discord_login_or_register_user( @p_email, @p_username, @p_discord_id, @p_avatar);";
        var userId = await connection.QueryFirstOrDefaultAsync<int>(sql,parameters);

        await connection.CloseAsync();

        return userId;
    }

    public async Task<int> SteamLogin(SteamUserData steamUserData)
    {
        await using var connection = new NpgsqlConnection(_connectionString);
        await connection.OpenAsync();

        DynamicParameters parameters = new();
        parameters.Add("@p_username", steamUserData.Username, DbType.String);
        parameters.Add("@p_steam_id", steamUserData.SteamID, DbType.String);
        parameters.Add("@p_avatar", steamUserData.Avatar, DbType.String);

        string sql = "select * from steam_login_or_register_user(@p_username, @p_steam_id, @p_avatar);";
        var userId = await connection.QueryFirstOrDefaultAsync<int>(sql,parameters);

        await connection.CloseAsync();

        return userId;
    }
}