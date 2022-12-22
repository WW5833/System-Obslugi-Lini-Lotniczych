using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using LotSystem.Logger.API;
using LotSystem.Repositories;
using LotSystem.Repositories.API;

namespace LotSystem.Services.Airport;

internal class AirportService : IAirportService
{
    private readonly IAirportRepository _repository;

    public AirportService(ILogger logger)
    {
        _repository = new Repository(logger);
    }

    public async Task<IEnumerable<Database.Models.Airport>> GetAirports(CancellationToken cancellationToken = default)
    {
        return await _repository.GetAirports(cancellationToken);
    }
}


public interface IAirportService
{
    Task<IEnumerable<Database.Models.Airport>> GetAirports(CancellationToken cancellationToken = default);
}