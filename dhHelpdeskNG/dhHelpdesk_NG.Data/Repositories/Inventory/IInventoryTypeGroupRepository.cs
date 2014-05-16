namespace DH.Helpdesk.Dal.Repositories.Inventory
{
    using System.Collections.Generic;

    using DH.Helpdesk.BusinessData.Models.Shared;
    using DH.Helpdesk.BusinessData.Models.Shared.Output;
    using DH.Helpdesk.Dal.Dal;

    public interface IInventoryTypeGroupRepository : INewRepository
    {
        List<ItemOverview> FindOverviews(int inventoryTypeId);
    }
}