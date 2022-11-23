using System;
using System.Threading.Tasks;

namespace LotSystem.UI.Windows;

public abstract class Window : IWindow
{
    public abstract string Id { get; }

    public abstract string Title { get; }

    public virtual void Close()
    {
        Console.Clear();
    }

    public virtual void Open()
    {
        this.SetTitle();
        this.WriteTitle();
    }

    protected void OpenWindow(string window)
    {
        UIManager.Instance.OpenWindow(window);
    }

    public abstract Task Update();
}