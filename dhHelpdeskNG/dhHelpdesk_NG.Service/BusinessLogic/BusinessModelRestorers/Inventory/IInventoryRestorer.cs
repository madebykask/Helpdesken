namespace DH.Helpdesk.Services.BusinessLogic.BusinessModelRestorers.Inventory
{
    using DH.Helpdesk.BusinessData.Models.Inventory.Edit.Inventory;
    using DH.Helpdesk.BusinessData.Models.Inventory.Output.Settings.ProcessingSetttings.InventorySettings;

    public interface IInventoryRestorer
    {
        void Restore(InventoryForUpdate model, InventoryForRead existingModel, InventoryFieldSettingsProcessing settings);
    }
}