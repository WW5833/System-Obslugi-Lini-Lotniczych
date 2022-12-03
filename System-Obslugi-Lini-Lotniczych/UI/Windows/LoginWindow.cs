using System.Threading.Tasks;
using LotSystem.Services.UserManagement;
using LotSystem.UI.Windows.API;

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

        var response = await _userService.LoginUser(email, password);

        if (!response.Successfull)
        {
            Console.WriteLine($"Error: {response.Message}");
            Console.WriteLine("Press any key to continue ...");
            Console.ReadLine();
            return;
        }

        UserInterfaceManager.Instance.CurrentSessionId = response.SessionId;

        Console.WriteLine("Logged in successfully, Press any key to continue ...");
        Console.ReadLine();

        OpenWindow("default");

    }
}
