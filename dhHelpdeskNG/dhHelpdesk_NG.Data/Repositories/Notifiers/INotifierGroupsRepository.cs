namespace dhHelpdesk_NG.Data.Repositories.Notifiers
{
    using System.Collections.Generic;

    using dhHelpdesk_NG.DTO.DTOs.Common.Output;
    using dhHelpdesk_NG.DTO.DTOs.Notifiers.Output;
    using dhHelpdesk_NG.Data.Infrastructure;
    using dhHelpdesk_NG.Domain;

    public interface INotifierGroupsRepository : IRepository<ComputerUserGroup>
    {
        List<ItemOverviewDto> FindOverviewByCustomerId(int customerId);
    }
}
