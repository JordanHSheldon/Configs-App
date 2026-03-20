namespace EsportsProfileWebApi.Web.Orchestrators;

public interface IStatsOrchestrator
{
    Task<string> GetStatsBySteamId(string steamId);
}