namespace DH.Helpdesk.Services.BusinessLogic.BusinessModelValidators.Inventory
{
    using DH.Helpdesk.BusinessData.Models.Inventory.Edit.Inventory;
    using DH.Helpdesk.BusinessData.Models.Inventory.Output.Settings.ProcessingSetttings.InventorySettings;

    public interface IInventoryValidator
    {
        void Validate(
            InventoryForUpdate updatedModel,
            InventoryForRead existingModel,
            InventoryFieldSettingsProcessing settings);

        void Validate(InventoryForInsert newModel, InventoryFieldSettingsProcessing settings);
    }
}