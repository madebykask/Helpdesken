namespace DH.Helpdesk.Web.Infrastructure.BusinessModelFactories.Inventory.Concrete
{
    using System;

    using DH.Helpdesk.BusinessData.Models.Inventory.Edit.Settings.PrinterSettings;
    using DH.Helpdesk.BusinessData.Models.Inventory.Edit.Settings.SharedSettings;
    using DH.Helpdesk.Web.Areas.Inventory.Models.EditModel.Settings.Printer;

    public class PrinterFieldsSettingsBuilder : IPrinterFieldsSettingsBuilder
    {
        private readonly IFieldSettingBuilder settingBuilder;

        public PrinterFieldsSettingsBuilder(IFieldSettingBuilder settingBuilder)
        {
            this.settingBuilder = settingBuilder;
        }

        public PrinterFieldsSettings BuildViewModel(PrinterFieldsSettingsViewModel settings, int customerId)
        {
            var name = this.settingBuilder.MapFieldSetting(settings.GeneralFieldsSettingsModel.NameFieldSettingModel);
            var manufacturer = this.settingBuilder.MapFieldSetting(settings.GeneralFieldsSettingsModel.ManufacturerFieldSettingModel);
            var model = this.settingBuilder.MapFieldSetting(settings.GeneralFieldsSettingsModel.ModelFieldSettingModel);
            var serialNumber = this.settingBuilder.MapFieldSetting(settings.GeneralFieldsSettingsModel.SerialNumberFieldSettingModel);
            var generalFieldsSettings = new GeneralFieldsSettings(
                name,
                manufacturer,
                model,
                serialNumber);

            var barCode = this.settingBuilder.MapFieldSetting(settings.InventoryFieldsSettingsModel.BarCodeFieldSettingModel);
            var purchaseDate = this.settingBuilder.MapFieldSetting(settings.InventoryFieldsSettingsModel.PurchaseDateFieldSettingModel);
            var inventoryFieldsSettings = new InventoryFieldsSettings(barCode, purchaseDate);

            var network = this.settingBuilder.MapFieldSetting(settings.CommunicationFieldsSettingsModel.NetworkAdapterFieldSettingModel);
            var ipaddress = this.settingBuilder.MapFieldSetting(settings.CommunicationFieldsSettingsModel.IPAddressFieldSettingModel);
            var macAddress = this.settingBuilder.MapFieldSetting(settings.CommunicationFieldsSettingsModel.MacAddressFieldSettingModel);
            var communicationFieldsSettings = new CommunicationFieldsSettings(
                network,
                ipaddress,
                macAddress);

            var number = this.settingBuilder.MapFieldSetting(settings.OtherFieldsSettingsModel.NumberOfTraysFieldSettingModel);
            var driver = this.settingBuilder.MapFieldSetting(settings.OtherFieldsSettingsModel.DriverFieldSettingModel);
            var info = this.settingBuilder.MapFieldSetting(settings.OtherFieldsSettingsModel.InfoFieldSettingModel);
            var url = this.settingBuilder.MapFieldSetting(settings.OtherFieldsSettingsModel.URLFieldSettingModel);
            var otherFieldsSettings = new OtherFieldsSettings(number, driver, info, url);

            var department = this.settingBuilder.MapFieldSetting(settings.OrganizationFieldsSettingsModel.DepartmentFieldSettingModel);
            var unit = this.settingBuilder.MapFieldSetting(settings.OrganizationFieldsSettingsModel.UnitFieldSettingModel);
            var organizationFieldsSettings = new OrganizationFieldsSettings(department, unit);

            var room = this.settingBuilder.MapFieldSetting(settings.PlaceFieldsSettingsModel.RoomFieldSettingModel);
            var location = this.settingBuilder.MapFieldSetting(settings.PlaceFieldsSettingsModel.LocationFieldSettingModel);
            var placeFieldsSettings = new PlaceFieldsSettings(room, location);

            var createdDate = this.settingBuilder.MapFieldSetting(settings.StateFieldsSettingsModel.CreatedDateFieldSettingModel);
            var changedDate = this.settingBuilder.MapFieldSetting(settings.StateFieldsSettingsModel.ChangedDateFieldSettingModel);
            var syncDate = this.settingBuilder.MapFieldSetting(settings.StateFieldsSettingsModel.SyncDateFieldSettingModel);

            var stateFieldsSettings = new StateFieldsSettings(createdDate, changedDate, syncDate);

            var businessModel = PrinterFieldsSettings.CreateUpdated(
                customerId,
                settings.LanguageId,
                DateTime.Now,
                generalFieldsSettings,
                inventoryFieldsSettings,
                communicationFieldsSettings,
                otherFieldsSettings,
                organizationFieldsSettings,
                placeFieldsSettings,
                stateFieldsSettings);

            return businessModel;
        }
    }
}