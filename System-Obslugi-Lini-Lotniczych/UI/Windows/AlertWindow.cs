using LotSystem.UI.Windows.API;
using LotSystem.UI.Windows.Elements;
using LotSystem.UI.Windows.Elements.API;

namespace LotSystem.UI.Windows;

public class AlertWindow : ModalWindow
{
    public override string Id => "__alert";
    public override string Title => _title;
    private string _title;

    private int _width;
    private int _height;

    public static void Show(string title, string message, int width = 30, int height = 5)
    {
        var alert = new AlertWindow
        {
            _title = title,
            _width = width,
            _height = height,
            _uiElements = new UserInterfaceElement[]
            {
                null,
                null,
            }
        };

        alert._uiElements[0] = new Label(alert, message);

        alert._uiElements[1] = new Button(
            alert,
            "Close",
            UserInterfaceManager.Instance.CloseCurrentWindow,
            alert.StartLeft + alert.Width - 5,
            alert.StartTop + alert.Height);

        UserInterfaceManager.Instance.OpenWindow(alert);
    }

    public override UserInterfaceElement[] UserInterfaceElements => _uiElements;
    private UserInterfaceElement[] _uiElements;

    public override int Width => _width;
    public override int Height => _height;
}