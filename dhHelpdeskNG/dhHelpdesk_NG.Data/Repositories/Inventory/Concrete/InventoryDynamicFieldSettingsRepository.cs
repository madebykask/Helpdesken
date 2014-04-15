namespace DH.Helpdesk.Dal.Repositories.Inventory.Concrete
{
    using System.Collections.Generic;
    using System.Linq;

    using DH.Helpdesk.BusinessData.Models.Inventory.Edit.Settings;
    using DH.Helpdesk.BusinessData.Models.Inventory.Edit.Settings.InventorySettings;
    using DH.Helpdesk.BusinessData.Models.Inventory.Output.Settings.ModelEdit.InventorySettings;
    using DH.Helpdesk.BusinessData.Models.Inventory.Output.Settings.ModelOverview.InventoryFieldSettings;
    using DH.Helpdesk.Common.Extensions.Integer;
    using DH.Helpdesk.Dal.Dal;
    using DH.Helpdesk.Dal.Infrastructure;

    public class InventoryDynamicFieldSettingsRepository : Repository<Domain.Inventory.InventoryTypeProperty>, IInventoryDynamicFieldSettingsRepository
    {
        public InventoryDynamicFieldSettingsRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }

        public void Add(InventoryDynamicFieldSetting businessModel)
        {
            var entity = new Domain.Inventory.InventoryTypeProperty()
                             {
                                 CreatedDate = businessModel.ChangedDate,
                                 PropertyPos = businessModel.Position,
                                 PropertySize = businessModel.PropertySize,
                                 PropertyValue = businessModel.Caption,
                                 PropertyType = (int)businessModel.FieldType,
                                 InventoryTypeGroup_Id =
                                     businessModel.InventoryTypeGroupId,
                             };

            this.DbSet.Add(entity);
            this.InitializeAfterCommit(businessModel, entity);
        }

        public void Update(InventoryDynamicFieldSetting businessModel)
        {
            var entity = this.DbSet.Find(businessModel.Id);
            entity.ChangedDate = businessModel.ChangedDate;
            entity.PropertyPos = businessModel.Position;
            entity.PropertySize = businessModel.PropertySize;
            entity.PropertyValue = businessModel.Caption;
            entity.PropertyType = (int)businessModel.FieldType;
            entity.InventoryTypeGroup_Id = businessModel.InventoryTypeGroupId;
        }

        public List<InventoryDynamicFieldSetting> GetFieldSettingsForEdit(int inventoryTypeId)
        {
            var settings = this.GetSettings(inventoryTypeId);

            var mapperData =
                settings.Select(
                    x =>
                    new
                        {
                            x.Id,
                            x.InventoryTypeGroup_Id,
                            x.PropertyValue,
                            x.PropertyPos,
                            x.PropertyType,
                            x.PropertySize,
                            x.Show,
                            x.ShowInList
                        }).ToList();

            var data =
                mapperData.Select(
                    x =>
                    InventoryDynamicFieldSetting.CreateForEdit(
                        x.Id,
                        x.InventoryTypeGroup_Id,
                        x.PropertyValue,
                        x.PropertyPos,
                        (FieldTypes)x.PropertyType,
                        x.PropertySize,
                        x.Show.ToBool(),
                        x.ShowInList.ToBool())).ToList();

            return data;
        }

        public List<InventoryDynamicFieldSettingForModelEdit> GetFieldSettingsForModelEdit(int inventoryTypeId)
        {
            var settings = this.GetSettings(inventoryTypeId).Where(x => x.Show == 1);

            var mapperData =
                settings.Select(
                    x =>
                    new
                        {
                            x.Id,
                            x.InventoryTypeGroup_Id,
                            x.PropertyValue,
                            x.PropertyPos,
                            x.PropertyType,
                            x.PropertySize
                        }).ToList();

            var data =
                mapperData.Select(
                    x =>
                    new InventoryDynamicFieldSettingForModelEdit(
                        x.Id,
                        x.InventoryTypeGroup_Id,
                        x.PropertyValue,
                        x.PropertyPos,
                        (FieldTypes)x.PropertyType,
                        x.PropertySize)).ToList();

            return data;
        }

        public List<InventoryDynamicFieldSettingOverview> GetFieldSettingsOverview(int inventoryTypeId)
        {
            var settings = this.GetSettings(inventoryTypeId).Where(x => x.ShowInList == 1);

            var mapperData = settings.Select(x => new { x.Id, x.PropertyValue }).ToList();

            var data = mapperData.Select(x => new InventoryDynamicFieldSettingOverview(x.Id, x.PropertyValue)).ToList();

            return data;
        }

        public IQueryable<Domain.Inventory.InventoryTypeProperty> GetSettings(int inventoryTypeId)
        {
            return this.DbSet.Where(x => x.InventoryType_Id == inventoryTypeId && x.PropertyType >= 0);
        }
    }
}