using LotSystem.Database.Models;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace LotSystem.Repositories.API;

public interface IUserRepository
{
    Task<User> GetUserById(int id, CancellationToken cancellationToken = default);
    Task<User> GetUserByEmail(string email, CancellationToken cancellationToken = default);

    Task AddUser(User user, CancellationToken cancellationToken = default);
    Task UpdateUser(User user, CancellationToken cancellationToken = default);

    Task<UserSession> GetSessionById(Guid id, CancellationToken cancellationToken = default);
    Task<UserSession> GetSessionByUser(int userId, CancellationToken cancellationToken = default);

    Task<User> GetUserBySession(UserSession session, CancellationToken cancellationToken = default);
    
    Task<Guid> CreateSession(User user, DateTime expireDate, CancellationToken cancellationToken = default);
    Task ExpireSession(Guid sessionId, CancellationToken cancellationToken = default);
}