namespace EsportsProfileWebApi.Web.Repository;

using EsportsProfileWebApi.Web.Orchestrators;

public interface IUserRepository
{
    Task<int> DiscordLogin(DiscordUserData discordUserData);

    Task<int> SteamLogin(SteamUserData steamUserData);
}
