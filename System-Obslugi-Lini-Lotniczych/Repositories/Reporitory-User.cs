using LotSystem.Database.Models;
using LotSystem.Repositories.API;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace LotSystem.Repositories;

internal sealed partial class Repository : IUserRepository
{
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
                .FirstOrDefaultAsync(x => x.Email == email, cancellationToken: cancellationToken);
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
                .Where(x => !x.IsExpiered)
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

                .Include(x => x.Tickets)
                .ThenInclude(x => x.Flight)
                .ThenInclude(x => x.StartFrom)

                .Include(x => x.Tickets)
                .ThenInclude(x => x.Flight)
                .ThenInclude(x => x.ArriveAt)

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
}