namespace DH.Helpdesk.Web.Infrastructure.ModelFactories.Inventory.Concrete
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;

    using DH.Helpdesk.BusinessData.Models.Inventory.Edit.Settings.PrinterSettings;
    using DH.Helpdesk.BusinessData.Models.Shared;
    using DH.Helpdesk.Web.Areas.Inventory.Models.EditModel.Settings.Printer;
    using DH.Helpdesk.Web.Areas.Inventory.Models.EditModel.Settings.Shared;

    public class PrinterFieldsSettingsViewModelBuilder : IPrinterFieldsSettingsViewModelBuilder
    {
        private readonly IFieldSettingModelBuilder settingModelBuilder;

        public PrinterFieldsSettingsViewModelBuilder(IFieldSettingModelBuilder settingModelBuilder)
        {
            this.settingModelBuilder = settingModelBuilder;
        }

        public PrinterFieldsSettingsViewModel BuildViewModel(PrinterFieldsSettings settings, List<ItemOverview> langauges, int langaugeId)
        {
            var name = this.settingModelBuilder.MapFieldSetting(settings.GeneralFieldsSettings.NameFieldSetting);
            var manufacturer = this.settingModelBuilder.MapFieldSetting(settings.GeneralFieldsSettings.ManufacturerFieldSetting);
            var model = this.settingModelBuilder.MapFieldSetting(settings.GeneralFieldsSettings.ModelFieldSetting);
            var serialNumber = this.settingModelBuilder.MapFieldSetting(settings.GeneralFieldsSettings.SerialNumberFieldSetting);
            var generalFieldsSettingsModel = new GeneralFieldsSettingsModel(
                name,
                manufacturer,
                model,
                serialNumber);

            var barCode = this.settingModelBuilder.MapFieldSetting(settings.InventoryFieldsSettings.BarCodeFieldSetting);
            var purchaseDate = this.settingModelBuilder.MapFieldSetting(settings.InventoryFieldsSettings.PurchaseDateFieldSetting);
            var inventoryFieldsSettingsModel = new InventoryFieldsSettingsModel(barCode, purchaseDate);

            var network = this.settingModelBuilder.MapFieldSetting(settings.CommunicationFieldsSettings.NetworkAdapterFieldSetting);
            var ipaddress = this.settingModelBuilder.MapFieldSetting(settings.CommunicationFieldsSettings.IPAddressFieldSetting);
            var macAddress = this.settingModelBuilder.MapFieldSetting(settings.CommunicationFieldsSettings.MacAddressFieldSetting);
            var communicationFieldsSettingsModel = new CommunicationFieldsSettingsModel(
                network,
                ipaddress,
                macAddress);
            
            var number = this.settingModelBuilder.MapFieldSetting(settings.OtherFieldsSettings.NumberOfTraysFieldSetting);
            var driver = this.settingModelBuilder.MapFieldSetting(settings.OtherFieldsSettings.DriverFieldSetting);
            var info = this.settingModelBuilder.MapFieldSetting(settings.OtherFieldsSettings.InfoFieldSetting);
            var url = this.settingModelBuilder.MapFieldSetting(settings.OtherFieldsSettings.URLFieldSetting);
            var otherFieldsSettingsModel = new OtherFieldsSettingsModel(number, driver, info, url);

            var department = this.settingModelBuilder.MapFieldSetting(settings.OrganizationFieldsSettings.DepartmentFieldSetting);
            var unit = this.settingModelBuilder.MapFieldSetting(settings.OrganizationFieldsSettings.UnitFieldSetting);
            var organizationFieldsSettingsModel = new OrganizationFieldsSettingsModel(department, unit);

            var room = this.settingModelBuilder.MapFieldSetting(settings.PlaceFieldsSettings.RoomFieldSetting);
            var location = this.settingModelBuilder.MapFieldSetting(settings.PlaceFieldsSettings.LocationFieldSetting);
            var placeFieldsSettingsModel = new PlaceFieldsSettingsModel(room, location);

            var createdDate = this.settingModelBuilder.MapFieldSetting(settings.StateFieldsSettings.CreatedDateFieldSetting);
            var changedDate = this.settingModelBuilder.MapFieldSetting(settings.StateFieldsSettings.ChangedDateFieldSetting);
            var syncDate = this.settingModelBuilder.MapFieldSetting(settings.StateFieldsSettings.SyncDateFieldSetting);

            var stateFieldsSettingsModel = new StateFieldsSettingsModel(
                createdDate,
                changedDate,
                syncDate);

            var localizedLanguages = langauges.Select(l => new ItemOverview(Translation.Get(l.Name), l.Value)).ToList();

            var langaugesSelectList = new SelectList(localizedLanguages, "Value", "Name");

            var viewModel = new PrinterFieldsSettingsViewModel(
                langaugeId,
                langaugesSelectList,
                generalFieldsSettingsModel,
                inventoryFieldsSettingsModel,
                communicationFieldsSettingsModel,
                otherFieldsSettingsModel,
                organizationFieldsSettingsModel,
                placeFieldsSettingsModel,
                stateFieldsSettingsModel);

            return viewModel;
        }
    }
}