using LotSystem.UI.Windows.API;

namespace LotSystem.UI.Windows.Elements;

public class ModalCloseButton : CloseButton
{
    public ModalCloseButton(ModalWindow window, string text = "Close") :
        base(window, text, window.StartLeft + window.Width - text.Length, window.StartTop + window.Height)
    {
    }
}