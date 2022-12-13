using System;
using System.Linq;
using JetBrains.Annotations;
using LotSystem.UI.Windows.Elements.API;

namespace LotSystem.UI.Windows.API;

[PublicAPI]
public abstract class GraphicsWindow : Window, IGWindow
{
    public abstract UserInterfaceElement[] UserInterfaceElements { get; }

    public InteractableUserInterfaceElement[] InteractableUserInterfaceElements => UserInterfaceElements.OfType<InteractableUserInterfaceElement>().ToArray();

    // ReSharper disable once ConvertToAutoProperty
    int IGWindow.CurrentElementIndex
    {
        get => _currentElementIndex;
        set => _currentElementIndex = value;
    }

    private int _currentElementIndex;

    public override void Open()
    {
        if (InteractableUserInterfaceElements.Length > 0)
            InteractableUserInterfaceElements[0].OnSelected();
    }

    public override void Close()
    {
        base.Close();
        _currentElementIndex = 0;
    }

    public virtual void Resume()
    {
        this.SetTitle();
        Console.RewriteScreen();
        foreach (var item in InteractableUserInterfaceElements)
            item.OnWindowResumed();
    }

    public void OnKeyPressed(ConsoleKeyInfo key)
    {
        switch (key.Key)
        {
            case ConsoleKey.Tab:
                if ((key.Modifiers & ConsoleModifiers.Shift) != 0)
                    MoveUpOneElement();
                else
                    MoveDownOneElement();
                return;

            case ConsoleKey.UpArrow:
                MoveUpOneElement();
                return;

            case ConsoleKey.DownArrow:
                MoveDownOneElement();
                return;
        }

        var element = this.InteractableUserInterfaceElements[_currentElementIndex];
        element.OnKeyPressed(key);
    }

    public void MoveDownOneElement()
    {
        var element = this.InteractableUserInterfaceElements[_currentElementIndex];
        element.OnUnselected();

        _currentElementIndex++;
        if (_currentElementIndex == this.InteractableUserInterfaceElements.Length)
            _currentElementIndex = 0;

        element = this.InteractableUserInterfaceElements[_currentElementIndex];
        element.OnSelected();
    }

    public void MoveUpOneElement()
    {
        var element = this.InteractableUserInterfaceElements[_currentElementIndex];
        element.OnUnselected();

        _currentElementIndex--;
        if (_currentElementIndex == -1)
            _currentElementIndex = this.InteractableUserInterfaceElements.Length - 1;

        element = this.InteractableUserInterfaceElements[_currentElementIndex];
        element.OnSelected();
    }
}