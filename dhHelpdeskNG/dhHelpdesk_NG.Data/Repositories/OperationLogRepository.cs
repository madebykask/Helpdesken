using System.Collections.Generic;
using System.Linq;
using dhHelpdesk_NG.Data.Infrastructure;
using dhHelpdesk_NG.Domain;
using dhHelpdesk_NG.DTO.DTOs;

namespace dhHelpdesk_NG.Data.Repositories
{
    #region OPERATIONLOG

    public interface IOperationLogRepository : IRepository<OperationLog>
    {
        IList<OperationLogList> ListForIndexPage();
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
                        join olc in this.DataContext.OperationLogCategories on ol.OperationLogCategory_Id equals olc.Id
                        join ob in this.DataContext.OperationObjects on ol.OperationObject_Id equals ob.Id
                        join u in this.DataContext.Users on ol.User_Id equals u.Id
                        group ol by new { ol.Id, olc.OLCName, ob.Name, u.UserID, ol.LogText, ol.LogAction, ol.CreatedDate } into g
                        select new OperationLogList
                        {
                            OperationLogAction = g.Key.LogAction,
                            OperationLogAdmin = g.Key.UserID,
                            CreatedDate = g.Key.CreatedDate,
                            OperationLogCategoryName = g.Key.OLCName,
                            OperationLogDescription = g.Key.LogText,
                            OperationObjectName = g.Key.Name,
                            Id = g.Key.Id
                        };

            return query.ToList();
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
