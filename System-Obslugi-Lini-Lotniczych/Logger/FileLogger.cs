using System;
using System.IO;
using System.Text;
using LotSystem.Logger.API;

namespace LotSystem.Logger;

internal class FileLogger : ILogger
{
    private readonly FileStream _stream;
    private readonly object _lock = new();

    public FileLogger()
    {
        _stream = File.Create($"Log_{DateTime.Now:yyyy-MM-dd_HH:mm:ss}.log");
    }

    private void Log(string msg)
    {
        lock (_lock)
            _stream.Write(Encoding.UTF8.GetBytes($"[{DateTime.Now:HH:mm:ss.fff}] {msg}{Environment.NewLine}"));
    }

    public void Debug(object message)
    {
        Log($"[DEBUG] {message}");
    }

    public void Info(object message)
    {
        Log($"[INFO] {message}");
    }

    public void Warn(object message)
    {
        Log($"[WARN] {message}");
    }

    public void Error(object message)
    {
        Log($"[ERROR] {message}");
    }

    public void Error(Exception ex)
    {
        Log($"[EXCEPTION] {ex}");
    }
}
