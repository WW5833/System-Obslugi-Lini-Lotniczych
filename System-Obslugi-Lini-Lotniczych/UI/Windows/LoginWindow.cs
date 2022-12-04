using LotSystem.Services.UserManagement;
using LotSystem.UI.Windows.API;
using LotSystem.UI.Windows.Elements;
using LotSystem.UI.Windows.Elements.API;
using LotSystem.UI.Windows.Elements.InputFields;

namespace LotSystem.UI.Windows;

public sealed class LoginWindow : ModalWindow
{
    public override UserInterfaceElement[] UserInterfaceElements { get; }

    public override int Width => 50;
    public override int Height => 5;

    private readonly IUserService _userService;

    private readonly InputField _emailInputField;
    private readonly PasswordInputField _passwordInputField;

    public LoginWindow(IUserService userService)
    {
        _userService = userService;

        _emailInputField = new InputField(this, "Email");
        _passwordInputField = new PasswordInputField(this, "Password");

        UserInterfaceElements = new UserInterfaceElement[]
        {
            _emailInputField,
            _passwordInputField,
            new Separator(this),
            new Button(this, "Login", OnLogin)
        };
    }

    public override string Id => "login";

    public override string Title => "Login";

    private bool _loginSuccessful;
    private void OnLogin()
    {
        var email = _emailInputField.Value;
        var password = _passwordInputField.Value;
        if (string.IsNullOrWhiteSpace(email))
        {
            AlertWindow.Show("Login failed", "Email must not be empty", 30, 3);
            return;
        }

        var response = _userService.LoginUser(email, password).GetAwaiter().GetResult();

        if (!response.Successfull)
        {
            AlertWindow.Show("Login failed", response.Message, 30, 3);
            return;
        }

        UserInterfaceManager.Instance.CurrentSessionId = response.SessionId;
        AlertWindow.Show("Alert", "Logged in successfully", 30, 3);
        _loginSuccessful = true;
    }

    public override void Resume()
    {
        if (_loginSuccessful)
            UserInterfaceManager.Instance.CloseCurrentWindow();
        else
            base.Resume();
    }

    public override void Close()
    {
        _loginSuccessful = false;
        _emailInputField.Reset();
        _passwordInputField.Reset();
        base.Close();
    }
}
