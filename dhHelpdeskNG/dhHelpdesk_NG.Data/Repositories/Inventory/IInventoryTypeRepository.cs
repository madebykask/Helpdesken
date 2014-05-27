namespace DH.Helpdesk.Dal.Repositories.Inventory
{
    using System.Collections.Generic;

    using DH.Helpdesk.BusinessData.Models.Changes;
    using DH.Helpdesk.BusinessData.Models.Inventory;
    using DH.Helpdesk.BusinessData.Models.Inventory.Edit.Inventory;
    using DH.Helpdesk.BusinessData.Models.Shared;
    using DH.Helpdesk.Dal.Dal;

    public interface IInventoryTypeRepository : INewRepository
    {
        void Add(InventoryType businessModel);

        void Delete(int id);

        void Update(InventoryType businessModel);

        InventoryType FindById(int id);

        List<ItemOverview> FindOverviews(int customerId);

        List<InventoryTypeWithInventories> FindInventoryTypesWithInventories(int customerId, int langaugeId);

        List<InventoryReportModel> FindInventoriesWithCount(int customerId, int? departmentId);
    }
}