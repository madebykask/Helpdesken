namespace DH.Helpdesk.Dal.Repositories.Inventory
{
    using System.Collections.Generic;

    using DH.Helpdesk.BusinessData.Models.Inventory.Edit.Inventory;
    using DH.Helpdesk.Dal.Dal;

    public interface IInventoryTypeGroupRepository : INewRepository
    {
        List<TypeGroupModel> Find(int inventoryTypeId);

        void DeleteByInventoryTypeId(int inventoryTypeId);
    }
}