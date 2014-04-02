namespace DH.Helpdesk.Dal.Repositories.Inventory
{
    using DH.Helpdesk.BusinessData.Models.Inventory.Edit.Settings.InventorySettings;
    using DH.Helpdesk.BusinessData.Models.Inventory.Output.Settings.ModelEdit.InventorySettings;
    using DH.Helpdesk.BusinessData.Models.Inventory.Output.Settings.ModelOverview.InventoryFieldSettings;
    using DH.Helpdesk.Dal.Dal;

    public interface IInventoryFieldSettingsRepository : INewRepository
    {
        void Update(InventoryFieldSettings businessModel);

        InventoryFieldSettings GetFieldSettingsForEdit(int customerId, int inventoryTypeId);

        InventoryFieldSettingsForModelEdit GetFieldSettingsForModelEdit(int customerId, int inventoryTypeId);

        InventoryFieldSettingsOverview GetFieldSettingsOverview(int customerId, int inventoryTypeId);
    }
}