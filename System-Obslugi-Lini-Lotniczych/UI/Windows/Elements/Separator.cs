using LotSystem.UI.Windows.API;
using LotSystem.UI.Windows.Elements.API;

namespace LotSystem.UI.Windows.Elements;

public class Separator : UserInterfaceElement
{
    public Separator(GraphicsWindow window)
        : base(window)
    {
    }

    public override void Draw()
    {
        System.Console.ResetColor();
        Console.WriteLine();
    }
}