namespace DH.Helpdesk.Services.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using DH.Helpdesk.Dal.Infrastructure;
    using DH.Helpdesk.Dal.Repositories;
    using DH.Helpdesk.Domain;

    public interface IOperationLogEmailLogService
    {
        IDictionary<string, string> Validate(OperationLogCategory operationLogCategoryToValidate);

        IList<OperationLogEMailLog> GetOperationLogEmailLogs(int operationLogId);

        void Commit();
    }

    public class OperationLogEmailLogService : IOperationLogEmailLogService
    {
        private readonly IOperationLogEMailLogRepository _operationLogEmailLogRepository;
        private readonly IUnitOfWork _unitOfWork;

        public OperationLogEmailLogService(
            IOperationLogEMailLogRepository operationLogEmailLogRepository,
            IUnitOfWork unitOfWork)
        {
            this._operationLogEmailLogRepository = operationLogEmailLogRepository;
            this._unitOfWork = unitOfWork;
        }

        public IDictionary<string, string> Validate(OperationLogCategory operationLogCategoryToValidate)
        {
            if (operationLogCategoryToValidate == null)
                throw new ArgumentNullException("operationlogcategorytovalidate");

            var errors = new Dictionary<string, string>();

            return errors;
        }

        public IList<OperationLogEMailLog> GetOperationLogEmailLogs(int operationLogId)
        {
            return this._operationLogEmailLogRepository.GetMany(x => x.OperationLog_Id == operationLogId).OrderByDescending(x => x.CreatedDate).ToList();
        }
        
        public void Commit()
        {
            this._unitOfWork.Commit();
        }

    }
}
