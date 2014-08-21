namespace DH.Helpdesk.Services.BusinessLogic.BusinessModelValidators.Inventory.Concrete
{
    using DH.Helpdesk.BusinessData.Models.Inventory.Edit.Printer;
    using DH.Helpdesk.BusinessData.Models.Inventory.Edit.Shared;
    using DH.Helpdesk.BusinessData.Models.Inventory.Output.Settings.ProcessingSetttings;
    using DH.Helpdesk.BusinessData.Models.Inventory.Output.Settings.ProcessingSetttings.PrinterSettings;
    using DH.Helpdesk.BusinessData.Models.Inventory.Output.Settings.ProcessingSetttings.SharedSettings;
    using DH.Helpdesk.Services.BusinessLogic.BusinessModelValidators.Common;

    public class PrinterValidator : IPrinterValidator
    {
        private readonly IElementaryRulesValidator elementaryRulesValidator;

        public PrinterValidator(IElementaryRulesValidator elementaryRulesValidator)
        {
            this.elementaryRulesValidator = elementaryRulesValidator;
        }

        public void Validate(PrinterForUpdate updatedPrinter, PrinterForRead existingPrinter, PrinterFieldsSettingsProcessing settings)
        {
            this.ValidateGeneral(updatedPrinter.GeneralFields, existingPrinter.GeneralFields, settings.GeneralFieldsSettings);
            this.ValidateInventoring(updatedPrinter.InventoryFields, existingPrinter.InventoryFields, settings.InventoryFieldsSettings);
            this.ValidateCommunication(updatedPrinter.CommunicationFields, existingPrinter.CommunicationFields, settings.CommunicationFieldsSettings);
            this.ValidateOther(updatedPrinter.OtherFields, existingPrinter.OtherFields, settings.OtherFieldsSettings);
            this.ValidateOrganization(updatedPrinter.OrganizationFields, existingPrinter.OrganizationFields, settings.OrganizationFieldsSettings);
        }

        public void Validate(PrinterForInsert newPrinter, PrinterFieldsSettingsProcessing settings)
        {
            this.ValidateGeneral(newPrinter.GeneralFields, settings.GeneralFieldsSettings);
            this.ValidateInventoring(newPrinter.InventoryFields, settings.InventoryFieldsSettings);
            this.ValidateCommunication(newPrinter.CommunicationFields, settings.CommunicationFieldsSettings);
            this.ValidateOther(newPrinter.OtherFields, settings.OtherFieldsSettings);
            this.ValidateOrganization(newPrinter.OrganizationFields, settings.OrganizationFieldsSettings);
        }

        private void ValidateGeneral(
            GeneralFields updated,
            GeneralFields existing,
            GeneralFieldsSettings updatedSettings)
        {
            this.elementaryRulesValidator.ValidateStringField(
                updated.Name,
                existing.Name,
                BusinessData.Enums.Inventory.Fields.Printer.GeneralFields.Name,
                this.CreateValidationRule(updatedSettings.NameFieldSetting));
            this.elementaryRulesValidator.ValidateStringField(
                updated.Manufacturer,
                existing.Manufacturer,
                BusinessData.Enums.Inventory.Fields.Printer.GeneralFields.Manufacturer,
                this.CreateValidationRule(updatedSettings.ManufacturerFieldSetting));
            this.elementaryRulesValidator.ValidateStringField(
                updated.Model,
                existing.Model,
                BusinessData.Enums.Inventory.Fields.Printer.GeneralFields.Model,
                this.CreateValidationRule(updatedSettings.ModelFieldSetting));
            this.elementaryRulesValidator.ValidateStringField(
                updated.SerialNumber,
                existing.SerialNumber,
                BusinessData.Enums.Inventory.Fields.Printer.GeneralFields.SerialNumber,
                this.CreateValidationRule(updatedSettings.SerialNumberFieldSetting));
        }

        private void ValidateInventoring(
            InventoryFields updated,
            InventoryFields existing,
            InventoryFieldsSettings updatedSettings)
        {
            this.elementaryRulesValidator.ValidateStringField(
                updated.BarCode,
                existing.BarCode,
                BusinessData.Enums.Inventory.Fields.Shared.InventoryFields.BarCode,
                this.CreateValidationRule(updatedSettings.BarCodeFieldSetting));
            this.elementaryRulesValidator.ValidateDateTimeField(
                updated.PurchaseDate,
                existing.PurchaseDate,
                BusinessData.Enums.Inventory.Fields.Shared.InventoryFields.PurchaseDate,
                this.CreateValidationRule(updatedSettings.PurchaseDateFieldSetting));
        }

        private void ValidateCommunication(
            CommunicationFields updated,
            CommunicationFields existing,
            CommunicationFieldsSettings updatedSettings)
        {
            this.elementaryRulesValidator.ValidateStringField(
                updated.NetworkAdapterName,
                existing.NetworkAdapterName,
                BusinessData.Enums.Inventory.Fields.Printer.CommunicationFields.NetworkAdapter,
                this.CreateValidationRule(updatedSettings.NetworkAdapterFieldSetting));
            this.elementaryRulesValidator.ValidateStringField(
                updated.IPAddress,
                existing.IPAddress,
                BusinessData.Enums.Inventory.Fields.Printer.CommunicationFields.IPAddress,
                this.CreateValidationRule(updatedSettings.IPAddressFieldSetting));
            this.elementaryRulesValidator.ValidateStringField(
                updated.MacAddress,
                existing.MacAddress,
                BusinessData.Enums.Inventory.Fields.Printer.CommunicationFields.MacAddress,
                this.CreateValidationRule(updatedSettings.MacAddressFieldSetting));
        }

        private void ValidateOther(OtherFields updated, OtherFields existing, OtherFieldsSettings updatedSettings)
        {
            this.elementaryRulesValidator.ValidateStringField(
                updated.Info,
                existing.Info,
                BusinessData.Enums.Inventory.Fields.Printer.OtherFields.Info,
                this.CreateValidationRule(updatedSettings.InfoFieldSetting));
            this.elementaryRulesValidator.ValidateStringField(
                updated.Driver,
                existing.Driver,
                BusinessData.Enums.Inventory.Fields.Printer.OtherFields.Driver,
                this.CreateValidationRule(updatedSettings.DriverFieldSetting));
            this.elementaryRulesValidator.ValidateStringField(
                updated.URL,
                existing.URL,
                BusinessData.Enums.Inventory.Fields.Printer.OtherFields.URL,
                this.CreateValidationRule(updatedSettings.URLFieldSetting));
            this.elementaryRulesValidator.ValidateStringField(
                updated.NumberOfTrays,
                existing.NumberOfTrays,
                BusinessData.Enums.Inventory.Fields.Printer.OtherFields.NumberOfTrays,
                this.CreateValidationRule(updatedSettings.NumberOfTraysFieldSetting));
        }

        private void ValidateOrganization(
            OrganizationFields updated,
            OrganizationFields existing,
            OrganizationFieldsSettings updatedSettings)
        {
            this.elementaryRulesValidator.ValidateIntegerField(
                updated.DepartmentId,
                existing.DepartmentId,
                BusinessData.Enums.Inventory.Fields.Computer.OrganizationFields.Department,
                this.CreateValidationRule(updatedSettings.DepartmentFieldSetting));
            this.elementaryRulesValidator.ValidateStringField(
                updated.UnitId,
                existing.UnitId,
                BusinessData.Enums.Inventory.Fields.Computer.OrganizationFields.Unit,
                this.CreateValidationRule(updatedSettings.UnitFieldSetting));
        }

        private void ValidateGeneral(GeneralFields updated, GeneralFieldsSettings updatedSettings)
        {
            this.elementaryRulesValidator.ValidateStringField(
                updated.Name,
                BusinessData.Enums.Inventory.Fields.Printer.GeneralFields.Name,
                this.CreateValidationRule(updatedSettings.NameFieldSetting));
            this.elementaryRulesValidator.ValidateStringField(
                updated.Manufacturer,
                BusinessData.Enums.Inventory.Fields.Printer.GeneralFields.Manufacturer,
                this.CreateValidationRule(updatedSettings.ManufacturerFieldSetting));
            this.elementaryRulesValidator.ValidateStringField(
                updated.Model,
                BusinessData.Enums.Inventory.Fields.Printer.GeneralFields.Model,
                this.CreateValidationRule(updatedSettings.ModelFieldSetting));
            this.elementaryRulesValidator.ValidateStringField(
                updated.SerialNumber,
                BusinessData.Enums.Inventory.Fields.Printer.GeneralFields.SerialNumber,
                this.CreateValidationRule(updatedSettings.SerialNumberFieldSetting));
        }

        private void ValidateInventoring(InventoryFields updated, InventoryFieldsSettings updatedSettings)
        {
            this.elementaryRulesValidator.ValidateStringField(
                updated.BarCode,
                BusinessData.Enums.Inventory.Fields.Shared.InventoryFields.BarCode,
                this.CreateValidationRule(updatedSettings.BarCodeFieldSetting));
            this.elementaryRulesValidator.ValidateDateTimeField(
                updated.PurchaseDate,
                BusinessData.Enums.Inventory.Fields.Shared.InventoryFields.PurchaseDate,
                this.CreateValidationRule(updatedSettings.PurchaseDateFieldSetting));
        }

        private void ValidateCommunication(CommunicationFields updated, CommunicationFieldsSettings updatedSettings)
        {
            this.elementaryRulesValidator.ValidateStringField(
                updated.NetworkAdapterName,
                BusinessData.Enums.Inventory.Fields.Printer.CommunicationFields.NetworkAdapter,
                this.CreateValidationRule(updatedSettings.NetworkAdapterFieldSetting));
            this.elementaryRulesValidator.ValidateStringField(
                updated.IPAddress,
                BusinessData.Enums.Inventory.Fields.Printer.CommunicationFields.IPAddress,
                this.CreateValidationRule(updatedSettings.IPAddressFieldSetting));
            this.elementaryRulesValidator.ValidateStringField(
                updated.MacAddress,
                BusinessData.Enums.Inventory.Fields.Printer.CommunicationFields.MacAddress,
                this.CreateValidationRule(updatedSettings.MacAddressFieldSetting));
        }

        private void ValidateOther(OtherFields updated, OtherFieldsSettings updatedSettings)
        {
            this.elementaryRulesValidator.ValidateStringField(
                updated.Info,
                BusinessData.Enums.Inventory.Fields.Printer.OtherFields.Info,
                this.CreateValidationRule(updatedSettings.InfoFieldSetting));
            this.elementaryRulesValidator.ValidateStringField(
                updated.Driver,
                BusinessData.Enums.Inventory.Fields.Printer.OtherFields.Driver,
                this.CreateValidationRule(updatedSettings.DriverFieldSetting));
            this.elementaryRulesValidator.ValidateStringField(
                updated.URL,
                BusinessData.Enums.Inventory.Fields.Printer.OtherFields.URL,
                this.CreateValidationRule(updatedSettings.URLFieldSetting));
            this.elementaryRulesValidator.ValidateStringField(
                updated.NumberOfTrays,
                BusinessData.Enums.Inventory.Fields.Printer.OtherFields.NumberOfTrays,
                this.CreateValidationRule(updatedSettings.NumberOfTraysFieldSetting));
        }

        private void ValidateOrganization(OrganizationFields updated, OrganizationFieldsSettings updatedSettings)
        {
            this.elementaryRulesValidator.ValidateIntegerField(
                updated.DepartmentId,
                BusinessData.Enums.Inventory.Fields.Computer.OrganizationFields.Department,
                this.CreateValidationRule(updatedSettings.DepartmentFieldSetting));
            this.elementaryRulesValidator.ValidateStringField(
                updated.UnitId,
                BusinessData.Enums.Inventory.Fields.Computer.OrganizationFields.Unit,
                this.CreateValidationRule(updatedSettings.UnitFieldSetting));
        }

        private ElementaryValidationRule CreateValidationRule(ProcessingFieldSetting setting)
        {
            return new ElementaryValidationRule(!setting.IsShow, setting.IsRequired);
        }
    }
}