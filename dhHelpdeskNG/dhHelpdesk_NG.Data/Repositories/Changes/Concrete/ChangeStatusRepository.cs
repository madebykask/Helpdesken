namespace dhHelpdesk_NG.Data.Repositories.Changes.Concrete
{
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;

    using dhHelpdesk_NG.DTO.DTOs.Common.Output;
    using dhHelpdesk_NG.Data.Infrastructure;
    using dhHelpdesk_NG.Domain;

    public class ChangeStatusRepository : RepositoryBase<ChangeStatus>, IChangeStatusRepository
    {
        public ChangeStatusRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }

        public List<ItemOverviewDto> FindOverviews(int customerId)
        {
            var statuses = this.DataContext.Statuses.Where(s => s.Customer_Id == customerId);
            var overviews = statuses.Select(s => new { Name = s.Name, Value = s.Id }).ToList();

            return
                overviews.Select(
                    o => new ItemOverviewDto { Name = o.Name, Value = o.Value.ToString(CultureInfo.InvariantCulture) })
                         .ToList();
        }
    }
}