using System.Threading.Tasks;
using LotSystem.Services.UserManagement;
using LotSystem.UI.Windows.API;

namespace LotSystem.UI.Windows;

public sealed class RegisterWindow : Window
{
    private readonly IUserService _userService;

    public RegisterWindow(IUserService userService)
    {
        _userService = userService;
    }

    public override string Id => "register";

    public override string Title => "Register";

    public override async Task Update()
    {
        var email = this.Prompt("Email", (x) => !string.IsNullOrWhiteSpace(x));
        var firstName = this.Prompt("First Name", (x) => !string.IsNullOrWhiteSpace(x));
        var lastName = this.Prompt("Last Name", (x) => !string.IsNullOrWhiteSpace(x));
        var phone = this.PromptPhoneNumber("Phone Number");
        var password = this.PromptPassword("Password");
        var repeatedPassword = this.PromptPassword("Repeat Password");

        if (password != repeatedPassword)
        {
            Console.WriteLine("Password and Repeated Password doesn't match, Press any key to continue ...");
            Console.ReadLine();
            return;
        }

        var response = await _userService.RegisterUser(email, password, firstName, lastName, phone);

        if (!response.Successfull)
        {
            Console.WriteLine($"Error: {response.Message}");
            Console.WriteLine("Press any key to continue ...");
            Console.ReadLine();
            return;
        }

        Console.WriteLine("Registered successfully, Press any key to continue ...");
        Console.ReadLine();
        OpenWindow("Login");
    }
}