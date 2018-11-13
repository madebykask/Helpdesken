using System.Threading.Tasks;
using DH.Helpdesk.Common.Logger;

namespace DH.Helpdesk.WebApi.Infrastructure.ClientLogger
{
    public interface IClientLogger
    {
        void Log(IClientLogEntry logEntry);
        Task LogAsync(IClientLogEntry logEntry);
    }

    public class ClientLogger : IClientLogger
    {
        private readonly IClientLogMessageFormatter _messageFormatter;
        private readonly ILoggerService _loggerService;

        #region ctor()

        public ClientLogger(IClientLogMessageFormatter formatter, ILoggerService logger)
        {
            _loggerService = logger;
            _messageFormatter = formatter ?? new ClientLogMessageFormatter();
        }

        #endregion

        public void Log(IClientLogEntry logEntry)
        {
            var msg = _messageFormatter.Format(logEntry);
            _loggerService.Error(msg);
        }

        public async Task LogAsync(IClientLogEntry logEntry)
        {
            var msg = _messageFormatter.Format(logEntry);
            await Task.Run(() => _loggerService.Error(msg)).ConfigureAwait(false);
        }
    }
}