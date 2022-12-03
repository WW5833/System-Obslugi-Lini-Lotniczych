using System.Threading.Tasks;

namespace LotSystem.UI.Windows.API;

public abstract class Window : IWindow
{
    public WindowUInterfaceManager Console { get; } = new();

    public virtual bool PreserveContentOnTransferControl => true;

    public abstract string Id { get; }

    public abstract string Title { get; }

    public virtual void Close()
    {
        Console.Clear();
    }

    public virtual void Open()
    {
        Console.Clear();

        this.SetTitle();
        this.WriteTitle();
    }

    protected static void OpenWindow(string window)
    {
        UserInterfaceManager.Instance.OpenWindow(window);
    }

    public abstract Task Update();
}