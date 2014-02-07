namespace DH.Helpdesk.Dal.Repositories.Changes
{
    using System.Collections.Generic;

    using DH.Helpdesk.BusinessData.Models.Common.Output;
    using DH.Helpdesk.Dal.Infrastructure;
    using DH.Helpdesk.Domain.Changes;

    public interface IChangeCategoryRepository : IRepository<ChangeCategoryEntity>
    {
        List<ItemOverviewDto> FindOverviews(int customerId);
    }
}