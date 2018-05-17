namespace DH.Helpdesk.Dal.Repositories.Printers.Concrete
{
    using System.Collections.Generic;
    using System.Linq;

    using DH.Helpdesk.BusinessData.Models.Inventory.Edit.Printer;
    using DH.Helpdesk.BusinessData.Models.Inventory.Output.Printer;
    using DH.Helpdesk.Common.Types;
    using DH.Helpdesk.Dal.Dal;
    using DH.Helpdesk.Dal.Infrastructure;

    public class PrinterRepository : Repository<Domain.Printers.Printer>, IPrinterRepository
    {
        public PrinterRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }

        public void Add(PrinterForInsert businessModel)
        {
            var entity = new Domain.Printers.Printer();
            Map(entity, businessModel);
            entity.Customer_Id = businessModel.CustomerId;
            entity.CreatedDate = businessModel.CreatedDate;
            entity.ChangedByUser_Id = businessModel.ChangedByUserId;

            entity.ChangedDate = businessModel.CreatedDate; // todo
            entity.Theftmark = string.Empty;

            this.DbSet.Add(entity);
            this.InitializeAfterCommit(businessModel, entity);
        }

        public void Update(PrinterForUpdate businessModel)
        {
            var entity = this.DbSet.Find(businessModel.Id);
            Map(entity, businessModel);
            entity.ChangedDate = businessModel.ChangedDate;
            entity.ChangedByUser_Id = businessModel.ChangedByUserId;
        }

        public override void DeleteById(int id)
        {
            var entity = this.DbSet.Find(id);
            entity.OperationObjects.Clear();
            base.DeleteById(id);
        }

        public PrinterForRead FindById(int id)
        {
            var anonymus =
                this.DbSet.Where(x => x.Id == id)
                    .Select(
                        x =>
                        new
                            {
                                Entity = x,
                                FloorId = (int?)x.Room.Floor_Id,
                                BuildingId = (int?)x.Room.Floor.Building_Id
                            })
                    .GroupJoin(
                        this.DbContext.Users,
                        s => s.Entity.ChangedByUser_Id,
                        u => u.Id,
                        (s, res) => new { s, res })
                    .SelectMany(
                        t => t.res.DefaultIfEmpty(),
                        (t, k) =>
                        new
                            {
                                t.s.Entity,
                                t.s.BuildingId,
                                t.s.FloorId,
                                UserFirstName = k.FirstName,
                                UserSurName = k.SurName
                            })
                    .Single();

            var serverAggregate = new PrinterForRead(
                anonymus.Entity.Id,
                new BusinessData.Models.Inventory.Edit.Shared.InventoryFields(
                    anonymus.Entity.BarCode,
                    anonymus.Entity.PurchaseDate),
                new BusinessData.Models.Inventory.Edit.Printer.GeneralFields(
                    anonymus.Entity.PrinterName,
                    anonymus.Entity.Manufacturer,
                    anonymus.Entity.PrinterType,
                    anonymus.Entity.SerialNumber),
                new BusinessData.Models.Inventory.Edit.Printer.CommunicationFields(
                    anonymus.Entity.PrinterServer,
                    anonymus.Entity.IPAddress,
                    anonymus.Entity.MACAddress),
                new BusinessData.Models.Inventory.Edit.Printer.OtherFields(
                    anonymus.Entity.NumberOfTrays,
                    anonymus.Entity.DriverName,
                    anonymus.Entity.Info,
                    anonymus.Entity.URL),
                new BusinessData.Models.Inventory.Edit.Printer.OrganizationFields(
                    anonymus.Entity.Department_Id,
                    anonymus.Entity.OU),
                new BusinessData.Models.Inventory.Edit.Shared.PlaceFields(
                    anonymus.BuildingId,
                    anonymus.FloorId,
                    anonymus.Entity.Room_Id,
                    anonymus.Entity.Location),
                anonymus.Entity.CreatedDate,
                anonymus.Entity.ChangedDate,
                anonymus.Entity.SyncChangedDate,
                new StateFields(
                    !string.IsNullOrWhiteSpace(anonymus.UserFirstName)
                        ? new UserName(anonymus.UserFirstName, anonymus.UserSurName)
                        : null));

            return serverAggregate;
        }

        public List<PrinterOverview> FindOverviews(int customerId, int? departmentId, string searchFor, int? recordCount)
        {
            var query = this.DbSet.Where(x => x.Customer_Id == customerId);

            if (!string.IsNullOrEmpty(searchFor))
            {
                var pharseInLowerCase = searchFor.ToLower();

                query =
                    query.Where(
                        c =>
                        c.PrinterName.ToLower().Contains(pharseInLowerCase)
                        || c.Manufacturer.ToLower().Contains(pharseInLowerCase)
                        || c.PrinterType.ToLower().Contains(pharseInLowerCase)
                        || c.SerialNumber.ToLower().Contains(pharseInLowerCase));
            }

            if (departmentId.HasValue)
            {
                query = query.Where(x => x.Department_Id == departmentId);
            }
            if (recordCount.HasValue)
                query = query.OrderBy(x => x.PrinterName).Take(recordCount.Value);

            var anonymus = query.Select(x => new
                        {
                            Entity = x,
                            RoomName = x.Room.Name,
                            x.Department.DepartmentName
                        }).ToList();

            var overviewAggregates =
                anonymus.Select(
                    x =>
                    new PrinterOverview(
                        x.Entity.Id,
                        x.Entity.Customer_Id,
                        x.Entity.CreatedDate,
                        x.Entity.ChangedDate,
                        x.Entity.SyncChangedDate,
                        new BusinessData.Models.Inventory.Output.Printer.GeneralFields(
                        x.Entity.PrinterName,
                        x.Entity.Manufacturer,
                        x.Entity.PrinterType,
                        x.Entity.SerialNumber),
                        new BusinessData.Models.Inventory.Output.Shared.InventoryFields(
                        x.Entity.BarCode,
                        x.Entity.PurchaseDate),
                        new BusinessData.Models.Inventory.Output.Printer.CommunicationFields(
                        x.Entity.PrinterServer,
                        x.Entity.IPAddress,
                        x.Entity.MACAddress),
                        new BusinessData.Models.Inventory.Output.Printer.OtherFields(
                        x.Entity.NumberOfTrays,
                        x.Entity.DriverName,
                        x.Entity.Info,
                        x.Entity.URL),
                        new BusinessData.Models.Inventory.Output.Printer.OrganizationFields(
                        x.DepartmentName,
                        x.Entity.OU),
                        new BusinessData.Models.Inventory.Output.Shared.PlaceFields(
                        x.RoomName,
                        x.Entity.Location))).ToList();

            return overviewAggregates;
        }

        public int GetPrinterCount(int customerId, int? departmentId)
        {
            var query = this.DbSet.Where(x => x.Customer_Id == customerId);

            if (departmentId.HasValue)
            {
                query = query.Where(x => x.Department_Id == departmentId);
            }

            var result = query.Count();

            return result;
        }

        public int GetIdByName(string printerName, int customerId)
        {
            var printer = DbSet.FirstOrDefault(x => x.Customer_Id == customerId && x.PrinterName.Equals(printerName));
            return printer?.Id ?? 0;
        }

        private static void Map(Domain.Printers.Printer entity, Printer businessModel)
        {
            entity.PrinterName = businessModel.GeneralFields.Name ?? string.Empty;
            entity.Manufacturer = businessModel.GeneralFields.Manufacturer ?? string.Empty;
            entity.PrinterType = businessModel.GeneralFields.Model ?? string.Empty;
            entity.SerialNumber = businessModel.GeneralFields.SerialNumber ?? string.Empty;

            entity.BarCode = businessModel.InventoryFields.BarCode ?? string.Empty;
            entity.PurchaseDate = businessModel.InventoryFields.PurchaseDate;

            entity.PrinterServer = businessModel.CommunicationFields.NetworkAdapterName ?? string.Empty;
            entity.IPAddress = businessModel.CommunicationFields.IPAddress ?? string.Empty;
            entity.MACAddress = businessModel.CommunicationFields.MacAddress ?? string.Empty;

            entity.NumberOfTrays = businessModel.OtherFields.NumberOfTrays ?? string.Empty;
            entity.DriverName = businessModel.OtherFields.Driver ?? string.Empty;
            entity.Info = businessModel.OtherFields.Info ?? string.Empty;
            entity.URL = businessModel.OtherFields.URL ?? string.Empty;

            entity.Room_Id = businessModel.PlaceFields.RoomId;
            entity.Location = businessModel.PlaceFields.Location ?? string.Empty;

            entity.Department_Id = businessModel.OrganizationFields.DepartmentId;
            entity.OU = businessModel.OrganizationFields.UnitId ?? string.Empty;
        }
    }
}
