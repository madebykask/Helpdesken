namespace DH.Helpdesk.Dal.Repositories.Inventory.Concrete
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;

    using DH.Helpdesk.BusinessData.Models.Inventory.Edit.Settings;
    using DH.Helpdesk.BusinessData.Models.Inventory.Edit.Settings.InventorySettings;
    using DH.Helpdesk.BusinessData.Models.Inventory.Output.Settings.ModelEdit.InventorySettings;
    using DH.Helpdesk.BusinessData.Models.Inventory.Output.Settings.ModelOverview;
    using DH.Helpdesk.BusinessData.Models.Inventory.Output.Settings.ModelOverview.InventoryFieldSettings;
    using DH.Helpdesk.Common.Collections;
    using DH.Helpdesk.Common.Extensions.Boolean;
    using DH.Helpdesk.Common.Extensions.Integer;
    using DH.Helpdesk.Dal.Dal;
    using DH.Helpdesk.Dal.Enums.Inventory.Inventory;
    using DH.Helpdesk.Dal.Infrastructure;
    using DH.Helpdesk.Dal.MapperData.Inventory;

    public class InventoryFieldSettingsRepository : Repository<Domain.Inventory.InventoryTypeProperty>, IInventoryFieldSettingsRepository
    {
        public InventoryFieldSettingsRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }

        public void Update(InventoryFieldSettings businessModel)
        {
            var fieldSettings = this.GetSettings(businessModel.InventoryTypeId).ToList();
            var fieldSettingCollection = new NamedObjectCollection<Domain.Inventory.InventoryTypeProperty>(fieldSettings);

            MapFieldSetting(businessModel.DefaultSettings.DepartmentFieldSetting, fieldSettingCollection.FindByName(InventoryFields.Department.ToString(CultureInfo.InvariantCulture)), businessModel.ChangedDate);
            MapFieldSetting(businessModel.DefaultSettings.NameFieldSetting, fieldSettingCollection.FindByName(InventoryFields.Name.ToString(CultureInfo.InvariantCulture)), businessModel.ChangedDate);
            MapFieldSetting(businessModel.DefaultSettings.ModelFieldSetting, fieldSettingCollection.FindByName(InventoryFields.Model.ToString(CultureInfo.InvariantCulture)), businessModel.ChangedDate);
            MapFieldSetting(businessModel.DefaultSettings.ManufacturerFieldSetting, fieldSettingCollection.FindByName(InventoryFields.Manufacturer.ToString(CultureInfo.InvariantCulture)), businessModel.ChangedDate);
            MapFieldSetting(businessModel.DefaultSettings.SerialNumberFieldSetting, fieldSettingCollection.FindByName(InventoryFields.SerialNumber.ToString(CultureInfo.InvariantCulture)), businessModel.ChangedDate);
            MapFieldSetting(businessModel.DefaultSettings.TheftMarkFieldSetting, fieldSettingCollection.FindByName(InventoryFields.TheftMark.ToString(CultureInfo.InvariantCulture)), businessModel.ChangedDate);
            MapFieldSetting(businessModel.DefaultSettings.BarCodeFieldSetting, fieldSettingCollection.FindByName(InventoryFields.BarCode.ToString(CultureInfo.InvariantCulture)), businessModel.ChangedDate);
            MapFieldSetting(businessModel.DefaultSettings.PurchaseDateFieldSetting, fieldSettingCollection.FindByName(InventoryFields.PurchaseDate.ToString(CultureInfo.InvariantCulture)), businessModel.ChangedDate);
            MapFieldSetting(businessModel.DefaultSettings.PlaceFieldSetting, fieldSettingCollection.FindByName(InventoryFields.Place.ToString(CultureInfo.InvariantCulture)), businessModel.ChangedDate);
            MapFieldSetting(businessModel.DefaultSettings.WorkstationFieldSetting, fieldSettingCollection.FindByName(InventoryFields.Workstation.ToString(CultureInfo.InvariantCulture)), businessModel.ChangedDate);
            MapFieldSetting(businessModel.DefaultSettings.InfoFieldSetting, fieldSettingCollection.FindByName(InventoryFields.Info.ToString(CultureInfo.InvariantCulture)), businessModel.ChangedDate);
        }

        public InventoryFieldSettings GetFieldSettingsForEdit(int inventoryTypeId)
        {
            var settings = this.GetSettings(inventoryTypeId);

            var mapperData =
                settings.Select(
                    s =>
                    new InventoryFieldSettingMapperData
                    {
                        Caption = s.PropertyValue,
                        FieldName = s.PropertyType.ToString(CultureInfo.InvariantCulture),
                        ShowInDetails = s.Show,
                        ShowInList = s.ShowInList,
                        PropertySize = s.PropertySize,
                        Position = s.PropertyPos
                    }).ToList();

            var settingCollection = new NamedObjectCollection<InventoryFieldSettingMapperData>(mapperData);
            var department = CreateFieldSetting(settingCollection.FindByName(InventoryFields.Department.ToString(CultureInfo.InvariantCulture)), FieldTypes.Numeric);
            var name = CreateFieldSetting(settingCollection.FindByName(InventoryFields.Name.ToString(CultureInfo.InvariantCulture)), FieldTypes.Text);
            var model = CreateFieldSetting(settingCollection.FindByName(InventoryFields.Model.ToString(CultureInfo.InvariantCulture)), FieldTypes.Text);
            var manufacturer = CreateFieldSetting(settingCollection.FindByName(InventoryFields.Manufacturer.ToString(CultureInfo.InvariantCulture)), FieldTypes.Text);
            var serialNumber = CreateFieldSetting(settingCollection.FindByName(InventoryFields.SerialNumber.ToString(CultureInfo.InvariantCulture)), FieldTypes.Text);
            var theftMark = CreateFieldSetting(settingCollection.FindByName(InventoryFields.TheftMark.ToString(CultureInfo.InvariantCulture)), FieldTypes.Text);
            var barCode = CreateFieldSetting(settingCollection.FindByName(InventoryFields.BarCode.ToString(CultureInfo.InvariantCulture)), FieldTypes.Text);
            var purchaseDate = CreateFieldSetting(settingCollection.FindByName(InventoryFields.PurchaseDate.ToString(CultureInfo.InvariantCulture)), FieldTypes.Text);
            var place = CreateFieldSetting(settingCollection.FindByName(InventoryFields.Place.ToString(CultureInfo.InvariantCulture)), FieldTypes.Text);
            var workstation = CreateFieldSetting(settingCollection.FindByName(InventoryFields.Workstation.ToString(CultureInfo.InvariantCulture)), FieldTypes.Text);
            var info = CreateFieldSetting(settingCollection.FindByName(InventoryFields.Info.ToString(CultureInfo.InvariantCulture)), FieldTypes.Text);

            var settingAgregate =
                InventoryFieldSettings.CreateForEdit(
                    new BusinessData.Models.Inventory.Edit.Settings.InventorySettings.DefaultFieldSettings(
                        department,
                        name,
                        model,
                        manufacturer,
                        serialNumber,
                        theftMark,
                        barCode,
                        purchaseDate,
                        place,
                        workstation,
                        info));

            return settingAgregate;
        }

        public InventoryFieldSettingsForModelEdit GetFieldSettingsForModelEdit(int inventoryTypeId)
        {
            var settings = this.GetSettings(inventoryTypeId);

            var mapperData =
                settings.Select(
                    s =>
                    new InventoryFieldSettingMapperDataForModelEdit
                    {
                        Caption = s.PropertyValue,
                        FieldName = s.PropertyType.ToString(CultureInfo.InvariantCulture),
                        Show = s.Show,
                        PropertySize = s.PropertySize
                    }).ToList();

            var settingCollection = new NamedObjectCollection<InventoryFieldSettingMapperDataForModelEdit>(mapperData);
            var department = CreateFieldSettingForModelEdit(settingCollection.FindByName(InventoryFields.Department.ToString(CultureInfo.InvariantCulture)), FieldTypes.Numeric);
            var name = CreateFieldSettingForModelEdit(settingCollection.FindByName(InventoryFields.Name.ToString(CultureInfo.InvariantCulture)), FieldTypes.Text);
            var model = CreateFieldSettingForModelEdit(settingCollection.FindByName(InventoryFields.Model.ToString(CultureInfo.InvariantCulture)), FieldTypes.Text);
            var manufacturer = CreateFieldSettingForModelEdit(settingCollection.FindByName(InventoryFields.Manufacturer.ToString(CultureInfo.InvariantCulture)), FieldTypes.Text);
            var serialNumber = CreateFieldSettingForModelEdit(settingCollection.FindByName(InventoryFields.SerialNumber.ToString(CultureInfo.InvariantCulture)), FieldTypes.Text);
            var theftMark = CreateFieldSettingForModelEdit(settingCollection.FindByName(InventoryFields.TheftMark.ToString(CultureInfo.InvariantCulture)), FieldTypes.Text);
            var barCode = CreateFieldSettingForModelEdit(settingCollection.FindByName(InventoryFields.BarCode.ToString(CultureInfo.InvariantCulture)), FieldTypes.Text);
            var purchaseDate = CreateFieldSettingForModelEdit(settingCollection.FindByName(InventoryFields.PurchaseDate.ToString(CultureInfo.InvariantCulture)), FieldTypes.Text);
            var place = CreateFieldSettingForModelEdit(settingCollection.FindByName(InventoryFields.Place.ToString(CultureInfo.InvariantCulture)), FieldTypes.Text);
            var workstation = CreateFieldSettingForModelEdit(settingCollection.FindByName(InventoryFields.Workstation.ToString(CultureInfo.InvariantCulture)), FieldTypes.Text);
            var info = CreateFieldSettingForModelEdit(settingCollection.FindByName(InventoryFields.Info.ToString(CultureInfo.InvariantCulture)), FieldTypes.Text);

            var settingAgregate =
                new InventoryFieldSettingsForModelEdit(
                    new BusinessData.Models.Inventory.Output.Settings.ModelEdit.InventorySettings.DefaultFieldSettings(
                        department,
                        name,
                        model,
                        manufacturer,
                        serialNumber,
                        theftMark,
                        barCode,
                        purchaseDate,
                        place,
                        workstation,
                        info));

            return settingAgregate;
        }

        public List<InventoryFieldSettingsOverviewWithType> GetFieldSettingsOverviews(List<int> inventoryTypeIds)
        {
            var overviews = new List<InventoryFieldSettingsOverviewWithType>();

            var settings =
                this.DbSet.Where(x => inventoryTypeIds.Contains(x.InventoryType_Id))
                    .Select(
                        s =>
                        new
                            {
                                FieldName = s.PropertyType,
                                Caption = s.PropertyValue,
                                Show = s.ShowInList,
                                TypeId = s.InventoryType_Id,
                                TypeName = s.InventoryType.Name,
                            })
                    .ToList()
                    .GroupBy(x => new { x.TypeId, x.TypeName });

            foreach (var item in settings)
            {
                var mapperData =
                    item.Select(
                        s =>
                        new FieldOverviewSettingMapperData
                            {
                                FieldName =
                                    s.FieldName.ToString(CultureInfo.InvariantCulture),
                                Caption = s.Caption,
                                Show = s.Show
                            }).ToList();

                var settingAgregate = GetInventoryFieldSettingsOverview(mapperData);

                var overview = new InventoryFieldSettingsOverviewWithType(
                    item.Key.TypeId,
                    item.Key.TypeName,
                    settingAgregate);

                overviews.Add(overview);
            }

            return overviews;
        }

        public InventoryFieldSettingsOverview GetFieldSettingsOverview(int inventoryTypeId)
        {
            var settings = this.GetSettings(inventoryTypeId);

            var anonymus =
                settings.Select(
                    s =>
                    new
                        {
                            FieldName = s.PropertyType,
                            Caption = s.PropertyValue,
                            Show = s.ShowInList
                        }).ToList();

            var mapperData =
                anonymus.Select(
                    s =>
                    new FieldOverviewSettingMapperData
                        {
                            FieldName = s.FieldName.ToString(CultureInfo.InvariantCulture),
                            Caption = s.Caption,
                            Show = s.Show
                        }).ToList();

            var settingAgregate = GetInventoryFieldSettingsOverview(mapperData);

            return settingAgregate;
        }

        public InventoryFieldsSettingsOverviewForFilter GetFieldSettingsOverviewForFilter(int inventoryTypeId)
        {
            var settings = this.GetSettings(inventoryTypeId).Where(x => x.PropertyType == InventoryFields.Department);

            var mapperData = settings.Select(
                s =>
                new
                    {
                        Caption = s.PropertyValue,
                        s.Show
                    }).Single();

            var setting = new FieldSettingOverview(mapperData.Show.ToBool(), mapperData.Caption);
            var overview = new InventoryFieldsSettingsOverviewForFilter(setting);

            return overview;
        }

        private static InventoryFieldSettingsOverview GetInventoryFieldSettingsOverview(List<FieldOverviewSettingMapperData> mapperData)
        {
            var settingCollection = new NamedObjectCollection<FieldOverviewSettingMapperData>(mapperData);
            var department =
                CreateFieldSettingOverview(
                    settingCollection.FindByName(InventoryFields.Department.ToString(CultureInfo.InvariantCulture)));
            var name =
                CreateFieldSettingOverview(
                    settingCollection.FindByName(InventoryFields.Name.ToString(CultureInfo.InvariantCulture)));
            var model =
                CreateFieldSettingOverview(
                    settingCollection.FindByName(InventoryFields.Model.ToString(CultureInfo.InvariantCulture)));
            var manufacturer =
                CreateFieldSettingOverview(
                    settingCollection.FindByName(InventoryFields.Manufacturer.ToString(CultureInfo.InvariantCulture)));
            var serialNumber =
                CreateFieldSettingOverview(
                    settingCollection.FindByName(InventoryFields.SerialNumber.ToString(CultureInfo.InvariantCulture)));
            var theftMark =
                CreateFieldSettingOverview(
                    settingCollection.FindByName(InventoryFields.TheftMark.ToString(CultureInfo.InvariantCulture)));
            var barCode =
                CreateFieldSettingOverview(
                    settingCollection.FindByName(InventoryFields.BarCode.ToString(CultureInfo.InvariantCulture)));
            var purchaseDate =
                CreateFieldSettingOverview(
                    settingCollection.FindByName(InventoryFields.PurchaseDate.ToString(CultureInfo.InvariantCulture)));
            var place =
                CreateFieldSettingOverview(
                    settingCollection.FindByName(InventoryFields.Place.ToString(CultureInfo.InvariantCulture)));
            var workstation =
                CreateFieldSettingOverview(
                    settingCollection.FindByName(InventoryFields.Workstation.ToString(CultureInfo.InvariantCulture)));
            var info =
                CreateFieldSettingOverview(
                    settingCollection.FindByName(InventoryFields.Info.ToString(CultureInfo.InvariantCulture)));

            var settingAgregate =
                    new BusinessData.Models.Inventory.Output.Settings.ModelOverview.InventoryFieldSettings.DefaultFieldSettings(
                        department,
                        name,
                        model,
                        manufacturer,
                        serialNumber,
                        theftMark,
                        barCode,
                        purchaseDate,
                        place,
                        workstation,
                        info);

            return new InventoryFieldSettingsOverview(settingAgregate);
        }

        private static FieldSettingOverview CreateFieldSettingOverview(FieldOverviewSettingMapperData fieldSetting)
        {
            return new FieldSettingOverview(fieldSetting.Show.ToBool(), fieldSetting.Caption);
        }

        private static InventoryFieldSettingForModelEdit CreateFieldSettingForModelEdit(InventoryFieldSettingMapperDataForModelEdit fieldSetting, FieldTypes fieldType)
        {
            return new InventoryFieldSettingForModelEdit(fieldSetting.Caption, fieldType, fieldSetting.PropertySize, fieldSetting.Show.ToBool());
        }

        private static InventoryFieldSetting CreateFieldSetting(InventoryFieldSettingMapperData fieldSetting, FieldTypes fieldType)
        {
            return new InventoryFieldSetting(
                fieldSetting.Caption,
                fieldSetting.Position,
                fieldType,
                fieldSetting.PropertySize,
                fieldSetting.ShowInDetails.ToBool(),
                fieldSetting.ShowInList.ToBool());
        }

        private static void MapFieldSetting(
            InventoryFieldSetting updatedSetting,
            Domain.Inventory.InventoryTypeProperty fieldSetting,
            DateTime changedDate)
        {
            fieldSetting.ChangedDate = changedDate;
            fieldSetting.Show = updatedSetting.ShowInDetails.ToInt();
            fieldSetting.ShowInList = updatedSetting.ShowInList.ToInt();
            fieldSetting.PropertyValue = updatedSetting.Caption;
        }

        private IQueryable<Domain.Inventory.InventoryTypeProperty> GetSettings(int inventoryTypeId)
        {
            return this.DbSet.Where(x => x.InventoryType_Id == inventoryTypeId);
        }
    }
}