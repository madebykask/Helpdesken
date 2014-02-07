namespace DH.Helpdesk.Dal.Repositories.Changes.Concrete
{
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;

    using DH.Helpdesk.BusinessData.Models.Common.Output;
    using DH.Helpdesk.Dal.Infrastructure;
    using DH.Helpdesk.Domain.Changes;

    public class ChangeStatusRepository : RepositoryBase<ChangeStatusEntity>, IChangeStatusRepository
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