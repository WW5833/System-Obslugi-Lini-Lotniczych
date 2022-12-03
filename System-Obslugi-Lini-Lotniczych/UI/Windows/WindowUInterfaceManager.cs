using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace LotSystem.UI.Windows;

public class WindowUInterfaceManager
{
    private readonly object _lock = new();

    private readonly List<string> _screenLines = new() { string.Empty };

    public void Clear()
    {
        lock (_lock)
        {
            _screenLines.Clear();
            _screenLines.Add(string.Empty);
            _cursorLeftPosition = 0;
            _cursorTopPosition = 0;
            Console.Clear();
        }
    }

    public void Write(char text)
    {
        Write(text.ToString());
    }

    public void Write(string text)
    {
        _internalWrite(text.Split('\n')[0]);

        int offset;
        while ((offset = text.IndexOf('\n')) != -1)
            _internalWriteLine(text.Substring(offset + 1));
    }

    public void RemoveOneChar()
    {
        this.CursorLeft--;
        this._internalWrite(" ");
        this.CursorLeft--;
    }

    private void _internalWrite(string text)
    {
        if (text.Contains('\n'))
        {
            Debug.Fail($"\"{text}\" contained '\\n'");
            throw new ArgumentException("can't contain '\\n'", nameof(text));
        }

        lock (_lock)
        {
            while (_screenLines[_cursorTopPosition].Length < _cursorLeftPosition)
                _screenLines[_cursorTopPosition] += " ";
            _screenLines[_cursorTopPosition] = _screenLines[_cursorTopPosition].Insert(_cursorLeftPosition, text);
            _cursorLeftPosition += text.Length;
            Console.Write(text);
        }
    }

    private void _internalWriteLine(string text)
    {
        _screenLines.Add(text);
        _cursorLeftPosition = 0;
        _cursorTopPosition++;
        Console.WriteLine();
        _internalWrite(text);
    }

    public void WriteLine(string text)
    {
        Write(text);
        WriteLine();
    }

    public void WriteLine()
    {
        lock (_lock)
        {
            _screenLines.Add(string.Empty);
            _cursorLeftPosition = 0;
            _cursorTopPosition++;
            Console.WriteLine();
        }
    }

    public ConsoleKeyInfo ReadKey(bool intercept = false)
    {
        lock (_lock)
        {
            var key = Console.ReadKey(intercept);
            if (!intercept)
            {
                _cursorLeftPosition++;
                _screenLines[_cursorTopPosition] += key.KeyChar;
            }
            return key;
        }
    }

    public string ReadLine()
    {
        lock (_lock)
        {
            var line = Console.ReadLine();
            _screenLines[_cursorTopPosition] += line;
            _screenLines.Add(string.Empty);
            _cursorLeftPosition = 0;
            _cursorTopPosition++;
            return line;
        }
    }

    public void RewriteScreen()
    {
        lock (_lock)
        {
            Console.Clear();
            Console.Write(string.Join('\n', _screenLines));
        }
    }

    private int _cursorLeftPosition;
    public int CursorLeft
    {
        get => _cursorLeftPosition;
        set
        {
            lock(_lock) Console.CursorLeft = _cursorLeftPosition = value;
        }
    }

    private int _cursorTopPosition;
    public int CursorTop
    {
        get => _cursorTopPosition;
        set
        {
            lock (_lock) Console.CursorTop = _cursorTopPosition = value;
        }
    }
}