using LotSystem.UI.Windows.API;

namespace LotSystem.UI.Windows.Elements;

public class CloseButton : Button
{
    public CloseButton(GraphicsWindow window, string text = "Close", int? forceX = null, int? forceY = null) :
        base(window, text, UserInterfaceManager.Instance.CloseCurrentWindow, forceX, forceY)
    {
    }
}