namespace DH.Helpdesk.Dal.Repositories.Notifiers
{
    using System.Collections.Generic;

    using DH.Helpdesk.BusinessData.Models.Shared;
    using DH.Helpdesk.BusinessData.Models.Shared.Output;
    using DH.Helpdesk.Dal.Infrastructure;
    using DH.Helpdesk.Domain;
    using DH.Helpdesk.Domain.Computers;

    public interface INotifierGroupRepository : IRepository<ComputerUserGroup>
    {
        List<ItemOverview> FindOverviewsByCustomerId(int customerId);
    }
}
