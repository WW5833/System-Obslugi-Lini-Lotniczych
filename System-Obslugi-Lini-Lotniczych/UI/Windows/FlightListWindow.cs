using LotSystem.Database.Models;
using LotSystem.Logger;
using LotSystem.Repositories;
using LotSystem.Services.Airport;
using LotSystem.Services.UserManagement;
using LotSystem.UI.Windows.API;
using LotSystem.UI.Windows.Elements;
using LotSystem.UI.Windows.Elements.API;
using System;
using System.Collections.Generic;
using System.Linq;

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
    public FlightListWindow(IAirportService airportService)
    {
        _results = new Label(this, "Empty");
        _airportService = airportService;

        _filterWindow = new FlightListFilterWindow(airportService);

        _uiElements.Add(new Button(this, "Filter", () => UserInterfaceManager.Instance.OpenWindow(_filterWindow)));
        _uiElements.Add(new Separator(this));
    }

    public override void Resume()
    {
        base.Resume();
        /*_results.Text = string.Join("\n", _filterWindow.Value.Select(x => $"{x.StartFrom.ShortName}->{x.ArriveAt.ShortName} Id: {x.Id}"));
        _results.Redraw();*/

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


public sealed class FlightListFilterWindow : ModalWindow, IModalSelectWindow<IEnumerable<Flight>>
{
    private readonly IAirportService _airportService;

    public override UserInterfaceElement[] UserInterfaceElements { get; }

    public override string Id => "flight_list_filter";

    public override string Title => "flight_list_filter";

    public override int Width => 30;

    public override int Height => 6;

    public IEnumerable<Flight> Value { get; private set; }

    private readonly SelectField<AirportSelectionWindow, Airport> _fromSelector;
    private readonly SelectField<AirportSelectionWindow, Airport> _toSelector;

    public FlightListFilterWindow(IAirportService airportService)
    {
        _fromSelector = new SelectField<AirportSelectionWindow, Airport>(this, "From");
        _toSelector = new SelectField<AirportSelectionWindow, Airport>(this, "To");
        _airportService = airportService;

        UserInterfaceElements = new UserInterfaceElement[]
        {
            _fromSelector,
            _toSelector,
            new Button(this, "Apply", onUpdate),
        };
    }

    private async void onUpdate()
    {
        // Todo: Dummy code
        var repo = new Repository(new DebugLogger());
        var flights = await repo.GetFlights();

        if (_fromSelector.Value is not null)
            flights = flights.Where(x => x.StartFromId == _fromSelector.Value.Id);
        if (_toSelector.Value is not null)
            flights = flights.Where(x => x.ArriveAtId == _toSelector.Value.Id);

        Value = flights.ToArray();
        CloseThisWindow();
    }
}

public sealed class FlightDetailsWindow : ModalWindow
{
    public override UserInterfaceElement[] UserInterfaceElements { get; }

    public override string Id => "flight_details";

    public override string Title => "Flight Details";

    public override int Width => 40;

    public override int Height => 6;

    private readonly Label _fromLabel;
    private readonly Label _toLabel;
    private readonly Label _startAtLabel;
    private readonly Label _endAtLabel;
    private readonly Label _stateLabel;

    public FlightDetailsWindow(Flight flight)
    {
        _fromLabel = new Label(this, $"From: {flight.StartFrom}");
        _toLabel = new Label(this, $"To: {flight.ArriveAt}");
        _startAtLabel = new Label(this, $"Takes off at: {flight.TakeOffTime}");
        _endAtLabel = new Label(this, $"Lands at: {flight.ArriveTime}");
        _stateLabel = new Label(this, $"Status: {flight.State}");

        BuyTicketWindow.Flight = flight;

        UserInterfaceElements = new UserInterfaceElement[]
        {
            _fromLabel,
            _toLabel,
            _startAtLabel,
            _endAtLabel,
            _stateLabel,

            new NavButton(this, "Buy Ticket", "buy_ticket"),
            new ModalCloseButton(this),
        };
    }
}

public sealed class BuyTicketWindow : ModalWindow
{
    private readonly IAirportService _airportService;

    public override UserInterfaceElement[] UserInterfaceElements { get; }

    public override string Id => "buy_ticket";

    public override string Title => "Buy a Ticket!, DO IT NOW!";

    public override int Width => 50;

    public override int Height => 10;

    public static Flight Flight { get; set; }

    private readonly CheckBoxElement _additionalLuggageCheckBox;
    private readonly CheckBoxElement _buisnessCLassCheckBox;

    private readonly IUserService _userService;
    public BuyTicketWindow(IAirportService airportService, IUserService userService)
    {
        _airportService = airportService;
        _userService = userService;
        _additionalLuggageCheckBox = new CheckBoxElement(this, "Additional Luggage?");
        _buisnessCLassCheckBox = new CheckBoxElement(this, "Buisness Class?");

        UserInterfaceElements = new UserInterfaceElement[]
        {
            // new SelectField<SeatSelectionWindow, string>(this, "Seat"),
            new Label(this, "Seat: Auto Assigned"),
            _additionalLuggageCheckBox,
            _buisnessCLassCheckBox,

            new Separator(this),
            new Button(this, "Buy", onBuy),
            new ModalCloseButton(this),
        };
    }

    private bool closeOnResume = false;
    private async void onBuy()
    {
        // Todo: Dummy code
        var repo = new Repository(new DebugLogger());
        await repo.AddTicket(new Ticket
        {
            BoughtTime = DateTime.Now,
            Flags = (TicketFlags)((byte)(_additionalLuggageCheckBox.Value ? TicketFlags.ADDITIONAL_LUGGAGE : TicketFlags.NONE) + (byte)(_buisnessCLassCheckBox.Value ? TicketFlags.BUSINESS_TICKET : TicketFlags.ECONOMY_TICKET)),
            FlightId = Flight.Id,
            UserId = (await _userService.GetUser(UserInterfaceManager.Instance.CurrentSessionId.Value)).Id,
            Seat = $"{Random.Shared.Next(0, Flight.SeatCount)}",
            State = TicketState.AWAITING_PAYMENT,
        });

        AlertWindow.Show("Ticket reserved", "Now you have to pay for it!");
        closeOnResume = true;
    }

    public override void Resume()
    {
        // base.Resume();
    
        if(closeOnResume)
            CloseThisWindow();
        else
        {
            if (!UserInterfaceManager.Instance.CurrentSessionId.HasValue)
            {
                CloseThisWindow();
                return;
            }

            this.Open();
        }
    }

    public override void Open()
    {
        if (!UserInterfaceManager.Instance.CurrentSessionId.HasValue)
        {
            OpenWindow("require_user");
            return;
        }

        base.Open();
    }
}

public class SeatSelectionWindow : ModalWindow, IModalSelectWindow<string>
{
    public override int Width => 50;

    public override int Height => 30;

    public override UserInterfaceElement[] UserInterfaceElements => _userInterfaceElements;
    private UserInterfaceElement[] _userInterfaceElements;

    public override string Id => "seat-select-window";

    public override string Title => "Select seat";

    public string Value { get; private set; }

    public override void Open()
    {
        var flight = BuyTicketWindow.Flight;
        var elements = new List<UserInterfaceElement>();

        if (flight.SeatCount % 9 == 0)
            WriteWith3Rows(flight, elements);
        else if (flight.SeatCount % 6 == 0)
            WriteWith2Rows(flight, elements);
        else
            throw new Exception("Invalid seat count");

        _userInterfaceElements = elements.ToArray();

        base.Open();
    }

    private void WriteWith2Rows(Flight flight, List<UserInterfaceElement> list)
    {
        int rows = flight.SeatCount / 6;
        var initialLablel = new Label(this, "∙–––––––∙");
        list.Add(initialLablel);
        for (int i = 0; i < rows; i++)
        {
            list.Add(new Label(this, "|       |"));
        }

        list.Add(new Label(this, "∙–––––––∙"));

        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < 6; j++)
            {
                list.Add(new Button(this, "X", () => { }, StartLeft + 3 + j + (j > 2 ? 1 : 0), StartTop + i + 2));
            }
        }
    }

    private void WriteWith3Rows(Flight flight, List<UserInterfaceElement> list)
    {
        int rows = flight.SeatCount / 9;
        list.Add(new Label(this, "∙–––––––––––∙"));
        for (int i = 0; i < rows; i++)
        {
            var label = new Label(this, "|           |");
            list.Add(label);

            for (int j = 0; j < 9; j++)
            {
                list.Add(new Button(this, "X", () => { }, label.XStartPos + 1 + j, label.YPos));
            }
        }

        list.Add(new Label(this, "∙–––––––––––∙"));
    }
}