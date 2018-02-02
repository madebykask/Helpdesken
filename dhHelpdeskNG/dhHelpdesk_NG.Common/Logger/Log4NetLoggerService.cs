namespace DH.Helpdesk.Common.Logger
{
    using System;

    using log4net;

    public class Log4NetLoggerService : ILoggerService
    {
        private readonly ILog logger;

        public Log4NetLoggerService(string logType)
        {
            this.logger = log4net.LogManager.GetLogger(logType);
        }

        public void Info(string message)
        {
            if (this.logger.IsInfoEnabled)
            {
                this.logger.Info(message);
            }
        }

        public void InfoFormat(string message, params object[] args)
        {
            if (this.logger.IsInfoEnabled)
            {
                this.logger.InfoFormat(message, args);
            }
        }

        public void Warn(string message)
        {
            if (this.logger.IsWarnEnabled)
            {
                this.logger.Warn(message);
            }
        }

        public void WarnFormat(string message, params object[] args)
        {
            if (this.logger.IsWarnEnabled)
            {
                this.logger.WarnFormat(message, args);
            }
        }

        public void Debug(string message)
        {
            if (this.logger.IsDebugEnabled)
            {
                this.logger.Debug(message);
            }
        }

        public void DebugFormat(string message, params object[] args)
        {
            if (this.logger.IsDebugEnabled)
            {
                this.logger.DebugFormat(message, args);
            }
        }

        public void Error(string message)
        {
            if (this.logger.IsErrorEnabled)
            {
                this.logger.Error(message);
            }
        }

        public void ErrorFormat(string message, params object[] args)
        {
            if (this.logger.IsErrorEnabled)
            {
                this.logger.ErrorFormat(message, args);
            }
        }

        public void Error(Exception ex)
        {
            if (this.logger.IsErrorEnabled)
            {
                this.logger.Error(ex.Message, ex);
            }
        }

        public void Fatal(string message)
        {
            if (this.logger.IsFatalEnabled)
            {
                this.logger.Fatal(message);
            }
        }

        public void FatalFormat(string message, params object[] args)
        {
            if (this.logger.IsFatalEnabled)
            {
                this.logger.FatalFormat(message, args);
            }
        }

        public void Fatal(Exception ex)
        {
            if (this.logger.IsFatalEnabled)
            {
                this.logger.Fatal(ex.Message, ex);
            }
        }

        public static class LogType
        {
            public const string EMAIL = "EmailLog";

            public const string ERROR = "AppErrorLog";

            public const string DATA_IMPORT = "DataImportLog";

            public const string Session = "SessionLog";
        }
    }
}