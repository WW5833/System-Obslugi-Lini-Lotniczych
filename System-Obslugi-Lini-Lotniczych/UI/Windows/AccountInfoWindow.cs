using LotSystem.Services.UserManagement;
using LotSystem.UI.Windows.API;
using LotSystem.UI.Windows.Elements;
using LotSystem.UI.Windows.Elements.API;

namespace LotSystem.UI.Windows;

public sealed class AccountInfoWindow : FullScreenWindow
{
    public override UserInterfaceElement[] UserInterfaceElements { get; }
    

    private readonly IUserService _userService;

    private readonly Label _emailLabel;

    public AccountInfoWindow(IUserService userService)
    {
        _userService = userService;

        _emailLabel = new Label(this, "HI");

        UserInterfaceElements = new UserInterfaceElement[]
        {
            _emailLabel,
            new Button(this, "Logout", OnLogout),
            new Button(this, "Close", CloseThisWindow)
        };
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

        _emailLabel.Text = _userService
            .GetUser(UserInterfaceManager.Instance.CurrentSessionId.Value)
            .GetAwaiter()
            .GetResult()
            .Email;

        base.Open();
    }

    public override void Resume()
    {
        if (!UserInterfaceManager.Instance.CurrentSessionId.HasValue)
        {
            CloseThisWindow();
            return;
        }

        this.Open();
    }
}