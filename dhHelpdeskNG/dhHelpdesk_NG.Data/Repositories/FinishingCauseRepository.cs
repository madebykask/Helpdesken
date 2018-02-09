using DH.Helpdesk.Dal.Infrastructure.Helpers;

namespace DH.Helpdesk.Dal.Repositories
{
    using System.Collections.Generic;
    using System.Linq;

    using DH.Helpdesk.BusinessData.Models.FinishingCause;
    using DH.Helpdesk.Dal.Infrastructure;
    using DH.Helpdesk.Domain;

    #region FINISHINGCAUSE

    public interface IFinishingCauseRepository : IRepository<FinishingCause>
    {
        IEnumerable<FinishingCause> GetActiveByCustomer(int customerId);

        List<FinishingCauseOverview> GetFinishingCauseOverviews(int customerId);

        IEnumerable<FinishingCauseInfo> GetFinishingCauseInfos(int customerId);
    }

    public class FinishingCauseRepository : RepositoryBase<FinishingCause>, IFinishingCauseRepository
    {
        public FinishingCauseRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }

        public IEnumerable<FinishingCause> GetActiveByCustomer(int customerId)
        {
            var query = from f in this.DataContext.FinishingCauses
                        where f.Customer_Id == customerId && f.IsActive == 1
                        select f;

            return query.OrderBy(f => f.Name);
        }

        #region CreateFinishingCauseOverviewTree

        public List<FinishingCauseOverview> GetFinishingCauseOverviews(int customerId)
        {
            var causeEntities = 
                this.DataContext.FinishingCauses
                    .Where(c => c.Customer_Id == customerId && c.IsActive == 1)
                    .OrderBy(c => c.Name)
                    .Select(c => new FinishingCauseOverview
                    {
                        Id = c.Id,
                        ParentId = c.Parent_FinishingCause_Id,
                        Name = c.Name,
                        IsActive = c.IsActive
                    }).ToList();

            var parentCauses = causeEntities.Where(c => c.ParentId == null).ToList();

            foreach (var parentCategory in parentCauses)
            {
                CreateFinishingCauseOverviewTree(parentCategory, causeEntities);
            }

            return parentCauses;
        }

        private void CreateFinishingCauseOverviewTree(FinishingCauseOverview parentCategory, IList<FinishingCauseOverview> allCategories)
        {
            var children = allCategories.Where(c => c.ParentId == parentCategory.Id).OrderBy(c => c.Name).ToList();
            if (children.Any())
            {
                parentCategory.ChildFinishingCauses.AddRange(children);
                foreach (var child in children)
                {
                    this.CreateFinishingCauseOverviewTree(child, allCategories);
                }
            }
        }

        #endregion
        
        public IEnumerable<FinishingCauseInfo> GetFinishingCauseInfos(int customerId)
        {
            var entities =
                this.DataContext.FinishingCauses.Where(c => c.Customer_Id == customerId && c.IsActive != 0)
                    .Select(c => new { c.Id, ParentId = c.Parent_FinishingCause_Id, c.Name })
                    .OrderBy(c => c.Name)
                    .ToList();

            return entities
                .Select(c => new FinishingCauseInfo
                {
                    Id = c.Id,
                    ParentId = c.ParentId,
                    Name = c.Name
                });
        }
    }

    #endregion

    #region FINISHINGCAUSECATEGORY

    public interface IFinishingCauseCategoryRepository : IRepository<FinishingCauseCategory>
    {
    }

    public class FinishingCauseCategoryRepository : RepositoryBase<FinishingCauseCategory>, IFinishingCauseCategoryRepository
    {
        public FinishingCauseCategoryRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }
    }

    #endregion
}
