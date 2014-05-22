namespace DH.Helpdesk.Web.Infrastructure.ModelFactories.Inventory.Concrete
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;

    using DH.Helpdesk.BusinessData.Models.Inventory.Edit.Inventory;
    using DH.Helpdesk.BusinessData.Models.Inventory.Output.Settings.ModelEdit.InventorySettings;
    using DH.Helpdesk.BusinessData.Models.Shared;
    using DH.Helpdesk.Web.Models.Inventory.EditModel.Inventory;
    using DH.Helpdesk.Web.Models.Inventory.OptionsAggregates;

    public class InventoryViewModelBuilder : IInventoryViewModelBuilder
    {
        public static ConfigurableFieldModel<string> CreateStringField(InventoryFieldSettingForModelEdit setting, string value)
        {
            return !setting.ShowInDetails
                       ? ConfigurableFieldModel<string>.CreateUnshowable()
                       : new ConfigurableFieldModel<string>(setting.Caption, value);
        }

        public static SelectList CreateSelectList(
            InventoryFieldSettingForModelEdit setting,
            List<ItemOverview> items,
            string selectedValue)
        {
            if (!setting.ShowInDetails)
            {
                return new SelectList(Enumerable.Empty<SelectListItem>());
            }

            var list = new SelectList(items, "Value", "Name", selectedValue);
            return list;
        }

        public static ConfigurableFieldModel<SelectList> CreateSelectListField(
            InventoryFieldSettingForModelEdit setting,
            List<ItemOverview> items,
            string selectedValue)
        {
            if (!setting.ShowInDetails)
            {
                return ConfigurableFieldModel<SelectList>.CreateUnshowable();
            }

            var list = new SelectList(items, "Value", "Name", selectedValue);
            return new ConfigurableFieldModel<SelectList>(setting.Caption, list);
        }

        public static ConfigurableFieldModel<DateTime?> CreateNullableDateTimeField(
            InventoryFieldSettingForModelEdit setting,
            DateTime? value)
        {
            return !setting.ShowInDetails
                       ? ConfigurableFieldModel<DateTime?>.CreateUnshowable()
                       : new ConfigurableFieldModel<DateTime?>(setting.Caption, value);
        }

        public static ConfigurableFieldModel<DateTime> CreateDateTimeField(InventoryFieldSettingForModelEdit setting, DateTime value)
        {
            return !setting.ShowInDetails
                       ? ConfigurableFieldModel<DateTime>.CreateUnshowable()
                       : new ConfigurableFieldModel<DateTime>(setting.Caption, value);
        }

        public InventoryViewModel BuildViewModel(
            Inventory model,
            InventoryEditOptions options,
            InventoryFieldSettingsForModelEdit settings)
        {
            const string Delimeter = "; ";

            var name = CreateStringField(settings.DefaultSettings.NameFieldSetting, model.Name);
            var inventoryModel = CreateStringField(settings.DefaultSettings.ModelFieldSetting, model.Model);
            var manufacturer = CreateStringField(settings.DefaultSettings.ManufacturerFieldSetting, model.Manufacturer);
            var serial = CreateStringField(settings.DefaultSettings.SerialNumberFieldSetting, model.SerialNumber);
            var theftMark = CreateStringField(settings.DefaultSettings.TheftMarkFieldSetting, model.TheftMark);
            var barCode = CreateStringField(settings.DefaultSettings.BarCodeFieldSetting, model.BarCode);
            var purchaseDate = CreateNullableDateTimeField(settings.DefaultSettings.PurchaseDateFieldSetting, model.PurchaseDate);
            var workstation = CreateStringField(settings.DefaultSettings.WorkstationFieldSetting, string.Join(Delimeter, model.Workstations));
            var info = CreateStringField(settings.DefaultSettings.InfoFieldSetting, model.Info);

            var buildings = CreateSelectList(
                settings.DefaultSettings.PlaceFieldSetting,
                options.Buildings,
                model.BuildingId.ToString());
            var floors = CreateSelectList(
                settings.DefaultSettings.PlaceFieldSetting,
                options.Floors,
                model.FloorId.ToString());
            var rooms =
                CreateSelectListField(
                    settings.DefaultSettings.PlaceFieldSetting,
                    options.Rooms,
                    model.RoomId.ToString());

            var departments = CreateSelectListField(
                settings.DefaultSettings.DepartmentFieldSetting,
                options.Departments,
                model.DepartmentId.ToString());

            var defaultFieldsModel = new DefaultFieldsModel(
                model.DepartmentId,
                model.BuildingId,
                model.FloorId,
                model.RoomId,
                model.ChangeByUserId,
                name,
                inventoryModel,
                manufacturer,
                serial,
                theftMark,
                barCode,
                purchaseDate,
                workstation,
                info);

            var defaultFieldsViewModel = new DefaultFieldsViewModel(
                defaultFieldsModel,
                departments,
                buildings,
                floors,
                rooms);

            return new InventoryViewModel(model.InventoryTypeId, defaultFieldsViewModel)
                       {
                           Id = model.Id,
                           CreatedDate =
                               model.CreatedDate,
                           ChangeDate = model.ChangeDate
                       };
        }

        public InventoryViewModel BuildViewModel(
            InventoryEditOptions options,
            InventoryFieldSettingsForModelEdit settings,
            int inventoryTypeId,
            int currentCustomerId)
        {
            var name = CreateStringField(settings.DefaultSettings.NameFieldSetting, null);
            var inventoryModel = CreateStringField(settings.DefaultSettings.ModelFieldSetting, null);
            var manufacturer = CreateStringField(settings.DefaultSettings.ManufacturerFieldSetting, null);
            var serial = CreateStringField(settings.DefaultSettings.SerialNumberFieldSetting, null);
            var theftMark = CreateStringField(settings.DefaultSettings.TheftMarkFieldSetting, null);
            var barCode = CreateStringField(settings.DefaultSettings.BarCodeFieldSetting, null);
            var purchaseDate = CreateNullableDateTimeField(settings.DefaultSettings.PurchaseDateFieldSetting, null);
            var workstation = CreateStringField(settings.DefaultSettings.WorkstationFieldSetting, null);
            var info = CreateStringField(settings.DefaultSettings.InfoFieldSetting, null);

            var buildings = CreateSelectList(
                settings.DefaultSettings.PlaceFieldSetting,
                options.Buildings,
                null);
            var floors = CreateSelectList(
                settings.DefaultSettings.PlaceFieldSetting,
                options.Floors,
                null);
            var rooms =
                CreateSelectListField(
                    settings.DefaultSettings.PlaceFieldSetting,
                    options.Rooms,
                    null);

            var departments = CreateSelectListField(
                settings.DefaultSettings.DepartmentFieldSetting,
                options.Departments,
                null);

            var defaultFieldsModel = new DefaultFieldsModel(
                null,
                null,
                null,
                null,
                null,
                name,
                inventoryModel,
                manufacturer,
                serial,
                theftMark,
                barCode,
                purchaseDate,
                workstation,
                info);

            var defaultFieldsViewModel = new DefaultFieldsViewModel(
                defaultFieldsModel,
                departments,
                buildings,
                floors,
                rooms);

            return new InventoryViewModel(inventoryTypeId, defaultFieldsViewModel);
        }
    }
}