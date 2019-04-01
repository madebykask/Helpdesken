// --------------------------------------------------------------------------------------------------------------------
// <copyright file="OperationLogRepository.cs" company="">
//   
// </copyright>
// <summary>
//   Defines the IOperationLogRepository type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace DH.Helpdesk.Dal.Repositories
{
    using System.Collections.Generic;
    using System.Linq;

    using DH.Helpdesk.BusinessData.Models.Inventory.Output;
    using DH.Helpdesk.Common.Types;
    using DH.Helpdesk.Dal.Infrastructure;
    using DH.Helpdesk.Dal.Infrastructure.Context;
    using DH.Helpdesk.Domain;

    #region OPERATIONLOG

    public interface IOperationLogRepository : IRepository<OperationLog>
    {
       List<OperationServerLogOverview> GetOperationServerLogOverviews(int customerId, int serverId);

        void DeleteByOperationObjectId(int operationObjectId);

        List<int> FindOperationObjectId(int operationObjectId);

        int GetOperationLogId();
    }

    /// <summary>
    /// The operation log repository.
    /// </summary>
    public class OperationLogRepository : RepositoryBase<OperationLog>, IOperationLogRepository
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="OperationLogRepository"/> class.
        /// </summary>
        /// <param name="databaseFactory">
        /// The database factory.
        /// </param>
        /// <param name="workContext">
        /// The work context.
        /// </param>
        public OperationLogRepository(
            IDatabaseFactory databaseFactory,
            IWorkContext workContext)
            : base(databaseFactory)
        {
        }

        public List<OperationServerLogOverview> GetOperationServerLogOverviews(int customerId, int serverId)
        {
            var query = from obj in DataContext.OperationObjects
                        from server in DataContext.Servers
                        from log in this.Table
                        where
                            server.Id == serverId && obj.Name == server.ServerName && log.OperationObject_Id == obj.Id
                            && log.Customer_Id == customerId
                        select
                            new
                                {
                                    obj.Description,
                                    log.CreatedDate,
                                    log.Admin.FirstName,
                                    log.Admin.SurName,
                                    log.LogAction
                                };

            var anonymus = query.ToList();

            var overviews =
                anonymus.Select(
                    x =>
                    new OperationServerLogOverview(
                        new UserName(x.FirstName, x.SurName),
                        x.CreatedDate,
                        x.Description,
                        x.LogAction)).ToList();

            return overviews;
        }

        public void DeleteByOperationObjectId(int operationObjectId)
        {
            var entities = this.Table.Where(x => x.OperationObject_Id == operationObjectId).ToList();
            entities.ForEach(
                x =>
                {
                    x.Us.Clear();
                    x.WGs.Clear();
                    this.Table.Remove(x);
                });
        }

        public List<int> FindOperationObjectId(int operationObjectId)
        {
            var entities =
                this.Table.Where(x => x.OperationObject_Id == operationObjectId)
                    .Select(x => x.Id)
                    .ToList();
            return entities;
        }

        public int GetOperationLogId()
        {
            var Ids = (from op in this.DataContext.OperationLogs select op.Id).ToList();

            if (Ids.Any())
                return Ids.Max();
            else
                return 0;
        }
    }

    #endregion

    #region OPERATIONLOGCATEGORY

    /// <summary>
    /// The OperationLogCategoryRepository interface.
    /// </summary>
    public interface IOperationLogCategoryRepository : IRepository<OperationLogCategory>
    {
    }

    /// <summary>
    /// The operation log category repository.
    /// </summary>
    public class OperationLogCategoryRepository : RepositoryBase<OperationLogCategory>, IOperationLogCategoryRepository
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="OperationLogCategoryRepository"/> class.
        /// </summary>
        /// <param name="databaseFactory">
        /// The database factory.
        /// </param>
        public OperationLogCategoryRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }
    }

    #endregion

    public interface IOperationLogEMailLogRepository : IRepository<OperationLogEMailLog>
    {
        void DeleteByOperationLogIds(List<int> operationLogId);
    }

    public class OperationLogEMailLogRepository : RepositoryBase<OperationLogEMailLog>, IOperationLogEMailLogRepository
    {
        public OperationLogEMailLogRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }

        public void DeleteByOperationLogIds(List<int> operationLogId)
        {
            var entities = this.Table.Where(x => operationLogId.Contains(x.OperationLog_Id.Value)).ToList();
            entities.ForEach(x => this.Table.Remove(x));
        }
    }
}
