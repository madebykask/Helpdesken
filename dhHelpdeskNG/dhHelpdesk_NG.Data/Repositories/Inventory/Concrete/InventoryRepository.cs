namespace DH.Helpdesk.Dal.Repositories.Inventory.Concrete
{
    using System.Collections.Generic;
    using System.Linq;

    using DH.Helpdesk.BusinessData.Models.Inventory.Edit.Inventory;
    using DH.Helpdesk.BusinessData.Models.Inventory.Output;
    using DH.Helpdesk.Common.Types;
    using DH.Helpdesk.Dal.Dal;
    using DH.Helpdesk.Dal.Infrastructure;

    public class InventoryRepository : Repository<Domain.Inventory.Inventory>, IInventoryRepository
    {
        public InventoryRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }

        public void Add(Inventory businessModel)
        {
            var entity = new Domain.Inventory.Inventory
            {
                Department_Id = businessModel.DepartmentId,
                InventoryType_Id = businessModel.InventoryTypeId,
                Room_Id = businessModel.RoomId,
                ChangedByUser_Id = businessModel.ChangeByUserId,
                InventoryName = businessModel.Name,
                InventoryModel = businessModel.Model,
                Manufacturer = businessModel.Manufacturer,
                SerialNumber = businessModel.SerialNumber,
                TheftMark = businessModel.TheftMark,
                BarCode = businessModel.BarCode,
                PurchaseDate = businessModel.PurchaseDate,
                Info = businessModel.Info,
                SyncChangedDate = businessModel.SyncChangeDate,
                CreatedDate = businessModel.CreatedDate
            };

            this.DbSet.Add(entity);
            this.InitializeAfterCommit(businessModel, entity);
        }

        public void Update(Inventory businessModel)
        {
            var entity = this.DbSet.Find(businessModel.Id);
            entity.Department_Id = businessModel.DepartmentId;
            entity.InventoryType_Id = businessModel.InventoryTypeId;
            entity.Room_Id = businessModel.RoomId;
            entity.ChangedByUser_Id = businessModel.ChangeByUserId;
            entity.InventoryName = businessModel.Name;
            entity.InventoryModel = businessModel.Model;
            entity.Manufacturer = businessModel.Manufacturer;
            entity.SerialNumber = businessModel.SerialNumber;
            entity.TheftMark = businessModel.TheftMark;
            entity.BarCode = businessModel.BarCode;
            entity.PurchaseDate = businessModel.PurchaseDate;
            entity.Info = businessModel.Info;
            entity.SyncChangedDate = businessModel.SyncChangeDate;
            entity.ChangedDate = entity.ChangedDate;
        }

        public Inventory FindById(int id)
        {
            var entity = this.DbSet.Find(id);
            var businessModel = Inventory.CreateForEdit(
                entity.InventoryType_Id,
                entity.Id,
                entity.Department_Id,
                entity.Room_Id,
                entity.ChangedByUser_Id,
                entity.InventoryName,
                entity.InventoryModel,
                entity.Manufacturer,
                entity.SerialNumber,
                entity.TheftMark,
                entity.BarCode,
                entity.PurchaseDate,
                entity.Info,
                entity.SyncChangedDate,
                entity.CreatedDate,
                entity.ChangedDate);

            return businessModel;
        }

        public List<InventoryOverview> FindConnectedToComputerInventories(int computerId)
        {
            var query =
                DbSet.Where(
                    i =>
                    DbContext.ComputerInventories.Where(x => x.Computer_Id == computerId)
                        .Select(x => x.Inventory_Id)
                        .Contains(i.Id));

            var overviews = GetOverviews(query);

            return overviews;
        }

        public List<InventoryOverview> FindOverviews(
            int inventoryTypeId,
            int? departmentId,
            string searchString,
            int pageSize)
        {
            var query = DbSet.Where(x => x.InventoryType_Id == inventoryTypeId);

            if (departmentId.HasValue)
            {
                query = query.Where(x => x.Department_Id == departmentId);
            }

            if (!string.IsNullOrEmpty(searchString))
            {
                query =
                    query.Where(
                        x =>
                        x.InventoryName == searchString || x.InventoryModel == searchString
                        || x.Manufacturer == searchString || x.SerialNumber == searchString);
            }

            query = query.Take(pageSize);

            var overviews = GetOverviews(query);

            return overviews;
        }

        private static List<InventoryOverview> GetOverviews(IQueryable<Domain.Inventory.Inventory> query)
        {
            var anonymus =
                query.Select(
                    entity =>
                    new
                        {
                            entity.InventoryType_Id,
                            entity.InventoryType.Name,
                            entity.Id,
                            entity.Department.DepartmentName,
                            RoomName = entity.Room.Name,
                            UserFirstName = entity.ChangedByUser.FirstName,
                            UserLastName = entity.ChangedByUser.SurName,
                            entity.InventoryName,
                            entity.InventoryModel,
                            entity.Manufacturer,
                            entity.SerialNumber,
                            entity.TheftMark,
                            entity.BarCode,
                            entity.PurchaseDate,
                            entity.Info,
                        }).ToList();

            var overviews =
                anonymus.Select(
                    a =>
                    new InventoryOverview(
                        a.Id,
                        a.InventoryType_Id,
                        a.InventoryName,
                        a.DepartmentName,
                        a.RoomName,
                        new UserName(a.UserFirstName, a.UserLastName),
                        a.InventoryName,
                        a.InventoryModel,
                        a.Manufacturer,
                        a.SerialNumber,
                        a.TheftMark,
                        a.BarCode,
                        a.PurchaseDate,
                        string.Empty,
                        // todo
                        a.Info)).ToList();

            return overviews;
        }
    }
}
