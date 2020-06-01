namespace DH.Helpdesk.Dal.Repositories.Inventory.Concrete
{
    using System.Collections.Generic;
    using System.Linq;

    using DH.Helpdesk.BusinessData.Models.Inventory;
    using DH.Helpdesk.BusinessData.Models.Inventory.Edit.Settings;
    using DH.Helpdesk.Dal.Dal;
    using DH.Helpdesk.Dal.Infrastructure;

    public class InventoryTypePropertyValueRepository : Repository<Domain.Inventory.InventoryTypePropertyValue>,
                                                        IInventoryTypePropertyValueRepository
    {
        public InventoryTypePropertyValueRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }

        public void Add(InventoryValueForWrite businessModel)
        {
            var entity = new Domain.Inventory.InventoryTypePropertyValue
                             {
                                 Inventory_Id = businessModel.InventoryId,
                                 InventoryTypeProperty_Id = businessModel.InventoryTypePropertyId,
                                 Value = businessModel.Value
                             };

            this.DbSet.Add(entity);
        }

        public void Add(List<InventoryValueForWrite> businessModels)
        {
            foreach (var businessModel in businessModels)
            {
                this.Add(businessModel);
            }
        }

        public void Update(InventoryValueForWrite businessModel)
        {
            var entity =
                this.DbSet.SingleOrDefault(
                    x =>
                    x.InventoryTypeProperty_Id == businessModel.InventoryTypePropertyId
                    && x.Inventory_Id == businessModel.InventoryId);

            if (entity == null)
            {
                this.Add(businessModel);
            }
            else
            {
                entity.Value = businessModel.Value;
            }
        }

        public void Update(List<InventoryValueForWrite> businessModels)
        {
            foreach (var businessModel in businessModels)
            {
                this.Update(businessModel);
            }
        }

        public List<InventoryValue> GetData(int id)
        {
            var entities =
                this.DbSet.Where(x => x.Inventory_Id == id && x.InventoryTypeProperty.Show == 1)
                    .Select(x => new { Entity = x, x.InventoryTypeProperty.PropertyType })
                    .ToList();

            var overviews =
                entities.Select(
                    x =>
                    new InventoryValue(
                        x.Entity.Inventory_Id,
                        x.Entity.InventoryTypeProperty_Id,
                        (FieldTypes)x.PropertyType,
                        x.Entity.Value)).ToList();

            return overviews;
        }

        public List<InventoryValue> GetData(List<int> ids, int? inventoryTypeId = null)
        {
            var query = this.DbSet.Where(x => ids.Contains(x.Inventory_Id) && x.InventoryTypeProperty.ShowInList == 1);
            if (inventoryTypeId.HasValue)
                query = query.Where(x => x.InventoryTypeProperty.InventoryType_Id == inventoryTypeId.Value);
            var entities = query.Select(x => new { Entity = x, x.InventoryTypeProperty.PropertyType })
                    .ToList();

            var overviews =
                entities.Select(
                    x =>
                    new InventoryValue(
                        x.Entity.Inventory_Id,
                        x.Entity.InventoryTypeProperty_Id,
                        (FieldTypes)x.PropertyType,
                        x.Entity.Value)).ToList();

            return overviews;
        }

        public void DeleteByInventoryTypePropertyId(int inventoryTypePropertyId)
        {
            var models = this.DbSet.Where(x => x.InventoryTypeProperty_Id == inventoryTypePropertyId).ToList();
            models.ForEach(x => this.DbSet.Remove(x));
        }

        public void DeleteByInventoryTypeId(int inventoryTypeId)
        {
            var models = this.DbSet.Where(x => x.Inventory.InventoryType_Id == inventoryTypeId).ToList();
            models.ForEach(x => this.DbSet.Remove(x));
        }

        public void DeleteByInventoryId(int inventoryId)
        {
            var models = this.DbSet.Where(x => x.Inventory_Id == inventoryId).ToList();
            models.ForEach(x => this.DbSet.Remove(x));
        }
    }
}