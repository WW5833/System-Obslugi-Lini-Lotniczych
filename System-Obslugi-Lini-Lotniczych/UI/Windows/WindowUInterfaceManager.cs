using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using JetBrains.Annotations;

namespace LotSystem.UI.Windows;

public class WindowUInterfaceManager
{
    [CanBeNull] public WindowUInterfaceManager Parent { get; set; }

    private readonly object _lock = new();

    private readonly List<List<Character>> _screenLines = new() { new List<Character>() };

    private readonly struct Character
    {
        public readonly char Char;
        public readonly ConsoleColor ForegroundColor;
        public readonly ConsoleColor BackgroundColor;

        public Character(char c)
        {
            Char = c;
            ForegroundColor = Console.ForegroundColor;
            BackgroundColor = Console.BackgroundColor;
        }

        public static implicit operator Character(char c)
        {
            return new Character(c);
        }

        public static IEnumerable<Character> Convert(string str)
        {
            return str.Select(c => new Character(c));
        }
    }

    public void Clear()
    {
        lock (_lock)
        {
            _screenLines.Clear();
            _screenLines.Add(new List<Character>());
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
        if (text.Length == 0)
            return;

        _internalWrite(text.Split('\n')[0]);

        int offset = 0;
        while ((offset = text.IndexOf('\n', offset + 1)) != -1)
        {
            var nextOffset = text.IndexOf('\n', offset + 1);
            if (nextOffset == -1)
                nextOffset = text.Length;
            _internalWriteLine(text[(offset + 1)..nextOffset]);
        }
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
            while (_screenLines[_cursorTopPosition].Count < _cursorLeftPosition + text.Length)
                _screenLines[_cursorTopPosition].Add('\0');

            _screenLines[_cursorTopPosition].RemoveRange(_cursorLeftPosition, text.Length);
            _screenLines[_cursorTopPosition].InsertRange(_cursorLeftPosition, Character.Convert(text));

            _cursorLeftPosition += text.Length;
            Console.Write(text);
        }
    }

    private void _internalWriteLine(string text)
    {
        _screenLines.Add(new List<Character>(Character.Convert(text)));
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
            _screenLines.Add(new List<Character>());
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
                _screenLines[_cursorTopPosition].Add(key.KeyChar);
            }
            return key;
        }
    }

    public string ReadLine()
    {
        lock (_lock)
        {
            var line = Console.ReadLine();
            _screenLines[_cursorTopPosition].AddRange(Character.Convert(line));
            _screenLines.Add(new List<Character>());
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

            Parent?.RewriteScreen();

            Console.CursorTop = 0;
            Console.CursorLeft = 0;

            foreach (var line in _screenLines)
            {
                foreach (var character in line)
                {
                    if (character.Char == '\0')
                    {
                        Console.CursorLeft++;
                        continue;
                    }

                    Console.ForegroundColor = character.ForegroundColor;
                    Console.BackgroundColor = character.BackgroundColor;
                    Console.Write(character.Char);
                }

                Console.ResetColor();
                Console.WriteLine();
            }

            Console.CursorTop = _cursorTopPosition;
            Console.CursorLeft = _cursorLeftPosition;
        }
    }

    private int _cursorLeftPosition;
    public int CursorLeft
    {
        get => _cursorLeftPosition;
        set
        {
            lock (_lock) Console.CursorLeft = _cursorLeftPosition = value;
        }
    }

    private int _cursorTopPosition;
    public int CursorTop
    {
        get => _cursorTopPosition;
        set
        {
            lock (_lock)
            {
                Console.CursorTop = _cursorTopPosition = value;
                while (_screenLines.Count <= value)
                    _screenLines.Add(new List<Character>());
            }
        }
    }
}