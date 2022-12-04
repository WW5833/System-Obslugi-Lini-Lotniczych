using LotSystem.UI.Windows.API;

namespace LotSystem.UI.Windows.Elements.API;

public abstract class UserInterfaceElement : IUserInterfaceElement
{
    public WindowUInterfaceManager Console => Window.Console;
    public abstract void Draw();

    protected readonly GraphicsWindow Window;

    public UserInterfaceElement(GraphicsWindow window)
    {
        Window = window;
    }
}