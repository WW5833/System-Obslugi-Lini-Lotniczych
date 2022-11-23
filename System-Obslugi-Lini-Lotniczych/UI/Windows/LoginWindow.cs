using System;
using System.Threading.Tasks;
using LotSystem.Services.UserManagement;

namespace LotSystem.UI.Windows;

public sealed class LoginWindow : Window
{
    private readonly IUserService _userService;

    public LoginWindow(IUserService userService)
    {
        _userService = userService;
    }

    public override string Id => "login";

    public override string Title => "Login";

    public override async Task Update()
    {
        var email = this.Prompt("Email", (x) => !string.IsNullOrWhiteSpace(x));
        var password = this.PromptPassword("password");

        Console.WriteLine($"{email}: {password}");
        var response = await _userService.LoginUser(email, password);

        if (response.Successfull)
        {
            UIManager.Instance.CurrentSessionId = response.SessionId;
            OpenWindow("default");
            return;
        }
        else
        {
            Console.WriteLine($"Error: {response.Message}");
        }
    }
}