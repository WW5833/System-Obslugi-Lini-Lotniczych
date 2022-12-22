using LotSystem.Database.Models;
using LotSystem.Repositories.API;
using LotSystem.Services.Airport;
using LotSystem.Services.UserManagement;
using LotSystem.UI.Windows.API;
using LotSystem.UI.Windows.Elements;
using LotSystem.UI.Windows.Elements.API;
using LotSystem.UI.Windows.Elements.InputFields;
using System;
using JetBrains.Annotations;

namespace LotSystem.UI.Windows;

[UsedImplicitly]
public sealed class BuyTicketWindow : ModalWindow
{
    public override UserInterfaceElement[] UserInterfaceElements { get; }

    public override string Id => "buy_ticket";

    public override string Title => "Buy a Ticket!, DO IT NOW!";

    public override int Width => 50;

    public override int Height => 10;

    public static Flight Flight { get; set; }

    private readonly CheckBoxElement _additionalLuggageCheckBox;
    private readonly CheckBoxElement _businessCLassCheckBox;
    private readonly SeatInputField _seatInputField;

    private readonly IUserService _userService;
    private readonly ITicketRepository _ticketRepository;

    public BuyTicketWindow(IAirportService airportService, IUserService userService, ITicketRepository ticketRepository)
    {
        _userService = userService;
        _additionalLuggageCheckBox = new CheckBoxElement(this, "Additional Luggage?");
        _businessCLassCheckBox = new CheckBoxElement(this, "Business Class?");

        _seatInputField = new SeatInputField(this, "Seat", 30);

        UserInterfaceElements = new UserInterfaceElement[]
        {
            _seatInputField,
            // new Label(this, "Seat: Auto Assigned"),
            _additionalLuggageCheckBox,
            _businessCLassCheckBox,

            new Separator(this),
            new Button(this, "Buy", OnBuy),
            new ModalCloseButton(this),
        };

        _ticketRepository = ticketRepository;
    }

    private bool _closeOnResume;

    private async void OnBuy()
    {
        if (!_seatInputField.IsValid)
            return;

        await _ticketRepository.AddTicket(
            new Ticket
            {
                BoughtTime = DateTime.Now,
                Flags =
                    (TicketFlags)((byte)(_additionalLuggageCheckBox.Value
                        ? TicketFlags.ADDITIONAL_LUGGAGE
                        : TicketFlags.NONE) + (byte)(_businessCLassCheckBox.Value
                        ? TicketFlags.BUSINESS_TICKET
                        : TicketFlags.ECONOMY_TICKET)),
                FlightId = Flight.Id,
                UserId = (await _userService.GetUser(UserInterfaceManager.Instance.CurrentSessionId!.Value)).Id,
                Seat = _seatInputField.Value,
                State = TicketState.PAYED,
            });

        AlertWindow.Show("Ticket reserved", "Ticket has been reserved");
        _closeOnResume = true;
    }

    public override void Resume()
    {
        // base.Resume();

        if (_closeOnResume)
        {
            CloseThisWindow();
            CloseThisWindow();
            CloseThisWindow();
        }
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

        _seatInputField.RowCount = Flight.SeatCount / 3;

        base.Open();
    }
}
