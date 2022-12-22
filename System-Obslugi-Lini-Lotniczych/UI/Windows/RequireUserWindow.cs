using LotSystem.UI.Windows.API;
using LotSystem.UI.Windows.Elements;
using LotSystem.UI.Windows.Elements.API;

namespace LotSystem.UI.Windows;

public sealed class RequireUserWindow : FullScreenWindow
{
    public override UserInterfaceElement[] UserInterfaceElements { get; }

    public RequireUserWindow()
    {
        UserInterfaceElements = new UserInterfaceElement[]
        {
            new Label(this, "You need to login to continue!"),
            new Separator(this),
            new Button(this, "Login", () => OpenWindow("login")),
            new Button(this, "Register", () => OpenWindow("register")),
            new Separator(this),
            new Button(this, "Cancel", CloseThisWindow)
        };
    }

    public override string Id => "require_user";

    public override string Title => "You need to login";

    public override void Resume()
    {
        CloseThisWindow();
    }
}