namespace DH.Helpdesk.Dal.Repositories.Changes
{
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;

    using DH.Helpdesk.BusinessData.Models.Shared;
    using DH.Helpdesk.BusinessData.Models.Shared.Output;
    using DH.Helpdesk.Dal.Infrastructure;
    using DH.Helpdesk.Domain.Changes;

    public interface IChangeStatusRepository : IRepository<ChangeStatusEntity>
    {
        List<ItemOverview> FindOverviews(int customerId);

        string GetStatusName(int statusId);

        void ResetDefault(int exclude);
    }

}