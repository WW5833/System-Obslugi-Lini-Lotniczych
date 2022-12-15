using LotSystem.Services.UserManagement;
using LotSystem.UI.Windows.API;
using LotSystem.UI.Windows.Elements;
using LotSystem.UI.Windows.Elements.API;
using System.Collections.Generic;
using LotSystem.Repositories.API;

namespace LotSystem.UI.Windows;

public sealed class AccountInfoWindow : FullScreenWindow
{
    private readonly List<UserInterfaceElement> userInterfaceElements = new();
    public override UserInterfaceElement[] UserInterfaceElements => userInterfaceElements.ToArray();

    private readonly IUserService _userService;

    private readonly Label _emailLabel;
    private readonly Label _phoneLabel;
    private readonly Label _nameLabel;
    private readonly Label _ticketsLabel;

    private readonly ITicketRepository _ticketRepository;

    public AccountInfoWindow(IUserService userService, ITicketRepository ticketRepository)
    {
        _userService = userService;
        _ticketRepository = ticketRepository;

        _emailLabel = new Label(this, "Email: ");
        _phoneLabel = new Label(this, "Phone: ");
        _nameLabel = new Label(this, "Name & Last Name");
        _ticketsLabel = new Label(this, "Tickets: ");

        userInterfaceElements.Add(_emailLabel);
        userInterfaceElements.Add(_nameLabel);
        userInterfaceElements.Add(_phoneLabel);
        userInterfaceElements.Add(_ticketsLabel);
        userInterfaceElements.Add(new Button(this, "Logout", OnLogout));
        userInterfaceElements.Add(new Button(this, "Close", CloseThisWindow));
    }

    public async void OnLogout()
    {
        await _userService.LogoutUser(UserInterfaceManager.Instance.CurrentSessionId!.Value);
        UserInterfaceManager.Instance.CurrentSessionId = null;

        CloseThisWindow();
    }

    public override string Id => "account_info";

    public override string Title => "Account Info";

    public override void Open()
    {
        if (!UserInterfaceManager.Instance.CurrentSessionId.HasValue)
        {
            OpenWindow("require_user");
            return;
        }

        initialized = true;

        var user = _userService
            .GetUser(UserInterfaceManager.Instance.CurrentSessionId.Value)
            .GetAwaiter()
            .GetResult();

        _emailLabel.Text = "Email: " + user.Email;
        _phoneLabel.Text = "Phone: " + user.PhoneNumber;

        _nameLabel.Text = "Name: " + user.FirstName + " " + user.LastName;
        _ticketsLabel.Text = "Tickets: ";

        foreach (var ticket in user.Tickets)
        {
            userInterfaceElements.Insert(userInterfaceElements.Count - 2, new Button(this, $"\t{ticket.Flight.StartFrom.ShortName}->{ticket.Flight.ArriveAt.ShortName}, {ticket.Flight.TakeOffTime}, {ticket.Flight.State}, {ticket.State}", () =>
            {
                UserInterfaceManager.Instance.OpenWindow(new TicketDetailsWindow(_ticketRepository, ticket));
            }));
        }

        base.Open();
    }

    private bool initialized = false;
    public override void Resume()
    {
        if (!UserInterfaceManager.Instance.CurrentSessionId.HasValue)
        {
            CloseThisWindow();
            return;
        }

        if (initialized)
            base.Resume();
        else
            this.Open();
    }
}