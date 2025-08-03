namespace EsportsProfileWebApi.Web.Repository;

using Dapper;
using Orchestrators.Models.User;
using Microsoft.Extensions.Configuration;
using System.Data;
using EsportsProfileWebApi.Web.Orchestrators;
using Npgsql;

public class UserRepository(IConfiguration configuration) : IUserRepository
{
    private readonly string _connectionString = configuration.GetConnectionString("DefaultConnection")
                                                ?? throw new NotImplementedException();

   public async Task<int> RegisterUser(UserRegisterRequestModel request)
    {
        await using var connection = new NpgsqlConnection(_connectionString);
        await connection.OpenAsync();

        var parameters = new DynamicParameters();
        parameters.Add("@Email", request.Email, DbType.String);
        parameters.Add("@Password", Helpers.PasswordHashing.HashPassword(request.Password), DbType.String);
        parameters.Add("@Username", request.Username, DbType.String);

        string sql = "SELECT register_user(@Email, @Password, @Username);";
        int userId = await connection.QuerySingleAsync<int>(sql, parameters);

        await connection.CloseAsync();

        return userId;
    }


    public async Task<int> LoginUser(UserLoginRequestModel request)
    {
        await using var connection = new NpgsqlConnection(_connectionString);
        await connection.OpenAsync();

        DynamicParameters parameters = new ();
        parameters.Add("@user_password", Helpers.PasswordHashing.HashPassword(request.Password), DbType.String);
        parameters.Add("@user_email", request.Email, DbType.String);

        string sql = "SELECT login_user(@user_password, @user_email);";
        int userId = await connection.QuerySingleAsync<int>(sql, parameters);

        await connection.CloseAsync();  

        return userId;
    }   

    public async Task<int> DiscordLogin(DiscordUserData discordUserData)
    {
        await using var connection = new NpgsqlConnection(_connectionString);
        await connection.OpenAsync();

        DynamicParameters parameters = new ();
        parameters.Add("@email", discordUserData.email, DbType.String);
        parameters.Add("@username", discordUserData.username, DbType.String);
        parameters.Add("@discord_id", discordUserData.id, DbType.String);

        var userId = await connection.QueryFirstOrDefaultAsync<int>("discord_login_user", parameters, commandType: CommandType.StoredProcedure);
        
        await connection.CloseAsync();  

        return userId;
    }
}