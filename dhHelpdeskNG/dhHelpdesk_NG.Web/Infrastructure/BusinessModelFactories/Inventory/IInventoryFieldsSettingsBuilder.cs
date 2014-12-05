namespace DH.Helpdesk.Web.Infrastructure.BusinessModelFactories.Inventory
{
    using DH.Helpdesk.BusinessData.Models.Inventory.Edit.Settings.InventorySettings;
    using DH.Helpdesk.Web.Areas.Inventory.Models.EditModel.Settings.Inventory;

    public interface IInventoryFieldsSettingsBuilder
    {
        InventoryFieldSettings BuildBusinessModelForUpdate(int inventoryTypeId, DefaultFieldSettingsModel defaultFieldSettingsModel);

        InventoryFieldSettings BuildBusinessModelForAdd(int inventoryTypeId, DefaultFieldSettingsModel defaultFieldSettingsModel);
    }
}