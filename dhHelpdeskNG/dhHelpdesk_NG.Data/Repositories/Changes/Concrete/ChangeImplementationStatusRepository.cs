namespace DH.Helpdesk.Dal.Repositories.Changes.Concrete
{
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;

    using DH.Helpdesk.BusinessData.Models.Shared;
    using DH.Helpdesk.Dal.Infrastructure;
    using DH.Helpdesk.Domain.Changes;

    public sealed class ChangeImplementationStatusRepository : RepositoryBase<ChangeImplementationStatusEntity>,
        IChangeImplementationStatusRepository
    {
        public ChangeImplementationStatusRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }

        public List<ItemOverview> FindOverviews(int customerId)
        {
            var statuses =
                this.DataContext.ChangeImplementationStatuses.Where(s => s.Customer_Id == customerId)
                    .Select(s => new { s.Id, s.ImplementationStatus })
                    .ToList();

            return
                statuses.Select(
                    s => new ItemOverview(s.ImplementationStatus, s.Id.ToString(CultureInfo.InvariantCulture))).ToList();
        }

        public string GetStatusName(int statusId)
        {
            return
                this.DataContext.ChangeImplementationStatuses.Where(s => s.Id == statusId)
                    .Select(s => s.ImplementationStatus)
                    .FirstOrDefault();
        }
    }
}