using DH.Helpdesk.BusinessData.Models.Common.Output;
using DH.Helpdesk.BusinessData.Models.OperationLog.Output;

namespace DH.Helpdesk.Dal.Repositories
{
    using System.Collections.Generic;
    using System.Linq;

    using DH.Helpdesk.BusinessData.Models;
    using DH.Helpdesk.Dal.Infrastructure;
    using DH.Helpdesk.Domain;

    #region OPERATIONLOG

    public interface IOperationLogRepository : IRepository<OperationLog>
    {
        IList<OperationLogList> ListForIndexPage();
        IEnumerable<OperationLogOverview> GetOperationLogOverviews(int[] customers);
    }

    public class OperationLogRepository : RepositoryBase<OperationLog>, IOperationLogRepository
    {
        public OperationLogRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }

        public IList<OperationLogList> ListForIndexPage()
        {
            var query = from ol in this.DataContext.OperationLogs
                        join olc in this.DataContext.OperationLogCategories on ol.OperationLogCategory_Id equals olc.Id into gj
                        from x in gj.DefaultIfEmpty()

                        join ob in this.DataContext.OperationObjects on ol.OperationObject_Id equals ob.Id
                        join u in this.DataContext.Users on ol.User_Id equals u.Id
                        group ol by new
                        {
                            ol.Id,
                            OLCName = (x == null ? string.Empty : x.OLCName),
                            ob.Name,
                            u.UserID,
                            ol.LogText,
                            ol.LogAction,
                            ol.CreatedDate,
                            ol.Customer_Id,
                            OOI = ob.Id,
                            OLCID = (x == null ? 0 : x.Id)
                        } into g
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

        public IEnumerable<OperationLogOverview> GetOperationLogOverviews(int[] customers)
        {
            return DataContext.OperationLogs
                .Where(o => customers.Contains(o.Customer_Id))
                .Select(o => new OperationLogOverview()
                {
                    Customer_Id = o.Customer_Id,
                    ChangedDate = o.ChangedDate,
                    CreatedDate = o.CreatedDate,
                    LogText = o.LogText,
                    Category = new OperationLogCategoryOverview()
                    {
                        OLCName = o.Category != null ? o.Category.OLCName : null
                    }
                })
                .OrderByDescending(p => p.CreatedDate); 
        }
    }

    #endregion

    #region OPERATIONLOGCATEGORY

    public interface IOperationLogCategoryRepository : IRepository<OperationLogCategory>
    {
    }

    public class OperationLogCategoryRepository : RepositoryBase<OperationLogCategory>, IOperationLogCategoryRepository
    {
        public OperationLogCategoryRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }
    }

    #endregion

    #region OPERATIONLOGEMAILLOG

    public interface IOperationLogEMailLogRepository : IRepository<OperationLogEMailLog>
    {
    }

    public class OperationLogEMailLogRepository : RepositoryBase<OperationLogEMailLog>, IOperationLogEMailLogRepository
    {
        public OperationLogEMailLogRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }
    }

    #endregion
}
