namespace DH.Helpdesk.Dal.Repositories.Inventory.Concrete
{
    using System.Collections.Generic;
    using System.Linq;

    using DH.Helpdesk.BusinessData.Models.Inventory.Edit.Settings;
    using DH.Helpdesk.BusinessData.Models.Inventory.Edit.Settings.InventorySettings;
    using DH.Helpdesk.BusinessData.Models.Inventory.Output.Settings.ModelEdit.InventorySettings;
    using DH.Helpdesk.BusinessData.Models.Inventory.Output.Settings.ModelOverview.InventoryFieldSettings;
    using DH.Helpdesk.Common.Extensions.Boolean;
    using DH.Helpdesk.Common.Extensions.Integer;
    using DH.Helpdesk.Dal.Dal;
    using DH.Helpdesk.Dal.Infrastructure;

    public class InventoryDynamicFieldSettingsRepository : Repository<Domain.Inventory.InventoryTypeProperty>, IInventoryDynamicFieldSettingsRepository
    {
        public const int True = 1;

        public const int MinDynamicSettingTypeId = 0;

        public const int DefaultPropertySize = 50;

        public const string PropertyDefaultValue = "";

        public InventoryDynamicFieldSettingsRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }

        public void Add(InventoryDynamicFieldSetting businessModel)
        {
            var entity = new Domain.Inventory.InventoryTypeProperty
                             {
                                 InventoryType_Id = businessModel.InventoryTypeId,
                                 CreatedDate = businessModel.CreatedDate,
                                 ChangedDate = businessModel.CreatedDate, // todo
                                 PropertyPos = businessModel.Position,
                                 PropertySize = businessModel.PropertySize ?? DefaultPropertySize,
                                 PropertyValue = businessModel.Caption,
                                 PropertyType = (int)businessModel.FieldType,
                                 PropertyDefault = PropertyDefaultValue,
                                 Show = businessModel.ShowInDetails.ToInt(),
                                 ShowInList = businessModel.ShowInList.ToInt(),
                                 InventoryTypeGroup_Id = businessModel.InventoryTypeGroupId,
                                 XMLTag = businessModel.XMLTag,
                                 ReadOnly = businessModel.ReadOnly.ToInt()
                             };

            this.DbSet.Add(entity);
            this.InitializeAfterCommit(businessModel, entity);
        }

        public void Update(InventoryDynamicFieldSetting businessModel)
        {
            var entity = this.DbSet.Find(businessModel.Id);
            entity.ChangedDate = businessModel.ChangedDate;
            entity.PropertyPos = businessModel.Position;

            if (businessModel.PropertySize.HasValue)
            {
                entity.PropertySize = businessModel.PropertySize.Value;
            }

            entity.PropertyValue = businessModel.Caption;
            entity.PropertyType = (int)businessModel.FieldType;
            entity.InventoryTypeGroup_Id = businessModel.InventoryTypeGroupId;
            entity.Show = businessModel.ShowInDetails.ToInt();
            entity.ShowInList = businessModel.ShowInList.ToInt();
            entity.InventoryTypeGroup_Id = businessModel.InventoryTypeGroupId;
            entity.XMLTag = businessModel.XMLTag;
            entity.ReadOnly = businessModel.ReadOnly.ToInt();
        }

        public void Update(List<InventoryDynamicFieldSetting> businessModels)
        {
            foreach (var businessModel in businessModels)
            {
                this.Update(businessModel);
            }
        }

