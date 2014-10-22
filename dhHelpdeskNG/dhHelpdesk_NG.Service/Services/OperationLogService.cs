using DH.Helpdesk.BusinessData.Models.OperationLog.Output;

namespace DH.Helpdesk.Services.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using DH.Helpdesk.BusinessData.Models;
    using DH.Helpdesk.Dal.Infrastructure.Context;
    using DH.Helpdesk.Dal.NewInfrastructure;
    using DH.Helpdesk.Dal.Repositories;
    using DH.Helpdesk.Domain;
    using DH.Helpdesk.Services.BusinessLogic.Mappers.OperationLogs;
    using DH.Helpdesk.Services.BusinessLogic.Specifications;

    using IUnitOfWork = DH.Helpdesk.Dal.Infrastructure.IUnitOfWork;

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

        /// <summary>
        /// The get operation log overviews.
        /// </summary>
        /// <param name="customers">
        /// The customers.
        /// </param>
        /// <param name="count">
        /// The count.
        /// </param>
        /// <param name="forStartPage">
        /// The for Start Page.
        /// </param>
        /// <returns>
        /// The result.
        /// </returns>
        IEnumerable<OperationLogOverview> GetOperationLogOverviews(int[] customers, int? count, bool forStartPage);
    }

    public class OperationLogService : IOperationLogService
    {
        private readonly IOperationLogRepository _operationLogRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWorkingGroupRepository _workingGroupRepository;

        private readonly IUnitOfWorkFactory unitOfWorkFactory;

        private readonly IWorkContext workContext;

        public OperationLogService(
            IOperationLogRepository operationLogRepository,
            IUnitOfWork unitOfWork,
            IWorkingGroupRepository workingGroupRepository, 
            IUnitOfWorkFactory unitOfWorkFactory, 
            IWorkContext workContext)
        {
            this._operationLogRepository = operationLogRepository;
            this._unitOfWork = unitOfWork;
            this._workingGroupRepository = workingGroupRepository;
            this.unitOfWorkFactory = unitOfWorkFactory;
            this.workContext = workContext;
        }

        public IList<OperationLog> GetOperationLogs(int customerId)
        {
            return this._operationLogRepository.GetMany(x => x.Customer_Id == customerId).ToList();
        }

        public IList<OperationLogList> SearchAndGenerateOperationLog(int customerId, IOperationLogSearch SearchOperationLogs)
        {
            int CID = customerId; // Current CustomerID
            if (SearchOperationLogs.CustomerId > 0)
                CID = SearchOperationLogs.CustomerId;

            var query = (from c in this._operationLogRepository.ListForIndexPage().Where(x => x.Customer_Id == CID)
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
                query = query.Where(x => SearchOperationLogs.OperationCategory_Filter.Contains(x.OperationCategoriy_ID));

            if (SearchOperationLogs.PeriodFrom != null)
                query = query.Where(x => x.CreatedDate >= SearchOperationLogs.PeriodFrom);

            if (SearchOperationLogs.PeriodTo != null)
                query = query.Where(x => x.CreatedDate <= SearchOperationLogs.PeriodTo);

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
            return this._operationLogRepository.GetById(id);
        }

        public IList<OperationLog> getAllOpertionLogs()
        {
            return this._operationLogRepository.GetAll().ToList();
        }

        public IList<OperationLogList> getListForIndexPage()
        {
            return this._operationLogRepository.ListForIndexPage();
        }

        public DeleteMessage DeleteOperationLog(int id)
        {
            var operationlog = this._operationLogRepository.GetById(id);

            if (operationlog != null)
            {
                try
                {
                    this._operationLogRepository.Delete(operationlog);
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
                    var wg = this._workingGroupRepository.GetById(id);

                    if (wg != null)
                        operationlog.WGs.Add(wg);
                }
            }

            if (operationlog.Id == 0)
                this._operationLogRepository.Add(operationlog);
            else
            {
                this._operationLogRepository.Update(operationlog);
            }

            if (errors.Count == 0)
                this.Commit();
        }
        public void Commit()
        {
            this._unitOfWork.Commit();
        }

        public IEnumerable<OperationLogOverview> GetOperationLogOverviews(int[] customers, int? count, bool forStartPage)
        {
            using (var uow = this.unitOfWorkFactory.Create())
            {
                var operationLogRepository = uow.GetRepository<OperationLog>();

                return operationLogRepository.GetAll()
                        .RestrictByWorkingGroupsAndUsers(this.workContext)
                        .GetForStartPage(customers, count, forStartPage)
                        .MapToOverviews();
            }
        }
    }
}
