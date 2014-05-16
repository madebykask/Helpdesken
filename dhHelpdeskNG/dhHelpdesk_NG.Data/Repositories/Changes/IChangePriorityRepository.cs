namespace DH.Helpdesk.Dal.Repositories.Changes
{
    using System.Collections.Generic;

    using DH.Helpdesk.BusinessData.Models.Shared;
    using DH.Helpdesk.BusinessData.Models.Shared.Output;
    using DH.Helpdesk.Dal.Infrastructure;
    using DH.Helpdesk.Domain.Changes;

    public interface IChangePriorityRepository : IRepository<ChangePriorityEntity>
    {
        List<ItemOverview> FindOverviews(int customerId);

        string GetPriorityName(int priorityId);
    }
}