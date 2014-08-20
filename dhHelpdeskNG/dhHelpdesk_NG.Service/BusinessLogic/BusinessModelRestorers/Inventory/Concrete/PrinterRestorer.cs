namespace DH.Helpdesk.Services.BusinessLogic.BusinessModelRestorers.Inventory.Concrete
{
    using DH.Helpdesk.BusinessData.Models.Inventory.Edit.Printer;
    using DH.Helpdesk.BusinessData.Models.Inventory.Edit.Shared;
    using DH.Helpdesk.BusinessData.Models.Inventory.Output.Settings.ProcessingSetttings;
    using DH.Helpdesk.BusinessData.Models.Inventory.Output.Settings.ProcessingSetttings.PrinterSettings;
    using DH.Helpdesk.BusinessData.Models.Inventory.Output.Settings.ProcessingSetttings.SharedSettings;

    public class PrinterRestorer : Restorer, IPrinterRestorer
    {
        public void Restore(Printer printer, Printer existingPrinter, PrinterFieldsSettingsProcessing settings)
        {
            this.RestoreGeneral(printer.GeneralFields, existingPrinter.GeneralFields, settings.GeneralFieldsSettings);
            this.RestoreInventoring(
                printer.InventoryFields,
                existingPrinter.InventoryFields,
                settings.InventoryFieldsSettings);
            this.RestoreCommunication(
                printer.CommunicationFields,
                existingPrinter.CommunicationFields,
                settings.CommunicationFieldsSettings);
            this.RestoreOther(printer.OtherFields, existingPrinter.OtherFields, settings.OtherFieldsSettings);
            this.RestorePlace(printer.PlaceFields, existingPrinter.PlaceFields, settings.PlaceFieldsSettings);
            this.RestoreOrganization(
                printer.OrganizationFields,
                existingPrinter.OrganizationFields,
                settings.OrganizationFieldsSettings);
        }

        private void RestoreGeneral(
            GeneralFields updated,
            GeneralFields existing,
            GeneralFieldsSettings updatedSettings)
        {
            this.RestoreFieldIfNeeded(
                updated,
                () => updated.Name,
                existing.Name,
                this.CreateValidationRule(updatedSettings.NameFieldSetting));
            this.RestoreFieldIfNeeded(
                updated,
                () => updated.Manufacturer,
                existing.Manufacturer,
                this.CreateValidationRule(updatedSettings.ManufacturerFieldSetting));
            this.RestoreFieldIfNeeded(
                updated,
                () => updated.Model,
                existing.Model,
                this.CreateValidationRule(updatedSettings.ModelFieldSetting));
            this.RestoreFieldIfNeeded(
                updated,
                () => updated.SerialNumber,
                existing.SerialNumber,
                this.CreateValidationRule(updatedSettings.SerialNumberFieldSetting));
        }

        private void RestoreInventoring(
            InventoryFields updated,
            InventoryFields existing,
            InventoryFieldsSettings updatedSettings)
        {
            this.RestoreFieldIfNeeded(
                updated,
                () => updated.BarCode,
                existing.BarCode,
                this.CreateValidationRule(updatedSettings.BarCodeFieldSetting));
            this.RestoreFieldIfNeeded(
                updated,
                () => updated.PurchaseDate,
                existing.PurchaseDate,
                this.CreateValidationRule(updatedSettings.PurchaseDateFieldSetting));
        }

        private void RestoreCommunication(
            CommunicationFields updated,
            CommunicationFields existing,
            CommunicationFieldsSettings updatedSettings)
        {
            this.RestoreFieldIfNeeded(
                updated,
                () => updated.NetworkAdapterName,
                existing.NetworkAdapterName,
                this.CreateValidationRule(updatedSettings.NetworkAdapterFieldSetting));
            this.RestoreFieldIfNeeded(
                updated,
                () => updated.IPAddress,
                existing.IPAddress,
                this.CreateValidationRule(updatedSettings.IPAddressFieldSetting));
            this.RestoreFieldIfNeeded(
                updated,
                () => updated.MacAddress,
                existing.MacAddress,
                this.CreateValidationRule(updatedSettings.MacAddressFieldSetting));
        }

        private void RestoreOther(OtherFields updated, OtherFields existing, OtherFieldsSettings updatedSettings)
        {
            this.RestoreFieldIfNeeded(
                updated,
                () => updated.Info,
                existing.Info,
                this.CreateValidationRule(updatedSettings.InfoFieldSetting));
            this.RestoreFieldIfNeeded(
                updated,
                () => updated.Driver,
                existing.Driver,
                this.CreateValidationRule(updatedSettings.DriverFieldSetting));
            this.RestoreFieldIfNeeded(
                updated,
                () => updated.URL,
                existing.URL,
                this.CreateValidationRule(updatedSettings.URLFieldSetting));
            this.RestoreFieldIfNeeded(
                updated,
                () => updated.NumberOfTrays,
                existing.NumberOfTrays,
                this.CreateValidationRule(updatedSettings.NumberOfTraysFieldSetting));
        }

        private void RestorePlace(PlaceFields updated, PlaceFields existing, PlaceFieldsSettings updatedSettings)
        {
            this.RestoreFieldIfNeeded(
                updated,
                () => updated.RoomId,
                existing.RoomId,
                this.CreateValidationRule(updatedSettings.RoomFieldSetting));
            this.RestoreFieldIfNeeded(
                updated,
                () => updated.Location,
                existing.Location,
                this.CreateValidationRule(updatedSettings.LocationFieldSetting));
        }

        private void RestoreOrganization(
            OrganizationFields updated,
            OrganizationFields existing,
            OrganizationFieldsSettings updatedSettings)
        {
            this.RestoreFieldIfNeeded(
                updated,
                () => updated.DepartmentId,
                existing.DepartmentId,
                this.CreateValidationRule(updatedSettings.DepartmentFieldSetting));
            this.RestoreFieldIfNeeded(
                updated,
                () => updated.UnitId,
                existing.UnitId,
                this.CreateValidationRule(updatedSettings.UnitFieldSetting));
        }

        private bool CreateValidationRule(ProcessingFieldSetting setting)
        {
            return setting.IsShow;
        }
    }
}