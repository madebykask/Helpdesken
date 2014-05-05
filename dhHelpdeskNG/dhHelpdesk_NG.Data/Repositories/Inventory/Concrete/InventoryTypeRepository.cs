namespace DH.Helpdesk.Dal.Repositories.Inventory.Concrete
{
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;

    using DH.Helpdesk.BusinessData.Enums.Inventory;
    using DH.Helpdesk.BusinessData.Models.Changes;
    using DH.Helpdesk.BusinessData.Models.Common.Output;
    using DH.Helpdesk.BusinessData.Models.Inventory.Edit.Inventory;
    using DH.Helpdesk.Dal.Dal;
    using DH.Helpdesk.Dal.Infrastructure;

    public class InventoryTypeRepository : Repository<Domain.Inventory.InventoryType>, IInventoryTypeRepository
    {
        public InventoryTypeRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }

        public void Add(InventoryType businessModel)
        {
            var entity = new Domain.Inventory.InventoryType
            {
                Name = businessModel.Name,
                Customer_Id = businessModel.CustomerId,
                CreatedDate = businessModel.CreatedDate
            };

            this.DbSet.Add(entity);
            this.InitializeAfterCommit(businessModel, entity);
        }

        public void Delete(int id)
        {
            var entity = this.DbSet.Find(id);
            this.DbSet.Remove(entity);
        }

        public void Update(InventoryType businessModel)
        {
            var entity = this.DbSet.Find(businessModel.Id);
            entity.Name = businessModel.Name;
            entity.ChangedDate = entity.ChangedDate;
        }

        public InventoryType FindById(int id)
        {
            var entity = this.DbSet.Find(id);
            var businessModel = InventoryType.CreateForEdit(
                entity.Name,
                entity.Id,
                entity.CreatedDate,
                entity.ChangedDate);

            return businessModel;
        }

        public List<ItemOverview> FindOverviews(int customerId)
        {
            var anonymus =
              this.DbSet
                  .Select(c => new { c.Name, c.Id })
                  .ToList();

            var overviews =
                anonymus.Select(c => new ItemOverview(c.Name, c.Id.ToString(CultureInfo.InvariantCulture))).ToList();

            return overviews;
        }

        public List<InventoryTypeWithInventories> FindInventoryTypesWithInventories(int customerId, int langaugeId)
        {
            var workstationsTypeName = CurrentModes.Workstations.ToString();
            var serversTypeName = CurrentModes.Servers.ToString();
            var printersTypeName = CurrentModes.Printers.ToString();

            var computers =
                this.DbContext.Computers.Where(c => c.Customer_Id == customerId)
                    .Select(
                        c =>
                            new
                            {
                                TypeId = (int)CurrentModes.Workstations,
                                TypeName = workstationsTypeName,
                                c.Id,
                                Name = c.ComputerName
                            });

            var servers =
                this.DbContext.Servers.Where(s => s.Customer_Id == customerId)
                    .Select(
                        s =>
                            new
                            {
                                TypeId = (int)CurrentModes.Servers,
                                TypeName = serversTypeName,
                                s.Id,
                                Name = s.ServerName
                            });

            var printers =
                this.DbContext.Printers.Where(p => p.Customer_Id == customerId)
                    .Select(
                        p =>
                            new
                            {
                                TypeId = (int)CurrentModes.Printers,
                                TypeName = printersTypeName,
                                p.Id,
                                Name = p.PrinterName
                            });

            var customInventories =
                this.DbContext.Inventories.Where(i => i.InventoryType.Customer_Id == customerId)
                    .Select(
                        i =>
                            new
                            {
                                TypeId = i.InventoryType_Id,
                                TypeName = i.InventoryType.Name,
                                i.Id,
                                Name = i.InventoryName
                            });

            var allInventories =
                computers.Concat(servers)
                    .Concat(printers)
                    .Concat(customInventories)
                    .ToList()
                    .GroupBy(i => new { i.TypeId, i.TypeName });

            var inventoryTypesWithInventories = new List<InventoryTypeWithInventories>();

            foreach (var inventoryType in allInventories)
            {
                var inventories =
                    inventoryType.Select(t => new ItemOverview(t.Name, t.Id.ToString(CultureInfo.InvariantCulture)))
                        .ToList();

                var inventoryTypeWithInventories = new InventoryTypeWithInventories(
                    inventoryType.Key.TypeId,
                    inventoryType.Key.TypeName,
                    inventories);

                inventoryTypesWithInventories.Add(inventoryTypeWithInventories);
            }

            return inventoryTypesWithInventories;
        }
    }
}