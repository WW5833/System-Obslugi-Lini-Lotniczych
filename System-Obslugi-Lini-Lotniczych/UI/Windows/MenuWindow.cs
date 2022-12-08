using System;
using LotSystem.UI.Windows.API;
using LotSystem.UI.Windows.Elements;
using LotSystem.UI.Windows.Elements.API;

namespace LotSystem.UI.Windows;

public sealed class MenuWindow : FullScreenWindow
{
    public override UserInterfaceElement[] UserInterfaceElements { get; }

    public MenuWindow()
    {
        UserInterfaceElements = new UserInterfaceElement[] {
            new Button(this, "Account", () => OpenWindow("account_info")),
            new Button(this, "Select Airport", () => OpenWindow("airport_selection")),
            new Separator(this),
            new Button(this, "Exit", () => Environment.Exit(0)),
        };
    }
    
    public override string Id => "default";
    public override string Title => "Menu";

    public override void Resume()
    {
        base.Resume();
        WriteTitle();
    }
}