namespace DH.Helpdesk.Dal.Repositories
{
    using System.Collections.Generic;

    using DH.Helpdesk.BusinessData.Models.Shared.Output;
    using DH.Helpdesk.Dal.Infrastructure;
    using DH.Helpdesk.Domain;

    public interface IEmailGroupRepository : IRepository<EmailGroupEntity>
    {
        List<IdAndNameOverview> FindActiveIdAndNameOverviews(int customerId);
    }
}