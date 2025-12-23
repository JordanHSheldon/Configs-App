namespace EsportsProfileWebApi.Web.Orchestrators;

using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text.Json;
using Helpers;
using Models.User;
using Repository;

public struct DiscordUserResponse
{
    public string token_type {get;set;}
    public string access_token {get;set;}
    public int expires_in {get;set;}
    public string refresh_token {get;set;}
    public string scope {get;set;}
}

public struct DiscordUserData
{
    public string id {get;set;}
    public string username {get;set;}
    public string avatar {get;set;}
    public string discriminator {get;set;}
    public int public_flags {get;set;}
    public int flags {get;set;}
    public object banner {get;set;}
    public object accent_color {get;set;}
    public object global_name {get;set;}
    public object avatar_decoration_data {get;set;}
    public object collectibles {get;set;}
    public object banner_color {get;set;}
    public object clan {get;set;}
    public object primary_guild {get;set;}
    public bool mfa_enabled {get;set;}
    public string locale {get;set;}
    public int premium_type {get;set;}
    public string email {get;set;}
    public bool verified {get;set;}
}

public struct SteamUserData
{
    public string SteamID {get;set;}

    public string Username {get;set;}

    public string Avatar {get;set;}
}

public class UserOrchestrator(IUserRepository userRepository, IConfiguration config, ILogger<UserOrchestrator> logger) : IUserOrchestrator
{
    private readonly IUserRepository _userRepository = userRepository ?? throw new NotImplementedException();

    private readonly TokenBuilder _tokenBuilder = new(config);

    private readonly ILogger<UserOrchestrator> _logger = logger;

    private const string NotFoundResult = "Not found";
    
    private const string ErrorResult = "Error creating user data";

    private readonly string clientID = Environment.GetEnvironmentVariable("discord_client_id")
        ?? throw new NotImplementedException();

    private readonly string clientSecret = Environment.GetEnvironmentVariable("discord_client_secret")
        ?? throw new NotImplementedException();

    private readonly string redirectURI = "https://app.configs.cc/api/user/DiscordRedirect";

    private readonly string tokenURL = "https://discord.com/api/oauth2/token";

    private readonly string userURL = "https://discord.com/api/users/@me";

    public async Task<UserLoginResponseModel> DiscordLogin(string code)
    {
        var values = new Dictionary<string, string>
        {
            { "client_id", clientID },
            { "client_secret", clientSecret },
            { "grant_type", "authorization_code" },
            { "code", code },
            { "scope", "identify email" },
            { "redirect_uri", redirectURI }
        };

        using var client = new HttpClient();
        var response = await client.PostAsync(tokenURL, new FormUrlEncodedContent(values));

        if (!response.IsSuccessStatusCode)
        {
            var text = await response.Content.ReadAsStringAsync();

            return new UserLoginResponseModel { Result = ErrorResult};
        }

        var responseString = await response.Content.ReadAsStringAsync();

        var tokenResponse = JsonSerializer.Deserialize<DiscordUserResponse>(responseString);

        using var requestMessage = new HttpRequestMessage(HttpMethod.Get, userURL);
        
        requestMessage.Headers.Authorization = new AuthenticationHeaderValue("Bearer", tokenResponse.access_token);
        
        var discordUserResponse = await client.SendAsync(requestMessage);
        var discordUserData = await discordUserResponse.Content.ReadAsStringAsync();
        var discordUser = JsonSerializer.Deserialize<DiscordUserData>(discordUserData);

        var userID = await _userRepository.DiscordLogin(discordUser);
        if (userID == 0)
        {
            return new UserLoginResponseModel
            {
                Result = NotFoundResult
            };
        }

        return new UserLoginResponseModel
        {
            Result = await _tokenBuilder.BuildToken([new Claim("user", userID.ToString())])
        };
    }

    public async Task<UserLoginResponseModel> SteamLogin(IQueryCollection qs)
    {
        string api_key = Environment.GetEnvironmentVariable("steam_api_key")
        ?? throw new NotImplementedException();
        
        // Validate openid via Steam
        var verificationUrl = "https://steamcommunity.com/openid/login";

        var form = new Dictionary<string, string>();
        foreach (var kv in qs)
        {
            form.Add(kv.Key, kv.Value);
        }

        form["openid.mode"] = "check_authentication";

        using var client = new HttpClient();
        var response = await client.PostAsync(verificationUrl, new FormUrlEncodedContent(form));
        var body = await response.Content.ReadAsStringAsync();
        _logger.LogInformation(body);
        // Steam responds with "is_valid:true"
        if (!body.Contains("is_valid:true")) return new UserLoginResponseModel { Result = NotFoundResult };

        // Extract Steam64 ID
        string claimedId = qs["openid.claimed_id"];
        
        string steamId = claimedId.Split('/').Last();

        string url = $"https://api.steampowered.com/ISteamUser/GetPlayerSummaries/v2/?key={api_key}&steamids={steamId}";

        var steam_user_response = await client.GetAsync(url);
        response.EnsureSuccessStatusCode();

        var json = await steam_user_response.Content.ReadAsStringAsync();

        using var doc = JsonDocument.Parse(json);

        var player = doc.RootElement
            .GetProperty("response")
            .GetProperty("players")[0];

        string username = player.GetProperty("personaname").GetString();
        string avatar = player.GetProperty("avatarfull").GetString();

        var userID = await _userRepository.SteamLogin(new SteamUserData
        {
            Avatar = avatar,
            Username = username,
            SteamID = steamId
        });

        if (userID == 0)
        {
            return new UserLoginResponseModel
            {
                Result = NotFoundResult
            };
        }

        return new UserLoginResponseModel
        {
            Result = await _tokenBuilder.BuildToken([new Claim("user", userID.ToString())])
        };
    }
}