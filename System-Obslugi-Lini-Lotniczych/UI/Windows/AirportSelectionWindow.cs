using System.Threading.Tasks;
using LotSystem.UI.Windows.API;

namespace LotSystem.UI.Windows;

public sealed class AirportSelectionWindow : Window
{
    public override string Id => "airport_selection";

    public override string Title => "airport_selection";

    public override async Task Update()
    {
        /*var email = this.Prompt("Email", (x) => !string.IsNullOrWhiteSpace(x));
        var password = this.PromptPassword("password");

        var response = await _userService.LoginUser(email, password);

        if (response.Successfull)
        {
            UIManager.Instance.CurrentSessionId = response.SessionId;

            Console.WriteLine("Logged in successfully, Press any key to continue ...");
            Console.ReadLine();

            OpenWindow("default");
            return;
        }
        else
        {
            Console.WriteLine($"Error: {response.Message}");
            Console.WriteLine("Press any key to continue ...");
            Console.ReadLine();
        }*/
    }
}
