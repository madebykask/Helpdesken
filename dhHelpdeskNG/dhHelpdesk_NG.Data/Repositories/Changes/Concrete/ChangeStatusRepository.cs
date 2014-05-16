namespace DH.Helpdesk.Dal.Repositories.Changes.Concrete
{
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;

    using DH.Helpdesk.BusinessData.Models.Shared;
    using DH.Helpdesk.BusinessData.Models.Shared.Output;
    using DH.Helpdesk.Dal.Infrastructure;
    using DH.Helpdesk.Domain.Changes;

    public sealed class ChangeStatusRepository : RepositoryBase<ChangeStatusEntity>, IChangeStatusRepository
    {
        public ChangeStatusRepository(IDatabaseFactory databaseFactory)
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

        public string GetStatusName(int statusId)
        {
            return this.DataContext.ChangeStatuses.Where(s => s.Id == statusId).Select(s => s.ChangeStatus).Single();
        }

        public void ResetDefault(int exclude)
        {
            foreach (ChangeStatusEntity obj in this.GetMany(s => s.isDefault == 1 && s.Id != exclude))
            {
                obj.isDefault = 0;
                this.Update(obj);
            }
        }
    }
}