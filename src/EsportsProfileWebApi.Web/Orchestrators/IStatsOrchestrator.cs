namespace EsportsProfileWebApi.Web.Orchestrators;

public interface IStatsOrchestrator
{
    Task<Dictionary<string, object?>> GetStatsBySteamId(string steamId);
}