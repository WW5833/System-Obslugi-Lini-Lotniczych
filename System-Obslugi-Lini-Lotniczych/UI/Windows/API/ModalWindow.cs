using JetBrains.Annotations;

namespace LotSystem.UI.Windows.API;

[PublicAPI]
public abstract class ModalWindow : GraphicsWindow
{
    public abstract int Width { get; }
    public abstract int Height { get; }

    public override void Open()
    {
        this.SetTitle();
        this.WriteOutline();
        this.WriteHeader();

        this.Console.CursorTop = StartTop + 1;
        this.Console.CursorLeft = StartLeft + 2;

        this.WriteElements();

        base.Open();
    }

    private void WriteElements()
    {
        for (var i = 0; i < UserInterfaceElements.Length; i++)
        {
            this.Console.CursorTop = StartTop + 1 + i;
            this.Console.CursorLeft = StartLeft + 2;

            UserInterfaceElements[i].Draw();
        }
    }

    public int StartTop => (System.Console.WindowHeight - Height) / 2;
    public int StartLeft => (System.Console.WindowWidth - Width) / 2;

    public void WriteOutline()
    {
        // Clear
        this.Console.CursorTop = StartTop + 1;
        this.Console.CursorLeft = StartLeft + 1;
        for (var y = 0; y < Height; y++)
        {
            for (var x = 0; x < Width; x++)
                Console.Write(' ');

            this.Console.CursorTop = StartTop + 1 + y;
            this.Console.CursorLeft = StartLeft + 1;
        }

        //Top Header
        this.Console.CursorTop = StartTop;
        this.Console.CursorLeft = StartLeft;
        Console.Write('∙');
        for (var x = 0; x < Width; x++)
            Console.Write("–");
        Console.Write('∙');


        // Bottom Header
        this.Console.CursorTop = StartTop + Height + 1;
        this.Console.CursorLeft = StartLeft;
        Console.Write('∙');
        for (var x = 0; x < Width; x++)
            Console.Write("–");
        Console.Write('∙');

        // Left Side
        for (var y = 0; y < Height; y++)
        {
            this.Console.CursorTop = StartTop + y + 1;
            this.Console.CursorLeft = StartLeft;
            Console.Write('|');
        }

        // Right Side
        for (var y = 0; y < Height; y++)
        {
            this.Console.CursorTop = StartTop + y + 1;
            this.Console.CursorLeft = StartLeft + Width + 1;
            Console.Write('|');
        }
    }

    public void WriteHeader()
    {
        this.Console.CursorTop = StartTop;
        this.Console.CursorLeft = StartLeft + 3;

        this.Console.Write(this.Title);
    }
}

public interface IModalSelectWindow<T> : IGWindow
{
    T Value { get; }
}