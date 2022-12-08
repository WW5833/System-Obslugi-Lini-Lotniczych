using System.Collections.Generic;
using LotSystem.Database.Models;
using System.Threading;
using System.Threading.Tasks;

namespace LotSystem.Repositories.API;

public interface IAirportRepository
{
    Task<IEnumerable<Airport>> GetAirports(CancellationToken cancellationToken = default);
    Task<Airport> GetAirportById(int id, CancellationToken cancellationToken = default);
    Task<Airport> GetAirportByName(string name, CancellationToken cancellationToken = default);
}

public interface IAirportAdminRepository : IAirportRepository
{
    Task AddAirport(Airport airport, CancellationToken cancellationToken = default);
    Task UpdateAirport(Airport airport, CancellationToken cancellationToken = default);
    Task RemoveAirport(int id, CancellationToken cancellationToken = default);
}

internal sealed class DummyAirportRepository : IAirportRepository
{
    public Task<Airport> GetAirportById(int id, CancellationToken cancellationToken = default)
    {
        return Task.FromResult(new Airport { Name = "Test Airport", ShortName = "TST" });
    }

    public Task<Airport> GetAirportByName(string name, CancellationToken cancellationToken = default)
    {
        return Task.FromResult(new Airport { Name = "Test Airport", ShortName = "TST" });
    }

    public Task<IEnumerable<Airport>> GetAirports(CancellationToken cancellationToken = default)
    {
        return Task.FromResult<IEnumerable<Airport>>(new[] 
        { 
            new Airport 
            {
                Name = "Test Airport",
                ShortName = "TST"
            },
            new Airport
            {
                Name = "Traktor Airport",
                ShortName = "TRA"
            },
            new Airport
            {
                Name = "Mine Airport",
                ShortName = "MIN"
            }
        });
    }
}