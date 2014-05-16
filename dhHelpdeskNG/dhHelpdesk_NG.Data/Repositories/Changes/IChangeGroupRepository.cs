namespace DH.Helpdesk.Dal.Repositories.Changes
{
    using System.Collections.Generic;

    using DH.Helpdesk.BusinessData.Models.Shared;
    using DH.Helpdesk.BusinessData.Models.Shared.Output;
    using DH.Helpdesk.Dal.Infrastructure;
    using DH.Helpdesk.Domain.Changes;

    public interface IChangeGroupRepository : IRepository<ChangeGroupEntity>
    {
        List<ItemOverview> FindOverviews(int customerId);

        string GetChangeGroupName(int changeGroupId);
    }
}