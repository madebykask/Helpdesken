namespace dhHelpdesk_NG.Data.Repositories.Notifiers
{
    using System.Collections.Generic;

    using dhHelpdesk_NG.DTO.DTOs.Notifiers.Output;
    using dhHelpdesk_NG.Data.Infrastructure;
    using dhHelpdesk_NG.Domain;

    public interface INotifierGroupsRepository : IRepository<ComputerUserGroup>
    {
        List<NotifierGroupOverviewDto> FindOverviewByCustomerId(int customerId);
    }
}
