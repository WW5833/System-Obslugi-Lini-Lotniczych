using LotSystem.UI;
using System;
using System.Collections.Generic;
using LotSystem.Logger.API;
using LotSystem.Repositories;
using LotSystem.Services.UserManagement;
using LotSystem.Logger;

namespace LotSystem
{
    internal static class Program
    {
        private static IUserService _userService;
        private static IEmailService _emailService;
        private static ILogger _logger;

        private static void Main()
        {
            _logger = new DebugLogger();

            Console.WriteLine("Starting ...");

            _userService = new UserService(new Repository(_logger), _logger, _emailService);

            UIManager.Instance.Start(new Dictionary<Type, object>
            {
                { typeof(ILogger), _logger },
                { typeof(IUserService), _userService },
                { typeof(IEmailService), _emailService }
            });
        }
    }
}
