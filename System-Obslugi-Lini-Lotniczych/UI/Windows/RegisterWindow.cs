using System;
using LotSystem.Services.UserManagement;
using LotSystem.UI.Windows.API;
using LotSystem.UI.Windows.Elements;
using LotSystem.UI.Windows.Elements.API;
using LotSystem.UI.Windows.Elements.InputFields;

namespace LotSystem.UI.Windows;

public sealed class RegisterWindow : ModalWindow
{
    public override UserInterfaceElement[] UserInterfaceElements { get; }

    public override int Width => 50;
    public override int Height => 10;

    private readonly IUserService _userService;

    private readonly InputField _emailInputField;
    private readonly InputField _firstNameInputField;
    private readonly InputField _lastNameInputField;
    private readonly PhoneNumberInputField _phoneInputField;
    private readonly PasswordInputField _passwordInputField;
    private readonly PasswordInputField _repeatedPasswordInputField;

    private readonly Label _errorLabel;

    public RegisterWindow(IUserService userService)
    {
        _userService = userService;

        _emailInputField = new InputField(this, "Email");
        _firstNameInputField = new InputField(this, "First name");
        _lastNameInputField = new InputField(this, "Last name");
        _phoneInputField = new PhoneNumberInputField(this, "Phone number");
        _passwordInputField = new PasswordInputField(this, "Password");
        _repeatedPasswordInputField = new PasswordInputField(this, "Repeat Password");
        _errorLabel = new Label(this, string.Empty);

        UserInterfaceElements = new UserInterfaceElement[]
        {
            _emailInputField,
            _firstNameInputField,
            _lastNameInputField,
            _phoneInputField,
            _passwordInputField,
            _repeatedPasswordInputField,
            new Separator(this),
            _errorLabel,
            new Separator(this),
            new Button(this, "Register", OnRegister)
        };
    }

    private bool _registerSuccessful;
    private void OnRegister()
    {
        var email = _emailInputField.Value;
        var firstName = _firstNameInputField.Value;
        var lastName = _lastNameInputField.Value;
        var phone = _phoneInputField.Value; // Todo: this.PromptPhoneNumber("Phone Number");
        var password = _passwordInputField.Value;
        var repeatedPassword = _repeatedPasswordInputField.Value;

        _errorLabel.Text = string.Empty;

        if (string.IsNullOrWhiteSpace(email))
        {
            System.Console.ForegroundColor = ConsoleColor.DarkRed;
            _emailInputField.Redraw();
            System.Console.ResetColor();

            _errorLabel.Text = "Email must not be empty";
            _errorLabel.Redraw();
            return;
        }

        if (string.IsNullOrWhiteSpace(firstName))
        {
            System.Console.ForegroundColor = ConsoleColor.DarkRed;
            _firstNameInputField.Redraw();
            System.Console.ResetColor();

            _errorLabel.Text = "First name must not be empty";
            _errorLabel.Redraw();
            return;
        }

        if (string.IsNullOrWhiteSpace(lastName))
        {
            System.Console.ForegroundColor = ConsoleColor.DarkRed;
            _lastNameInputField.Redraw();
            System.Console.ResetColor();

            _errorLabel.Text = "Last name must not be empty";
            _errorLabel.Redraw();
            return;
        }

        if (phone?.Length != 9)
        {
            System.Console.ForegroundColor = ConsoleColor.DarkRed;
            _phoneInputField.Redraw();
            System.Console.ResetColor();

            _errorLabel.Text = "Phone number has to be exactly 9 digits";
            _errorLabel.Redraw();
            return;
        }

        if (password != repeatedPassword)
        {
            System.Console.ForegroundColor = ConsoleColor.DarkRed;
            _repeatedPasswordInputField.Redraw();
            System.Console.ResetColor();

            _errorLabel.Text = "Password and Repeated Password doesn't match";
            _errorLabel.Redraw();
            return;
        }

        var response = _userService.RegisterUser(email, password, firstName, lastName, phone).GetAwaiter().GetResult();

        if (!response.Successfull)
        {
            AlertWindow.Show("Register failed", response.Message, 30, 3);
            _errorLabel.Redraw();
            return;
        }

        var loginResponse = _userService.LoginUser(email, password).GetAwaiter().GetResult();

        if (!loginResponse.Successfull)
            throw new Exception($"Login shouldn't fail just after register, message: {loginResponse.Message}");

        _registerSuccessful = true;
        UserInterfaceManager.Instance.CurrentSessionId = loginResponse.SessionId;
        _errorLabel.Redraw();
        AlertWindow.Show("Registered successfully", "You have registered and logged in.", 40, 3);
    }

    public override string Id => "register";

    public override string Title => "Register";

    public override void Resume()
    {
        if(_registerSuccessful)
            UserInterfaceManager.Instance.CloseCurrentWindow();
        else
            base.Resume();
    }
}