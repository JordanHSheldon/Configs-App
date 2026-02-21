namespace EsportsProfileWebApi.Web.Orchestrators;

using EsportsProfileWebApi.Web.Orchestrators.Models.Data;

public interface IPeripheralOrchestrator
{
    Task<List<PeripheralModel>> GetPeripheralsAsync();
}