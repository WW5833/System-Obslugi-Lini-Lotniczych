using LotSystem.Database.Models;
using LotSystem.Repositories.API;
using LotSystem.UI.Windows.API;
using LotSystem.UI.Windows.Elements;
using LotSystem.UI.Windows.Elements.API;

namespace LotSystem.UI.Windows;

public sealed class TicketDetailsWindow : ModalWindow
{
    public override UserInterfaceElement[] UserInterfaceElements { get; }

    public override string Id => "ticket_details";

    public override string Title => "Ticket Details";

    public override int Width => 40;

    public override int Height => 6;

    private readonly Button _seatLabel;

    private readonly ITicketRepository _ticketRepository;

    private readonly IModalSelectWindow<string> _seatSelectWindow;
    private readonly Ticket _ticket;
    public TicketDetailsWindow(ITicketRepository ticketRepository, Ticket ticket)
    {
        _ticketRepository = ticketRepository;
        _ticket = ticket;
        var flight = ticket.Flight;

        _seatSelectWindow = new TicketSeatEditor(ticket.Seat, ticket.Flight.SeatCount / 6);

        var fromToLabel = new Label(this, $"{flight.StartFrom.ShortName}->{flight.ArriveAt.ShortName}");
        var takeOffTimeLabel = new Label(this, $"Takes off at: {flight.TakeOffTime}");
        var classLabel = new Label(this, "Class: " + ((ticket.Flags & TicketFlags.ECONOMY_TICKET) != 0 ? "Economy" : "Business"));
        var additionalLuggageLabel = new Label(this, "Additional Luggage: " + ((ticket.Flags & TicketFlags.ADDITIONAL_LUGGAGE) != 0 ? "Yes" : "No"));
        _seatLabel = new Button(
            this,
            $"Seat: {ticket.Seat}",
            () =>
            {
                UserInterfaceManager.Instance.OpenWindow(_seatSelectWindow);
            });
        var stateLabel = new Label(this, $"Status: {ticket.State}");

        UserInterfaceElements = new UserInterfaceElement[]
        {
            fromToLabel,
            takeOffTimeLabel,
            classLabel,
            additionalLuggageLabel,
            _seatLabel,
            stateLabel,

            new ModalCloseButton(this),
        };
    }

    public override void Resume()
    {
        _seatLabel.Text = _seatSelectWindow.Value;
        _seatLabel.Redraw();
        _ticketRepository.UpdateSeat(_ticket, _seatSelectWindow.Value);
        base.Resume();
    }
}