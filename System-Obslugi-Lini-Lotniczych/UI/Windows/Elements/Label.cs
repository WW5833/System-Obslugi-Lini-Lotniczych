using LotSystem.UI.Windows.API;
using LotSystem.UI.Windows.Elements.API;

namespace LotSystem.UI.Windows.Elements;

public class Label : UserInterfaceElement
{
    public string Text { get; set; }

    public Label(GraphicsWindow window, string text)
        : base(window)
    {
        Text = text;
    }

    public override void Draw()
    {
        _yPos = Console.CursorTop;
        _xStartPos = Console.CursorLeft;
        Console.Write(Text);
    }

    private int _yPos;
    private int _xStartPos;

    public void Redraw()
    {
        Console.CursorTop = _yPos;
        Console.CursorLeft = _xStartPos;
        Console.Write(Text);
    }
}