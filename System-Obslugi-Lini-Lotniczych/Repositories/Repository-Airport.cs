using LotSystem.Database.Models;
using LotSystem.Repositories.API;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace LotSystem.Repositories;

internal sealed partial class Repository : IAirportRepository
{
    public async Task<IEnumerable<Airport>> GetAirports(CancellationToken cancellationToken = default)
    {
        await EnableLock(cancellationToken);
        try
        {
            return await _context.Airports.AsNoTracking()
                .ToArrayAsync(cancellationToken: cancellationToken);
        }
        finally
        {
            _isLocked = false;
        }
    }

    public async Task<Airport> GetAirportById(int id, CancellationToken cancellationToken = default)
    {
        await EnableLock(cancellationToken);
        try
        {
            return await _context.Airports.AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id, cancellationToken: cancellationToken);
        }
        finally
        {
            _isLocked = false;
        }
    }

    public async Task<Airport> GetAirportByName(string name, CancellationToken cancellationToken = default)
    {
        await EnableLock(cancellationToken);
        try
        {
            return await _context.Airports.AsNoTracking()
                .FirstOrDefaultAsync(x => x.Name == name, cancellationToken: cancellationToken);
        }
        finally
        {
            _isLocked = false;
        }
    }
}
