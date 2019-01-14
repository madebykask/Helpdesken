using DH.Helpdesk.BusinessData.Enums.Inventory;

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

    using Inventory = DH.Helpdesk.BusinessData.Models.Inventory.Edit.Inventory.Inventory;

    public class InventoryRepository : Repository<Domain.Inventory.Inventory>, IInventoryRepository
    {
        public InventoryRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }

        public void Add(InventoryForInsert businessModel)
        {
            var entity = new Domain.Inventory.Inventory();
            this.Map(businessModel, entity);

            entity.InventoryType_Id = businessModel.InventoryTypeId;
            entity.CreatedDate = businessModel.CreatedDate;
            entity.ChangedByUser_Id = businessModel.ChangeByUserId;

            entity.ChangedDate = businessModel.CreatedDate; 
            entity.SyncChangedDate = null; // todo?

            this.DbSet.Add(entity);
            this.InitializeAfterCommit(businessModel, entity);
        }

        public void Update(InventoryForUpdate businessModel)
        {
            var entity = this.DbSet.Find(businessModel.Id);
            this.Map(businessModel, entity);

            entity.ChangedDate = businessModel.ChangeDate;
            entity.ChangedByUser_Id = businessModel.ChangeByUserId;
        }

        public InventoryForRead FindById(int id)
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
                    new InventoryForRead(
                        x.Key.Entity.Id,
                        x.Key.Entity.Department_Id,
                        x.Key.Entity.Room_Id,
                        x.Key.Entity.InventoryName,
                        x.Key.Entity.InventoryModel,
                        x.Key.Entity.Manufacturer,
                        x.Key.Entity.SerialNumber,
                        x.Key.Entity.TheftMark,
                        x.Key.Entity.BarCode,
                        x.Key.Entity.PurchaseDate,
                        x.Key.Entity.Info,
                        x.Key.Entity.CreatedDate,
                        x.Key.Entity.ChangedDate,
                        x.Select(w => w.Workstation).ToArray(),
                        x.Key.Entity.InventoryType_Id,
                        x.Key.InventoryTypeName,
                        x.Key.BuildingId,
                        x.Key.FloorId,
                        x.Key.Entity.SyncChangedDate)).Single();

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
                            Worstations = computerName,
                            entity.CreatedDate,
                            entity.ChangedDate,
                            entity.SyncChangedDate
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
                            a.Info,
                            a.CreatedDate,
                            a.ChangedDate,
                            a.SyncChangedDate)).ToList();

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
                query = query.Where( x =>
                        x.InventoryName == searchString || x.InventoryModel == searchString
                        || x.Manufacturer == searchString || x.SerialNumber == searchString);
            }

            /*-1: take all records*/
            if (pageSize != -1)
                query = query.OrderBy(x => x.InventoryName).Take(pageSize);            

            const string Delimeter = "; ";

            var anonymus = (from i in query
                join ci in DbContext.ComputerInventories on i.Id equals ci.Inventory_Id into res
                from k in res.DefaultIfEmpty()
                select new
                {
                    i.Id,
                    DepartmentName = i.Department != null ? i.Department.DepartmentName : string.Empty,
                    RoomName = i.Room != null ? i.Room.Name : string.Empty,
                    UserFirstName = i.ChangedByUser != null ? i.ChangedByUser.FirstName : string.Empty,
                    UserLastName = i.ChangedByUser != null ? i.ChangedByUser.SurName : string.Empty,
                    i.InventoryName,
                    i.InventoryModel,
                    i.Manufacturer,
                    i.SerialNumber,
                    i.TheftMark,
                    i.BarCode,
                    i.PurchaseDate,
                    i.Info,
                    WorkstationName = k.Computer != null ? k.Computer.ComputerName : string.Empty,
                    i.CreatedDate,
                    i.ChangedDate,
                    i.SyncChangedDate
                }).ToList()
                .GroupBy(x => new
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
                    x.CreatedDate,
                    x.ChangedDate,
                    x.SyncChangedDate
                });

            var overviews = anonymus.Select(a =>
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
                    a.Key.Info,
                    a.Key.CreatedDate,
                    a.Key.ChangedDate,
                    a.Key.SyncChangedDate)).ToList();

            return overviews;
        }

        public List<ItemOverview> FindNotConnectedOverviews(int inventoryTypeId, int computerId)
        {
            var anonymus =
                this.DbSet.Where(
                    x =>
                    x.InventoryType_Id == inventoryTypeId
                    && !DbContext.ComputerInventories
                            .Where(ci => ci.Computer_Id == computerId)
                            .Select(ci => ci.Inventory_Id)
                            .Contains(x.Id)).Select(x => new { x.Id, Name = x.InventoryName }).ToList();

            var overviews =
                anonymus.Select(c => new ItemOverview(c.Name, c.Id.ToString(CultureInfo.InvariantCulture))).ToList();

            return overviews;
        }

        public ReportModelWithInventoryType FindAllConnectedInventory(int inventoryTypeId, int? departmentId, string searchFor)
        {
            var query =
                this.DbSet.Where(x => x.InventoryType.Id == inventoryTypeId);

            if (departmentId.HasValue)
            {
                query = query.Where(x => x.Department_Id == departmentId);
            }

            if (!string.IsNullOrEmpty(searchFor))
            {
                var pharseInLowerCase = searchFor.ToLower();
                query = query.Where(x => x.InventoryName.ToLower().Contains(pharseInLowerCase));
            }

            var anonymus = (from ti in DbContext.InventoryTypes.Where(x => x.Id == inventoryTypeId)
                            join q in query on ti.Id equals q.InventoryType_Id into fake
                            from c in fake.DefaultIfEmpty()
                            join ci in DbContext.ComputerInventories on c.Id equals ci.Inventory_Id into result
                            from k in result.DefaultIfEmpty()
                            select
                                new
                                    {
                                        c.InventoryName,
                                        k.Computer.ComputerName,
                                        ComputerId = (int?)k.Computer.Id,
                                        InventoryType_Id = ti.Id,
                                        InventoryTypeName = ti.Name
                                    }).ToList();

            var grouped = anonymus.GroupBy(x => new { x.InventoryType_Id, x.InventoryTypeName }).Single();

            var models =
                grouped.Where(x => x.InventoryName != null)
                    .Select(x => new ReportModel(x.InventoryName, x.ComputerName, x.ComputerId))
                    .ToList();

            var model = new ReportModelWithInventoryType(
                grouped.Key.InventoryType_Id,
                grouped.Key.InventoryTypeName,
                models);

            return model;
        }

        public void DeleteByInventoryTypeId(int inventoryTypeId)
        {
            var models = this.DbSet.Where(x => x.InventoryType_Id == inventoryTypeId).ToList();
            models.ForEach(x => this.DbSet.Remove(x));
        }

        public int GetIdByName(string inventoryName, int inventoryTypeId)
        {
            var inventory = DbSet.FirstOrDefault(x => x.InventoryType_Id == inventoryTypeId && x.InventoryName.Equals(inventoryName));
            return inventory?.Id ?? 0;
        }

        public List<InventorySearchResult> SearchPcNumber(int customerId, string searchFor)
        {
            var s = searchFor.ToLower();

            var workstations =
                from c in this.DbContext.Computers
                join ct in this.DbContext.ComputerTypes on c.ComputerType_Id equals ct.Id into res
                from k in res.DefaultIfEmpty()
                where c.Customer_Id == customerId && (c.ComputerName.ToLower().Contains(s) || c.Location.ToLower().Contains(s) || k.ComputerTypeDescription.ToLower().Contains(s))
                select new InventorySearchResult
                {
                    Id = c.Id,
                    Name = c.ComputerName,
                    Location = c.Location,
                    TypeDescription = k.ComputerTypeDescription, //Computer Type
                    TypeName = "Arbetsstation",
                    NeedTranslate = true
                };
            var result = workstations.ToList();

            var servers = DbContext.Servers.Where(x => x.Customer_Id == customerId && (x.ServerName.ToLower().Contains(s) || x.Location.ToLower().Contains(s)))
                    .Select(x => new InventorySearchResult
                    {
                        Id = x.Id,
                        Name = x.ServerName,
                        Location = x.Location,
                        TypeDescription = string.Empty,
                        TypeName = "Server",
                        NeedTranslate = true
                    }).ToList();
            result.AddRange(servers);
            var printers = DbContext.Printers.Where(x => x.Customer_Id == customerId && (x.PrinterName.ToLower().Contains(s) || x.Location.ToLower().Contains(s)))
                    .Select(x => new InventorySearchResult
                    {
                        Id = x.Id,
                        Name = x.PrinterName,
                        Location = x.Location,
                        TypeDescription = string.Empty,
                        TypeName = "Skrivare",
                        NeedTranslate = true
                    }).ToList();
            result.AddRange(printers);
            var customInventories = DbContext.Inventories.Where(x => x.InventoryType.Customer_Id == customerId).OrderBy(x => x.InventoryType.Name)
                .Where(x => x.InventoryName.ToLower().Contains(s))
                    .Select(x => new InventorySearchResult
                    {
                        Id = x.Id,
                        Name = x.InventoryName,
                        Location = string.Empty,
                        TypeDescription = string.Empty,
                        TypeName = x.InventoryType.Name
                    }).ToList();
            result.AddRange(customInventories);

            return result;
        }

        public InventorySearchResult SearchPcNumberByUserId(int customerId, int userId)
        {

            var workstations =
                from c in this.DbContext.Computers
                join cu in this.DbContext.ComputerUsers on c.User_Id equals cu.Id into res
                from k in res.DefaultIfEmpty()
                where c.Customer_Id == customerId && (k.Id == userId)
                orderby c.SyncChangedDate descending
                select new InventorySearchResult
                {
                    Id = c.Id,
                    Name = c.ComputerName,
                    Location = c.Location,
                    TypeDescription = "", //Computer Type
                    TypeName = "Arbetsstation",
                    NeedTranslate = true
                };
            var result = workstations.FirstOrDefault();
            return result;
        }

        public List<int> GetRelatedCaseIds(CurrentModes inventoryType, int inventoryId, int customerId)
        {
            var caseIds = new List<int>();
            string inventoryName;
            switch (inventoryType)
            {
                case CurrentModes.Workstations:
                    inventoryName = DbContext.Computers.Single(x => x.Id == inventoryId).ComputerName;
                    break;
                case CurrentModes.Servers:
                    inventoryName = DbContext.Servers.Single(x => x.Id == inventoryId).ServerName;
                    break;
                case CurrentModes.Printers:
                    inventoryName = DbContext.Printers.Single(x => x.Id == inventoryId).PrinterName;
                    break;

                default:
                    inventoryName = DbContext.Inventories.Single(x => x.Id == inventoryId).InventoryName;
                    break;
            }
            if (!string.IsNullOrEmpty(inventoryName))
            {
                caseIds = DbContext.Cases.Where(x => x.Customer_Id == customerId && x.InventoryNumber.Equals(inventoryName))
                            .Select(x => x.Id)
                            .ToList();
            }
            return caseIds;
        }

        private void Map(Inventory businessModel, Domain.Inventory.Inventory entity)
        {
            entity.Department_Id = businessModel.DepartmentId;
            entity.Room_Id = businessModel.RoomId;
            entity.InventoryName = businessModel.Name ?? string.Empty;
            entity.InventoryModel = businessModel.Model ?? string.Empty;
            entity.Manufacturer = businessModel.Manufacturer ?? string.Empty;
            entity.SerialNumber = businessModel.SerialNumber ?? string.Empty;
            entity.TheftMark = businessModel.TheftMark ?? string.Empty;
            entity.BarCode = businessModel.BarCode ?? string.Empty;
            entity.PurchaseDate = businessModel.PurchaseDate;
            entity.Info = businessModel.Info ?? string.Empty;
        }
    }
}
