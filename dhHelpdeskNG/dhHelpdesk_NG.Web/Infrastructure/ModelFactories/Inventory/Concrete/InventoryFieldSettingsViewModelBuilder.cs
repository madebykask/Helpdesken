namespace DH.Helpdesk.Web.Infrastructure.ModelFactories.Inventory.Concrete
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;

    using DH.Helpdesk.BusinessData.Models.Inventory.Edit.Inventory;
    using DH.Helpdesk.BusinessData.Models.Inventory.Edit.Settings;
    using DH.Helpdesk.BusinessData.Models.Inventory.Edit.Settings.InventorySettings;
    using DH.Helpdesk.Services.Response.Inventory;
    using DH.Helpdesk.Web.Models.Inventory.EditModel.Settings;
    using DH.Helpdesk.Web.Models.Inventory.EditModel.Settings.Inventory;

    public class InventoryFieldSettingsViewModelBuilder : IInventoryFieldSettingsViewModelBuilder
    {
        public InventoryFieldSettingsViewModel BuildViewModel(
            int inventoryTypeId,
            InventoryFieldSettingsForEditResponse response,
            List<TypeGroupModel> groupModels)
        {
            var department = MapInventoryFieldSetting(response.InventoryFieldSettings.DefaultSettings.DepartmentFieldSetting);
            var name = MapInventoryFieldSetting(response.InventoryFieldSettings.DefaultSettings.NameFieldSetting);
            var model = MapInventoryFieldSetting(response.InventoryFieldSettings.DefaultSettings.ModelFieldSetting);
            var manufacturer = MapInventoryFieldSetting(response.InventoryFieldSettings.DefaultSettings.ManufacturerFieldSetting);
            var serial = MapInventoryFieldSetting(response.InventoryFieldSettings.DefaultSettings.SerialNumberFieldSetting);
            var theftMark = MapInventoryFieldSetting(response.InventoryFieldSettings.DefaultSettings.TheftMarkFieldSetting);
            var barCode = MapInventoryFieldSetting(response.InventoryFieldSettings.DefaultSettings.BarCodeFieldSetting);
            var purchaseDate = MapInventoryFieldSetting(response.InventoryFieldSettings.DefaultSettings.PurchaseDateFieldSetting);
            var place = MapInventoryFieldSetting(response.InventoryFieldSettings.DefaultSettings.PlaceFieldSetting);
            var workstation = MapInventoryFieldSetting(response.InventoryFieldSettings.DefaultSettings.WorkstationFieldSetting);
            var info = MapInventoryFieldSetting(response.InventoryFieldSettings.DefaultSettings.InfoFieldSetting);

            var defaultFieldSettingsModel = new DefaultFieldSettingsModel(
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

            var inventoryDynamicFieldSettings =
                response.InventoryDynamicFieldSettings.Select(
                    dynamicSetting => MapInventoryDynamicFieldSetting(dynamicSetting, groupModels)).ToList();

            var viewModel = new InventoryFieldSettingsViewModel(
                inventoryTypeId,
                defaultFieldSettingsModel,
                inventoryDynamicFieldSettings);

            return viewModel;
        }

        public NewInventoryFieldSettingsViewModel BuildDefaultViewModel(
            int inventoryTypeId,
            List<TypeGroupModel> groupModels)
        {
            var department = InventoryFieldSettingModel.GetDefault(null);
            var name = InventoryFieldSettingModel.GetDefault(50);
            var model = InventoryFieldSettingModel.GetDefault(50);
            var manufacturer = InventoryFieldSettingModel.GetDefault(50);
            var serial = InventoryFieldSettingModel.GetDefault(50);
            var theftMark = InventoryFieldSettingModel.GetDefault(20);
            var barCode = InventoryFieldSettingModel.GetDefault(20);
            var purchaseDate = InventoryFieldSettingModel.GetDefault(12);
            var place = InventoryFieldSettingModel.GetDefault(null);
            var workstation = InventoryFieldSettingModel.GetDefault(null);
            var info = InventoryFieldSettingModel.GetDefault(1000);

            var defaultFieldSettingsModel = new DefaultFieldSettingsModel(
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

            var newDynamicSetting = new InventoryDynamicFieldSettingModel();

            var viewModel = new NewInventoryFieldSettingsViewModel(
                inventoryTypeId,
                defaultFieldSettingsModel,
                newDynamicSetting);

            return viewModel;
        }

        private static InventoryFieldSettingModel MapInventoryFieldSetting(InventoryFieldSetting setting)
        {
            var settingModel = new InventoryFieldSettingModel(
                setting.Caption,
                setting.FieldType,
                setting.PropertySize,
                setting.ShowInDetails,
                setting.ShowInList);

            return settingModel;
        }

        private static InventoryDynamicFieldSettingModel MapInventoryDynamicFieldSetting(
            InventoryDynamicFieldSetting setting,
            List<TypeGroupModel> groupModels)
        {
            var groupSelectList = new SelectList(groupModels, "Id", "Name");

            var settingModel = new InventoryDynamicFieldSettingModel(
                setting.Id,
                setting.InventoryTypeGroupId,
                setting.Caption,
                setting.Position,
                setting.FieldType,
                setting.PropertySize,
                setting.ShowInDetails,
                setting.ShowInList,
                groupSelectList);

            return settingModel;
        }
    }
}
