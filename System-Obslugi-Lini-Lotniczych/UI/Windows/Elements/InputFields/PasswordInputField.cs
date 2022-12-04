using System;
using LotSystem.UI.Windows.API;

namespace LotSystem.UI.Windows.Elements.InputFields;

public class PasswordInputField : InputField
{
    public PasswordInputField(GraphicsWindow window, string text) : base(window, text)
    {
    }

    public override void OnKeyPressed(ConsoleKeyInfo key)
    {
        if (!HandleBasicKeyLogic(key))
            return;

        Value += key.KeyChar;
        Console.Write('*');
    }
}