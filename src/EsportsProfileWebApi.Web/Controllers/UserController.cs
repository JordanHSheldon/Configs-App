namespace EsportsProfileWebApi.Web.Controllers;

using Orchestrators;
using Microsoft.AspNetCore.Mvc;

[Route("api/[controller]")]
[ApiController]
public class UserController(IUserOrchestrator userOrchestrator,ILogger<UserController> logger) : Controller
{
    private readonly ILogger<UserController> _logger = logger;

    private readonly IUserOrchestrator _userOrchestrator = userOrchestrator
        ?? throw new NotImplementedException();

    private readonly string redirect_uri = "https://app.configs.cc";

    [HttpGet]
    [Route("DiscordLogin")]
    public IActionResult DiscordLogin()
    {
        return Redirect("https://discord.com/oauth2/authorize?client_id=1362549805502431262&response_type=code&redirect_uri=https%3A%2F%2Fapp.configs.cc%2Fapi%2Fuser%2FDiscordRedirect&scope=identify+email");
    }

    [HttpGet]
    [Route("DiscordRedirect")]
    public async Task<IActionResult> DiscordRedirect(string code)
    {
        var result = await _userOrchestrator.DiscordLogin(code);

        HttpContext.Response.Cookies.Append("user", result.Result, new CookieOptions
        {
            HttpOnly = false,
            Secure = true,
            SameSite = SameSiteMode.None
        });

        return Redirect(redirect_uri);
    }

    [HttpGet]
    [Route("SteamLogin")]
    public IActionResult SteamLogin()
    {
           string steamOpenId = "https://steamcommunity.com/openid/login";

            var returnTo = $"{redirect_uri}/api/user/SteamRedirect";
            var realm = $"{redirect_uri}";

            var query = new QueryString()
                .Add("openid.ns", "http://specs.openid.net/auth/2.0")
                .Add("openid.mode", "checkid_setup")
                .Add("openid.return_to", returnTo)
                .Add("openid.realm", realm)
                .Add("openid.identity", "http://specs.openid.net/auth/2.0/identifier_select")
                .Add("openid.claimed_id", "http://specs.openid.net/auth/2.0/identifier_select");

        return Redirect($"{steamOpenId}{query}");
    }

    [HttpGet]
    [Route("SteamRedirect")]
    public async Task<IActionResult> SteamRedirect()
    {
        try 
        {
            var qs = Request.Query;
            
            if(qs == null) return Unauthorized("");

            var result = await _userOrchestrator.SteamLogin(qs);

            HttpContext.Response.Cookies.Append("user", result.Result, new CookieOptions
            {
                HttpOnly = false,
                Secure = true,
                SameSite = SameSiteMode.None
            });
        }
        catch(Exception e)
        {
            _logger.LogInformation(e.Message);   
        }
        return Redirect(redirect_uri);
    }
}
