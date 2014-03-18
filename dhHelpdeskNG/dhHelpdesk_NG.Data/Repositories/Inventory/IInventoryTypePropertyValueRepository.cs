namespace DH.Helpdesk.Dal.Repositories.Inventory
{
    using DH.Helpdesk.BusinessData.Models.Inventory.Edit.Settings.InventorySettings;
    using DH.Helpdesk.Dal.Dal;

    public interface IInventoryTypePropertyValueRepository : INewRepository
    {
        void Add(InventorySettings businessModel);

        void Update(InventorySettings businessModel);
    }
}