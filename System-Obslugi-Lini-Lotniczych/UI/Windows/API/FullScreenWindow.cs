using JetBrains.Annotations;

namespace LotSystem.UI.Windows.API;

[PublicAPI]
public abstract class FullScreenWindow : GraphicsWindow
{
    public override bool PreserveContentOnTransferControl => true;

    public override void Close()
    {
        Console.Clear();
    }

    public override void Open()
    {
        Console.Clear();

        this.SetTitle();
        this.WriteTitle();
        this.WriteElements();

        base.Open();
    }

    private void WriteElements()
    {
        for (var i = 0; i < UserInterfaceElements.Length; i++)
        {
            this.Console.CursorTop = 5 + i;
            this.Console.CursorLeft = 1;
            UserInterfaceElements[i].Draw();
        }
    }

    protected void WriteTitle()
    {
        this.Console.CursorTop = 0;
        this.Console.CursorLeft = 0;
        this.WriteSeparator();

        if (UserInterfaceManager.Instance.CurrentSessionId.HasValue)
        {
            var oldPos = this.Console.CursorLeft;
            this.Console.CursorLeft = System.Console.WindowWidth - 37 - 9;

            this.Console.Write("Session: " + UserInterfaceManager.Instance.CurrentSessionId.Value);

            this.Console.CursorLeft = oldPos;
        }

        this.Console.WriteLine(this.Title);
        this.Console.WriteLine();

        this.WriteSeparator();
    }

    protected void WriteSeparator()
    {
        for (var i = 0; i < System.Console.WindowWidth; i++)
            Console.Write("=");

        Console.WriteLine();
    }
}