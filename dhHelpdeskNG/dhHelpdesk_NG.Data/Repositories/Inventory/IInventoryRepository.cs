using DH.Helpdesk.BusinessData.Enums.Inventory;

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
        void Add(InventoryForInsert businessModel);

        void DeleteById(int id);

        void Update(InventoryForUpdate businessModel);

        InventoryForRead FindById(int id);

        List<InventoryOverviewWithType> FindConnectedToComputerInventories(int computerId);

        List<InventoryOverview> FindOverviews(int inventoryTypeId, int? departmentId, string searchString, int pageSize);

        List<ItemOverview> FindNotConnectedOverviews(int inventoryTypeId, int computerId);

        ReportModelWithInventoryType FindAllConnectedInventory(int inventoryTypeId, int? departmentId, string searchFor);

        void DeleteByInventoryTypeId(int inventoryTypeId);

        int GetIdByName(string inventoryName, int inventoryTypeId);
        List<InventorySearchResult> SearchPcNumber(int customerId, string query);
        InventorySearchResult SearchPcNumberByUserId(int customerId, int userId);
        List<int> GetRelatedCaseIds(CurrentModes inventoryType, int inventoryId, int customerId);
    }
}