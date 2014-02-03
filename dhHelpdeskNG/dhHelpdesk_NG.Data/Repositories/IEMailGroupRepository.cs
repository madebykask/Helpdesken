namespace dhHelpdesk_NG.Data.Repositories
{
    using System.Collections.Generic;

    using dhHelpdesk_NG.Data.Infrastructure;
    using dhHelpdesk_NG.Domain;
    using dhHelpdesk_NG.DTO.DTOs.Common.Output;

    public interface IEmailGroupRepository : IRepository<EmailGroupEntity>
    {
        List<ItemOverviewDto> FindActiveOverviews(int customerId);
    }
}