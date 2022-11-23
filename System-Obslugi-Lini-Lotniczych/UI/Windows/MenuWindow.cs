using System;
using System.Threading.Tasks;
using LotSystem.Services.UserManagement;

namespace LotSystem.UI.Windows
{
    public sealed class MenuWindow : Window
    {
        private readonly IUserService _userService;

        public MenuWindow(IUserService userService)
        {
            _userService = userService;
        }

        public override string Id => "default";
        public override string Title => "Menu";

        public void Login()
        {
            OpenWindow("login");
        }

        public async Task Logout()
        {
            await _userService.LogoutUser(UIManager.Instance.CurrentSessionId!.Value);
            UIManager.Instance.CurrentSessionId = null;
            Open();
        }

        public void Register()
        {
            OpenWindow("register");
        }

        public override async Task Update()
        {
            if (UIManager.Instance.CurrentSessionId.HasValue)
                Console.WriteLine("[1] - Logout");
            else
                Console.WriteLine("[1] - Login");
            Console.WriteLine("[2] - Register");
            Console.WriteLine();
            Console.WriteLine("[0] - Exit");
            var option = this.PromptSingleNumber("Select option", (x) => x is 1 or 2 or 0);

            switch (option)
            {
                case 1:
                    if (UIManager.Instance.CurrentSessionId.HasValue)
                        await Logout();
                    else
                        Login();
                    return;

                case 2:
                    Register();
                    return;

                case 0:
                    Console.WriteLine("Bye :)");
                    Environment.Exit(0);
                    return;

                default:
                    throw new Exception("Invalid option, this should had been prevented by prompt validator");
            }
        }
    }
}