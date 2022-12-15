using LotSystem.Database.Models;
using LotSystem.UI.Windows.API;
using LotSystem.UI.Windows.Elements;
using LotSystem.UI.Windows.Elements.API;

namespace LotSystem.UI.Windows;

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
