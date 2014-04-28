namespace DH.Helpdesk.Dal.Repositories.WorkstationModules
{
    using System.Collections.Generic;

    using DH.Helpdesk.BusinessData.Models.Common.Output;
    using DH.Helpdesk.Dal.Dal;

    public interface INICRepository : INewRepository
    {
        List<ItemOverview> FindOverviews();
    }
}