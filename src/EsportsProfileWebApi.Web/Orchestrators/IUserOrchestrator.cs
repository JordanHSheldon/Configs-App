using EsportsProfileWebApi.Web.Orchestrators.Models.User;

namespace EsportsProfileWebApi.Web.Orchestrators;


public interface IUserOrchestrator
{
    Task<UserLoginResponseModel> DiscordLogin(string code);

    Task<UserLoginResponseModel> SteamLogin(IQueryCollection qs);
}
