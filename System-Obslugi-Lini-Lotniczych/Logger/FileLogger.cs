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
        if (!Directory.Exists("Logs"))
            Directory.CreateDirectory("Logs");
        _stream = File.Open($"Logs/Log_{DateTime.Now:yyyy-MM-dd_HH-mm-ss}.log", FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.Read);
    }

    private void Log(string msg)
    {
        lock (_lock)
        {
            _stream.Write(Encoding.UTF8.GetBytes($"[{DateTime.Now:HH:mm:ss.fff}] {msg}{Environment.NewLine}"));
            _stream.Flush();
        }
    }

    public void Database(object message)
    {
        lock (_lock)
        {
            _stream.Write(Encoding.UTF8.GetBytes($"{message}{Environment.NewLine}"));
            _stream.Flush();
        }
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
