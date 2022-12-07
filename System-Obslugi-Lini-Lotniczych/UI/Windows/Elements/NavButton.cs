using LotSystem.UI.Windows.API;

namespace LotSystem.UI.Windows.Elements;

public class NavButton : Button
{
    public NavButton(GraphicsWindow window, string text, string targetWindow) :
        base(window, text, () => UserInterfaceManager.Instance.OpenWindow(targetWindow))
    {
    }
}