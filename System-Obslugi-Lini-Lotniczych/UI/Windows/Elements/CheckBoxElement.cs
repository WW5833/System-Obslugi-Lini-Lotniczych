using System;
using LotSystem.UI.Windows.API;
using LotSystem.UI.Windows.Elements.API;

namespace LotSystem.UI.Windows.Elements;

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