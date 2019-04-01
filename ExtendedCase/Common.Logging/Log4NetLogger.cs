using System;
using log4net;

namespace Common.Logging
{
    public class Log4NetLogger : ILogger
    {
        private readonly ILog _log;
        private readonly IErrorMessageFormatter _messageFormatter;

        public Log4NetLogger()
        {
            _log = LogManager.GetLogger("Default");
            _messageFormatter = new DefaultErrorFormatter();
        }

        public Log4NetLogger(IErrorMessageFormatter formatter) 
            : this()
        {
            _messageFormatter = formatter;
        }

        /// <inheritdoc/>
        public void Info(string message)
        {
            _log.Info(message);
        }

        /// <inheritdoc/>
        public void InfoFormat(string message, params object[] args)
        {
            _log.InfoFormat(message, args);
        }

        /// <inheritdoc/>
        public void Debug(string message)
        {
            _log.Debug(message);
        }

        /// <inheritdoc/>
        public void Debug(string message, Exception ex)
        {
            _log.Debug(message, ex);
        }

        /// <inheritdoc/>
        public void DebugFormat(string message, params object[] args)
        {
            if (String.IsNullOrEmpty(message))
                return; //nothing to log

            _log.DebugFormat(message, args);
        }

        /// <inheritdoc/>
        public string Error(Exception ex)
        {
            return LogError("", ex);
        }

        /// <inheritdoc/>
        public string Error(string message, Exception ex)
        {
            return LogError(message, ex);
        }

        /// <inheritdoc/>
        public string Error(string message)
        {
            return LogError(message, null);
        }

        /// <inheritdoc/>
        public string ErrorFormat(string message, params object[] args)
        {
            return LogErrorFormat(message, args);
        }

        /// <inheritdoc/>
        public void Warning(string message)
        {
            LogWarning(message, null);
        }

        /// <inheritdoc/>
        public string Warning(string message, Exception ex)
        {
            return LogWarning(message, ex);
        }

        /// <inheritdoc/>
        public void WarningFormat(string message, params object[] args)
        {
            _log.WarnFormat(message, args);
        }

        private string LogError(string messageText, Exception ex)
        {
            var errorIdentifier = Guid.NewGuid().ToString();
            var message = _messageFormatter.FormatError(messageText, errorIdentifier);

            if (ex == null)
                _log.Error(message);
            else
                _log.Error(message, ex);
            return errorIdentifier;
        }

        private string LogWarning(string messageText, Exception ex)
        {
            var errorIdentifier = Guid.NewGuid().ToString();
            var message = _messageFormatter.FormatWarning(messageText, errorIdentifier, ex);

            if (ex == null)
                _log.Warn(message);
            else
            {
                _log.Warn(message, ex);
            }
            return errorIdentifier;
        }

        private string LogErrorFormat(string messageText, object[] args)
        {
            var errorIdentifier = Guid.NewGuid().ToString();
            var message = _messageFormatter.FormatError(messageText, errorIdentifier);
            _log.ErrorFormat(message, args);
            return errorIdentifier;
        }
    }

    public interface ILogger
    {
        /// <summary>
        /// Logs informational message.
        /// </summary>
        /// <param name="message">Message to be logged.</param>
        void Info(string message);

        /// <summary>
        /// Logs formatted informational message.
        /// </summary>
        /// <param name="message">Message to be logged.</param>
        /// /// <param name="args">The arguments.</param>
        void InfoFormat(string message, params object[] args);

        /// <summary>
        /// Logs debug message.
        /// </summary>
        /// <param name="message">Message to be logged.</param>
        void Debug(string message);

        /// <summary>
        /// Logs debug message.
        /// </summary>
        /// <param name="message">Message to be logged.</param>
        /// <param name="ex">Exception to be logged.</param>
        void Debug(string message, Exception ex);

        /// <summary>
        /// Writes the formatted debug information.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="args">The arguments.</param>
        void DebugFormat(string message, params object[] args);

        /// <summary>
        /// Logs error message.
        /// </summary>
        /// <param name="ex">Exception to be logged.</param>
        string Error(Exception ex);

        /// <summary>
        /// Logs error message.
        /// </summary>
        /// <param name="message">Message to be logged.</param>
        /// <param name="ex">Exception to be logged.</param>
        string Error(string message, Exception ex);

        /// <summary>
        /// Logs error message.
        /// </summary>
        /// <param name="message">Message to be logged.</param>
        string Error(string message);

        /// <summary>
        /// Writes the formatted error information.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="args">The arguments.</param>
        string ErrorFormat(string message, params object[] args);

        /// <summary>
        /// Logs warning message.
        /// </summary>
        /// <param name="message">Message to be logged.</param>
        void Warning(string message);

        /// <summary>
        /// Logs warning message.
        /// </summary>
        /// <param name="message">Message to be logged.</param>
        /// <param name="ex">Exception to be logged.</param>
        string Warning(string message, Exception ex);

        /// <summary>
        /// Writes the formatted warning information.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="args">The arguments.</param>
        void WarningFormat(string message, params object[] args);
    }
}