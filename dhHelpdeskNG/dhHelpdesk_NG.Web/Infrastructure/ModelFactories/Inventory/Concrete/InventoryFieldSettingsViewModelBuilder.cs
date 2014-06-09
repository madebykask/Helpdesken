namespace DH.Helpdesk.Web.Infrastructure.ModelFactories.Inventory.Concrete
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;

    using DH.Helpdesk.BusinessData.Enums.Inventory.Fields.Inventory;
    using DH.Helpdesk.BusinessData.Models.Inventory.Edit.Inventory;
    using DH.Helpdesk.BusinessData.Models.Inventory.Edit.Settings;
    using DH.Helpdesk.BusinessData.Models.Inventory.Edit.Settings.InventorySettings;
    using DH.Helpdesk.Services.Response.Inventory;
    using DH.Helpdesk.Web.Models.Inventory.EditModel.Settings;
    using DH.Helpdesk.Web.Models.Inventory.EditModel.Settings.Inventory;

    public class InventoryFieldSettingsEditViewModelBuilder : IInventoryFieldSettingsEditViewModelBuilder
    {
        public InventoryFieldSettingsEditViewModel BuildViewModel(
            InventoryType inventoryType,
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
                    dynamicSetting => MapInventoryDynamicFieldSetting(dynamicSetting, groupModels, inventoryType.Id)).ToList();

            var inventoryTypeModel = new InventoryTypeModel(inventoryType.Id, inventoryType.Name);
            var newDynamicFieldSettingViewModel = CreateNewInventoryDynamicFieldSettingViewModel(groupModels);

            var viewModel = new InventoryFieldSettingsEditViewModel(
                inventoryTypeModel,
                new InventoryFieldSettingsViewModel(newDynamicFieldSettingViewModel, defaultFieldSettingsModel, inventoryDynamicFieldSettings));

            return viewModel;
        }

        public InventoryFieldSettingsEditViewModel BuildDefaultViewModel(
            List<TypeGroupModel> groupModels)
        {
            var department = InventoryFieldSettingModel.GetDefault(null, InventoryFieldNames.Department);
            var name = InventoryFieldSettingModel.GetDefault(50, InventoryFieldNames.Name);
            var model = InventoryFieldSettingModel.GetDefault(50, InventoryFieldNames.Model);
            var manufacturer = InventoryFieldSettingModel.GetDefault(50, InventoryFieldNames.Manufacturer);
            var serial = InventoryFieldSettingModel.GetDefault(50, InventoryFieldNames.SerialNumber);
            var theftMark = InventoryFieldSettingModel.GetDefault(20, InventoryFieldNames.TheftMark);
            var barCode = InventoryFieldSettingModel.GetDefault(20, InventoryFieldNames.BarCode);
            var purchaseDate = InventoryFieldSettingModel.GetDefault(12, InventoryFieldNames.PurchaseDate);
            var place = InventoryFieldSettingModel.GetDefault(null, InventoryFieldNames.Place);
            var workstation = InventoryFieldSettingModel.GetDefault(null, InventoryFieldNames.Workstation);
            var info = InventoryFieldSettingModel.GetDefault(1000, InventoryFieldNames.Info);

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

            var inventoryTypeModel = InventoryTypeModel.CreateDefault();
            var newDynamicFieldSettingViewModel = CreateNewInventoryDynamicFieldSettingViewModel(groupModels);

            var viewModel = new InventoryFieldSettingsEditViewModel(
                inventoryTypeModel,
                new InventoryFieldSettingsViewModel(
                    newDynamicFieldSettingViewModel,
                    defaultFieldSettingsModel,
                    new List<InventoryDynamicFieldSettingViewModel>()));

            return viewModel;
        }

        private static NewInventoryDynamicFieldSettingViewModel CreateNewInventoryDynamicFieldSettingViewModel(List<TypeGroupModel> groupModels)
        {
            var newDynamicFieldSettingModel = new NewInventoryDynamicFieldSettingModel { PropertySize = 50 };
            var newDynamicFieldSettingViewModel = new NewInventoryDynamicFieldSettingViewModel(
                newDynamicFieldSettingModel,
                new SelectList(groupModels, "Id", "Name"));
            return newDynamicFieldSettingViewModel;
        }

        private static InventoryFieldSettingModel MapInventoryFieldSetting(InventoryFieldSetting setting)
        {
            var settingModel = new InventoryFieldSettingModel(
                setting.Caption,
                setting.PropertySize,
                setting.ShowInDetails,
                setting.ShowInList);

            return settingModel;
        }

        private static InventoryDynamicFieldSettingViewModel MapInventoryDynamicFieldSetting(
            InventoryDynamicFieldSetting setting,
            List<TypeGroupModel> groupModels,
            int inventoryTypeId)
        {
            var groupSelectList = new SelectList(groupModels, "Id", "Name", setting.InventoryTypeGroupId.ToString());

            var settingModel = new InventoryDynamicFieldSettingModel(
                setting.Id,
                inventoryTypeId,
                setting.InventoryTypeGroupId,
                setting.Caption,
                setting.Position,
                setting.FieldType,
                setting.PropertySize,
                setting.ShowInDetails,
                setting.ShowInList);

            var settingsViewModel = new InventoryDynamicFieldSettingViewModel(settingModel, groupSelectList);

            return settingsViewModel;
        }
    }
}
