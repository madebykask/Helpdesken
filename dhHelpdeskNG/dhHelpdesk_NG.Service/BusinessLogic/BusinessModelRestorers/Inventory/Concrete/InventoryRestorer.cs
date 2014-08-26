namespace DH.Helpdesk.Services.BusinessLogic.BusinessModelRestorers.Inventory.Concrete
{
    using DH.Helpdesk.BusinessData.Models.Inventory.Edit.Inventory;
    using DH.Helpdesk.BusinessData.Models.Inventory.Output.Settings.ProcessingSetttings.InventorySettings;

    public class InventoryRestorer : Restorer, IInventoryRestorer
    {
        public void Restore(
            InventoryForUpdate model,
            InventoryForRead existingModel,
            InventoryFieldSettingsProcessing settings)
        {
            this.RestoreFieldIfNeeded(
                model,
                () => model.DepartmentId,
                existingModel.DepartmentId,
                this.CreateValidationRule(settings.DefaultSettings.DepartmentFieldSetting));

            this.RestoreFieldIfNeeded(
                model,
                () => model.Name,
                existingModel.Name,
                this.CreateValidationRule(settings.DefaultSettings.NameFieldSetting));
            this.RestoreFieldIfNeeded(
                model,
                () => model.Model,
                existingModel.Model,
                this.CreateValidationRule(settings.DefaultSettings.ModelFieldSetting));
            this.RestoreFieldIfNeeded(
                model,
                () => model.Manufacturer,
                existingModel.Manufacturer,
                this.CreateValidationRule(settings.DefaultSettings.ManufacturerFieldSetting));
            this.RestoreFieldIfNeeded(
                model,
                () => model.SerialNumber,
                existingModel.SerialNumber,
                this.CreateValidationRule(settings.DefaultSettings.SerialNumberFieldSetting));
            this.RestoreFieldIfNeeded(
                model,
                () => model.TheftMark,
                existingModel.TheftMark,
                this.CreateValidationRule(settings.DefaultSettings.TheftMarkFieldSetting));
            this.RestoreFieldIfNeeded(
                model,
                () => model.BarCode,
                existingModel.BarCode,
                this.CreateValidationRule(settings.DefaultSettings.BarCodeFieldSetting));
            this.RestoreFieldIfNeeded(
                model,
                () => model.PurchaseDate,
                existingModel.PurchaseDate,
                this.CreateValidationRule(settings.DefaultSettings.PurchaseDateFieldSetting));
            this.RestoreFieldIfNeeded(
                model,
                () => model.RoomId,
                existingModel.RoomId,
                this.CreateValidationRule(settings.DefaultSettings.PlaceFieldSetting));
            this.RestoreFieldIfNeeded(
                model,
                () => model.Info,
                existingModel.Info,
                this.CreateValidationRule(settings.DefaultSettings.InfoFieldSetting));
        }

        private bool CreateValidationRule(InventoryFieldSettingForProcessing setting)
        {
            return setting.ShowInDetails;
        }
    }
}