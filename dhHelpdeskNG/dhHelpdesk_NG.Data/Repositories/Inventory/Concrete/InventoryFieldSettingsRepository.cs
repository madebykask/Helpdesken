namespace DH.Helpdesk.Dal.Repositories.Inventory.Concrete
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Globalization;
    using System.Linq;

    using DH.Helpdesk.BusinessData.Models.Inventory.Edit.Settings;
    using DH.Helpdesk.BusinessData.Models.Inventory.Edit.Settings.InventorySettings;
    using DH.Helpdesk.BusinessData.Models.Inventory.Output.Settings.ModelEdit.InventorySettings;
    using DH.Helpdesk.BusinessData.Models.Inventory.Output.Settings.ModelOverview;
    using DH.Helpdesk.BusinessData.Models.Inventory.Output.Settings.ModelOverview.InventoryFieldSettings;
    using DH.Helpdesk.BusinessData.Models.Inventory.Output.Settings.ProcessingSetttings.InventorySettings;
    using DH.Helpdesk.Common.Collections;
    using DH.Helpdesk.Common.Extensions.Boolean;
    using DH.Helpdesk.Common.Extensions.Integer;
    using DH.Helpdesk.Dal.Dal;
    using DH.Helpdesk.Dal.Enums.Inventory.Inventory;
    using DH.Helpdesk.Dal.Infrastructure;
    using DH.Helpdesk.Dal.MapperData.Inventory;

    using InventoryFieldSettingsForModelEdit = DH.Helpdesk.BusinessData.Models.Inventory.Output.Settings.ModelEdit.InventorySettings.InventoryFieldSettingsForModelEdit;

    public class InventoryFieldSettingsRepository : Repository<Domain.Inventory.InventoryTypeProperty>, IInventoryFieldSettingsRepository
    {
        public const int DefaultPosition = 0;

        public const int DefaultPropertySize = 50;

        public const string PropertyDefaultValue = "";

        public const int MinDynamicSettingTypeId = 0;

        public InventoryFieldSettingsRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }

        public void Add(InventoryFieldSettings businessModel)
        {
            AddFieldSetting(businessModel.InventoryTypeId, InventoryFields.Department, businessModel.DefaultSettings.DepartmentFieldSetting, businessModel.CreatedDate, this.DbSet);
            AddFieldSetting(businessModel.InventoryTypeId, InventoryFields.Name, businessModel.DefaultSettings.NameFieldSetting, businessModel.CreatedDate, this.DbSet);
            AddFieldSetting(businessModel.InventoryTypeId, InventoryFields.Model, businessModel.DefaultSettings.ModelFieldSetting, businessModel.CreatedDate, this.DbSet);
            AddFieldSetting(businessModel.InventoryTypeId, InventoryFields.Manufacturer, businessModel.DefaultSettings.ManufacturerFieldSetting, businessModel.CreatedDate, this.DbSet);
            AddFieldSetting(businessModel.InventoryTypeId, InventoryFields.SerialNumber, businessModel.DefaultSettings.SerialNumberFieldSetting, businessModel.CreatedDate, this.DbSet);
            AddFieldSetting(businessModel.InventoryTypeId, InventoryFields.TheftMark, businessModel.DefaultSettings.TheftMarkFieldSetting, businessModel.CreatedDate, this.DbSet);
            AddFieldSetting(businessModel.InventoryTypeId, InventoryFields.BarCode, businessModel.DefaultSettings.BarCodeFieldSetting, businessModel.CreatedDate, this.DbSet);
            AddFieldSetting(businessModel.InventoryTypeId, InventoryFields.PurchaseDate, businessModel.DefaultSettings.PurchaseDateFieldSetting, businessModel.CreatedDate, this.DbSet);
            AddFieldSetting(businessModel.InventoryTypeId, InventoryFields.Place, businessModel.DefaultSettings.PlaceFieldSetting, businessModel.CreatedDate, this.DbSet);
            AddFieldSetting(businessModel.InventoryTypeId, InventoryFields.Workstation, businessModel.DefaultSettings.WorkstationFieldSetting, businessModel.CreatedDate, this.DbSet);
            AddFieldSetting(businessModel.InventoryTypeId, InventoryFields.Info, businessModel.DefaultSettings.InfoFieldSetting, businessModel.CreatedDate, this.DbSet);
            AddFieldSetting(businessModel.InventoryTypeId, InventoryFields.CreatedDate, businessModel.DefaultSettings.CreatedDateFieldSetting, businessModel.CreatedDate, this.DbSet);
            AddFieldSetting(businessModel.InventoryTypeId, InventoryFields.ChangedDate, businessModel.DefaultSettings.ChangedDateFieldSetting, businessModel.CreatedDate, this.DbSet);
            AddFieldSetting(businessModel.InventoryTypeId, InventoryFields.SyncDate, businessModel.DefaultSettings.SyncDateFieldSetting, businessModel.CreatedDate, this.DbSet);
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
            MapFieldSetting(businessModel.DefaultSettings.CreatedDateFieldSetting, fieldSettingCollection.FindByName(InventoryFields.CreatedDate.ToString(CultureInfo.InvariantCulture)), businessModel.ChangedDate);
            MapFieldSetting(businessModel.DefaultSettings.ChangedDateFieldSetting, fieldSettingCollection.FindByName(InventoryFields.ChangedDate.ToString(CultureInfo.InvariantCulture)), businessModel.ChangedDate);
            MapFieldSetting(businessModel.DefaultSettings.SyncDateFieldSetting, fieldSettingCollection.FindByName(InventoryFields.SyncDate.ToString(CultureInfo.InvariantCulture)), businessModel.ChangedDate);
        }

        public InventoryFieldSettings GetFieldSettingsForEdit(int inventoryTypeId)
        {
            var settings = this.GetSettings(inventoryTypeId);

            var anonymus = settings.Select(s => new { s.PropertyValue, s.PropertyType, s.Show, s.ShowInList, s.PropertySize, s.PropertyPos, s.XMLTag, s.ReadOnly }).ToList();

            var mapperData = anonymus.Select(s =>
                    new InventoryFieldSettingMapperData
                        {
                            Caption = s.PropertyValue,
                            FieldName =
                                s.PropertyType.ToString(CultureInfo.InvariantCulture),
                            ShowInDetails = s.Show,
                            ShowInList = s.ShowInList,
                            PropertySize = s.PropertySize,
                            Position = s.PropertyPos,
                            XMLTag = s.XMLTag,
                            ReadOnly = s.ReadOnly
                        }).ToList();

            var settingCollection = new NamedObjectCollection<InventoryFieldSettingMapperData>(mapperData);
            var department = CreateFieldSetting(settingCollection.FindByName(InventoryFields.Department.ToString(CultureInfo.InvariantCulture)));
            var name = CreateFieldSetting(settingCollection.FindByName(InventoryFields.Name.ToString(CultureInfo.InvariantCulture)));
            var model = CreateFieldSetting(settingCollection.FindByName(InventoryFields.Model.ToString(CultureInfo.InvariantCulture)));
            var manufacturer = CreateFieldSetting(settingCollection.FindByName(InventoryFields.Manufacturer.ToString(CultureInfo.InvariantCulture)));
            var serialNumber = CreateFieldSetting(settingCollection.FindByName(InventoryFields.SerialNumber.ToString(CultureInfo.InvariantCulture)));
            var theftMark = CreateFieldSetting(settingCollection.FindByName(InventoryFields.TheftMark.ToString(CultureInfo.InvariantCulture)));
            var barCode = CreateFieldSetting(settingCollection.FindByName(InventoryFields.BarCode.ToString(CultureInfo.InvariantCulture)));
            var purchaseDate = CreateFieldSetting(settingCollection.FindByName(InventoryFields.PurchaseDate.ToString(CultureInfo.InvariantCulture)));
            var place = CreateFieldSettingWithDefaultPropertySize(settingCollection.FindByName(InventoryFields.Place.ToString(CultureInfo.InvariantCulture)));
            var workstation = CreateFieldSettingWithDefaultPropertySize(settingCollection.FindByName(InventoryFields.Workstation.ToString(CultureInfo.InvariantCulture)));
            var info = CreateFieldSetting(settingCollection.FindByName(InventoryFields.Info.ToString(CultureInfo.InvariantCulture)));
            var createdDate = CreateFieldSetting(settingCollection.FindByName(InventoryFields.CreatedDate.ToString(CultureInfo.InvariantCulture)));
            var changedDate = CreateFieldSetting(settingCollection.FindByName(InventoryFields.ChangedDate.ToString(CultureInfo.InvariantCulture)));
            var syncDate = CreateFieldSetting(settingCollection.FindByName(InventoryFields.SyncDate.ToString(CultureInfo.InvariantCulture)));

            var settingAgregate = InventoryFieldSettings.CreateForEdit(
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
                        info,
                        createdDate,
                        changedDate,
                        syncDate));

            return settingAgregate;
        }

        public InventoryFieldSettingsForModelEdit GetFieldSettingsForModelEdit(int inventoryTypeId, bool isReadonly = false)
        {
            var settings = this.GetSettings(inventoryTypeId);

            var anonymus = settings.Select(s =>
                    new
                    {
                        Caption = s.PropertyValue,
                        FieldName = s.PropertyType,
                        s.Show,
                        s.PropertySize,
                        s.ReadOnly
                    }).ToList();

            var mapperData = anonymus.Select(s =>
                    new InventoryFieldSettingMapperDataForModelEdit
                    {
                        Caption = s.Caption,
                        FieldName = s.FieldName.ToString(CultureInfo.InvariantCulture),
                        Show = s.Show,
                        ReadOnly = isReadonly ? 1 : s.ReadOnly,
                        PropertySize = s.PropertySize
                    }).ToList();

            var settingCollection = new NamedObjectCollection<InventoryFieldSettingMapperDataForModelEdit>(mapperData);
            var department = CreateFieldSettingForModelEdit(settingCollection.FindByName(InventoryFields.Department.ToString(CultureInfo.InvariantCulture)));
            var name = CreateFieldSettingForModelEdit(settingCollection.FindByName(InventoryFields.Name.ToString(CultureInfo.InvariantCulture)));
            var model = CreateFieldSettingForModelEdit(settingCollection.FindByName(InventoryFields.Model.ToString(CultureInfo.InvariantCulture)));
            var manufacturer = CreateFieldSettingForModelEdit(settingCollection.FindByName(InventoryFields.Manufacturer.ToString(CultureInfo.InvariantCulture)));
            var serialNumber = CreateFieldSettingForModelEdit(settingCollection.FindByName(InventoryFields.SerialNumber.ToString(CultureInfo.InvariantCulture)));
            var theftMark = CreateFieldSettingForModelEdit(settingCollection.FindByName(InventoryFields.TheftMark.ToString(CultureInfo.InvariantCulture)));
            var barCode = CreateFieldSettingForModelEdit(settingCollection.FindByName(InventoryFields.BarCode.ToString(CultureInfo.InvariantCulture)));
            var purchaseDate = CreateFieldSettingForModelEdit(settingCollection.FindByName(InventoryFields.PurchaseDate.ToString(CultureInfo.InvariantCulture)));
            var place = CreateFieldSettingForModelEdit(settingCollection.FindByName(InventoryFields.Place.ToString(CultureInfo.InvariantCulture)));
            var workstation = CreateFieldSettingForModelEdit(settingCollection.FindByName(InventoryFields.Workstation.ToString(CultureInfo.InvariantCulture)));
            var info = CreateFieldSettingForModelEdit(settingCollection.FindByName(InventoryFields.Info.ToString(CultureInfo.InvariantCulture)));

            var settingAgregate = new InventoryFieldSettingsForModelEdit(
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

        public InventoryFieldSettingsProcessing GetFieldSettingsForProcessing(int inventoryTypeId)
        {
            var settings = this.GetSettings(inventoryTypeId);

            var anonymus =
                settings.Select(
                    s =>
                    new
                    {
                        FieldName = s.PropertyType,
                        s.Show,
                    }).ToList();

            var mapperData =
                anonymus.Select(
                    s =>
                    new InventoryFieldSettingMapperDataForProcessing
                        {
                            FieldName =
                                s.FieldName.ToString(
                                    CultureInfo.InvariantCulture),
                            Show = s.Show,
                        }).ToList();

            var settingCollection = new NamedObjectCollection<InventoryFieldSettingMapperDataForProcessing>(mapperData);
            var department = CreateFieldSettingForProcessing(settingCollection.FindByName(InventoryFields.Department.ToString(CultureInfo.InvariantCulture)));
            var name = CreateFieldSettingForProcessing(settingCollection.FindByName(InventoryFields.Name.ToString(CultureInfo.InvariantCulture)));
            var model = CreateFieldSettingForProcessing(settingCollection.FindByName(InventoryFields.Model.ToString(CultureInfo.InvariantCulture)));
            var manufacturer = CreateFieldSettingForProcessing(settingCollection.FindByName(InventoryFields.Manufacturer.ToString(CultureInfo.InvariantCulture)));
            var serialNumber = CreateFieldSettingForProcessing(settingCollection.FindByName(InventoryFields.SerialNumber.ToString(CultureInfo.InvariantCulture)));
            var theftMark = CreateFieldSettingForProcessing(settingCollection.FindByName(InventoryFields.TheftMark.ToString(CultureInfo.InvariantCulture)));
            var barCode = CreateFieldSettingForProcessing(settingCollection.FindByName(InventoryFields.BarCode.ToString(CultureInfo.InvariantCulture)));
            var purchaseDate = CreateFieldSettingForProcessing(settingCollection.FindByName(InventoryFields.PurchaseDate.ToString(CultureInfo.InvariantCulture)));
            var place = CreateFieldSettingForProcessing(settingCollection.FindByName(InventoryFields.Place.ToString(CultureInfo.InvariantCulture)));
            var workstation = CreateFieldSettingForProcessing(settingCollection.FindByName(InventoryFields.Workstation.ToString(CultureInfo.InvariantCulture)));
            var info = CreateFieldSettingForProcessing(settingCollection.FindByName(InventoryFields.Info.ToString(CultureInfo.InvariantCulture)));

            var settingAgregate =
                new InventoryFieldSettingsProcessing(
                    new BusinessData.Models.Inventory.Output.Settings.ProcessingSetttings.InventorySettings.DefaultFieldSettings(
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

        public void DeleteByInventoryTypeId(int inventoryTypeId)
        {
            var models = this.GetSettings(inventoryTypeId).Where(x => x.InventoryType_Id == inventoryTypeId).ToList();
            models.ForEach(x => this.DbSet.Remove(x));
        }

        private static InventoryFieldSettingsOverview GetInventoryFieldSettingsOverview(List<FieldOverviewSettingMapperData> mapperData)
        {
            var settingCollection = new NamedObjectCollection<FieldOverviewSettingMapperData>(mapperData);
            var department = CreateFieldSettingOverview(settingCollection.FindByName(InventoryFields.Department.ToString(CultureInfo.InvariantCulture)));
            var name = CreateFieldSettingOverview(settingCollection.FindByName(InventoryFields.Name.ToString(CultureInfo.InvariantCulture)));
            var model = CreateFieldSettingOverview(settingCollection.FindByName(InventoryFields.Model.ToString(CultureInfo.InvariantCulture)));
            var manufacturer = CreateFieldSettingOverview(settingCollection.FindByName(InventoryFields.Manufacturer.ToString(CultureInfo.InvariantCulture)));
            var serialNumber = CreateFieldSettingOverview(settingCollection.FindByName(InventoryFields.SerialNumber.ToString(CultureInfo.InvariantCulture)));
            var theftMark = CreateFieldSettingOverview(settingCollection.FindByName(InventoryFields.TheftMark.ToString(CultureInfo.InvariantCulture)));
            var barCode = CreateFieldSettingOverview(settingCollection.FindByName(InventoryFields.BarCode.ToString(CultureInfo.InvariantCulture)));
            var purchaseDate = CreateFieldSettingOverview(settingCollection.FindByName(InventoryFields.PurchaseDate.ToString(CultureInfo.InvariantCulture)));
            var place = CreateFieldSettingOverview(settingCollection.FindByName(InventoryFields.Place.ToString(CultureInfo.InvariantCulture)));
            var workstation = CreateFieldSettingOverview(settingCollection.FindByName(InventoryFields.Workstation.ToString(CultureInfo.InvariantCulture)));
            var info = CreateFieldSettingOverview(settingCollection.FindByName(InventoryFields.Info.ToString(CultureInfo.InvariantCulture)));
            var createdDate = CreateFieldSettingOverview(settingCollection.FindByName(InventoryFields.CreatedDate.ToString(CultureInfo.InvariantCulture)));
            var changedDate = CreateFieldSettingOverview(settingCollection.FindByName(InventoryFields.ChangedDate.ToString(CultureInfo.InvariantCulture)));
            var syncDate = CreateFieldSettingOverview(settingCollection.FindByName(InventoryFields.SyncDate.ToString(CultureInfo.InvariantCulture)));

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
                        info,
                        createdDate,
                        changedDate,
                        syncDate);

            return new InventoryFieldSettingsOverview(settingAgregate);
        }

        private static FieldSettingOverview CreateFieldSettingOverview(FieldOverviewSettingMapperData fieldSetting)
        {
            return new FieldSettingOverview(fieldSetting.Show.ToBool(), fieldSetting.Caption);
        }

        private static InventoryFieldSettingForModelEdit CreateFieldSettingForModelEdit(InventoryFieldSettingMapperDataForModelEdit fieldSetting)
        {
            return new InventoryFieldSettingForModelEdit(fieldSetting.Caption, fieldSetting.PropertySize, fieldSetting.Show.ToBool(), fieldSetting.ReadOnly.ToBool());
        }

        private static InventoryFieldSettingForProcessing CreateFieldSettingForProcessing(
            InventoryFieldSettingMapperDataForProcessing fieldSetting)
        {
            return new InventoryFieldSettingForProcessing(fieldSetting.Show.ToBool());
        }

        private static InventoryFieldSetting CreateFieldSetting(InventoryFieldSettingMapperData fieldSetting)
        {
            return new InventoryFieldSetting(
                fieldSetting.Caption,
                fieldSetting.PropertySize,
                fieldSetting.ShowInDetails.ToBool(),
                fieldSetting.ShowInList.ToBool(),
                fieldSetting.XMLTag,
                fieldSetting.ReadOnly.ToBool());
        }

        private static InventoryFieldSetting CreateFieldSettingWithDefaultPropertySize(InventoryFieldSettingMapperData fieldSetting)
        {
            return new InventoryFieldSetting(
                fieldSetting.Caption,
                null,
                fieldSetting.ShowInDetails.ToBool(),
                fieldSetting.ShowInList.ToBool(),
                fieldSetting.XMLTag,
                fieldSetting.ReadOnly.ToBool());
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
            fieldSetting.XMLTag = updatedSetting.XMLTag;
            fieldSetting.ReadOnly = updatedSetting.ReadOnly.ToInt();
        }

        private static void AddFieldSetting(
            int inventoryTypeId,
            int propertyType,
            InventoryFieldSetting newSetting,
            DateTime createdDate,
            DbSet<Domain.Inventory.InventoryTypeProperty> settings)
        {
            var setting = new Domain.Inventory.InventoryTypeProperty
                       {
                           PropertyValue = newSetting.Caption,
                           CreatedDate = createdDate,
                           ChangedDate = DateTime.Now, // todo
                           InventoryType_Id = inventoryTypeId,
                           Show = newSetting.ShowInDetails.ToInt(),
                           ShowInList = newSetting.ShowInList.ToInt(),
                           PropertySize = newSetting.PropertySize ?? DefaultPropertySize,
                           PropertyPos = DefaultPosition,
                           PropertyType = propertyType,
                           PropertyDefault = PropertyDefaultValue,
                           XMLTag = newSetting.XMLTag,
                           ReadOnly = newSetting.ReadOnly.ToInt()
                       };

            settings.Add(setting);
        }

        private IQueryable<Domain.Inventory.InventoryTypeProperty> GetSettings(int inventoryTypeId)
        {
            return this.DbSet.Where(x => x.InventoryType_Id == inventoryTypeId && x.PropertyType < MinDynamicSettingTypeId);
        }
    }
}