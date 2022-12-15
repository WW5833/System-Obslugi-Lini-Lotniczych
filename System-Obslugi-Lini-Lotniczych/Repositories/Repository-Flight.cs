using LotSystem.Database.Models;
using LotSystem.Repositories.API;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace LotSystem.Repositories;

internal sealed partial class Repository : IFlightRepository
{
    public async Task<Flight> GetFlightById(int id, CancellationToken cancellationToken = default)
    {
        await EnableLock(cancellationToken);
        try
        {
            return await _context.Flights.AsNoTracking()
                .Include(x => x.StartFrom)
                .Include(x => x.ArriveAt)
                .FirstOrDefaultAsync(x => x.Id == id, cancellationToken: cancellationToken);
        }
        finally
        {
            _isLocked = false;
        }
    }

    public async Task<IEnumerable<Flight>> GetFlightsByEndAirport(
        int airportId,
        CancellationToken cancellationToken = default)
    {
        await EnableLock(cancellationToken);
        try
        {
            return await _context.Flights.AsNoTracking()
                .Where(x => x.ArriveAtId == airportId)
                .Include(x => x.StartFrom)
                .Include(x => x.ArriveAt)
                .ToArrayAsync(cancellationToken: cancellationToken);
        }
        finally
        {
            _isLocked = false;
        }
    }

    public async Task<IEnumerable<Flight>> GetFlightsByStartAirport(
        int airportId,
        CancellationToken cancellationToken = default)
    {
        await EnableLock(cancellationToken);
        try
        {
            return await _context.Flights.AsNoTracking()
                .Where(x => x.StartFromId == airportId)
                .Include(x => x.StartFrom)
                .Include(x => x.ArriveAt)
                .ToArrayAsync(cancellationToken: cancellationToken);
        }
        finally
        {
            _isLocked = false;
        }
    }

    public async Task<IEnumerable<Flight>> GetFlightsByState(
        FlightState state,
        CancellationToken cancellationToken = default)
    {
        await EnableLock(cancellationToken);
        try
        {
            return await _context.Flights.AsNoTracking()
                .Where(x => x.State == state)
                .Include(x => x.StartFrom)
                .Include(x => x.ArriveAt)
                .ToArrayAsync(cancellationToken: cancellationToken);
        }
        finally
        {
            _isLocked = false;
        }
    }

    public async Task<IEnumerable<Flight>> GetFlights(CancellationToken cancellationToken = default)
    {
        await EnableLock(cancellationToken);
        try
        {
            return await _context.Flights.AsNoTracking()
                .Include(x => x.StartFrom)
                .Include(x => x.ArriveAt)
                .ToArrayAsync(cancellationToken: cancellationToken);
        }
        finally
        {
            _isLocked = false;
        }
    }
}

