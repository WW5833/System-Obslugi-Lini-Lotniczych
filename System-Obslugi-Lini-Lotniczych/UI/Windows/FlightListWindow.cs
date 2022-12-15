using LotSystem.Database.Models;
using LotSystem.Repositories.API;
using LotSystem.Services.Airport;
using LotSystem.UI.Windows.API;
using LotSystem.UI.Windows.Elements;
using LotSystem.UI.Windows.Elements.API;
using System.Collections.Generic;

namespace LotSystem.UI.Windows;

public sealed class FlightListWindow : FullScreenWindow
{
    private readonly IAirportService _airportService;

    public override UserInterfaceElement[] UserInterfaceElements => _uiElements.ToArray();
    private readonly List<UserInterfaceElement> _uiElements = new();

    public override string Id => "flight_list";

    public override string Title => "flight_list";

    private readonly IModalSelectWindow<IEnumerable<Flight>> _filterWindow;
    private readonly Label _results;
    public FlightListWindow(IAirportService airportService, IFlightRepository flightRepository)
    {
        _results = new Label(this, "Empty");
        _airportService = airportService;

        _filterWindow = new FlightListFilterWindow(airportService, flightRepository);

        _uiElements.Add(new Button(this, "Filter", () => UserInterfaceManager.Instance.OpenWindow(_filterWindow)));
        _uiElements.Add(new Separator(this));
    }

    public override void Resume()
    {
        base.Resume();

        _uiElements.RemoveRange(2, _uiElements.Count - 2);
        foreach (var flight in _filterWindow.Value)
        {
            _uiElements.Add(new Button(
                this,
                $"{flight.StartFrom.ShortName}->{flight.ArriveAt.ShortName} {flight.TakeOffTime.Date:yyyy-MM-dd}",
                () => UserInterfaceManager.Instance.OpenWindow(new FlightDetailsWindow(flight))
            ));
        }

        WriteElements();
    }
}
