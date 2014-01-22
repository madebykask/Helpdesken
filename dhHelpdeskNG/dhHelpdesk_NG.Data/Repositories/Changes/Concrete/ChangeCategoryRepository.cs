namespace dhHelpdesk_NG.Data.Repositories.Changes.Concrete
{
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;

    using dhHelpdesk_NG.Data.Infrastructure;
    using dhHelpdesk_NG.Domain;
    using dhHelpdesk_NG.DTO.DTOs.Common.Output;

    public class ChangeCategoryRepository : RepositoryBase<ChangeCategory>, IChangeCategoryRepository
    {
        public ChangeCategoryRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }

        public List<ItemOverviewDto> FindOverviews(int customerId)
        {
            var categories =
                this.DataContext.ChangeCategories.Where(c => c.Customer_Id == customerId)
                    .Select(c => new { c.Name, c.Id })
                    .ToList();

            return
                categories.Select(
                    c => new ItemOverviewDto { Name = c.Name, Value = c.Id.ToString(CultureInfo.InvariantCulture) })
                    .ToList();
        }
    }
}