namespace DH.Helpdesk.Dal.Repositories.Changes.Concrete
{
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;

    using DH.Helpdesk.BusinessData.Models.Common.Output;
    using DH.Helpdesk.Dal.Infrastructure;
    using DH.Helpdesk.Domain.Changes;

    public sealed class ChangeImplementationStatusRepository : RepositoryBase<ChangeImplementationStatusEntity>, IChangeImplementationStatusRepository
    {
        public ChangeImplementationStatusRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }

        public List<ItemOverview> FindOverviews(int customerId)
        {
            var statuses =
                this.DataContext.ChangeStatuses.Where(s => s.Customer_Id == customerId)
                    .Select(s => new { s.Id, s.ChangeStatus })
                    .ToList();

            return
                statuses.Select(s => new ItemOverview(s.ChangeStatus, s.Id.ToString(CultureInfo.InvariantCulture)))
                    .ToList();
        }
    }
}