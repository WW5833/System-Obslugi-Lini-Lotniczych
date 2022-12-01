using LotSystem.Database.Models;
using LotSystem.Logger.API;
using LotSystem.Repositories.API;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace LotSystem.Repositories
{
    internal sealed class Repository
        : IUserRepository,
        IAirportRepository,
        ITicketRepository,
        IFlightRepository
    {
        public Repository(ILogger logger)
        {
            _logger = logger;
            _context = new Database.DatabaseContext(logger);
        }

        private bool _isLocked;
        private readonly ILogger _logger;
        private readonly Database.DatabaseContext _context;

        private async Task EnableLock(CancellationToken cancellationToken)
        {
            while (_isLocked)
                await Task.Delay(10, cancellationToken);

            _isLocked = true;
        }

        #region User

        public async Task AddUser(User user, CancellationToken cancellationToken = default)
        {
            await EnableLock(cancellationToken);
            try
            {
                _context.Users.Add(user);

                await _context.SaveChangesAsync(cancellationToken);
                _context.ChangeTracker.Clear();
            }
            finally
            {
                _isLocked = false;
            }
        }

        public async Task UpdateUser(User user, CancellationToken cancellationToken = default)
        {
            await EnableLock(cancellationToken);
            try
            {
                _context.Users.Update(user);

                await _context.SaveChangesAsync(cancellationToken);
                _context.ChangeTracker.Clear();
            }
            finally
            {
                _isLocked = false;
            }
        }

        public async Task<User> GetUserByEmail(string email, CancellationToken cancellationToken = default)
        {
            await EnableLock(cancellationToken);
            try
            {
                return await _context.Users.AsNoTracking()
                    .FirstOrDefaultAsync(x => x.Email == email);
            }
            finally
            {
                _isLocked = false;
            }
        }

        public async Task<User> GetUserById(int id, CancellationToken cancellationToken = default)
        {
            await EnableLock(cancellationToken);
            try
            {
                return await _context.Users.AsNoTracking()
                    .FirstOrDefaultAsync(x => x.Id == id, cancellationToken: cancellationToken);
            }
            finally
            {
                _isLocked = false;
            }
        }

        public async Task<UserSession> GetSessionById(Guid id, CancellationToken cancellationToken = default)
        {
            await EnableLock(cancellationToken);
            try
            {
                return await _context.Sessions.AsNoTracking()
                    .FirstOrDefaultAsync(x => x.Id == id, cancellationToken: cancellationToken);
            }
            finally
            {
                _isLocked = false;
            }
        }

        public async Task<UserSession> GetSessionByUser(int userId, CancellationToken cancellationToken = default)
        {
            await EnableLock(cancellationToken);
            try
            {
                return await _context.Sessions.AsNoTracking()
                    .FirstOrDefaultAsync(x => x.UserId == userId && !x.IsExpiered, cancellationToken: cancellationToken);
            }
            finally
            {
                _isLocked = false;
            }
        }

        public async Task<User> GetUserBySession(UserSession session, CancellationToken cancellationToken = default)
        {
            await EnableLock(cancellationToken);
            try
            {
                return await _context.Users.AsNoTracking()
                    .FirstOrDefaultAsync(x => session.UserId == x.Id, cancellationToken: cancellationToken);
            }
            finally
            {
                _isLocked = false;
            }
        }

        public async Task<Guid> CreateSession(User user, DateTime expireDate, CancellationToken cancellationToken = default)
        {
            await EnableLock(cancellationToken);
            try
            {
                var session = new UserSession
                {
                    UserId = user.Id,
                    ExpireTime = expireDate,
                    CreationTime = DateTime.Now,
                    Id = Guid.NewGuid()
                };

                _context.Sessions.Add(session);

                await _context.SaveChangesAsync(cancellationToken);
                _context.ChangeTracker.Clear();

                return session.Id;
            }
            finally
            {
                _isLocked = false;
            }
        }

        public async Task ExpireSession(Guid sessionId, CancellationToken cancellationToken = default)
        {
            await EnableLock(cancellationToken);
            try
            {
                var session = await _context.Sessions.AsNoTracking()
                    .FirstOrDefaultAsync(x => x.Id == sessionId, cancellationToken: cancellationToken);

                session.IsExpiered = true;

                _context.Update(session);

                await _context.SaveChangesAsync(cancellationToken);
                _context.ChangeTracker.Clear();
            }
            finally
            {
                _isLocked = false;
            }
        }

        #endregion

        #region Airport

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

        #endregion

        #region Ticket

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

        public async Task<IEnumerable<Ticket>> GetTicketsByFlightId(int flightId, CancellationToken cancellationToken = default)
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

        #endregion

        #region Flight

        public async Task<Flight> GetFlightById(int id, CancellationToken cancellationToken = default)
        {
            await EnableLock(cancellationToken);
            try
            {
                return await _context.Flights.AsNoTracking()
                    .FirstOrDefaultAsync(x => x.Id == id, cancellationToken: cancellationToken);
            }
            finally
            {
                _isLocked = false;
            }
        }

        public async Task<IEnumerable<Flight>> GetFlightsByEndAirport(int airportId, CancellationToken cancellationToken = default)
        {
            await EnableLock(cancellationToken);
            try
            {
                return await _context.Flights.AsNoTracking()
                    .Where(x => x.ArriveAtId == airportId).ToArrayAsync(cancellationToken: cancellationToken);
            }
            finally
            {
                _isLocked = false;
            }
        }

        public async Task<IEnumerable<Flight>> GetFlightsByStartAirport(int airportId, CancellationToken cancellationToken = default)
        {
            await EnableLock(cancellationToken);
            try
            {
                return await _context.Flights.AsNoTracking()
                    .Where(x => x.StartFromId == airportId).ToArrayAsync(cancellationToken: cancellationToken);
            }
            finally
            {
                _isLocked = false;
            }
        }

        public async Task<IEnumerable<Flight>> GetFlightsByState(FlightState state, CancellationToken cancellationToken = default)
        {
            await EnableLock(cancellationToken);
            try
            {
                return await _context.Flights.AsNoTracking()
                    .Where(x => x.State == state).ToArrayAsync(cancellationToken: cancellationToken);
            }
            finally
            {
                _isLocked = false;
            }
        }

        #endregion
    }
}
