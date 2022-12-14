using LotSystem.Database.Models;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace LotSystem.Repositories.API;

public interface ITicketRepository
{
    Task<Ticket> GetTicketById(int id, CancellationToken cancellationToken = default);
    Task<IEnumerable<Ticket>> GetTicketsByFlightId(int flightId, CancellationToken cancellationToken = default);
    Task<IEnumerable<Ticket>> GetTicketsByUserId(int userId, CancellationToken cancellationToken = default);

    Task AddTicket(Ticket ticket, CancellationToken cancellationToken = default);

    Task UpdateSeat(Ticket ticket, string newSeat, CancellationToken cancellationToken = default);
}