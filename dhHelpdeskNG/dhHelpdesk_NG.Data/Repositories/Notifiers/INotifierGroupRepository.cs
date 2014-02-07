namespace DH.Helpdesk.Dal.Repositories.Notifiers
{
    using System.Collections.Generic;

    using DH.Helpdesk.BusinessData.Models.Common.Output;
    using DH.Helpdesk.Dal.Infrastructure;
    using DH.Helpdesk.Domain;

    public interface INotifierGroupRepository : IRepository<ComputerUserGroup>
    {
        List<ItemOverviewDto> FindOverviewsByCustomerId(int customerId);
    }
}
