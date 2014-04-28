namespace DH.Helpdesk.Dal.Repositories.Inventory
{
    using System.Collections.Generic;

    using DH.Helpdesk.BusinessData.Models.Changes;
    using DH.Helpdesk.BusinessData.Models.Common.Output;
    using DH.Helpdesk.BusinessData.Models.Inventory.Edit.Inventory;
    using DH.Helpdesk.Dal.Dal;

    public interface IInventoryTypeRepository : INewRepository
    {
        void Add(InventoryType businessModel);

        void Delete(int id);

        void Update(InventoryType businessModel);

        InventoryType FindById(int id);

        List<ItemOverview> FindOverviews(int customerId);

        List<InventoryTypeWithInventories> FindInventoryTypeWithInventories(int customerId, int langaugeId);
    }
}