namespace DH.Helpdesk.Web.Infrastructure.ModelFactories.Inventory
{
    using DH.Helpdesk.BusinessData.Models.Inventory.Output.Settings.ModelEdit.InventorySettings;
    using DH.Helpdesk.Services.Response.Inventory;
    using DH.Helpdesk.Web.Models.Inventory.EditModel.Inventory;

    public interface IInventoryViewModelBuilder
    {
        InventoryViewModel BuildViewModel(
            BusinessData.Models.Inventory.Edit.Inventory.Inventory model,
            InventoryEditOptionsResponse options,
            InventoryFieldSettingsForModelEdit settings);

        InventoryViewModel BuildViewModel(
            InventoryEditOptionsResponse options,
            InventoryFieldSettingsForModelEdit settings,
            int inventoryTypeId,
            int currentCustomerId);
    }
}