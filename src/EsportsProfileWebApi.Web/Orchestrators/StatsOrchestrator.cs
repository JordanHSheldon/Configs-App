namespace EsportsProfileWebApi.Web.Orchestrators;

public class StatsOrchestrator : IStatsOrchestrator
{
    private readonly int cs2AppId = 730;

    private readonly string _apiKey = Environment.GetEnvironmentVariable("steam_api_key")
        ?? throw new NotImplementedException("Steam API Key");

    private readonly HttpClient _httpClient = new();
    
    public async Task<Dictionary<string, object?>> GetStatsBySteamId(string steamId)
    {
        var result = new Dictionary<string, object?>
        {
            ["cs2_stats"] = await GetCs2StatsAsync(steamId),
            ["deadlock_stats"] = await GetDeadlockStatsAsync(steamId)
        };

        return result;
    }
    
    private async Task<Models.Stat.CS2Stats?> GetCs2StatsAsync(string steamId)
    {
        string url = $"https://api.steampowered.com/ISteamUserStats/GetUserStatsForGame/v2/?key={_apiKey}&steamid={steamId}&appid={cs2AppId}";
        
        var cs2_stats = await _httpClient.GetFromJsonAsync<Models.Stat.CS2Stats>(url);
        
        return cs2_stats;
    }

    private async Task<object?> GetDeadlockStatsAsync(string steamId)
    {
        return null;
    }
}