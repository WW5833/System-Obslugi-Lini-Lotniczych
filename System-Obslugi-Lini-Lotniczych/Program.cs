using LotSystem.UI;
using System;
using System.Collections.Generic;
using System.Text;
using LotSystem.Logger.API;
using LotSystem.Repositories;
using LotSystem.Services.UserManagement;
using LotSystem.Logger;
using System.Threading;
using LotSystem.Services;
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

    #region ImportFlights
    /*
    public static async void ImportData()
    {
        var context = new DatabaseContext(new DebugLogger());

        string data = @"
WAW;WMI;10:55;13:00;01.02.2023
WAW;WRO;10:55;13:00;23.02.2023
WAW;POZ;10:55;13:00;13.02.2023
WAW;KRK;10:55;13:00;13.02.2023
WAW;KTW;10:55;13:00;12.02.2023
WAW;LCJ;10:55;13:00;01.02.2023
WAW;GDN;10:55;13:00;13.02.2023
WAW;BZG;10:55;13:00;12.02.2023
WAW;RZE;10:55;13:00;09.02.2023
WAW;SZZ;10:55;13:00;28.02.2023
WAW;IEG;10:55;13:00;13.02.2023
WMI;WAW;10:55;13:00;11.02.2023
WMI;WRO;10:55;13:00;12.02.2023
WMI;POZ;10:55;13:00;10.02.2023
WMI;KRK;10:55;13:00;05.02.2023
WMI;KTW;10:55;13:00;05.02.2023
WMI;LCJ;10:55;13:00;04.02.2023
WMI;GDN;10:55;13:00;03.02.2023
WMI;BZG;10:55;13:00;02.02.2023
WMI;RZE;10:55;13:00;01.02.2023
WMI;SZZ;10:55;13:00;27.02.2023
WMI;IEG;10:55;13:00;28.02.2023
WRO;WAW;10:55;13:00;25.02.2023
WRO;RZE;10:55;13:00;23.02.2023
WRO;SZZ;10:55;13:00;25.02.2023
WRO;IEG;10:55;13:00;12.02.2023
WRO;WAW;10:55;13:00;24.02.2023
WRO;WRO;10:55;13:00;24.02.2023
WRO;POZ;10:55;13:00;05.02.2023
WRO;KRK;10:55;13:00;15.02.2023
WRO;KTW;10:55;13:00;18.02.2023
WRO;LCJ;10:55;13:00;07.02.2023
WRO;GDN;10:55;13:00;08.02.2023
POZ;WMI;10:55;13:00;21.02.2023
POZ;WRO;10:55;13:00;28.02.2023
POZ;WAW;10:55;13:00;06.02.2023
POZ;KRK;10:55;13:00;17.02.2023
POZ;KTW;10:55;13:00;12.02.2023
POZ;LCJ;10:55;13:00;21.02.2023
POZ;GDN;10:55;13:00;24.02.2023
POZ;BZG;10:55;13:00;22.02.2023
POZ;RZE;10:55;13:00;11.02.2023
POZ;SZZ;10:55;13:00;10.02.2023
POZ;IEG;10:55;13:00;17.02.2023
KRK;WAW;10:55;13:00;18.02.2023
KRK;WRO;10:55;13:00;19.02.2023
KRK;POZ;10:55;13:00;20.02.2023
KRK;WMI;10:55;13:00;05.02.2023
KRK;KTW;10:55;13:00;06.02.2023
KRK;LCJ;10:55;13:00;01.02.2023
KRK;GDN;10:55;13:00;19.02.2023
KRK;BZG;10:55;13:00;19.02.2023
KRK;RZE;10:55;13:00;19.02.2023
KRK;SZZ;10:55;13:00;28.02.2023
KRK;IEG;10:55;13:00;26.02.2023
KTW;WAW;10:55;13:00;25.02.2023
KTW;RZE;10:55;13:00;22.02.2023
KTW;SZZ;10:55;13:00;11.02.2023
KTW;IEG;10:55;13:00;03.02.2023
KTW;WAW;10:55;13:00;13.02.2023
KTW;WRO;10:55;13:00;11.02.2023
KTW;POZ;10:55;13:00;10.02.2023
KTW;KRK;10:55;13:00;28.02.2023
KTW;WRO;10:55;13:00;05.02.2023
KTW;LCJ;10:55;13:00;03.02.2023
KTW;GDN;10:55;13:00;02.02.2023
LCJ;WMI;10:55;13:00;01.02.2023
LCJ;WRO;10:55;13:00;26.02.2023
LCJ;POZ;10:55;13:00;22.02.2023
LCJ;KRK;10:55;13:00;21.02.2023
LCJ;KTW;10:55;13:00;12.02.2023
LCJ;WAW;10:55;13:00;18.02.2023
LCJ;GDN;10:55;13:00;19.02.2023
LCJ;BZG;10:55;13:00;12.02.2023
LCJ;RZE;10:55;13:00;17.02.2023
LCJ;SZZ;10:55;13:00;15.02.2023
LCJ;IEG;10:55;13:00;11.02.2023
GDN;WAW;10:55;13:00;10.02.2023
GDN;WRO;10:55;13:00;09.02.2023
GDN;POZ;10:55;13:00;07.02.2023
GDN;KRK;10:55;13:00;04.02.2023
GDN;KTW;10:55;13:00;23.02.2023
GDN;LCJ;10:55;13:00;12.02.2023
GDN;WMI;10:55;13:00;18.02.2023
GDN;BZG;10:55;13:00;15.02.2023
GDN;RZE;10:55;13:00;16.02.2023
GDN;SZZ;10:55;13:00;19.02.2023
GDN;IEG;10:55;13:00;13.02.2023
BZG;WAW;10:55;13:00;27.02.2023
BZG;RZE;10:55;13:00;28.02.2023
BZG;SZZ;10:55;13:00;28.02.2023
BZG;IEG;10:55;13:00;10.02.2023
BZG;WMI;10:55;13:00;27.02.2023
BZG;WRO;10:55;13:00;21.02.2023
BZG;POZ;10:55;13:00;20.02.2023
BZG;KRK;10:55;13:00;10.02.2023
BZG;KTW;10:55;13:00;01.02.2023
BZG;LCJ;10:55;13:00;07.02.2023
BZG;GDN;10:55;13:00;06.02.2023
RZE;WMI;10:55;13:00;18.02.2023
RZE;WRO;10:55;13:00;20.02.2023
RZE;POZ;10:55;13:00;27.02.2023
RZE;KRK;10:55;13:00;04.02.2023
RZE;KTW;10:55;13:00;01.02.2023
RZE;LCJ;10:55;13:00;15.02.2023
RZE;GDN;10:55;13:00;25.02.2023
RZE;BZG;10:55;13:00;27.02.2023
RZE;WAW;10:55;13:00;28.02.2023
RZE;SZZ;10:55;13:00;01.02.2023
RZE;IEG;10:55;13:00;12.02.2023
SZZ;WAW;10:55;13:00;13.02.2023
SZZ;WRO;10:55;13:00;14.02.2023
SZZ;POZ;10:55;13:00;07.02.2023
SZZ;KRK;10:55;13:00;06.02.2023
SZZ;KTW;10:55;13:00;21.02.2023
SZZ;LCJ;10:55;13:00;20.02.2023
SZZ;GDN;10:55;13:00;21.02.2023
SZZ;BZG;10:55;13:00;27.02.2023
SZZ;RZE;10:55;13:00;01.02.2023
SZZ;WMI;10:55;13:00;02.02.2023
SZZ;IEG;10:55;13:00;06.02.2023
IEG;WAW;10:55;13:00;10.02.2023
IEG;RZE;10:55;13:00;13.02.2023
IEG;SZZ;10:55;13:00;15.02.2023
IEG;WRO;10:55;13:00;19.02.2023
IEG;WAW;10:55;13:00;21.02.2023
IEG;WRO;10:55;13:00;23.02.2023
IEG;POZ;10:55;13:00;27.02.2023
IEG;KRK;10:55;13:00;28.02.2023
IEG;KTW;10:55;13:00;10.02.2023
IEG;LCJ;10:55;13:00;05.02.2023
IEG;GDN;10:55;13:00;03.02.2023
";

        var array = data.Split('\n').Select(x => x.Split(';')).ToArray();

        var airports = await context.Airports.ToArrayAsync();

        var flights = array.Where(x => x.Length == 5).Select(x => new Flight()
        {
            StartFromId = airports.Single(a => a.ShortName == x[0]).Id,
            ArriveAtId = airports.Single(a => a.ShortName == x[1]).Id,
            TakeOffTime = DateOnly.Parse(x[4]).ToDateTime(TimeOnly.Parse(x[2])),
            ArriveTime = DateOnly.Parse(x[4]).ToDateTime(TimeOnly.Parse(x[3])),
            State = FlightState.PENDING,
            SeatCount = (byte)(Random.Shared.Next(1, 4) * 60),
        });

        context.Flights.AddRange(flights);
        await context.SaveChangesAsync();
        new DebugLogger().Info("IMPORTED");
        Environment.Exit(0);
        return;
    }
    */
    #endregion
}