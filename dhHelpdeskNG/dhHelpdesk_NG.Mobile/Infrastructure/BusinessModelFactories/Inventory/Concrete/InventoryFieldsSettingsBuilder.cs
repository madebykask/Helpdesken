namespace DH.Helpdesk.Mobile.Infrastructure.BusinessModelFactories.Inventory.Concrete
{
    using System;

    using DH.Helpdesk.BusinessData.Models.Inventory.Edit.Settings;
    using DH.Helpdesk.BusinessData.Models.Inventory.Edit.Settings.InventorySettings;
    using DH.Helpdesk.Mobile.Models.Inventory.EditModel.Settings;
    using DH.Helpdesk.Mobile.Models.Inventory.EditModel.Settings.Inventory;

    public class InventoryFieldsSettingsBuilder : IInventoryFieldsSettingsBuilder
    {
        public InventoryFieldSettings BuildBusinessModelForUpdate(
            int inventoryTypeId,
            DefaultFieldSettingsModel defaultFieldSettingsModel)
        {
            var defaultFieldSettings = CreateDefaultFieldSettings(defaultFieldSettingsModel);

            var businessModel = InventoryFieldSettings.CreateForUpdate(
                inventoryTypeId,
                defaultFieldSettings,
                DateTime.Now);

            return businessModel;
        }

        public InventoryFieldSettings BuildBusinessModelForAdd(
            int inventoryTypeId,
            DefaultFieldSettingsModel defaultFieldSettingsModel)
        {
            var defaultFieldSettings = CreateDefaultFieldSettings(defaultFieldSettingsModel);

            var businessModel = InventoryFieldSettings.CreateNew(
                inventoryTypeId,
                defaultFieldSettings,
                DateTime.Now);

            return businessModel;
        }

        private static DefaultFieldSettings CreateDefaultFieldSettings(DefaultFieldSettingsModel defaultFieldSettingsModel)
        {
            var department = MapInventoryFieldSettingModel(defaultFieldSettingsModel.DepartmentFieldSettingModel);
            var name = MapInventoryFieldSettingModel(defaultFieldSettingsModel.NameFieldSettingModel);
            var model = MapInventoryFieldSettingModel(defaultFieldSettingsModel.ModelFieldSettingModel);
            var manufacturer = MapInventoryFieldSettingModel(defaultFieldSettingsModel.ManufacturerFieldSettingModel);
            var serial = MapInventoryFieldSettingModel(defaultFieldSettingsModel.SerialNumberFieldSettingModel);
            var theftMark = MapInventoryFieldSettingModel(defaultFieldSettingsModel.TheftMarkFieldSettingModel);
            var barCode = MapInventoryFieldSettingModel(defaultFieldSettingsModel.BarCodeFieldSettingModel);
            var purchaseDate = MapInventoryFieldSettingModel(defaultFieldSettingsModel.PurchaseDateFieldSettingModel);
            var place = MapInventoryFieldSettingModel(defaultFieldSettingsModel.PlaceFieldSettingModel);
            var workstation = MapInventoryFieldSettingModel(defaultFieldSettingsModel.WorkstationFieldSettingModel);
            var info = MapInventoryFieldSettingModel(defaultFieldSettingsModel.InfoFieldSettingModel);

            var defaultFieldSettings = new DefaultFieldSettings(
                department,
                name,
                model,
                manufacturer,
                serial,
                theftMark,
                barCode,
                purchaseDate,
                place,
                workstation,
                info);
            return defaultFieldSettings;
        }

        private static InventoryFieldSetting MapInventoryFieldSettingModel(InventoryFieldSettingModel setting)
        {
            var settingModel = new InventoryFieldSetting(
                setting.Caption,
                setting.PropertySize,
                setting.ShowInDetails,
                setting.ShowInList);

            return settingModel;
        }
    }
}