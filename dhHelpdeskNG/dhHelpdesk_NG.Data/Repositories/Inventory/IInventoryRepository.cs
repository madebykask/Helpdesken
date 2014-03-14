namespace DH.Helpdesk.Dal.Repositories.Inventory
{
    using System.Collections.Generic;

    using DH.Helpdesk.BusinessData.Models.Inventory.Edit.Inventory;
    using DH.Helpdesk.BusinessData.Models.Inventory.Output;
    using DH.Helpdesk.Dal.Dal;

    public interface IInventoryRepository : INewRepository
    {
        void Add(Inventory businessModel);

        void Delete(int id);

        void Update(Inventory businessModel);

        Inventory FindById(int id);

        List<InventoryOverview> FindOverviews(int customerId, int inventoryTypeId, string searchString, int pageSize);
    }
}