        public List<InventoryDynamicFieldSetting> GetFieldSettingsForEdit(int inventoryTypeId)
        {
            var settings = this.GetSettings(inventoryTypeId);

            var mapperData = settings.Select(x => new
                        {
                            x.Id,
                            x.InventoryTypeGroup_Id,
                            x.PropertyValue,
                            x.PropertyPos,
                            x.PropertyType,
                            x.PropertySize,
                            x.Show,
                            x.ShowInList,
                            x.XMLTag,
                            x.ReadOnly
                        }).ToList();

            var data = mapperData.Select(x => InventoryDynamicFieldSetting.CreateForEdit(
                        x.Id,
                        x.InventoryTypeGroup_Id,
                        x.PropertyValue,
                        x.PropertyPos,
                        (FieldTypes)x.PropertyType,
                        x.PropertySize,
                        x.Show.ToBool(),
                        x.ShowInList.ToBool(),
                        x.XMLTag,
                        x.ReadOnly.ToBool())).ToList();

            return data;
        }

        public List<InventoryDynamicFieldSettingForModelEdit> GetFieldSettingsForModelEdit(int inventoryTypeId, bool isReadonly = false)
        {
            var settings = this.GetSettings(inventoryTypeId).Where(x => x.Show == True);

            var mapperData = settings.Select(x =>
                    new
                        {
                            x.Id,
                            x.InventoryTypeGroup_Id,
                            x.PropertyValue,
                            x.PropertyPos,
                            x.PropertyType,
                            x.PropertySize,
                            x.ReadOnly
                        }).ToList();

            var data = mapperData.Select(x =>
                    new InventoryDynamicFieldSettingForModelEdit(
                        x.Id,
                        x.InventoryTypeGroup_Id,
                        x.PropertyValue,
                        x.PropertyPos,
                        (FieldTypes)x.PropertyType,
                        x.PropertySize, 
                        isReadonly ? isReadonly : x.ReadOnly.ToBool())).ToList();

            return data;
        }

        public List<InventoryDynamicFieldSettingOverview> GetFieldSettingsOverview(int inventoryTypeId)
        {
            var anonymus =
                this.GetSettings(inventoryTypeId)
                    .Where(x => x.ShowInList == True)
                    .Select(x => new { x.Id, x.PropertyValue, x.PropertyPos })
                    .OrderBy(x => x.PropertyPos)
                    .ToList();

            var data = anonymus.Select(x => new InventoryDynamicFieldSettingOverview(x.Id, x.PropertyValue)).ToList();

            return data;
        }

        public List<InventoryDynamicFieldSettingOverviewWithType> GetFieldSettingsOverviewWithType(List<int> inventoryTypeIds)
        {
            var overviews = new List<InventoryDynamicFieldSettingOverviewWithType>();

            var anonymus =
                this.GetSettings(inventoryTypeIds)
                    .Where(x => x.ShowInList == True)
                    .Select(x => new { x.Id, x.PropertyValue, x.InventoryType_Id })
                    .ToList()
                    .GroupBy(x => new { InventoryTypeId = x.InventoryType_Id });

            foreach (var item in anonymus)
            {
                var settings =
                    item.Select(x => new InventoryDynamicFieldSettingOverview(x.Id, x.PropertyValue)).ToList();
                var overview = new InventoryDynamicFieldSettingOverviewWithType(item.Key.InventoryTypeId, settings);
                overviews.Add(overview);
            }

            return overviews;
        }

        public void DeleteByInventoryTypeId(int inventoryTypeId)
        {
            var models = this.GetSettings(inventoryTypeId).Where(x => x.InventoryType_Id == inventoryTypeId).ToList();
            models.ForEach(x => this.DbSet.Remove(x));
        }

        private IQueryable<Domain.Inventory.InventoryTypeProperty> GetSettings(int inventoryTypeId)
        {
            return this.DbSet.Where(x => x.InventoryType_Id == inventoryTypeId && x.PropertyType >= MinDynamicSettingTypeId);
        }

        private IQueryable<Domain.Inventory.InventoryTypeProperty> GetSettings(List<int> inventoryTypeIds)
        {
            return this.DbSet.Where(x => inventoryTypeIds.Contains(x.InventoryType_Id) && x.PropertyType >= MinDynamicSettingTypeId);
        }
    }
}