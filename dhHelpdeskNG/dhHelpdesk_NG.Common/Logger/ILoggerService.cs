namespace DH.Helpdesk.Common.Logger
{
    using System;

    public interface ILoggerService
    {
        void Info(string message);

        void InfoFormat(string message, params object[] args);

        void Warn(string message);

        void WarnFormat(string message, params object[] args);

        void Debug(string message);

        void DebugFormat(string message, params object[] args);

        void Error(string message);

        void ErrorFormat(string message, params object[] args);

        void Error(Exception ex);

        void Fatal(string message);

        void FatalFormat(string message, params object[] args);

        void Fatal(Exception ex);
    }
    
}