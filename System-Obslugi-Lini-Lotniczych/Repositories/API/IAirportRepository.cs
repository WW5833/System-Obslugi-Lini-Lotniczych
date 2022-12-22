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