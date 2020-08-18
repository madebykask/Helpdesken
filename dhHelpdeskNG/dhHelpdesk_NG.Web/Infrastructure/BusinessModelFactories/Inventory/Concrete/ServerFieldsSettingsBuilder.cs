namespace DH.Helpdesk.Web.Infrastructure.BusinessModelFactories.Inventory.Concrete
{
    using System;

    using DH.Helpdesk.BusinessData.Models.Inventory.Edit.Settings.ServerSettings;
    using DH.Helpdesk.BusinessData.Models.Inventory.Edit.Settings.SharedSettings;
    using DH.Helpdesk.Web.Areas.Inventory.Models.EditModel.Settings.Server;

    public class ServerFieldsSettingsBuilder : IServerFieldsSettingsBuilder
    {
        private readonly IFieldSettingBuilder settingBuilder;

        public ServerFieldsSettingsBuilder(IFieldSettingBuilder settingBuilder)
        {
            this.settingBuilder = settingBuilder;
        }

        public ServerFieldsSettings BuildViewModel(
            ServerFieldsSettingsViewModel settings,
            int customerId)
        {
            var name = this.settingBuilder.MapFieldSetting(settings.GeneralFieldsSettingsModel.NameFieldSettingModel);
            var manufacturer = this.settingBuilder.MapFieldSetting(settings.GeneralFieldsSettingsModel.ManufacturerFieldSettingModel);
            var description = this.settingBuilder.MapFieldSetting(settings.GeneralFieldsSettingsModel.DescriptionFieldSettingModel);
            var model = this.settingBuilder.MapFieldSetting(settings.GeneralFieldsSettingsModel.ModelFieldSettingModel);
            var serialNumber = this.settingBuilder.MapFieldSetting(settings.GeneralFieldsSettingsModel.SerialNumberFieldSettingModel);
            var generalFieldsSettings = new GeneralFieldsSettings(
                name,
                manufacturer,
                description,
                model,
                serialNumber);

            var info = this.settingBuilder.MapFieldSetting(settings.OtherFieldsSettingsModel.InfoFieldSettingModel);
            var other = this.settingBuilder.MapFieldSetting(settings.OtherFieldsSettingsModel.OtherFieldSettingModel);
            var url = this.settingBuilder.MapFieldSetting(settings.OtherFieldsSettingsModel.URLFieldSettingModel);
            var url2 = this.settingBuilder.MapFieldSetting(settings.OtherFieldsSettingsModel.URL2FieldSettingModel);
            var owner = this.settingBuilder.MapFieldSetting(settings.OtherFieldsSettingsModel.OwnerFieldSettingModel);
            var otherFieldsSettings = new OtherFieldsSettings(info, other, url, url2, owner);

            var createdDate = this.settingBuilder.MapFieldSetting(settings.StateFieldsSettingsModel.CreatedDateFieldSettingModel);
            var changedDate = this.settingBuilder.MapFieldSetting(settings.StateFieldsSettingsModel.ChangedDateFieldSettingModel);
            var syncDate = this.settingBuilder.MapFieldSetting(settings.StateFieldsSettingsModel.SyncChangeDateFieldSettingModel);
            var stateFieldsSettings = new StateFieldsSettings(
                createdDate,
                changedDate,
                syncDate);

            var storage = this.settingBuilder.MapFieldSetting(settings.StorageFieldsSettingsModel.CapasityFieldSettingModel);
            var storageFieldsSettings = new StorageFieldsSettings(storage);

            var chassis = this.settingBuilder.MapFieldSetting(settings.ChassisFieldsSettingsModel.ChassisFieldSettingModel);
            var chassisFieldsSettings = new ChassisFieldsSettings(chassis);

            var barCode = this.settingBuilder.MapFieldSetting(settings.InventoryFieldsSettingsModel.BarCodeFieldSettingModel);
            var purchaseDate = this.settingBuilder.MapFieldSetting(settings.InventoryFieldsSettingsModel.PurchaseDateFieldSettingModel);
            var inventoryFieldsSettings = new InventoryFieldsSettings(barCode, purchaseDate);

            var memory = this.settingBuilder.MapFieldSetting(settings.MemoryFieldsSettingsModel.RAMFieldSettingModel);
            var memoryFieldsSettings = new MemoryFieldsSettings(memory);

            var os = this.settingBuilder.MapFieldSetting(settings.OperatingSystemFieldsSettingsModel.OperatingSystemFieldSettingModel);
            var version = this.settingBuilder.MapFieldSetting(settings.OperatingSystemFieldsSettingsModel.VersionFieldSettingModel);
            var servicePack = this.settingBuilder.MapFieldSetting(settings.OperatingSystemFieldsSettingsModel.ServicePackSystemFieldSettingModel);
            var code = this.settingBuilder.MapFieldSetting(settings.OperatingSystemFieldsSettingsModel.RegistrationCodeSystemFieldSettingModel);
            var key = this.settingBuilder.MapFieldSetting(settings.OperatingSystemFieldsSettingsModel.ProductKeyFieldSettingModel);
            var operatingSystemFieldsSettings = new OperatingSystemFieldsSettings(
                os,
                version,
                servicePack,
                code,
                key);

            var proccesor = this.settingBuilder.MapFieldSetting(settings.ProccesorFieldsSettingsModel.ProccesorFieldSettingModel);
            var proccesorFieldsSettings = new ProcessorFieldsSettings(proccesor);

            var room = this.settingBuilder.MapFieldSetting(settings.PlaceFieldsSettingsModel.RoomFieldSettingModel);
            var location = this.settingBuilder.MapFieldSetting(settings.PlaceFieldsSettingsModel.LocationFieldSettingModel);
            var placeFieldsSettings = new PlaceFieldsSettings(
                room,
                location);

            var document = this.settingBuilder.MapFieldSetting(settings.DocumentFieldsSettingsModel.DocumentFieldSettingModel);
            var documentFieldsSettings = new DocumentFieldsSettings(document);

            var network = this.settingBuilder.MapFieldSetting(settings.CommunicationFieldsSettingsModel.NetworkAdapterFieldSettingModel);
            var ipaddress = this.settingBuilder.MapFieldSetting(settings.CommunicationFieldsSettingsModel.IPAddressFieldSettingModel);
            var macAddress = this.settingBuilder.MapFieldSetting(settings.CommunicationFieldsSettingsModel.MacAddressFieldSettingModel);
            var communicationFieldsSettings = new CommunicationFieldsSettings(
                network,
                ipaddress,
                macAddress);

            var businessModel = ServerFieldsSettings.CreateUpdated(
                customerId,
                settings.LanguageId,
                DateTime.Now,
                generalFieldsSettings,
                otherFieldsSettings,
                stateFieldsSettings,
                storageFieldsSettings,
                chassisFieldsSettings,
                inventoryFieldsSettings,
                operatingSystemFieldsSettings,
                memoryFieldsSettings,
                placeFieldsSettings,
                documentFieldsSettings,
                proccesorFieldsSettings,
                communicationFieldsSettings);

            return businessModel;
        }
    }
}