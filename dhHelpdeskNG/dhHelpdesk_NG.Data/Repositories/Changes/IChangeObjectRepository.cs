namespace dhHelpdesk_NG.Data.Repositories.Changes
{
    using System.Collections.Generic;

    using dhHelpdesk_NG.DTO.DTOs.Common.Output;
    using dhHelpdesk_NG.Data.Infrastructure;
    using dhHelpdesk_NG.Domain;

    public interface IChangeObjectRepository : IRepository<ChangeObject>
    {
        List<ItemOverviewDto> FindOverviews(int customerId);
    }
}