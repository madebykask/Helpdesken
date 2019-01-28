using AutoMapper;
using Common.Logging.ClientLogger;
using ExtendedCase.Models;

namespace ExtendedCase.Logic.Services
{
    public interface IClientLogService
    {
        void AddLogItem(ClientLogItemModel item);
    }

    public class ClientLogService : IClientLogService
    {
        private readonly IClientLogger _clientLogger;
        private readonly IMapper _mapper;

        #region ctor()

        public ClientLogService()
        {
        }

        public ClientLogService(IClientLogger clientLogger, IMapper mapper)
        {
            _clientLogger = clientLogger;
            _mapper = mapper;
        }

        #endregion
        
        public void AddLogItem(ClientLogItemModel item)
        {
            var logEntry = _mapper.Map<ClientLogItemModel, ClientLogEntry>(item);
            _clientLogger.Log(logEntry);
        }
    }
}