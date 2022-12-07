using System;
using JetBrains.Annotations;
using LotSystem.UI.Windows.API;
using LotSystem.UI.Windows.Elements.API;

namespace LotSystem.UI.Windows.Elements.InputFields;

public class InputField : InteractableUserInterfaceElement
{
    public string Value { get; protected set; } = string.Empty;

    [CanBeNull] public Action<string> OnSubmit;

    public InputField(GraphicsWindow window, string text)
        : base(window, text)
    {
    }

    public override void Draw()
    {
        base.Draw();

        System.Console.ResetColor();
        Console.Write(Text + ": ");
    }

    public override void Redraw()
    {
        Console.CursorTop = YPos;
        Console.CursorLeft = XStartPos;
        Console.Write(Text);
        Console.CursorLeft += 2 + Value.Length;
    }

    public void Reset()
    {
        Value = string.Empty;
    }

    protected bool HandleBasicKeyLogic(ConsoleKeyInfo key)
    {
        // ReSharper disable once SwitchStatementHandlesSomeKnownEnumValuesWithDefault
        switch (key.Key)
        {
            case ConsoleKey.Enter:
                OnSubmit?.Invoke(Value);
                Window.MoveDownOneElement();
                return false;
            case ConsoleKey.Backspace when Value.Length == 0:
                return false;
            case ConsoleKey.Backspace:
                Value = Value.Remove(Value.Length - 1);
                Console.RemoveOneChar();
                return false;

            default:
                if (key.KeyChar == '\0')
                    return false;

                var windowWidth = (Window is ModalWindow modal) ? modal.Width : System.Console.WindowWidth;
                if (Value.Length == windowWidth - Text.Length - 2 - 2)
                    return false;
                return true;
        }
    }

    public override void OnKeyPressed(ConsoleKeyInfo key)
    {
        if (!HandleBasicKeyLogic(key))
            return;

        Value += key.KeyChar;
        Console.Write(key.KeyChar);
    }
}