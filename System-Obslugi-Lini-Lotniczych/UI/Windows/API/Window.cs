﻿using JetBrains.Annotations;

namespace LotSystem.UI.Windows.API;

[PublicAPI]
public abstract class Window : IWindow
{
    public WindowUInterfaceManager Console { get; } = new();

    public abstract bool PreserveContentOnTransferControl { get; }

    public abstract string Id { get; }

    public abstract string Title { get; }

    public virtual void Open()
    {
    }

    public virtual void Close()
    {
    }

    protected static void OpenWindow(string window)
    {
        UserInterfaceManager.Instance.OpenWindow(window);
    }

    public void SetTitle()
    {
        System.Console.Title = this.Title;
    }
}