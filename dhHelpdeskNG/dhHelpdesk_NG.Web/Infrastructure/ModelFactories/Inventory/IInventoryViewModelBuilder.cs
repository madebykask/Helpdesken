namespace DH.Helpdesk.Web.Infrastructure.ModelFactories.Inventory
{
    using DH.Helpdesk.BusinessData.Models.Inventory.Output.Settings.ModelEdit.InventorySettings;
    using DH.Helpdesk.Web.Models.Inventory.EditModel.Inventory;
    using DH.Helpdesk.Web.Models.Inventory.OptionsAggregates;

    public interface IInventoryViewModelBuilder
    {
        InventoryViewModel BuildViewModel(
            BusinessData.Models.Inventory.Edit.Inventory.InventoryForRead model,
            InventoryEditOptions options,
            InventoryFieldSettingsForModelEdit settings);

        InventoryViewModel BuildViewModel(
            InventoryEditOptions options,
            InventoryFieldSettingsForModelEdit settings,
            int inventoryTypeId,
            int currentCustomerId);
    }
}