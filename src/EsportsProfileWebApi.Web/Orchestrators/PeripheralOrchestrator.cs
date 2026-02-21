namespace EsportsProfileWebApi.Web.Orchestrators;

using AutoMapper;
using EsportsProfileWebApi.Web.Repository;
using EsportsProfileWebApi.Web.Orchestrators.Models.Data;

public class PeripheralOrchestrator(IDataRepository dataRepository, IMapper mapper) : IPeripheralOrchestrator
{
    private readonly IDataRepository _dataRepository = dataRepository ?? throw new NotImplementedException();
    private readonly IMapper _mapper = mapper ?? throw new NotImplementedException();

    public async Task<List<PeripheralModel>> GetPeripheralsAsync()
    {
        var result = await _dataRepository.GetPeripheralsAsync();
        return _mapper.Map<List<PeripheralModel>>(result);
    }
}