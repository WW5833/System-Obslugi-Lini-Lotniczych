using System;

namespace LotSystem.Logger.API;

public interface ILogger
{
    void Debug(object message);
    void Info(object message);
    void Warn(object message);
    void Error(object message);
    void Error(Exception ex);
}