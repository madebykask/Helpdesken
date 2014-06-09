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

        public List<InventoryValue> GetData(List<int> ids)
        {
            var entities =
                this.DbSet.Where(x => ids.Contains(x.Inventory_Id) && x.InventoryTypeProperty.ShowInList == 1)
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

        public void DeleteByInventoryTypePropertyId(int inventoryTypePropertyId)
        {
            var models = this.DbSet.Where(x => x.InventoryTypeProperty_Id == inventoryTypePropertyId).ToList();
            models.ForEach(x => this.DbSet.Remove(x));
        }
    }
}