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

    public FlightDetailsWindow(Flight flight)
    {
        var fromLabel = new Label(this, $"From: {flight.StartFrom}");
        var toLabel = new Label(this, $"To: {flight.ArriveAt}");
        var startAtLabel = new Label(this, $"Takes off at: {flight.TakeOffTime}");
        var endAtLabel = new Label(this, $"Lands at: {flight.ArriveTime}");
        var stateLabel = new Label(this, $"Status: {flight.State}");

        BuyTicketWindow.Flight = flight;

        UserInterfaceElements = new UserInterfaceElement[]
        {
            fromLabel,
            toLabel,
            startAtLabel,
            endAtLabel,
            stateLabel,

            new NavButton(this, "Buy Ticket", "buy_ticket"),
            new ModalCloseButton(this),
        };
    }
}
