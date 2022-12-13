using LotSystem.Database.Models;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace LotSystem.Repositories.API;

public interface IFlightRepository
{
    Task<Flight> GetFlightById(int id, CancellationToken cancellationToken = default);
    Task<IEnumerable<Flight>> GetFlights(CancellationToken cancellationToken = default);
    Task<IEnumerable<Flight>> GetFlightsByStartAirport(int airportId, CancellationToken cancellationToken = default);
    Task<IEnumerable<Flight>> GetFlightsByEndAirport(int airportId, CancellationToken cancellationToken = default);
    Task<IEnumerable<Flight>> GetFlightsByState(FlightState state, CancellationToken cancellationToken = default);
}

public interface IFlightAdminRepository : IFlightRepository
{
    Task AddFlight(Flight flight, CancellationToken cancellationToken = default);
    Task UpdateFlight(Flight flight, CancellationToken cancellationToken = default);
}