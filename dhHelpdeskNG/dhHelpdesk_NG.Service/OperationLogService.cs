using System.Collections.Generic;
using System.Linq;
using dhHelpdesk_NG.Data.Infrastructure;
using dhHelpdesk_NG.Data.Repositories;
using dhHelpdesk_NG.Domain;
using dhHelpdesk_NG.DTO.DTOs;
using System;

namespace dhHelpdesk_NG.Service
{
    public interface IOperationLogService
    {
        IList<OperationLog> GetOperationLogs(int customerId);
        OperationLog getoperationlog(int id);
        IList<OperationLog> getAllOpertionLogs();
        IList<OperationLogList> getListForIndexPage();
        IList<OperationLogList> SearchAndGenerateOperationLog(int customerId, IOperationLogSearch SearchOperationLogs);
        void SaveOperationLog(OperationLog operationlog, int[] wgs, out IDictionary<string, string> errors);
        DeleteMessage DeleteOperationLog(int id);
        void Commit();
    }

    public class OperationLogService : IOperationLogService
    {
        private readonly IOperationLogRepository _operationLogRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWorkingGroupRepository _workingGroupRepository;
        private readonly IOperationLogCategoryService _operationLogCategoryService;
        private readonly IOperationObjectService _operationObjectService;

        public OperationLogService(
            IOperationLogRepository operationLogRepository,
            IUnitOfWork unitOfWork,
            IOperationObjectService operationObjectService,
            IOperationLogCategoryService operationLogCategoryService,
            IWorkingGroupRepository workingGroupRepository
            )
        {
            _operationLogRepository = operationLogRepository;
            _unitOfWork = unitOfWork;
            _workingGroupRepository = workingGroupRepository;
            _operationLogCategoryService = operationLogCategoryService;
            _operationObjectService = operationObjectService;

        }

        public IList<OperationLog> GetOperationLogs(int customerId)
        {
            return _operationLogRepository.GetMany(x => x.Customer_Id == customerId).ToList();
        }

        public IList<OperationLogList> SearchAndGenerateOperationLog(int customerId, IOperationLogSearch SearchOperationLogs)
        {
            int CID = customerId; // Current CustomerID
            if (SearchOperationLogs.CustomerId > 0)
                CID = SearchOperationLogs.CustomerId;  

            var query = (from c in _operationLogRepository.ListForIndexPage().Where(x => x.Customer_Id == CID)
                         select c);

            if (!string.IsNullOrEmpty(SearchOperationLogs.Text_Filter))
                query = query.Where(x => x.OperationLogAction.Contains(SearchOperationLogs.Text_Filter)
                                      || x.OperationLogDescription.Contains(SearchOperationLogs.Text_Filter)
                                      || x.OperationLogCategoryName.Contains(SearchOperationLogs.Text_Filter)
                                      || x.OperationObjectName.Contains(SearchOperationLogs.Text_Filter)                                                                  
                                   );

            if (SearchOperationLogs.OperationObject_Filter != null)
                query = query.Where(x => SearchOperationLogs.OperationObject_Filter.Contains(x.OperationObject_ID));

            if (SearchOperationLogs.OperationCategory_Filter != null)
                query = query.Where(x => SearchOperationLogs.OperationCategory_Filter.Contains(x.OperationCategoriy_ID) );

            if (SearchOperationLogs.PeriodFrom != null)
                query = query.Where(x => x.CreatedDate>= DateTime.Parse(SearchOperationLogs.PeriodFrom));

            if (SearchOperationLogs.PeriodTo != null)
                query = query.Where(x => x.CreatedDate <= DateTime.Parse(SearchOperationLogs.PeriodTo));
 
            if (!string.IsNullOrEmpty(SearchOperationLogs.SortBy) && (SearchOperationLogs.SortBy != "undefined"))
            {
                if (SearchOperationLogs.Ascending)
                    query = query.OrderBy(x => x.GetType().GetProperty(SearchOperationLogs.SortBy).GetValue(x, null));
                else
                    query = query.OrderByDescending(x => x.GetType().GetProperty(SearchOperationLogs.SortBy).GetValue(x, null));               
            }

            return query.ToList();


        }

        public OperationLog getoperationlog(int id)
        {            
            return _operationLogRepository.GetById(id);
        }

        public IList<OperationLog> getAllOpertionLogs()
        {
            return _operationLogRepository.GetAll().ToList();
        }

        public IList<OperationLogList> getListForIndexPage()
        {
            return _operationLogRepository.ListForIndexPage();
        }

        public DeleteMessage DeleteOperationLog(int id)
        {
            var operationlog = _operationLogRepository.GetById(id);

            if (operationlog != null)
            {
                try
                {
                    _operationLogRepository.Delete(operationlog);
                    this.Commit();

                    return DeleteMessage.Success;
                }
                catch
                {
                    return DeleteMessage.UnExpectedError;
                }
            }

            return DeleteMessage.Error;
        }


        public void SaveOperationLog(OperationLog operationlog, int[] wgs, out IDictionary<string, string> errors)
        {
            if (operationlog == null)
                throw new ArgumentNullException("operationlog");

            errors = new Dictionary<string, string>();

            operationlog.LogText = operationlog.LogText ?? string.Empty;
            operationlog.LogAction = operationlog.LogAction ?? string.Empty;
            operationlog.LogTextExternal = operationlog.LogTextExternal ?? string.Empty;

            if (operationlog.WGs != null)
                foreach (var delete in operationlog.WGs.ToList())
                    operationlog.WGs.Remove(delete);
            else
                operationlog.WGs = new List<WorkingGroupEntity>();

            if (wgs != null)
            {
                foreach (int id in wgs)
                {
                    var wg = _workingGroupRepository.GetById(id);

                    if (wg != null)
                        operationlog.WGs.Add(wg);
                }
            }

            if (operationlog.Id == 0)
                _operationLogRepository.Add(operationlog);
            else
            {
                _operationLogRepository.Update(operationlog);                
            }          

            if (errors.Count == 0)
                this.Commit();
        }
        public void Commit()
        {
            _unitOfWork.Commit();
        }

    }
}
