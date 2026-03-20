using System.Text.Json;
using EsportsProfileWebApi.Web.Orchestrators.Models.Stat;

namespace EsportsProfileWebApi.Web.Orchestrators;

public class StatsOrchestrator : IStatsOrchestrator
{
    private readonly string _apiKey = Environment.GetEnvironmentVariable("steam_api_key");
    private readonly HttpClient _httpClient = new();
    
    public async Task<string> GetStatsBySteamId(string steamId)
    {
        var result = await GetCs2StatsAsync(steamId);
        return result;
    }
    
    public async Task<JsonElement?> GetPlayerSummaryAsync(string steamId)
    {
        string url = $"https://api.steampowered.com/ISteamUser/GetPlayerSummaries/v2/?key={_apiKey}&steamids={steamId}";
        var response = await _httpClient.GetStringAsync(url);
        
        using JsonDocument doc = JsonDocument.Parse(response);

        var players = doc.RootElement.GetProperty("response").GetProperty("players");
        if (players.GetArrayLength() > 0)
        {
            return players[0];
        }
        
        return null;
    }

    public async Task<string?> GetCs2StatsAsync(string steamId)
    {
        int cs2AppId = 730;
        string url = $"https://api.steampowered.com/ISteamUserStats/GetUserStatsForGame/v2/?key={_apiKey}&steamid={steamId}&appid={cs2AppId}";
        var response = await _httpClient.GetStringAsync(url);
        
        // CS2Stats raw = JsonSerializer.Deserialize<CS2Stats>(response);
        //
        // CS2Stats stat = new CS2Stats(raw);

        return response;
    }

    public async Task<int> GetDeadlockStatsAsync(string steamId)
    {
        return -1;
    }

    public async Task<Dictionary<string, object>> GetCs2AndDeadlockStatsAsync(string steamId)
    {
        var result = new Dictionary<string, object>();
        var playerInfo = await GetPlayerSummaryAsync(steamId);
        var cs2Stats = await GetCs2StatsAsync(steamId);
        var deadlocks = await GetDeadlockStatsAsync(steamId);

        result["player_info"] = playerInfo;
        result["cs2_stats"] = cs2Stats;
        result["deadlock_stats"] = deadlocks;

        return result;
    }
}