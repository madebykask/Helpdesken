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
        OperationLog GetOperationLog(int id);
        IList<OperationLog> GetAllOpertionLogs();
        IList<OperationLogList> GetListForIndexPage();
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
            using (var uow = this.unitOfWorkFactory.Create())
            {
                var operationLogRep = uow.GetRepository<OperationLog>();

                return operationLogRep.GetAll()
                        .RestrictByWorkingGroupsAndUsers(this.workContext)
                        .GetByCustomer(customerId)
                        .ToList();
            }
        }

        public IList<OperationLogList> SearchAndGenerateOperationLog(int customerId, IOperationLogSearch SearchOperationLogs)
        {
            int CID = customerId; // Current CustomerID
            if (SearchOperationLogs.CustomerId > 0)
                CID = SearchOperationLogs.CustomerId;

            var query = (from c in this.GetListForIndexPage().Where(x => x.Customer_Id == CID)
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

        public OperationLog GetOperationLog(int id)
        {
            using (var uow = this.unitOfWorkFactory.Create())
            {
                var operationLogRep = uow.GetRepository<OperationLog>();

                return operationLogRep.GetAll()
                        .RestrictByWorkingGroupsAndUsers(this.workContext)
                        .GetById(id)
                        .IncludePath(o => o.Us)
                        .IncludePath(o => o.WGs)
                        .SingleOrDefault();
            }
        }

        public IList<OperationLog> GetAllOpertionLogs()
        {
            using (var uow = this.unitOfWorkFactory.Create())
            {
                var operationLogRep = uow.GetRepository<OperationLog>();

                return operationLogRep.GetAll()
                        .RestrictByWorkingGroupsAndUsers(this.workContext)
                        .ToList();
            }
        }

        public DeleteMessage DeleteOperationLog(int id)
        {
            try
            {
                using (var uow = this.unitOfWorkFactory.Create())
                {
                    var rep = uow.GetRepository<OperationLog>();

                    var entity = rep.GetAll()
                                  .RestrictByWorkingGroupsAndUsers(this.workContext)
                                  .GetById(id)
                                  .SingleOrDefault();

                    if (entity == null)
                    {
                        return DeleteMessage.Error;
                    }

                    entity.Us.Clear();
                    entity.WGs.Clear();
                    entity.EmailLogs.Clear();

                    rep.DeleteById(id);

                    uow.Save();
                    return DeleteMessage.Success;
                }
            }
            catch
            {
                return DeleteMessage.UnExpectedError;
            }
        }


        public void SaveOperationLog(OperationLog operationlog, int[] wgs, out IDictionary<string, string> errors)
        {
            if (operationlog == null)
            {
                throw new ArgumentNullException("operationlog");
            }

            errors = new Dictionary<string, string>();
            if (errors.Any())
            {
                return;
            }

            using (var uow = this.unitOfWorkFactory.Create())
            {
                var operationLogRep = uow.GetRepository<OperationLog>();
                var workingGroupRep = uow.GetRepository<WorkingGroupEntity>();

                OperationLog entity;
                var now = DateTime.Now;
                if (operationlog.IsNew())
                {
                    entity = new OperationLog();
                    OperationLogMapper.MapToEntity(operationlog, entity);
                    entity.CreatedDate = now;
                    entity.ChangedDate = now;
                    operationLogRep.Add(entity);
                }
                else
                {
                    entity = operationLogRep.GetById(operationlog.Id);
                    OperationLogMapper.MapToEntity(operationlog, entity);
                    entity.ChangedDate = now;
                    operationLogRep.Update(entity);
                }

                entity.WGs.Clear();
                if (wgs != null)
                {
                    foreach (var wg in wgs)
                    {
                        var workingGroupEntity = workingGroupRep.GetById(wg);
                        entity.WGs.Add(workingGroupEntity);
                    }
                }

                uow.Save();
            }            
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

        public IList<OperationLogList> GetListForIndexPage()
        {
            using (var uow = this.unitOfWorkFactory.Create())
            {
                var operationLogRep = uow.GetRepository<OperationLog>();
                var operationLogCategoryRep = uow.GetRepository<OperationLogCategory>();
                var operationObjectRep = uow.GetRepository<OperationObject>();
                var userRep = uow.GetRepository<User>();

                var query = from ol in operationLogRep.GetAll().RestrictByWorkingGroupsAndUsers(this.workContext)
                            join olc in operationLogCategoryRep.GetAll() on ol.OperationLogCategory_Id equals olc.Id into gj
                            from x in gj.DefaultIfEmpty()
                            join ob in operationObjectRep.GetAll() on ol.OperationObject_Id equals ob.Id
                            join u in userRep.GetAll() on ol.User_Id equals u.Id
                            group ol by new
                            {
                                ol.Id,
                                OLCName = x == null ? string.Empty : x.OLCName,
                                ob.Name,
                                u.UserID,
                                ol.LogText,
                                ol.LogAction,
                                ol.CreatedDate,
                                ol.Customer_Id,
                                OOI = ob.Id,
                                OLCID = x == null ? 0 : x.Id
                            }
                                into g
                                select new OperationLogList
                                {
                                    OperationLogAction = g.Key.LogAction,
                                    OperationLogAdmin = g.Key.UserID,
                                    CreatedDate = g.Key.CreatedDate,
                                    OperationLogCategoryName = g.Key.OLCName,
                                    OperationLogDescription = g.Key.LogText,
                                    OperationObjectName = g.Key.Name,
                                    Id = g.Key.Id,
                                    Customer_Id = g.Key.Customer_Id,
                                    OperationObject_ID = g.Key.OOI,
                                    OperationCategoriy_ID = g.Key.OLCID
                                };

                return query.ToList();
            }            
        }
    }
}
