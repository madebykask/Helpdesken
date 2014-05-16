namespace DH.Helpdesk.Dal.Repositories.Changes
{
    using System.Collections.Generic;

    using DH.Helpdesk.BusinessData.Models.Shared;
    using DH.Helpdesk.BusinessData.Models.Shared.Output;
    using DH.Helpdesk.Dal.Infrastructure;
    using DH.Helpdesk.Domain.Changes;

    public interface IChangeCategoryRepository : IRepository<ChangeCategoryEntity>
    {
        List<ItemOverview> FindOverviews(int customerId);

        string GetCategoryName(int categoryId);
    }
}