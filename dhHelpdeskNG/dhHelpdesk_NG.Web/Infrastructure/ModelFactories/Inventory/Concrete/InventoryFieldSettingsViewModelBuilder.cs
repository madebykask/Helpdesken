namespace DH.Helpdesk.Web.Infrastructure.ModelFactories.Inventory.Concrete
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Web.Mvc;

    using DH.Helpdesk.BusinessData.Enums.Inventory.Fields.Inventory;
    using DH.Helpdesk.BusinessData.Models.Inventory.Edit.Inventory;
    using DH.Helpdesk.BusinessData.Models.Inventory.Edit.Settings;
    using DH.Helpdesk.BusinessData.Models.Inventory.Edit.Settings.InventorySettings;
    using DH.Helpdesk.Services.Response.Inventory;
    using DH.Helpdesk.Web.Areas.Inventory.Models.EditModel.Settings;
    using DH.Helpdesk.Web.Areas.Inventory.Models.EditModel.Settings.Inventory;

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
            var createdDate = MapInventoryFieldSetting(response.InventoryFieldSettings.DefaultSettings.CreatedDateFieldSetting);
            var changedDate = MapInventoryFieldSetting(response.InventoryFieldSettings.DefaultSettings.ChangedDateFieldSetting);

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
                info,
                createdDate,
                changedDate);

            var inventoryDynamicFieldSettings = response.InventoryDynamicFieldSettings.Select(
                    dynamicSetting => MapInventoryDynamicFieldSetting(dynamicSetting, groupModels, inventoryType.Id)).ToList();

            var inventoryTypeModel = new InventoryTypeModel(inventoryType.Id, inventoryType.Name);
            var newDynamicFieldSettingViewModel = CreateNewInventoryDynamicFieldSettingViewModel(groupModels);

            var viewModel = new InventoryFieldSettingsEditViewModel(inventoryTypeModel,
                new InventoryFieldSettingsViewModel(newDynamicFieldSettingViewModel, defaultFieldSettingsModel, inventoryDynamicFieldSettings));

            return viewModel;
        }

        public InventoryFieldSettingsEditViewModel BuildDefaultViewModel(List<TypeGroupModel> groupModels)
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
            var createdDate = InventoryFieldSettingModel.GetDefault(12, InventoryFieldNames.CreatedDate);
            var changedDate = InventoryFieldSettingModel.GetDefault(12, InventoryFieldNames.ChangedDate);

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
                info,
                createdDate,
                changedDate);

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

        private static NewInventoryDynamicFieldSettingViewModel CreateNewInventoryDynamicFieldSettingViewModel(
            List<TypeGroupModel> groupModels)
        {
            var fieldTypesSelectList = CreateFieldTypesSelectList(null);

            var newDynamicFieldSettingModel = new NewInventoryDynamicFieldSettingModel { PropertySize = 50 };
            var newDynamicFieldSettingViewModel =
                new NewInventoryDynamicFieldSettingViewModel(
                    newDynamicFieldSettingModel,
                    new SelectList(groupModels, "Id", "Name"),
                    fieldTypesSelectList);
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

            var fieldTypesSelectList =
                CreateFieldTypesSelectList(((int)setting.FieldType).ToString(CultureInfo.InvariantCulture));

            var settingsViewModel = new InventoryDynamicFieldSettingViewModel(
                settingModel,
                groupSelectList,
                fieldTypesSelectList);

            return settingsViewModel;
        }

        private static SelectList CreateFieldTypesSelectList(string selectedValue)
        {
            var fieldTypes = from Enum d in Enum.GetValues(typeof(FieldTypes))
                             select
                                 new
                                     {
                                         Value = Convert.ToInt32(d).ToString(CultureInfo.InvariantCulture),
                                         Name = Translation.Get(d.ToString())
                                     };
            var fieldTypesSelectList = new SelectList(fieldTypes, "Value", "Name", selectedValue);
            return fieldTypesSelectList;
        }
    }
}
