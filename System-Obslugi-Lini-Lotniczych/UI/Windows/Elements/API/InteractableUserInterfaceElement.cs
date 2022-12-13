using System;
using JetBrains.Annotations;
using LotSystem.UI.Windows.API;

namespace LotSystem.UI.Windows.Elements.API;

[PublicAPI]
public abstract class InteractableUserInterfaceElement : UserInterfaceElement, IInteractable
{
    protected InteractableUserInterfaceElement(GraphicsWindow window, string text)
        : base(window)
    {
        Text = text;
    }

    public virtual string Text { get; }

    public override void Draw()
    {
        YPos = Console.CursorTop;
        XStartPos = Console.CursorLeft;
    }

    protected int YPos;
    protected int XStartPos;
    public virtual void Redraw()
    {
        Console.CursorTop = YPos;
        Console.CursorLeft = XStartPos;
        Console.Write(Text);
    }

    public virtual void OnSelected()
    {
        System.Console.ForegroundColor = ConsoleColor.Black;
        System.Console.BackgroundColor = ConsoleColor.DarkGray;

        Redraw();

        System.Console.ResetColor();
    }

    public virtual void OnUnselected()
    {
        System.Console.ResetColor();
        Redraw();
    }

    public virtual void OnKeyPressed(ConsoleKeyInfo key)
    {
    }

    public virtual void OnWindowResumed()
    {
    }
}