using LotSystem.Database.Models;
using System;
using System.Threading;
using System.Threading.Tasks;
using static LotSystem.Services.UserManagement.UserService;

namespace LotSystem.Services.UserManagement;

public interface IUserService : IService
{
    Task<User> GetUser(Guid sessionId, CancellationToken cancellationToken = default);
    Task<LoginResponse> LoginUser(string email, string password, CancellationToken cancellationToken = default);
    Task<GenericResponse> RegisterUser(string email, string password, string firstName, string lastName, string phoneNumber, CancellationToken cancellationToken = default);
    Task<GenericResponse> SendEmailVerificationRequest(int userId, CancellationToken cancellationToken = default);
    Task<GenericResponse> LogoutUser(Guid sessionId, CancellationToken cancellationToken = default);
}