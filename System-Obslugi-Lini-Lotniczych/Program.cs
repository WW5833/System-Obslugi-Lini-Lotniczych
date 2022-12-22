using LotSystem.UI;
using System;
using System.Collections.Generic;
using System.Text;
using LotSystem.Logger.API;
using LotSystem.Repositories;
using LotSystem.Services.UserManagement;
using LotSystem.Logger;
using System.Threading;
using LotSystem.Services.Airport;
using LotSystem.Repositories.API;

namespace LotSystem;

internal static class Program
{
    private static IUserService userService;
    private static ILogger logger;
    private static Repository repository;

    private static void Main()
    {
        Console.OutputEncoding = Encoding.UTF8;

        logger = new FileLogger();

        logger.Info("Starting ...");

        repository = new Repository(logger);

        userService = new UserService(logger);

        UserInterfaceManager.Instance.Start(logger, new Dictionary<Type, object>
        {
            { typeof(ILogger), logger },
            { typeof(IUserService), userService },
            { typeof(IAirportService), new AirportService(logger) },
            { typeof(IFlightRepository), repository },
            { typeof(ITicketRepository), repository }
        });

        while (!Environment.HasShutdownStarted)
            Thread.Sleep(1000);
    }
}