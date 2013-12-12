namespace dhHelpdesk_NG.Data.Repositories
{
    using System.Collections.Generic;

    using dhHelpdesk_NG.DTO.DTOs.Common.Output;
    using dhHelpdesk_NG.DTO.DTOs.Notifiers.Output;
    using dhHelpdesk_NG.Data.Infrastructure;
    using dhHelpdesk_NG.Domain;

    public interface IOrganizationUnitRepository : IRepository<OU>
    {
        List<ItemOverviewDto> FindActiveAndShowable();
    }
}
