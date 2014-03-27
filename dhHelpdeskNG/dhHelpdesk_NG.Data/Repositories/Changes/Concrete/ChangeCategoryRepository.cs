namespace DH.Helpdesk.Dal.Repositories.Changes.Concrete
{
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;

    using DH.Helpdesk.BusinessData.Models.Common.Output;
    using DH.Helpdesk.Dal.Infrastructure;
    using DH.Helpdesk.Domain.Changes;

    public sealed class ChangeCategoryRepository : RepositoryBase<ChangeCategoryEntity>, IChangeCategoryRepository
    {
        public ChangeCategoryRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }

        public List<ItemOverview> FindOverviews(int customerId)
        {
            var categories =
                this.DataContext.ChangeCategories.Where(c => c.Customer_Id == customerId)
                    .Select(c => new { c.Id, c.Name })
                    .ToList();

            return
                categories.Select(c => new ItemOverview(c.Name, c.Id.ToString(CultureInfo.InvariantCulture))).ToList();
        }

        public string GetCategoryName(int categoryId)
        {
            return this.DataContext.ChangeCategories.Where(c => c.Id == categoryId).Select(c => c.Name).Single();
        }
    }
}