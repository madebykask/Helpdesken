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
    using System.Security.Cryptography;

    using DH.Helpdesk.BusinessData.Models;
    using DH.Helpdesk.BusinessData.Models.Inventory.Edit.Server;
    using DH.Helpdesk.BusinessData.Models.Inventory.Output;
    using DH.Helpdesk.BusinessData.Models.OperationLog.Output;
    using DH.Helpdesk.Common.Extensions.Integer;
    using DH.Helpdesk.Common.Types;
    using DH.Helpdesk.Dal.Infrastructure;
    using DH.Helpdesk.Dal.Infrastructure.Context;
    using DH.Helpdesk.Domain;

    #region OPERATIONLOG

    /// <summary>
    /// The OperationLogRepository interface.
    /// </summary>
    public interface IOperationLogRepository : IRepository<OperationLog>
    {
        /// <summary>
        /// The list for index page.
        /// </summary>
        /// <returns>
        /// The result.
        /// </returns>
        IList<OperationLogList> ListForIndexPage();

        /// <summary>
        /// The get operation log overviews.
        /// </summary>
        /// <param name="customers">
        /// The customers.
        /// </param>
        /// <returns>
        /// The result.
        /// </returns>
        IEnumerable<OperationLogOverview> GetOperationLogOverviews(int[] customers);

        List<OperationServerLogOverview> GetOperationServerLogOverviews(int customerId, int serverId);
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
            : base(databaseFactory, workContext)
        {
        }

        /// <summary>
        /// The list for index page.
        /// </summary>
        /// <returns>
        /// The result.
        /// </returns>
        public IList<OperationLogList> ListForIndexPage()
        {
            var query = from ol in this.Table
                        join olc in this.DataContext.OperationLogCategories on ol.OperationLogCategory_Id equals olc.Id into gj
                        from x in gj.DefaultIfEmpty()
                        join ob in this.DataContext.OperationObjects on ol.OperationObject_Id equals ob.Id
                        join u in this.DataContext.Users on ol.User_Id equals u.Id
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

        /// <summary>
        /// The get operation log overviews.
        /// </summary>
        /// <param name="customers">
        /// The customers.
        /// </param>
        /// <returns>
        /// The result.
        /// </returns>
        public IEnumerable<OperationLogOverview> GetOperationLogOverviews(int[] customers)
        {
            return new List<OperationLogOverview>();
            var entities = this.GetSecuredEntities(this.Table
                .Where(o => customers.Contains(o.Customer_Id))
                .Select(o => new
                {
                    o.Customer_Id,
                    o.ChangedDate,
                    o.CreatedDate,
                    o.LogText,
                    o.Category,
                    o.ShowOnStartPage,
                    o.Us,
                    o.WGs
                })
                .OrderByDescending(p => p.CreatedDate)
                .ToList()
                .Select(o => new OperationLog
                {
                    Customer_Id = o.Customer_Id,
                    ChangedDate = o.ChangedDate,
                    CreatedDate = o.CreatedDate,
                    LogText = o.LogText,
                    Category = o.Category,
                    ShowOnStartPage = o.ShowOnStartPage,
                    Us = o.Us,
                    WGs = o.WGs
                })); 

            return entities.Select(o => new OperationLogOverview()
                {
                    CustomerId = o.Customer_Id,
                    ChangedDate = o.ChangedDate,
                    CreatedDate = o.CreatedDate,
                    LogText = o.LogText,
                    Category = new OperationLogCategoryOverview()
                    {
                        OLCName = o.Category != null ? o.Category.OLCName : null
                    },
                    ShowOnStartPage = o.ShowOnStartPage.ToBool()
                });
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

    #region OPERATIONLOGEMAILLOG

    /// <summary>
    /// The OperationLogEMailLogRepository interface.
    /// </summary>
    public interface IOperationLogEMailLogRepository : IRepository<OperationLogEMailLog>
    {
    }

    /// <summary>
    /// The operation log e mail log repository.
    /// </summary>
    public class OperationLogEMailLogRepository : RepositoryBase<OperationLogEMailLog>, IOperationLogEMailLogRepository
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="OperationLogEMailLogRepository"/> class.
        /// </summary>
        /// <param name="databaseFactory">
        /// The database factory.
        /// </param>
        public OperationLogEMailLogRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }
    }

    #endregion
}
