namespace dhHelpdesk_NG.Data.Repositories.Changes
{
    using System.Collections.Generic;

    using dhHelpdesk_NG.DTO.DTOs.Common.Output;
    using dhHelpdesk_NG.Data.Infrastructure;
    using dhHelpdesk_NG.Domain;

    public interface IChangeStatusRepository : IRepository<ChangeStatus>
    {
        List<ItemOverviewDto> FindOverviewsByCustomerId(int customerId);
    }
}