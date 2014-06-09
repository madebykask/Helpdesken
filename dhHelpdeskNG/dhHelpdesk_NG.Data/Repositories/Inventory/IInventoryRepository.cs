namespace DH.Helpdesk.Dal.Repositories.Inventory
{
    using System.Collections.Generic;

    using DH.Helpdesk.BusinessData.Models.Inventory;
    using DH.Helpdesk.BusinessData.Models.Inventory.Edit.Inventory;
    using DH.Helpdesk.BusinessData.Models.Inventory.Output;
    using DH.Helpdesk.BusinessData.Models.Shared;
    using DH.Helpdesk.Dal.Dal;

    public interface IInventoryRepository : INewRepository
    {
        void Add(Inventory businessModel);

        void DeleteById(int id);

        void Update(Inventory businessModel);

        Inventory FindById(int id);

        List<InventoryOverviewWithType> FindConnectedToComputerInventories(int computerId);

        List<InventoryOverview> FindOverviews(int inventoryTypeId, int? departmentId, string searchString, int pageSize);

        List<ItemOverview> FindNotConnectedOverviews(int inventoryTypeId, int computerId);

        ReportModelWithInventoryType FindAllConnectedInventory(int inventoryTypeId, int? departmentId, string searchFor);

        void DeleteByInventoryTypeId(int inventoryTypeId);
    }
}