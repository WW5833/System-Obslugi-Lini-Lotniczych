using System;
using LotSystem.UI.Windows.API;
using LotSystem.UI.Windows.Elements.API;

namespace LotSystem.UI.Windows.Elements;

public class Button : InteractableUserInterfaceElement
{
    public Button(GraphicsWindow window, string text, Action onClicked, int? forceX = null, int? forceY = null)
        : base(window, text)
    {
        OnClicked = onClicked;

        _forceX = forceX;
        _forceY = forceY;
    }

    private readonly int? _forceX;
    private readonly int? _forceY;

    public Action OnClicked { get; }

    public override void Draw()
    {
        if (_forceX.HasValue)
            Console.CursorLeft = _forceX.Value;
        if (_forceY.HasValue)
            Console.CursorTop = _forceY.Value;

        base.Draw();

        System.Console.ResetColor();
        Console.Write(Text);
    }

    public override void OnKeyPressed(ConsoleKeyInfo key)
    {
        if (key.Key == ConsoleKey.Enter)
        {
            OnClicked();
        }
    }
}

public class CheckBoxElement : InteractableUserInterfaceElement
{
    public CheckBoxElement(GraphicsWindow window, string text)
        : base(window, text)
    {
    }

    public bool Value { get; private set; }

    public override void Draw()
    {
        base.Draw();

        System.Console.ResetColor();
        Console.Write($"[{(Value ? "X": " ")}] {Text}");
    }

    public override void Redraw()
    {
        Console.CursorTop = YPos;
        Console.CursorLeft = XStartPos;
        Console.Write($"[{(Value ? "X": " ")}]");
        System.Console.ResetColor();
        Console.Write($" {Text}");
    }

    public override void OnKeyPressed(ConsoleKeyInfo key)
    {
        if (key.Key == ConsoleKey.Enter)
        {
            Value = !Value;
            Redraw();
        }
    }
}