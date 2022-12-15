using System;
using LotSystem.UI.Windows.API;

namespace LotSystem.UI.Windows.Elements.InputFields;

public class SeatInputField : InputField
{
    public int RowCount { get; set; }
    public bool IsValid { get; private set; }

    public SeatInputField(GraphicsWindow window, string text, int rowCount)
        : base(window, text)
    {
        RowCount = rowCount;
    }

    public SeatInputField(GraphicsWindow window, string text, int rowCount, string value)
        : base(window, text, value)
    {
        RowCount = rowCount;
    }

    public override void OnUnselected()
    {
        base.OnUnselected();

        if (Value.Length != 3)
        {
            IsValid = false;
            System.Console.ForegroundColor = ConsoleColor.Red;
            Redraw();
            System.Console.ResetColor();
        }
        else
        {
            var seat = int.Parse(Value[1..]);
            IsValid = !(seat == 0 || seat > RowCount);

            if (IsValid)
            {
                System.Console.ResetColor();
                Redraw();
            }
            else
            {
                System.Console.ForegroundColor = ConsoleColor.Red;
                Redraw();
                System.Console.ResetColor();
            }
        }
    }

    public override void OnKeyPressed(ConsoleKeyInfo key)
    {
        bool letter = false;
        switch (key.Key)
        {
            case ConsoleKey.Enter:
                if (Value.Length < 3)
                    return;

                
                var seat = int.Parse(Value[1..]);

                if (seat == 0 || seat > RowCount)
                {
                    System.Console.ForegroundColor = ConsoleColor.Red;
                    Redraw();
                    System.Console.ResetColor();
                    return;
                }
                

                OnSubmit?.Invoke(Value);
                IsValid = true;
                Window.MoveDownOneElement();
                return;
            case ConsoleKey.Backspace when Value.Length == 0:
                return;
            case ConsoleKey.Backspace:
                Value = Value.Remove(Value.Length - 1);
                Console.RemoveOneChar();
                return;

            case ConsoleKey.A:
            case ConsoleKey.B:
            case ConsoleKey.C:
            case ConsoleKey.D:
            case ConsoleKey.E:
            case ConsoleKey.F:
                letter = true;
                break;

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

        if (Value.Length == 3)
            return;

        if (Value.Length > 0)
        {
            if (letter)
                return;
        }
        else if (!letter)
            return;

        Value += key.KeyChar.ToString().ToUpper();
        Console.Write(key.KeyChar.ToString().ToUpper());
    }
}