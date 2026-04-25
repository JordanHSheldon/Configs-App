namespace EsportsProfileWebApi.Web.Orchestrators;

using EsportsProfileWebApi.Web.Orchestrators.Models.Peripheral;

public interface IPeripheralOrchestrator
{
    Task<List<PeripheralModel>> GetPeripheralsAsync();
}