namespace DH.Helpdesk.Dal.Repositories.Changes
{
    using System.Collections.Generic;
    using DH.Helpdesk.BusinessData.Models.Shared;
    using DH.Helpdesk.Dal.Infrastructure;
    using DH.Helpdesk.Domain.Changes;

    public interface IChangeStatusRepository : IRepository<ChangeStatusEntity>
    {
        List<ItemOverview> FindOverviews(int customerId);

        string GetStatusName(int statusId);

        void ResetDefault(int exclude);

        IList<ChangeStatusEntity> GetChangeStatuses(int customerId);
    }
}