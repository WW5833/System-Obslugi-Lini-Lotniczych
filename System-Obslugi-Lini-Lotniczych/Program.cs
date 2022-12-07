using LotSystem.UI;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using LotSystem.Logger.API;
using LotSystem.Repositories;
using LotSystem.Services.UserManagement;
using LotSystem.Logger;
using System.Threading;
using LotSystem.Services;

namespace LotSystem;

internal static class Program
{
    private static IUserService userService;
    private static IEmailService emailService;
    private static ILogger logger;

    private static void Main()
    {
        Console.OutputEncoding = Encoding.UTF8;

        logger = new DebugLogger();

        Console.WriteLine("Starting ...");

        userService = new UserService(new Repository(logger), logger, emailService);

        UserInterfaceManager.Instance.Start(logger, new Dictionary<Type, object>
        {
            { typeof(ILogger), logger },
            { typeof(IUserService), userService },
            { typeof(IEmailService), emailService }
        });

        while (!Environment.HasShutdownStarted)
            Thread.Sleep(1000);
    }
}