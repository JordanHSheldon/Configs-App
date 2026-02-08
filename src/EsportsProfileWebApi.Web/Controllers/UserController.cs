namespace EsportsProfileWebApi.Web.Controllers;

using Orchestrators;
using Microsoft.AspNetCore.Mvc;

[Route("api/[controller]")]
[ApiController]
public class UserController(
    IUserOrchestrator userOrchestrator,
    IConfiguration config,
    ILogger<UserController> logger,
    IWebHostEnvironment env) : Controller
{
    private readonly ILogger<UserController> _logger = logger;

    private readonly IUserOrchestrator _userOrchestrator = userOrchestrator
        ?? throw new NotImplementedException();

    private readonly string redirect_uri = config["Authentication:redirect_URL"] ?? throw new NotImplementedException();

    private readonly string discordLoginURL = config["Authentication:DiscordAuthUrl"] ?? throw new NotImplementedException();

    [HttpGet]
    [Route("DiscordLogin")]
    public IActionResult DiscordLogin()
    {
        return Redirect(discordLoginURL);
    }

    [HttpGet]
    [Route("DiscordRedirect")]
    public async Task<IActionResult> DiscordRedirect(string code)
    {
        var result = await _userOrchestrator.DiscordLogin(code);

        HttpContext.Response.Cookies.Append("user", result.Result, new CookieOptions
        {
            HttpOnly = false,
            Secure = false,
            SameSite = SameSiteMode.Lax,
        });

        return Redirect(redirect_uri);
    }

    [HttpGet]
    [Route("SteamLogin")]
    public IActionResult SteamLogin()
    {
        string steamOpenId = "https://steamcommunity.com/openid/login";

        var returnTo = $"{config["Authentication:Issuer"]}/api/user/SteamRedirect";
        var realm = $"{config["Authentication:Issuer"]}";

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
                HttpOnly = true,
                Secure = false,
                SameSite = SameSiteMode.Lax
            });
        }
        catch(Exception e)
        {
            _logger.LogInformation(e.Message);   
        }

        return Redirect(redirect_uri);
    }
}
