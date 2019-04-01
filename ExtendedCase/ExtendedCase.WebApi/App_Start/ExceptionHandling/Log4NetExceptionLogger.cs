using System.Web.Http.ExceptionHandling;
using Common.Logging;

namespace ExtendedCase.WebApi.ExceptionHandling
{
    public class Log4NetExceptionLogger : System.Web.Http.ExceptionHandling.ExceptionLogger
    {
        private readonly ILogger _logger;

        public Log4NetExceptionLogger(ILogger logger)
        {
            _logger = logger;
        }

        public override void Log(ExceptionLoggerContext context)
        {
            if (!context.CallsHandler)
                _logger.Error($"Unhandled exception (no response) requesting {context.Request.RequestUri.AbsoluteUri}. Error : {context.Exception}");
        }
    }
}