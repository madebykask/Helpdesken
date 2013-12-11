using System.Collections.Generic;
using System.Linq;
using dhHelpdesk_NG.Data.Infrastructure;
using dhHelpdesk_NG.Data.Repositories;
using dhHelpdesk_NG.Domain;
using dhHelpdesk_NG.DTO.DTOs;

namespace dhHelpdesk_NG.Service
{
    public interface IOperationLogService
    {
        OperationLog getoperationlog(int id);
        IList<OperationLog> getAllOpertionLogs();
        IList<OperationLogList> getListForIndexPage();
        void Commit();
    }

    public class OperationLogService : IOperationLogService
    {
        private readonly IOperationLogRepository _operationLogRepository;
        private readonly IUnitOfWork _unitOfWork;

        public OperationLogService(
            IOperationLogRepository operationLogRepository,
            IUnitOfWork unitOfWork)
        {
            _operationLogRepository = operationLogRepository;
            _unitOfWork = unitOfWork;
        }

        public OperationLog getoperationlog(int id)
        {
            return _operationLogRepository.Get(x => x.Id == id);
        }

        public IList<OperationLog> getAllOpertionLogs()
        {
            return _operationLogRepository.GetAll().ToList();
        }

        public IList<OperationLogList> getListForIndexPage()
        {
            return _operationLogRepository.ListForIndexPage();
        }
        public void Commit()
        {
            _unitOfWork.Commit();
        }

    }
}
