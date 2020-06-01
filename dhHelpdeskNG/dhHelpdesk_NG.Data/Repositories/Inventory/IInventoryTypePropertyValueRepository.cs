namespace DH.Helpdesk.Dal.Repositories.Inventory
{
    using System.Collections.Generic;

    using DH.Helpdesk.BusinessData.Models.Inventory;
    using DH.Helpdesk.Dal.Dal;

    public interface IInventoryTypePropertyValueRepository : INewRepository
    {
        void Add(InventoryValueForWrite businessModel);

        void Add(List<InventoryValueForWrite> businessModels);

        void Update(InventoryValueForWrite businessModel);

        void Update(List<InventoryValueForWrite> businessModels);

        List<InventoryValue> GetData(int id);

        List<InventoryValue> GetData(List<int> ids, int? inventoryTypeId = null);

        void DeleteByInventoryTypePropertyId(int inventoryTypePropertyId);

        void DeleteByInventoryTypeId(int inventoryTypeId);

        void DeleteByInventoryId(int inventoryId);
    }
}