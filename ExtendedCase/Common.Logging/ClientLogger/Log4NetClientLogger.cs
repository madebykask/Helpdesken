using log4net;

namespace Common.Logging.ClientLogger
{
    public interface IClientLogger
    {
        void Log(IClientLogEntry logEntry);
    }

    public class Log4NetClientLogger : IClientLogger
    {
        private readonly ILog _log;
        private readonly IClientLogMessageFormatter _messageFormatter;

        #region ctor()

        public Log4NetClientLogger()
            : this(null)
        {

        }

        public Log4NetClientLogger(IClientLogMessageFormatter formatter)
        {
            _log = LogManager.GetLogger("ClientLogger");
            _messageFormatter = formatter ?? new ClientLogMessageFormatter();
        }

        #endregion

        public void Log(IClientLogEntry logEntry)
        {
            var msg = _messageFormatter.Format(logEntry);
            _log.Error(msg);
        }
    }
}