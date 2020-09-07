namespace DH.Helpdesk.Web.Infrastructure.ModelFactories.Inventory.Concrete
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;

    using DH.Helpdesk.BusinessData.Models.Inventory.Edit.Settings.ServerSettings;
    using DH.Helpdesk.BusinessData.Models.Shared;
    using DH.Helpdesk.Web.Areas.Inventory.Models.EditModel.Settings.Server;
    using DH.Helpdesk.Web.Areas.Inventory.Models.EditModel.Settings.Shared;

    public class ServerFieldsSettingsViewModelBuilder : IServerFieldsSettingsViewModelBuilder
    {
        private readonly IFieldSettingModelBuilder settingModelBuilder;

        public ServerFieldsSettingsViewModelBuilder(IFieldSettingModelBuilder settingModelBuilder)
        {
            this.settingModelBuilder = settingModelBuilder;
        }

        public ServerFieldsSettingsViewModel BuildViewModel(
            ServerFieldsSettings settings,
            List<ItemOverview> langauges,
            int langaugeId)
        {
            var name = this.settingModelBuilder.MapFieldSetting(settings.GeneralFieldsSettings.NameFieldSetting);
            var manufacturer =
                this.settingModelBuilder.MapFieldSetting(settings.GeneralFieldsSettings.ManufacturerFieldSetting);
            var description =
                this.settingModelBuilder.MapFieldSetting(settings.GeneralFieldsSettings.DescriptionFieldSetting);
            var model = this.settingModelBuilder.MapFieldSetting(settings.GeneralFieldsSettings.ModelFieldSetting);
            var serialNumber =
                this.settingModelBuilder.MapFieldSetting(settings.GeneralFieldsSettings.SerialNumberFieldSetting);
            var generalFieldsSettingsModel = new GeneralFieldsSettingsModel(
                name,
                manufacturer,
                description,
                model,
                serialNumber);

            var info = this.settingModelBuilder.MapFieldSetting(settings.OtherFieldsSettings.InfoFieldSetting);
            var other = this.settingModelBuilder.MapFieldSetting(settings.OtherFieldsSettings.OtherFieldSetting);
            var url = this.settingModelBuilder.MapFieldSetting(settings.OtherFieldsSettings.URLFieldSetting);
            var url2 = this.settingModelBuilder.MapFieldSetting(settings.OtherFieldsSettings.URL2FieldSetting);
            var owner = this.settingModelBuilder.MapFieldSetting(settings.OtherFieldsSettings.OwnerFieldSetting);
            var otherFieldsSettingsModel = new OtherFieldsSettingsModel(info, other, url, url2, owner);

            var createdDate =
                this.settingModelBuilder.MapFieldSetting(settings.StateFieldsSettings.CreatedDateFieldSetting);
            var changedDate =
                this.settingModelBuilder.MapFieldSetting(settings.StateFieldsSettings.ChangedDateFieldSetting);
            var syncDate =
                this.settingModelBuilder.MapFieldSetting(settings.StateFieldsSettings.SyncChangeDateFieldSetting);
            var stateFieldsSettingsModel = new StateFieldsSettingsModel(createdDate, changedDate, syncDate);

            var storage = this.settingModelBuilder.MapFieldSetting(settings.StorageFieldsSettings.CapasityFieldSetting);
            var storageFieldsSettingsModel = new StorageFieldsSettingsModel(storage);

            var chassis = this.settingModelBuilder.MapFieldSetting(settings.ChassisFieldsSettings.ChassisFieldSetting);
            var chassisFieldsSettingsModel = new ChassisFieldsSettingsModel(chassis);

            var barCode = this.settingModelBuilder.MapFieldSetting(settings.InventoryFieldsSettings.BarCodeFieldSetting);
            var purchaseDate =
                this.settingModelBuilder.MapFieldSetting(settings.InventoryFieldsSettings.PurchaseDateFieldSetting);
            var inventoryFieldsSettingsModel = new InventoryFieldsSettingsModel(barCode, purchaseDate);

            var memory = this.settingModelBuilder.MapFieldSetting(settings.MemoryFieldsSettings.RAMFieldSetting);
            var memoryFieldsSettingsModel = new MemoryFieldsSettingsModel(memory);

            var os =
                this.settingModelBuilder.MapFieldSetting(
                    settings.OperatingSystemFieldsSettings.OperatingSystemFieldSetting);
            var version =
                this.settingModelBuilder.MapFieldSetting(settings.OperatingSystemFieldsSettings.VersionFieldSetting);
            var servicePack =
                this.settingModelBuilder.MapFieldSetting(
                    settings.OperatingSystemFieldsSettings.ServicePackSystemFieldSetting);
            var code =
                this.settingModelBuilder.MapFieldSetting(
                    settings.OperatingSystemFieldsSettings.RegistrationCodeSystemFieldSetting);
            var key =
                this.settingModelBuilder.MapFieldSetting(settings.OperatingSystemFieldsSettings.ProductKeyFieldSetting);
            var operatingSystemFieldsSettingsModel = new OperatingSystemFieldsSettingsModel(
                os,
                version,
                servicePack,
                code,
                key);

            var proccesor =
                this.settingModelBuilder.MapFieldSetting(settings.ProccesorFieldsSettings.ProccesorFieldSetting);
            var proccesorFieldsSettingsModel = new ProccesorFieldsSettingsModel(proccesor);

            var room = this.settingModelBuilder.MapFieldSetting(settings.PlaceFieldsSettings.RoomFieldSetting);
            var location = this.settingModelBuilder.MapFieldSetting(settings.PlaceFieldsSettings.LocationFieldSetting);
            var placeFieldsSettingsModel = new PlaceFieldsSettingsModel(room, location);
            var document = this.settingModelBuilder.MapFieldSetting(settings.DocumentFieldsSettings.DocuemntFieldSetting);
            var documentFieldsSettingsModel = new DocumentFieldsSettingsModel(document);

            var network =
                this.settingModelBuilder.MapFieldSetting(
                    settings.CommunicationFieldsSettings.NetworkAdapterFieldSetting);
            var ipaddress =
                this.settingModelBuilder.MapFieldSetting(settings.CommunicationFieldsSettings.IPAddressFieldSetting);
            var macAddress =
                this.settingModelBuilder.MapFieldSetting(settings.CommunicationFieldsSettings.MacAddressFieldSetting);
            var communicationFieldsSettingsModel = new CommunicationFieldsSettingsModel(network, ipaddress, macAddress);

            var localizedLanguages =
                langauges.Select(l => new ItemOverview(Translation.Get(l.Name), l.Value)).ToList();

            var langaugesSelectList = new SelectList(localizedLanguages, "Value", "Name");

            var viewModel = new ServerFieldsSettingsViewModel(
                langaugeId,
                langaugesSelectList,
                generalFieldsSettingsModel,
                otherFieldsSettingsModel,
                stateFieldsSettingsModel,
                storageFieldsSettingsModel,
                chassisFieldsSettingsModel,
                inventoryFieldsSettingsModel,
                memoryFieldsSettingsModel,
                operatingSystemFieldsSettingsModel,
                proccesorFieldsSettingsModel,
                placeFieldsSettingsModel,
                documentFieldsSettingsModel,
                communicationFieldsSettingsModel);

            return viewModel;
        }
    }
}