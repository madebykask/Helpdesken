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

        public List<InventoryTypeWithInventories> FindInventoryTypeWithInventories(int customerId, int langaugeId)
        {
            var anonymus = (from c in DbContext.Computers
                            where c.Customer_Id == customerId
                            select
                                new
                                    {
                                        TypeId = (int)CurrentModes.Workstations,
                                        TypeName = CurrentModes.Workstations.ToString(),
                                        c.Id,
                                        Name = c.ComputerName
                                    }).Concat(
                                        from c in DbContext.Servers
                                        where c.Customer_Id == customerId
                                        select
                                            new
                                                {
                                                    TypeId = (int)CurrentModes.Servers,
                                                    TypeName = CurrentModes.Servers.ToString(),
                                                    c.Id,
                                                    Name = c.ServerName
                                                }).Concat(
                                                    from c in DbContext.Printers
                                                    where c.Customer_Id == customerId
                                                    select
                                                        new
                                                            {
                                                                TypeId = (int)CurrentModes.Printers,
                                                                TypeName = CurrentModes.Printers.ToString(),
                                                                c.Id,
                                                                Name = c.PrinterName
                                                            })
                .Concat(
                    from c in DbContext.Inventories
                    where c.InventoryType.Customer_Id == customerId
                    select
                        new
                            {
                                TypeId = c.InventoryType_Id,
                                TypeName = c.InventoryType.Name,
                                c.Id,
                                Name = c.InventoryName
                            }).GroupBy(x => new { x.TypeId, x.TypeName });

            var models = new List<InventoryTypeWithInventories>();

            foreach (var item in anonymus)
            {
                var overviews = item.Select(x => new ItemOverview(x.Name, x.Id.ToString(CultureInfo.InvariantCulture))).ToList();
                var model = new InventoryTypeWithInventories(item.Key.TypeId, item.Key.TypeName, overviews);
                models.Add(model);
            }

            return models;
        }
    }
}