using System;
using LotSystem.UI.Windows.API;

namespace LotSystem.UI.Windows.Elements.InputFields;

public class PhoneNumberInputField : InputField
{
    public PhoneNumberInputField(GraphicsWindow window, string text) : base(window, text)
    {
    }

    public override void OnKeyPressed(ConsoleKeyInfo key)
    {
        switch (key.Key)
        {
            case ConsoleKey.Enter:
                if (Value.Length != 9)
                    return;

                OnSubmit?.Invoke(Value);
                Window.MoveDownOneElement();
                return;
            case ConsoleKey.Backspace when Value.Length == 0:
                return;
            case ConsoleKey.Backspace:
                if (Value.Length is > 0 and < 9 && Value.Length % 3 == 0)
                    Console.RemoveOneChar();
                Value = Value.Remove(Value.Length - 1);
                Console.RemoveOneChar();
                return;

            case ConsoleKey.D0:
            case ConsoleKey.NumPad0:
            case ConsoleKey.D1:
            case ConsoleKey.NumPad1:
            case ConsoleKey.D2:
            case ConsoleKey.NumPad2:
            case ConsoleKey.D3:
            case ConsoleKey.NumPad3:
            case ConsoleKey.D4:
            case ConsoleKey.NumPad4:
            case ConsoleKey.D5:
            case ConsoleKey.NumPad5:
            case ConsoleKey.D6:
            case ConsoleKey.NumPad6:
            case ConsoleKey.D7:
            case ConsoleKey.NumPad7:
            case ConsoleKey.D8:
            case ConsoleKey.NumPad8:
            case ConsoleKey.D9:
            case ConsoleKey.NumPad9:
                break;

            default:
                return;
        }

        if (Value.Length == 9)
            return;

        Value += key.KeyChar;
        Console.Write(key.KeyChar);
        if (Value.Length is > 0 and < 9 && Value.Length % 3 == 0)
            Console.Write("-");
    }

    public override void Redraw()
    {
        base.Redraw();

        switch (Value.Length)
        {
            case >= 6:
                Console.CursorLeft++;
                Console.CursorLeft++;
                break;

            case >= 3:
                Console.CursorLeft++;
                break;
        }
    }
}