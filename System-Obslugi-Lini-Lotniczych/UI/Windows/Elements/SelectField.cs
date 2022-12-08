using System;
using LotSystem.UI.Windows.API;
using LotSystem.UI.Windows.Elements.API;

namespace LotSystem.UI.Windows.Elements;

public class SelectField<T> : InteractableUserInterfaceElement where T : ModalWindow
{
    public string Value;
    public SelectField(GraphicsWindow window, string text)
        : base(window, text)
    {
    }

    public Action OnClicked { get; }

    public override void Draw()
    {
        base.Draw();

        System.Console.ResetColor();
        Console.Write(Text);
    }

    public override void OnKeyPressed(ConsoleKeyInfo key)
    {
        if (key.Key == ConsoleKey.Enter)
        {
            var window = typeof(T).CreateInstance<T>(new System.Collections.Generic.Dictionary<Type, object>() { { typeof(SelectField<T>), this } });
            UserInterfaceManager.Instance.OpenWindow(window);
        }
    }
}