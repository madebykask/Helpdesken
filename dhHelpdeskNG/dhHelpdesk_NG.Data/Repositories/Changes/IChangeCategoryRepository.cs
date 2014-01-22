namespace dhHelpdesk_NG.Data.Repositories.Changes
{
    using System.Collections.Generic;

    using dhHelpdesk_NG.Data.Infrastructure;
    using dhHelpdesk_NG.Domain;
    using dhHelpdesk_NG.DTO.DTOs.Common.Output;

    public interface IChangeCategoryRepository : IRepository<ChangeCategory>
    {
        List<ItemOverviewDto> FindOverviews(int customerId);
    }
}