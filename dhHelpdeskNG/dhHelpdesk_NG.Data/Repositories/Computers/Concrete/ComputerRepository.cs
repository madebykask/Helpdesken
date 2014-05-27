namespace DH.Helpdesk.Dal.Repositories.Computers.Concrete
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using DH.Helpdesk.BusinessData.Models;
    using DH.Helpdesk.BusinessData.Models.Inventory;
    using DH.Helpdesk.BusinessData.Models.Inventory.Edit.Computer;
    using DH.Helpdesk.BusinessData.Models.Inventory.Output.Computer;
    using DH.Helpdesk.Common.Extensions.Boolean;
    using DH.Helpdesk.Common.Extensions.Integer;
    using DH.Helpdesk.Common.Types;
    using DH.Helpdesk.Dal.Dal;
    using DH.Helpdesk.Dal.Infrastructure;

    public class ComputerRepository : Repository<Domain.Computers.Computer>, IComputerRepository
    {
        public ComputerRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }

        public void Add(Computer businessModel)
        {
            var entity = new Domain.Computers.Computer();
            Map(entity, businessModel);
            entity.Customer_Id = businessModel.CustomerId;
            entity.CreatedDate = businessModel.CreatedDate;

            this.DbSet.Add(entity);
            this.InitializeAfterCommit(businessModel, entity);
        }

        public void Update(Computer businessModel)
        {
            var entity = this.DbSet.Find(businessModel.Id);
            Map(entity, businessModel);
            entity.ChangedDate = businessModel.ChangedDate;
        }

        public Computer FindById(int id)
        {
            var anonymus =
                this.DbSet.Where(x => x.Id == id)
                    .Select(
                        x =>
                        new
                            {
                                Entity = x,
                                FloorId = (int?)x.Room.Floor_Id,
                                BuildingId = (int?)x.Room.Floor.Building_Id,
                                UserId = (int?)x.User.Id,
                                UserStringId = x.User.LogonName,
                                UserFirstName = x.User.FirstName,
                                UserSurName = x.User.SurName,
                                UserDepartmentName = x.User.Department.DepartmentName,
                                UserUnitName = x.User.OU.Name
                            })
                    .Single();

            var workstation = new BusinessData.Models.Inventory.Edit.Computer.WorkstationFields(
                anonymus.Entity.ComputerName,
                anonymus.Entity.Manufacturer,
                anonymus.Entity.ComputerModel_Id,
                anonymus.Entity.SerialNumber,
                anonymus.Entity.BIOSVersion,
                anonymus.Entity.BIOSDate,
                anonymus.Entity.TheftMark,
                anonymus.Entity.CarePackNumber,
                anonymus.Entity.ComputerType_Id,
                anonymus.Entity.Location);

            var processor = new BusinessData.Models.Inventory.Edit.Shared.ProcessorFields(anonymus.Entity.Processor_Id);

            var organization = new BusinessData.Models.Inventory.Edit.Computer.OrganizationFields(
                anonymus.Entity.Department_Id,
                anonymus.Entity.Domain_Id,
                anonymus.Entity.OU_Id);

            var os = new BusinessData.Models.Inventory.Edit.Shared.OperatingSystemFields(
                anonymus.Entity.OS_Id,
                anonymus.Entity.Version,
                anonymus.Entity.SP,
                anonymus.Entity.RegistrationCode,
                anonymus.Entity.ProductKey);

            var memory = new BusinessData.Models.Inventory.Edit.Shared.MemoryFields(anonymus.Entity.RAM_ID);

            var inventoryFields = new BusinessData.Models.Inventory.Edit.Shared.InventoryFields(
                anonymus.Entity.BarCode,
                anonymus.Entity.PurchaseDate);

            var chassis = new BusinessData.Models.Inventory.Edit.Shared.ChassisFields(anonymus.Entity.ChassisType);

            var state = new BusinessData.Models.Inventory.Edit.Computer.StateFields(
                anonymus.Entity.Status,
                anonymus.Entity.Stolen.ToBool(),
                anonymus.Entity.ReplacedWithComputerName,
                anonymus.Entity.SendBack.ToBool(),
                anonymus.Entity.ScrapDate);

            var sound = new BusinessData.Models.Inventory.Edit.Computer.SoundFields(anonymus.Entity.SoundCard);

            var place = new BusinessData.Models.Inventory.Edit.Computer.PlaceFields(
                anonymus.BuildingId,
                anonymus.FloorId,
                anonymus.Entity.Room_Id,
                anonymus.Entity.LocationAddress,
                anonymus.Entity.LocationPostalCode,
                anonymus.Entity.LocationPostalAddress,
                anonymus.Entity.LocationRoom,
                anonymus.Entity.Location2);

            var other = new BusinessData.Models.Inventory.Edit.Computer.OtherFields(anonymus.Entity.Info);

            var graphics = new BusinessData.Models.Inventory.Edit.Computer.GraphicsFields(anonymus.Entity.VideoCard);

            var contract = new BusinessData.Models.Inventory.Edit.Computer.ContractFields(
                anonymus.Entity.ContractStatus_Id,
                anonymus.Entity.ContractNumber,
                anonymus.Entity.ContractStartDate,
                anonymus.Entity.ContractEndDate,
                anonymus.Entity.Price,
                anonymus.Entity.AccountingDimension1,
                anonymus.Entity.AccountingDimension2,
                anonymus.Entity.AccountingDimension3,
                anonymus.Entity.AccountingDimension4,
                anonymus.Entity.AccountingDimension5);

            var contactInfo = new BusinessData.Models.Inventory.Edit.Computer.ContactInformationFields(
                anonymus.UserId,
                anonymus.UserStringId,
                anonymus.UserDepartmentName,
                anonymus.UserUnitName,
                anonymus.UserId.HasValue ? new UserName(anonymus.UserFirstName, anonymus.UserSurName) : null);

            var contact = new BusinessData.Models.Inventory.Edit.Computer.ContactFields(
                anonymus.Entity.ContactName,
                anonymus.Entity.ContactPhone,
                anonymus.Entity.ContactEmailAddress);

            var communication = new BusinessData.Models.Inventory.Edit.Computer.CommunicationFields(
                anonymus.Entity.NIC_ID,
                anonymus.Entity.IPAddress,
                anonymus.Entity.MACAddress,
                anonymus.Entity.RAS.ToBool(),
                anonymus.Entity.NovellClient);

            var date = new BusinessData.Models.Inventory.Edit.Computer.DateFields(
                anonymus.Entity.SyncChangedDate,
                anonymus.Entity.ScanDate,
                anonymus.Entity.LDAPPath);

            var computerAggregate = Computer.CreateForEdit(
                anonymus.Entity.Id,
                date,
                communication,
                contact,
                contactInfo,
                contract,
                graphics,
                other,
                place,
                sound,
                state,
                chassis,
                inventoryFields,
                memory,
                os,
                organization,
                processor,
                workstation,
                anonymus.Entity.CreatedDate,
                anonymus.Entity.ChangedDate);

            return computerAggregate;
        }

        public List<ComputerOverview> FindOverviews(
            int customerId,
            int? regionId,
            int? departmentId,
            int? computerTypeId,
            int? contractStatusId,
            DateTime? contractStartDateFrom,
            DateTime? contractStartDateTo,
            DateTime? contractEndDateFrom,
            DateTime? contractEndDateTo,
            DateTime? scanDateFrom,
            DateTime? scanDateTo,
            DateTime? scrapDateFrom,
            DateTime? scrapDateTo,
            string searchFor,
            bool isShowScrapped,
            int recordsOnPage)
        {
            var query = this.DbSet.Where(x => x.Customer_Id == customerId);

            query = isShowScrapped ? query.Where(x => x.ScrapDate != null) : query.Where(x => x.ScrapDate == null);

            if (regionId.HasValue)
            {
                query = query.Where(x => x.Department.Region_Id == regionId);
            }

            if (departmentId.HasValue)
            {
                query = query.Where(x => x.Department_Id == departmentId);
            }

            if (computerTypeId.HasValue)
            {
                query = query.Where(x => x.ComputerType_Id == computerTypeId);
            }

            if (contractStatusId.HasValue)
            {
                query = query.Where(x => x.ContractStatus_Id == contractStatusId);
            }

            if (contractStartDateFrom.HasValue)
            {
                query = query.Where(x => x.ContractStartDate >= contractStartDateFrom);
            }

            if (contractStartDateTo.HasValue)
            {
                query = query.Where(x => x.ContractStartDate <= contractStartDateFrom);
            }

            if (contractEndDateFrom.HasValue)
            {
                query = query.Where(x => x.ContractEndDate >= contractEndDateFrom);
            }

            if (contractEndDateTo.HasValue)
            {
                query = query.Where(x => x.ContractEndDate <= contractEndDateTo);
            }

            if (scanDateFrom.HasValue)
            {
                query = query.Where(x => x.ScanDate >= scanDateFrom);
            }

            if (scanDateTo.HasValue)
            {
                query = query.Where(x => x.ScanDate <= scanDateTo);
            }

            if (scrapDateFrom.HasValue)
            {
                query = query.Where(x => x.ScrapDate >= scrapDateFrom);
            }

            if (scrapDateTo.HasValue)
            {
                query = query.Where(x => x.ScrapDate <= scrapDateTo);
            }

            if (!string.IsNullOrEmpty(searchFor))
            {
                var pharseInLowerCase = searchFor.ToLower();

                query =
                    query.Where(
                        c =>
                        c.ComputerName.ToLower().Contains(pharseInLowerCase)
                        || c.ComputerModel.Name.ToLower().Contains(pharseInLowerCase)
                        || c.Manufacturer.ToLower().Contains(pharseInLowerCase)
                        || c.SerialNumber.ToLower().Contains(pharseInLowerCase)
                        || c.BIOSVersion.ToLower().Contains(pharseInLowerCase)
                        || c.TheftMark.ToLower().Contains(pharseInLowerCase)
                        || c.CarePackNumber.ToLower().Contains(pharseInLowerCase)
                        || c.ComputerType.Name.ToLower().Contains(pharseInLowerCase)
                        || c.Location.ToLower().Contains(pharseInLowerCase));
            }

            var anonymus =
                query.Select(
                    x =>
                    new
                        {
                            Entity = x,
                            ComputerTypeName = x.ComputerType.Name,
                            ComputerModelName = x.ComputerModel.Name,
                            OperatingName = x.OS.Name,
                            ProcessorName = x.Processor.Name,
                            RamName = x.RAM.Name,
                            NetworkAdapterName = x.NIC.Name,
                            x.Department.DepartmentName,
                            DomainName = x.Domain.Name,
                            UnitName = x.OU.Name,
                            RoomName = x.Room.Name,
                            UserId = x.User.LogonName,
                            UserDepartmentName = x.User.Department.DepartmentName,
                            UserUnitName = x.User.OU.Name
                        }).Take(recordsOnPage).ToList();

            var overviewAggregates =
                anonymus.Select(
                    x =>
                    new ComputerOverview(
                        x.Entity.Id,
                        x.Entity.Customer_Id,
                        x.Entity.CreatedDate,
                        x.Entity.ChangedDate,
                        new BusinessData.Models.Inventory.Output.Computer.WorkstationFields(
                        x.Entity.ComputerName,
                        x.Entity.Manufacturer,
                        x.ComputerModelName,
                        x.Entity.SerialNumber,
                        x.Entity.BIOSVersion,
                        x.Entity.BIOSDate,
                        x.Entity.TheftMark,
                        x.Entity.CarePackNumber,
                        x.ComputerTypeName,
                        x.Entity.Location),
                        new BusinessData.Models.Inventory.Output.Shared.ProcessorFields(x.ProcessorName),
                        new BusinessData.Models.Inventory.Output.Computer.OrganizationFields(
                        x.DepartmentName,
                        x.DomainName,
                        x.UnitName),
                        new BusinessData.Models.Inventory.Output.Shared.OperatingSystemFields(
                        x.OperatingName,
                        x.Entity.Version,
                        x.Entity.SP,
                        x.Entity.RegistrationCode,
                        x.Entity.ProductKey),
                        new BusinessData.Models.Inventory.Output.Shared.MemoryFields(x.RamName),
                        new BusinessData.Models.Inventory.Output.Shared.InventoryFields(
                        x.Entity.BarCode,
                        x.Entity.PurchaseDate),
                        new BusinessData.Models.Inventory.Output.Shared.ChassisFields(x.Entity.ChassisType),
                        new BusinessData.Models.Inventory.Output.Computer.StateFields(
                        (ComputerStatuses)x.Entity.Status,
                        x.Entity.Stolen.ToBool(),
                        x.Entity.ReplacedWithComputerName,
                        x.Entity.SendBack.ToBool(),
                        x.Entity.ScrapDate),
                        new BusinessData.Models.Inventory.Output.Computer.SoundFields(x.Entity.SoundCard),
                        new BusinessData.Models.Inventory.Output.Computer.PlaceFields(
                        x.RoomName,
                        x.Entity.LocationAddress,
                        x.Entity.LocationPostalCode,
                        x.Entity.LocationPostalAddress,
                        x.Entity.LocationRoom,
                        x.Entity.Location2),
                        new BusinessData.Models.Inventory.Output.Computer.OtherFields(x.Entity.Info),
                        new BusinessData.Models.Inventory.Output.Computer.GraphicsFields(x.Entity.VideoCard),
                        new BusinessData.Models.Inventory.Output.Computer.ContractFields(
                        (ContractStatuses?)x.Entity.ContractStatus_Id,
                        x.Entity.ContractNumber,
                        x.Entity.ContractStartDate,
                        x.Entity.ContractEndDate,
                        x.Entity.Price,
                        x.Entity.AccountingDimension1,
                        x.Entity.AccountingDimension2,
                        x.Entity.AccountingDimension3,
                        x.Entity.AccountingDimension4,
                        x.Entity.AccountingDimension5),
                        new BusinessData.Models.Inventory.Output.Computer.ContactInformationFields(
                        x.UserId,
                        x.UserDepartmentName,
                        x.UserUnitName),
                        new BusinessData.Models.Inventory.Output.Computer.ContactFields(
                        x.Entity.ContactName,
                        x.Entity.ContactPhone,
                        x.Entity.ContactEmailAddress),
                        new BusinessData.Models.Inventory.Output.Computer.CommunicationFields(
                        x.NetworkAdapterName,
                        x.Entity.IPAddress,
                        x.Entity.MACAddress,
                        x.Entity.RAS.ToBool(),
                        x.Entity.NovellClient),
                        new BusinessData.Models.Inventory.Output.Computer.DateFields(
                        x.Entity.SyncChangedDate,
                        x.Entity.ScanDate,
                        x.Entity.LDAPPath))).ToList();

            return overviewAggregates;
        }

        // todo
        public List<ComputerResults> Search(int customerId, string searchFor)
        {
            var s = searchFor.ToLower();

            var query =
                from c in this.DbContext.Computers
                join ct in this.DbContext.ComputerTypes on c.ComputerType_Id equals ct.Id into res
                from k in res.DefaultIfEmpty()
                where c.Customer_Id == customerId
                      && (
                             c.ComputerName.ToLower().Contains(s)
                             || c.Location.ToLower().Contains(s)
                             || k.ComputerTypeDescription.ToLower().Contains(s))
                select new ComputerResults
                           {
                               Id = c.Id,
                               ComputerName = c.ComputerName,
                               Location = c.Location,
                               ComputerType = k.Name,
                               ComputerTypeDescription = k.ComputerTypeDescription
                           };

            return query.OrderBy(x => x.ComputerName).ThenBy(x => x.Location).ToList();
        }

        public void RemoveReferenceOnNic(int id)
        {
            var models =
                this.DbSet.Where(x => x.NIC_ID == id).ToList();

            foreach (var item in models)
            {
                item.NIC_ID = null;
            }
        }

        public void RemoveReferenceOnRam(int id)
        {
            var models =
                this.DbSet.Where(x => x.RAM_ID == id).ToList();

            foreach (var item in models)
            {
                item.RAM_ID = null;
            }
        }

        public void RemoveReferenceOnProcessor(int id)
        {
            var models =
                this.DbSet.Where(x => x.Processor_Id == id).ToList();

            foreach (var item in models)
            {
                item.Processor_Id = null;
            }
        }

        public void RemoveReferenceOnOs(int id)
        {
            var models =
                this.DbSet.Where(x => x.OS_Id == id).ToList();

            foreach (var item in models)
            {
                item.OS_Id = null;
            }
        }

        public void RemoveReferenceOnComputerType(int id)
        {
            var models =
                this.DbSet.Where(x => x.ComputerType_Id == id).ToList();

            foreach (var item in models)
            {
                item.ComputerType_Id = null;
            }
        }

        public void RemoveReferenceOnComputerModel(int id)
        {
            var models =
                this.DbSet.Where(x => x.ComputerModel_Id == id).ToList();

            foreach (var item in models)
            {
                item.ComputerModel_Id = null;
            }
        }

        public int GetComputerCount(int customerId, int? departmentId)
        {
            var query = this.DbSet.Where(x => x.Customer_Id == customerId);

            if (departmentId.HasValue)
            {
                query = query.Where(x => x.Department_Id == departmentId);
            }

            var result = query.Count();

            return result;
        }

        private static void Map(Domain.Computers.Computer entity, Computer businessModel)
        {
            entity.ComputerName = businessModel.WorkstationFields.ComputerName;
            entity.Manufacturer = businessModel.WorkstationFields.Manufacturer;
            entity.ComputerModel_Id = businessModel.WorkstationFields.ComputerModelId;
            entity.SerialNumber = businessModel.WorkstationFields.SerialNumber;
            entity.BIOSVersion = businessModel.WorkstationFields.BIOSVersion;
            entity.BIOSDate = businessModel.WorkstationFields.BIOSDate;
            entity.TheftMark = businessModel.WorkstationFields.Theftmark;
            entity.CarePackNumber = businessModel.WorkstationFields.CarePackNumber;
            entity.ComputerType_Id = businessModel.WorkstationFields.ComputerTypeId;
            entity.Location = businessModel.WorkstationFields.Location;

            entity.Processor_Id = businessModel.ProccesorFields.ProccesorId;

            entity.Department_Id = businessModel.OrganizationFields.DepartmentId;
            entity.Domain_Id = businessModel.OrganizationFields.DomainId;
            entity.OU_Id = businessModel.OrganizationFields.UnitId;

            entity.OS_Id = businessModel.OperatingSystemFields.OperatingSystemId;
            entity.Version = businessModel.OperatingSystemFields.Version;
            entity.SP = businessModel.OperatingSystemFields.ServicePack;
            entity.RegistrationCode = businessModel.OperatingSystemFields.RegistrationCode;
            entity.ProductKey = businessModel.OperatingSystemFields.ProductKey;

            entity.RAM_ID = businessModel.MemoryFields.RAMId;

            entity.BarCode = businessModel.InventoryFields.BarCode;
            entity.PurchaseDate = businessModel.InventoryFields.PurchaseDate;

            entity.ChassisType = businessModel.ChassisFields.Chassis;

            entity.Status = businessModel.StateFields.State;
            entity.Stolen = businessModel.StateFields.IsStolen.ToInt();
            entity.ReplacedWithComputerName = businessModel.StateFields.Replaced;
            entity.SendBack = businessModel.StateFields.IsSendBack.ToInt();
            entity.ScrapDate = businessModel.StateFields.ScrapDate;

            entity.SoundCard = businessModel.SoundFields.SoundCard;

            entity.Room_Id = businessModel.PlaceFields.RoomId;
            entity.LocationAddress = businessModel.PlaceFields.Address;
            entity.LocationPostalCode = businessModel.PlaceFields.PostalCode;
            entity.LocationPostalAddress = businessModel.PlaceFields.PostalAddress;
            entity.LocationRoom = businessModel.PlaceFields.Location;
            entity.Location2 = businessModel.PlaceFields.Location2;

            entity.Info = businessModel.OtherFields.Info;

            entity.VideoCard = businessModel.GraphicsFields.VideoCard;

            entity.ContractStatus_Id = businessModel.ContractFields.ContractStatusId;
            entity.ContractNumber = businessModel.ContractFields.ContractNumber;
            entity.ContractStartDate = businessModel.ContractFields.ContractStartDate;
            entity.ContractEndDate = businessModel.ContractFields.ContractEndDate;
            entity.Price = businessModel.ContractFields.PurchasePrice;
            entity.AccountingDimension1 = businessModel.ContractFields.AccountingDimension1;
            entity.AccountingDimension2 = businessModel.ContractFields.AccountingDimension2;
            entity.AccountingDimension3 = businessModel.ContractFields.AccountingDimension3;
            entity.AccountingDimension4 = businessModel.ContractFields.AccountingDimension4;
            entity.AccountingDimension5 = businessModel.ContractFields.AccountingDimension5;

            entity.User_Id = businessModel.ContactInformationFields.UserId;

            entity.ContactName = businessModel.ContactFields.Name;
            entity.ContactPhone = businessModel.ContactFields.Phone;
            entity.ContactEmailAddress = businessModel.ContactFields.Email;

            entity.NIC_ID = businessModel.CommunicationFields.NetworkAdapterId;
            entity.IPAddress = businessModel.CommunicationFields.IPAddress;
            entity.MACAddress = businessModel.CommunicationFields.MacAddress;
            entity.RAS = businessModel.CommunicationFields.IsRAS.ToInt();
            entity.NovellClient = businessModel.CommunicationFields.NovellClient;

            entity.SyncChangedDate = businessModel.DateFields.SynchronizeDate;
            entity.ScanDate = businessModel.DateFields.ScanDate;
            entity.LDAPPath = businessModel.DateFields.PathDirectory;
        }
    }
}
