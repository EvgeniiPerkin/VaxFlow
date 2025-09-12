using System;

namespace VaxFlow.Services
{
    public interface IMyLogger
    {
        void Info(string message);
        void Debug(string message);
        void Warn(string message);
        void Error(Exception e, string message);
        void Fatal(Exception e, string message);
    }
}
