using LotSystem.Database.Models;
using LotSystem.Repositories.API;
using LotSystem.Services.Airport;
using LotSystem.UI.Windows.API;
using LotSystem.UI.Windows.Elements;
using LotSystem.UI.Windows.Elements.API;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LotSystem.UI.Windows;

public sealed class FlightListFilterWindow : ModalWindow, IModalSelectWindow<IEnumerable<Flight>>
{
    public override UserInterfaceElement[] UserInterfaceElements { get; }

    public override string Id => "flight_list_filter";

    public override string Title => "flight_list_filter";

    public override int Width => 30;

    public override int Height => 6;

    public IEnumerable<Flight> Value { get; private set; } = Array.Empty<Flight>();

    private readonly SelectField<AirportSelectionWindow, Airport> _fromSelector;
    private readonly SelectField<AirportSelectionWindow, Airport> _toSelector;

    private readonly IFlightRepository _flightRepository;

    public FlightListFilterWindow(IAirportService airportService, IFlightRepository flightRepository)
    {
        _fromSelector = new SelectField<AirportSelectionWindow, Airport>(this, "From");
        _toSelector = new SelectField<AirportSelectionWindow, Airport>(this, "To");

        UserInterfaceElements = new UserInterfaceElement[]
        {
            _fromSelector,
            _toSelector,
            new Button(this, "Apply", onUpdate),
        };

        _flightRepository = flightRepository;
    }

    private async void onUpdate()
    {
        var flights = await _flightRepository.GetFlights();

        if (_fromSelector.Value is not null)
            flights = flights.Where(x => x.StartFromId == _fromSelector.Value.Id);
        if (_toSelector.Value is not null)
            flights = flights.Where(x => x.ArriveAtId == _toSelector.Value.Id);

        Value = flights.ToArray();
        CloseThisWindow();
    }
}
