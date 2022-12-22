using LotSystem.Database.Models;
using LotSystem.Logger.API;
using LotSystem.Repositories;
using LotSystem.Repositories.API;
using System;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace LotSystem.Services.UserManagement;

public sealed class UserService : IUserService
{
    private readonly IUserRepository _repository;
    private readonly ILogger _logger;

    public UserService(ILogger logger)
    {
        _logger = logger;

        _repository = new Repository(logger);
    }

    public async Task<LoginResponse> LoginUser(string email, string password, CancellationToken cancellationToken = default)
    {
        var user = await _repository.GetUserByEmail(email, cancellationToken);

        if(user is null)
        {
            _logger.Debug($"[LOGIN] Failed to login {email}, user not found");
            return new LoginResponse("Incorrect credentials");
        }

        var passwordHash = Convert.ToBase64String(SHA256.HashData(Encoding.UTF8.GetBytes(password)));
        if (user.Password != passwordHash)
        {
            _logger.Debug($"[LOGIN] Failed to login {email}, password mismatch");
            return new LoginResponse("Incorrect credentials");
        }

        var session = (await _repository.GetSessionByUser(user.Id, cancellationToken))?.Id;

        if (session is not null)
        {
            _logger.Debug($"[LOGIN] Resumed session of {email}");
            return new LoginResponse(session.Value);
        }

        session = await _repository.CreateSession(user, DateTime.Now.AddHours(2), cancellationToken);
        _logger.Debug($"[LOGIN] Logged in {email}");
        return new LoginResponse(session.Value);
    }

    public async Task<GenericResponse> RegisterUser(string email, string password, string firstName, string lastName, string phoneNumber, CancellationToken cancellationToken = default)
    {
        var user = await _repository.GetUserByEmail(email, cancellationToken);

        if (user is not null)
        {
            _logger.Debug($"[REGISTER] Failed to register {email}, user already exists");
            return new GenericResponse("Email already used");
        }

        var passwordHash = Convert.ToBase64String(SHA256.HashData(Encoding.UTF8.GetBytes(password)));

        user = new User
        {
            Email = email,
            Password = passwordHash,
            FirstName = firstName,
            LastName = lastName,
            PhoneNumber = phoneNumber,
            Flags = UserFlags.NONE,
        };

        await _repository.AddUser(user, cancellationToken);
            
        _logger.Debug($"[REGISTER] User registered {email}");
        return new GenericResponse();
    }

    public async Task<GenericResponse> LogoutUser(Guid sessionId, CancellationToken cancellationToken = default)
    {
        await _repository.ExpireSession(sessionId, cancellationToken);
        return new GenericResponse();
    }

    public async Task<User> GetUser(Guid sessionId, CancellationToken cancellationToken = default)
    {
        var session = await _repository.GetSessionById(sessionId, cancellationToken);

        if (session is null)
            return null;

        return await _repository.GetUserBySession(session, cancellationToken);
    }

    public readonly struct LoginResponse
    {
        public bool Successfull => SessionId is not null;
        public readonly string Message;
        public readonly Guid? SessionId;


        public LoginResponse(string message)
        {
            Message = message;
            SessionId = null;
        }

        public LoginResponse(Guid sessionId)
        {
            SessionId = sessionId;
            Message = string.Empty;
        }

        public static implicit operator bool(LoginResponse me)
        {
            return me.Successfull;
        }
    }
}