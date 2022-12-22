using System;

namespace LotSystem.Logger.API;

public interface ILogger
{
    void Database(object message);
    void Debug(object message);
    void Info(object message);
    void Warn(object message);
    void Error(object message);
    void Error(Exception ex);
}