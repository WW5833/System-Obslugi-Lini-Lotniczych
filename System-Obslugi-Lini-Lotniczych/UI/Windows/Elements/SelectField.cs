using System;
using System.Linq;
using LotSystem.UI.Windows.API;
using LotSystem.UI.Windows.Elements.API;

namespace LotSystem.UI.Windows.Elements;

public class SelectField<T, TValue> : InteractableUserInterfaceElement where T : ModalWindow, IModalSelectWindow<TValue>
{
    private readonly T _window;
    public SelectField(GraphicsWindow window, string text)
        : base(window, text)
    {
        var types = UserInterfaceManager.Instance.InjectableTypes.ToDictionary(x => x.Key, x => x.Value);
        types.Add(typeof(SelectField<T, TValue>), this);
        types.Add(typeof(GraphicsWindow), window);
        _window = typeof(T).CreateInstance<T>(types);
    }

    public override void Draw()
    {
        base.Draw();

        System.Console.ResetColor();
        Console.Write(Text + ": ");
    }

    public override void Redraw()
    {
        base.Redraw();
        if (Value is not null)
            Console.Write(": " + Value.ToString());
    }

    public TValue Value { get; private set; }

    public override void OnKeyPressed(ConsoleKeyInfo key)
    {
        if (key.Key == ConsoleKey.Enter)
        {
            UserInterfaceManager.Instance.OpenWindow(_window);
        }
    }

    public override void OnWindowResumed()
    {
        if (_window.Value is not null)
        {
            Value = _window.Value;
            Redraw();
        }
        base.OnWindowResumed();
    }
}