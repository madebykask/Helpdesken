namespace DH.Helpdesk.Dal.Repositories.Inventory.Concrete
{
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;

    using DH.Helpdesk.BusinessData.Models.Inventory;
    using DH.Helpdesk.BusinessData.Models.Inventory.Edit.Inventory;
    using DH.Helpdesk.BusinessData.Models.Inventory.Output;
    using DH.Helpdesk.BusinessData.Models.Shared;
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
            var anonymus = (from i in this.DbSet.Where(x => x.Id == id)
                            join ci in DbContext.ComputerInventories on i.Id equals ci.Inventory_Id into res
                            from k in res.DefaultIfEmpty()
                            select
                                new
                                    {
                                        Entity = i,
                                        FloorId = (int?)i.Room.Floor.Id,
                                        BuildingId = (int?)i.Room.Floor.Building.Id,
                                        InventoryTypeName = i.InventoryType.Name,
                                        Workstation = k.Computer.ComputerName
                                    }).ToList()
                .GroupBy(x => new { x.Entity, x.BuildingId, x.FloorId, x.InventoryTypeName });

            var businessModel =
                anonymus.Select(
                    x =>
                    Inventory.CreateForEdit(
                        x.Key.Entity.InventoryType_Id,
                        x.Key.InventoryTypeName,
                        x.Key.Entity.Id,
                        x.Key.Entity.Department_Id,
                        x.Key.BuildingId,
                        x.Key.FloorId,
                        x.Key.Entity.Room_Id,
                        x.Key.Entity.ChangedByUser_Id,
                        x.Key.Entity.InventoryName,
                        x.Key.Entity.InventoryModel,
                        x.Key.Entity.Manufacturer,
                        x.Key.Entity.SerialNumber,
                        x.Key.Entity.TheftMark,
                        x.Key.Entity.BarCode,
                        x.Key.Entity.PurchaseDate,
                        x.Select(w => w.Workstation).ToArray(),
                        x.Key.Entity.Info,
                        x.Key.Entity.SyncChangedDate,
                        x.Key.Entity.CreatedDate,
                        x.Key.Entity.ChangedDate)).Single();

            return businessModel;
        }

        public List<InventoryOverviewWithType> FindConnectedToComputerInventories(int computerId)
        {
            var overviewsWithType = new List<InventoryOverviewWithType>();

            var query =
                DbSet.Where(
                    i =>
                    DbContext.ComputerInventories.Where(x => x.Computer_Id == computerId)
                        .Select(x => x.Inventory_Id)
                        .Contains(i.Id));

            var computerName = DbContext.Computers.Where(x => x.Id == computerId).Select(x => x.ComputerName).Single();

            var anonymus =
                query.Select(
                    entity =>
                    new
                        {
                            entity.Id,
                            InventoryTypeId = entity.InventoryType_Id,
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
                            Worstations = computerName
                        }).GroupBy(x => x.InventoryTypeId).ToList();

            foreach (var item in anonymus)
            {
                var overviews =
                    item.Select(
                        a =>
                        new InventoryOverview(
                            a.Id,
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
                            a.Worstations,
                            a.Info)).ToList();

                var overviewWithType = new InventoryOverviewWithType(item.Key, overviews);
                overviewsWithType.Add(overviewWithType);
            }

            return overviewsWithType;
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

            const string Delimeter = "; ";

            var anonymus = (from i in query
                            join ci in DbContext.ComputerInventories on i.Id equals ci.Inventory_Id into res
                            from k in res.DefaultIfEmpty()
                            select
                                new
                                    {
                                        i.Id,
                                        i.Department.DepartmentName,
                                        RoomName = i.Room.Name,
                                        UserFirstName = i.ChangedByUser.FirstName,
                                        UserLastName = i.ChangedByUser.SurName,
                                        i.InventoryName,
                                        i.InventoryModel,
                                        i.Manufacturer,
                                        i.SerialNumber,
                                        i.TheftMark,
                                        i.BarCode,
                                        i.PurchaseDate,
                                        i.Info,
                                        WorkstationName = k.Computer.ComputerName
                                    }).ToList()
                .GroupBy(
                    x =>
                    new
                        {
                            x.Id,
                            x.DepartmentName,
                            x.RoomName,
                            x.UserFirstName,
                            x.UserLastName,
                            x.InventoryName,
                            x.InventoryModel,
                            x.Manufacturer,
                            x.SerialNumber,
                            x.TheftMark,
                            x.BarCode,
                            x.PurchaseDate,
                            x.Info,
                        });

            var overviews =
                anonymus.Select(
                    a =>
                    new InventoryOverview(
                        a.Key.Id,
                        a.Key.DepartmentName,
                        a.Key.RoomName,
                        new UserName(a.Key.UserFirstName, a.Key.UserLastName),
                        a.Key.InventoryName,
                        a.Key.InventoryModel,
                        a.Key.Manufacturer,
                        a.Key.SerialNumber,
                        a.Key.TheftMark,
                        a.Key.BarCode,
                        a.Key.PurchaseDate,
                        string.Join(Delimeter, a.Select(x => x.WorkstationName)), // todo change to array
                        a.Key.Info)).ToList();

            return overviews;
        }

        public List<ItemOverview> FindNotConnectedOverviews(int inventoryTypeId, int computerId)
        {
            var anonymus =
                this.DbSet.Where(
                    x =>
                    x.InventoryType_Id == inventoryTypeId
                    && !DbContext.ComputerInventories.Where(ci => ci.Computer_Id == computerId)
                            .Select(ci => ci.Inventory_Id)
                            .Contains(x.Id)).Select(x => new { x.Id, Name = x.InventoryName }).ToList();

            var overviews =
                anonymus.Select(c => new ItemOverview(c.Name, c.Id.ToString(CultureInfo.InvariantCulture))).ToList();

            return overviews;
        }

        public ReportModelWithInventoryType FindAllConnectedInventory(int customerId, int inventoryTypeId, int? departmentId, string searchFor)
        {
            var query =
                this.DbSet.Where(
                    x => x.InventoryType.Customer_Id == customerId && x.InventoryType.Id == inventoryTypeId);

            if (departmentId.HasValue)
            {
                query = query.Where(x => x.Department_Id == departmentId);
            }

            if (!string.IsNullOrEmpty(searchFor))
            {
                var pharseInLowerCase = searchFor.ToLower();
                query = query.Where(x => x.InventoryName.ToLower().Contains(pharseInLowerCase));
            }

            var anonymus = (from q in query
                            join ci in DbContext.ComputerInventories on q.Id equals ci.Inventory_Id into result
                            from k in result.DefaultIfEmpty()
                            select
                                new
                                    {
                                        q.InventoryName,
                                        k.Computer.ComputerName,
                                        q.InventoryType_Id,
                                        InventoryTypeName = q.InventoryType.Name
                                    }).ToList();

            var grouped = anonymus.GroupBy(x => new { x.InventoryType_Id, x.InventoryTypeName }).Single();

            var models = grouped.Select(x => new ReportModel(x.InventoryName, x.ComputerName)).ToList();
            var model = new ReportModelWithInventoryType(
                grouped.Key.InventoryType_Id,
                grouped.Key.InventoryTypeName,
                models);

            return model;
        }
    }
}
