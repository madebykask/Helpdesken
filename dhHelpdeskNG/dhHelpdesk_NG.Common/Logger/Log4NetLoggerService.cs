namespace DH.Helpdesk.Common.Logger
{
    using System;

    using log4net;

    public class Log4NetLoggerService : ILoggerService
    {
        private readonly ILog _logger;

        public Log4NetLoggerService(string logType)
        {
            _logger = LogManager.GetLogger(logType);
        }

        public void Info(string message)
        {
            if (_logger.IsInfoEnabled)
            {
                _logger.Info(message);
            }
        }

        public void InfoFormat(string message, params object[] args)
        {
            if (_logger.IsInfoEnabled)
            {
                _logger.InfoFormat(message, args);
            }
        }

        public void Warn(string message)
        {
            if (_logger.IsWarnEnabled)
            {
                _logger.Warn(message);
            }
        }

        public void WarnFormat(string message, params object[] args)
        {
            if (_logger.IsWarnEnabled)
            {
                _logger.WarnFormat(message, args);
            }
        }

        public void Debug(string message)
        {
            if (_logger.IsDebugEnabled)
            {
                _logger.Debug(message);
            }
        }

        public void DebugFormat(string message, params object[] args)
        {
            if (_logger.IsDebugEnabled)
            {
                _logger.DebugFormat(message, args);
            }
        }

        public void Error(string message)
        {
            if (_logger.IsErrorEnabled)
            {
                _logger.Error(message);
            }
        }

        public void ErrorFormat(string message, params object[] args)
        {
            if (_logger.IsErrorEnabled)
            {
                _logger.ErrorFormat(message, args);
            }
        }

        public void Error(Exception ex)
        {
            if (_logger.IsErrorEnabled)
            {
                _logger.Error(ex.Message, ex);
            }
        }

        public void Fatal(string message)
        {
            if (_logger.IsFatalEnabled)
            {
                _logger.Fatal(message);
            }
        }

        public void FatalFormat(string message, params object[] args)
        {
            if (_logger.IsFatalEnabled)
            {
                _logger.FatalFormat(message, args);
            }
        }

        public void Fatal(Exception ex)
        {
            if (_logger.IsFatalEnabled)
            {
                _logger.Fatal(ex.Message, ex);
            }
        }

        public static class LogType
        {
            public const string EMAIL = "EmailLog";

            public const string ERROR = "AppErrorLog";

            public const string DATA_IMPORT = "DataImportLog";

            public const string Session = "SessionLog";

            public const string Client = "ClientLog";

            public const string reCaptcha = "reCaptchaLog";
        }
    }
}