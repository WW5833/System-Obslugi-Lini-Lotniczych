using System;
using LotSystem.Logger.API;

namespace LotSystem.Logger;

internal class DebugLogger : ILogger
{
    public void Debug(object message)
    {
        var old = Console.ForegroundColor;
        Console.ForegroundColor = ConsoleColor.DarkGray;
        Console.WriteLine($"[DEBUG] {message}");
        Console.ForegroundColor = old;
    }

    public void Info(object message)
    {
        var old = Console.ForegroundColor;
        Console.ForegroundColor = ConsoleColor.White;
        Console.WriteLine($"[INFO] {message}");
        Console.ForegroundColor = old;
    }

    public void Warn(object message)
    {
        var old = Console.ForegroundColor;
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine($"[WARN] {message}");
        Console.ForegroundColor = old;
    }

    public void Error(object message)
    {
        var old = Console.ForegroundColor;
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine($"[ERROR] {message}");
        Console.ForegroundColor = old;
    }

    public void Error(Exception ex)
    {
        var old = Console.ForegroundColor;
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine($"[EXCEPTION] {ex}");
        Console.ForegroundColor = old;
    }
}
