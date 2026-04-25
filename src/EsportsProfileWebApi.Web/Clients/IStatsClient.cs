namespace EsportsProfileWebApi.Web.Clients;

public interface IStatsClient
{
    Task<Dictionary<string, object?>> GetStatsBySteamId(string steamId);
}