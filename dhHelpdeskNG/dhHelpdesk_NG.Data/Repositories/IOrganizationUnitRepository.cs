namespace DH.Helpdesk.Dal.Repositories
{
    using System.Collections.Generic;

    using DH.Helpdesk.BusinessData.Models.Common.Output;
    using DH.Helpdesk.Dal.Infrastructure;
    using DH.Helpdesk.Domain;

    public interface IOrganizationUnitRepository : IRepository<OU>
    {
        List<ItemOverviewDto> FindActiveAndShowable();
    }
}
