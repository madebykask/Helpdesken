namespace DH.Helpdesk.Services.BusinessLogic.BusinessModelValidators.Inventory.Concrete
{
    using DH.Helpdesk.BusinessData.Models.Inventory.Edit.Inventory;
    using DH.Helpdesk.BusinessData.Models.Inventory.Output.Settings.ProcessingSetttings.InventorySettings;
    using DH.Helpdesk.Services.BusinessLogic.BusinessModelValidators.Common;

    public class InventoryValidator : IInventoryValidator
    {
        private readonly IElementaryRulesValidator elementaryRulesValidator;

        public InventoryValidator(IElementaryRulesValidator elementaryRulesValidator)
        {
            this.elementaryRulesValidator = elementaryRulesValidator;
        }

        public void Validate(
            InventoryForUpdate updatedModel,
            InventoryForRead existingModel,
            InventoryFieldSettingsProcessing settings)
        {
            this.elementaryRulesValidator.ValidateIntegerField(
                updatedModel.DepartmentId,
                existingModel.DepartmentId,
                BusinessData.Enums.Inventory.Fields.Inventory.InventoryFields.Department,
                this.CreateValidationRule(settings.DefaultSettings.DepartmentFieldSetting));

            this.elementaryRulesValidator.ValidateStringField(
                updatedModel.Name,
                existingModel.Name,
                BusinessData.Enums.Inventory.Fields.Inventory.InventoryFields.Name,
                this.CreateValidationRule(settings.DefaultSettings.NameFieldSetting));
            this.elementaryRulesValidator.ValidateStringField(
                updatedModel.Model,
                existingModel.Model,
                BusinessData.Enums.Inventory.Fields.Inventory.InventoryFields.Model,
                this.CreateValidationRule(settings.DefaultSettings.ModelFieldSetting));
            this.elementaryRulesValidator.ValidateStringField(
                updatedModel.Manufacturer,
                existingModel.Manufacturer,
                BusinessData.Enums.Inventory.Fields.Inventory.InventoryFields.Manufacturer,
                this.CreateValidationRule(settings.DefaultSettings.ManufacturerFieldSetting));
            this.elementaryRulesValidator.ValidateStringField(
                updatedModel.SerialNumber,
                existingModel.SerialNumber,
                BusinessData.Enums.Inventory.Fields.Inventory.InventoryFields.SerialNumber,
                this.CreateValidationRule(settings.DefaultSettings.SerialNumberFieldSetting));
            this.elementaryRulesValidator.ValidateStringField(
                updatedModel.TheftMark,
                existingModel.TheftMark,
                BusinessData.Enums.Inventory.Fields.Inventory.InventoryFields.TheftMark,
                this.CreateValidationRule(settings.DefaultSettings.TheftMarkFieldSetting));
            this.elementaryRulesValidator.ValidateStringField(
                updatedModel.BarCode,
                existingModel.BarCode,
                BusinessData.Enums.Inventory.Fields.Inventory.InventoryFields.BarCode,
                this.CreateValidationRule(settings.DefaultSettings.BarCodeFieldSetting));
            this.elementaryRulesValidator.ValidateDateTimeField(
                updatedModel.PurchaseDate,
                existingModel.PurchaseDate,
                BusinessData.Enums.Inventory.Fields.Inventory.InventoryFields.PurchaseDate,
                this.CreateValidationRule(settings.DefaultSettings.PurchaseDateFieldSetting));
            this.elementaryRulesValidator.ValidateIntegerField(
                updatedModel.RoomId,
                existingModel.RoomId,
                BusinessData.Enums.Inventory.Fields.Inventory.InventoryFields.Place,
                this.CreateValidationRule(settings.DefaultSettings.PlaceFieldSetting));
            this.elementaryRulesValidator.ValidateStringField(
                updatedModel.Info,
                existingModel.Info,
                BusinessData.Enums.Inventory.Fields.Inventory.InventoryFields.Info,
                this.CreateValidationRule(settings.DefaultSettings.InfoFieldSetting));
        }

        public void Validate(InventoryForInsert newModel, InventoryFieldSettingsProcessing settings)
        {
            this.elementaryRulesValidator.ValidateIntegerField(
                newModel.DepartmentId,
                BusinessData.Enums.Inventory.Fields.Inventory.InventoryFields.Department,
                this.CreateValidationRule(settings.DefaultSettings.DepartmentFieldSetting));

            this.elementaryRulesValidator.ValidateStringField(
                newModel.Name,
                BusinessData.Enums.Inventory.Fields.Inventory.InventoryFields.Name,
                this.CreateValidationRule(settings.DefaultSettings.NameFieldSetting));
            this.elementaryRulesValidator.ValidateStringField(
                newModel.Model,
                BusinessData.Enums.Inventory.Fields.Inventory.InventoryFields.Model,
                this.CreateValidationRule(settings.DefaultSettings.ModelFieldSetting));
            this.elementaryRulesValidator.ValidateStringField(
                newModel.Manufacturer,
                BusinessData.Enums.Inventory.Fields.Inventory.InventoryFields.Manufacturer,
                this.CreateValidationRule(settings.DefaultSettings.ManufacturerFieldSetting));
            this.elementaryRulesValidator.ValidateStringField(
                newModel.SerialNumber,
                BusinessData.Enums.Inventory.Fields.Inventory.InventoryFields.SerialNumber,
                this.CreateValidationRule(settings.DefaultSettings.SerialNumberFieldSetting));
            this.elementaryRulesValidator.ValidateStringField(
                newModel.TheftMark,
                BusinessData.Enums.Inventory.Fields.Inventory.InventoryFields.TheftMark,
                this.CreateValidationRule(settings.DefaultSettings.TheftMarkFieldSetting));
            this.elementaryRulesValidator.ValidateStringField(
                newModel.BarCode,
                BusinessData.Enums.Inventory.Fields.Inventory.InventoryFields.BarCode,
                this.CreateValidationRule(settings.DefaultSettings.BarCodeFieldSetting));
            this.elementaryRulesValidator.ValidateDateTimeField(
                newModel.PurchaseDate,
                BusinessData.Enums.Inventory.Fields.Inventory.InventoryFields.PurchaseDate,
                this.CreateValidationRule(settings.DefaultSettings.PurchaseDateFieldSetting));
            this.elementaryRulesValidator.ValidateIntegerField(
                newModel.RoomId,
                BusinessData.Enums.Inventory.Fields.Inventory.InventoryFields.Place,
                this.CreateValidationRule(settings.DefaultSettings.PlaceFieldSetting));
            this.elementaryRulesValidator.ValidateStringField(
                newModel.Info,
                BusinessData.Enums.Inventory.Fields.Inventory.InventoryFields.Info,
                this.CreateValidationRule(settings.DefaultSettings.InfoFieldSetting));
        }

        private ElementaryValidationRule CreateValidationRule(InventoryFieldSettingForProcessing setting)
        {
            return new ElementaryValidationRule(!setting.ShowInDetails, false);
        }
    }
}