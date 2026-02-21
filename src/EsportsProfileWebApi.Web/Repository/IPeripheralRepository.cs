namespace EsportsProfileWebApi.Web.Repository;

using System.Collections.Generic;

public interface IPeripheralRepository
{
    Task<List<PeripheralEntity>> GetPeripheralsAsync();
}