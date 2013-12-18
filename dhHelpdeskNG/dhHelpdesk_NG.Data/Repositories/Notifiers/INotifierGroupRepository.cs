namespace dhHelpdesk_NG.Data.Repositories.Notifiers
{
    using System.Collections.Generic;

    using dhHelpdesk_NG.DTO.DTOs.Common.Output;
    using dhHelpdesk_NG.Data.Infrastructure;
    using dhHelpdesk_NG.Domain;

    public interface INotifierGroupRepository : IRepository<ComputerUserGroup>
    {
        List<ItemOverviewDto> FindOverviewsByCustomerId(int customerId);
    }
}
