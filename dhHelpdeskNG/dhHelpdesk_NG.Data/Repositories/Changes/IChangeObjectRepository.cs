namespace DH.Helpdesk.Dal.Repositories.Changes
{
    using System.Collections.Generic;

    using DH.Helpdesk.BusinessData.Models.Common.Output;
    using DH.Helpdesk.Dal.Infrastructure;
    using DH.Helpdesk.Domain.Changes;

    public interface IChangeObjectRepository : IRepository<ChangeObjectEntity>
    {
        List<ItemOverview> FindOverviews(int customerId);

        string GetObjectName(int objectId);
    }
}