using LotSystem.Database.Models;
using System.Threading;
using System.Threading.Tasks;

namespace LotSystem.Repositories.API;

public interface IAirportRepository
{
    Task<Airport> GetAirportById(int id, CancellationToken cancellationToken = default);
    Task<Airport> GetAirportByName(string name, CancellationToken cancellationToken = default);
}

public interface IAirportAdminRepository : IAirportRepository
{
    Task AddAirport(Airport airport, CancellationToken cancellationToken = default);
    Task UpdateAirport(Airport airport, CancellationToken cancellationToken = default);
    Task RemoveAirport(int id, CancellationToken cancellationToken = default);
}