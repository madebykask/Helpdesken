namespace dhHelpdesk_NG.Data.Repositories
{
    using System.Collections.Generic;
    using System.Linq;

    using dhHelpdesk_NG.Data.Infrastructure;
    using dhHelpdesk_NG.Domain;
    using dhHelpdesk_NG.DTO.DTOs.FinishingCause;
    using dhHelpdesk_NG.DTO.DTOs.Problem.Output;

    #region FINISHINGCAUSE

    public interface IFinishingCauseRepository : IRepository<FinishingCause>
    {
        IEnumerable<FinishingCause> GetActiveByCustomer(int customerId);

        List<FinishingCauseOverview> GetFinishingCauseOverviews(int customerId);
    }

    public class FinishingCauseRepository : RepositoryBase<FinishingCause>, IFinishingCauseRepository
    {
        public FinishingCauseRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }

        public IEnumerable<FinishingCause> GetActiveByCustomer(int customerId)
        {
            var query = from f in DataContext.FinishingCauses
                        where f.Customer_Id == customerId && f.IsActive == 1
                        select f;

            return query.OrderBy(f => f.Name);
        }

        public List<FinishingCauseOverview> GetFinishingCauseOverviews(int customerId)
        {
            var causeEntities = this.DataContext.FinishingCauses.Where(c => c.Customer_Id == customerId).ToList();
            var parentCauses = causeEntities.Where(c => c.Parent_FinishingCause_Id == null).ToList();
            var categories = new List<FinishingCauseOverview>(parentCauses.Count);

            foreach (var parentCategory in parentCauses)
            {
                var category = this.CreateBrunchForParent(parentCategory, causeEntities);
                categories.Add(category);
            }

            return categories;
        }

        private FinishingCauseOverview CreateBrunchForParent(FinishingCause parentCategory, IList<FinishingCause> allCategories)
        {
            var cause = new FinishingCauseOverview { Id = parentCategory.Id, Name = parentCategory.Name };

            var subCauseEntities = allCategories.Where(c => c.Parent_FinishingCause_Id == parentCategory.Id).ToList();
            if (subCauseEntities.Any())
            {
                var subCauses =
                    subCauseEntities.Select(c => this.CreateBrunchForParent(c, allCategories)).ToList();

                cause.ChildFinishingCauses.AddRange(subCauses);
            }

            return cause;
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
