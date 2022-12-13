using LotSystem.Database.Models;
using LotSystem.Logger.API;
using LotSystem.Repositories.API;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace LotSystem.Repositories;

internal sealed partial class Repository : ITicketRepository
{
    public async Task<Ticket> GetTicketById(int id, CancellationToken cancellationToken = default)
    {
        await EnableLock(cancellationToken);
        try
        {
            return await _context.Tickets.AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id, cancellationToken: cancellationToken);
        }
        finally
        {
            _isLocked = false;
        }
    }

    public async Task<IEnumerable<Ticket>> GetTicketsByFlightId(
        int flightId,
        CancellationToken cancellationToken = default)
    {
        await EnableLock(cancellationToken);
        try
        {
            return await _context.Tickets.AsNoTracking()
                .Where(x => x.FlightId == flightId).ToArrayAsync(cancellationToken: cancellationToken);
        }
        finally
        {
            _isLocked = false;
        }
    }

    public async Task<IEnumerable<Ticket>> GetTicketsByUserId(int userId, CancellationToken cancellationToken = default)
    {
        await EnableLock(cancellationToken);
        try
        {
            return await _context.Tickets.AsNoTracking()
                .Where(x => x.UserId == userId).ToArrayAsync(cancellationToken: cancellationToken);
        }
        finally
        {
            _isLocked = false;
        }
    }

    public async Task AddTicket(Ticket ticket, CancellationToken cancellationToken = default)
    {
        Debug.Fail("NOT DONE"); // ToDo: FIx it
    }
}
