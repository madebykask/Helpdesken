namespace DH.Helpdesk.Dal.Repositories.Computers.Concrete
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;

    using DH.Helpdesk.BusinessData.Models;
    using DH.Helpdesk.BusinessData.Models.Inventory;
    using DH.Helpdesk.BusinessData.Models.Inventory.Edit.Computer;
    using DH.Helpdesk.BusinessData.Models.Inventory.Output.Computer;
    using DH.Helpdesk.BusinessData.Models.Shared.Input;
    using DH.Helpdesk.Common.Enums;
    using DH.Helpdesk.Common.Extensions.Boolean;
    using DH.Helpdesk.Common.Extensions.Integer;
    using DH.Helpdesk.Common.Types;
    using DH.Helpdesk.Dal.Dal;
    using DH.Helpdesk.Dal.Infrastructure;

    using ComputerFields = DH.Helpdesk.BusinessData.Enums.Inventory.Fields.Computer.WorkstationFields;

    public class ComputerRepository : Repository<Domain.Computers.Computer>, IComputerRepository
    {
        public ComputerRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }

        public void Add(ComputerForInsert businessModel)
        {
            var entity = new Domain.Computers.Computer();
            Map(entity, businessModel);
            entity.Customer_Id = businessModel.CustomerId;
            entity.CreatedDate = businessModel.CreatedDate;
            entity.ChangedByUser_Id = businessModel.ChangedByUserId;

            entity.ChangedDate = businessModel.CreatedDate; // todo

            // todo
            entity.ComputerGUID = string.Empty;
            entity.ComputerModelName = string.Empty;
            entity.HardDrive = string.Empty;
            entity.LoggedUser = string.Empty;
            entity.MonitorModel = string.Empty;
            entity.MonitorSerialnumber = string.Empty;
            entity.MonitorTheftMark = string.Empty;
            entity.LDAPPath = string.Empty;

            this.DbSet.Add(entity);
            this.InitializeAfterCommit(businessModel, entity);
        }

        public void Update(ComputerForUpdate businessModel)
        {
            var entity = this.DbSet.Find(businessModel.Id);
            Map(entity, businessModel);
            entity.ChangedDate = businessModel.ChangedDate;
            entity.ChangedByUser_Id = businessModel.ChangedByUserId;
        }

        public void UpdateInfo(int id, string info)
        {
            var entity = this.DbSet.Find(id);
            entity.Info = info;
        }

        public ComputerForRead FindById(int id)
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
                                UserStringId = x.User.UserId,
                                UserFirstName = x.User.FirstName,
                                UserSurName = x.User.SurName,
                                UserDepartmentName = x.User.Department.DepartmentName,
                                UserUnitName = x.User.OU.Name
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
                                t.s.UserId,
                                t.s.UserStringId,
                                t.s.UserFirstName,
                                t.s.UserSurName,
                                t.s.UserDepartmentName,
                                t.s.UserUnitName,
                                ChangedByUserId = (int?)k.Id,
                                ChangedByUserFirstName = k.FirstName,
                                ChangedByUserSurName = k.SurName
                            })
                    .Single();

            var workstation =
                new BusinessData.Models.Inventory.Edit.Computer.WorkstationFields(
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

            var organization =
                new BusinessData.Models.Inventory.Edit.Computer.OrganizationFields(
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

            var contract =
                new BusinessData.Models.Inventory.Edit.Computer.ContractFields(
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

            var communication =
                new BusinessData.Models.Inventory.Edit.Computer.CommunicationFields(
                    anonymus.Entity.NIC_ID,
                    anonymus.Entity.IPAddress,
                    anonymus.Entity.MACAddress,
                    anonymus.Entity.RAS.ToBool(),
                    anonymus.Entity.NovellClient);

            var date = new BusinessData.Models.Inventory.Edit.Computer.DateFields(
                anonymus.Entity.SyncChangedDate,
                anonymus.Entity.ScanDate,
                anonymus.Entity.LDAPPath,
                anonymus.ChangedByUserId.HasValue ? new UserName(anonymus.ChangedByUserFirstName, anonymus.ChangedByUserSurName) : null);

            var computerAggregate = new ComputerForRead(
                anonymus.Entity.Id,
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
                anonymus.Entity.ChangedDate,
                date);

            return computerAggregate;
        }

        public ComputerShortOverview FindShortOverview(int id)
        {
            var anonymus =
                this.DbSet.Where(x => x.Id == id)
                    .Select(
                        x =>
                        new
                            {
                                x.Id,
                                x.ComputerName,
                                x.Manufacturer,
                                x.ComputerModelName,
                                x.SerialNumber,
                                x.BIOSVersion,
                                x.BIOSDate,
                                OperatingSystemName = x.OS.Name,
                                x.SP,
                                ProcessorName = x.Processor.Name,
                                RamName = x.RAM.Name,
                                NicName = x.NIC.Name,
                                x.IPAddress,
                                x.MACAddress,
                                x.RAS,
                                x.Info
                            })
                    .Single();

            var workstation = new ComputerShortOverview(
                anonymus.Id,
                anonymus.ComputerName,
                anonymus.Manufacturer,
                anonymus.ComputerModelName,
                anonymus.SerialNumber,
                anonymus.BIOSVersion,
                anonymus.BIOSDate,
                anonymus.OperatingSystemName,
                anonymus.SP,
                anonymus.ProcessorName,
                anonymus.RamName,
                anonymus.NicName,
                anonymus.IPAddress,
                anonymus.MACAddress,
                anonymus.RAS.ToBool(),
                anonymus.Info);

            return workstation;
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
            int recordsOnPage,
            SortField sortOptions)
        {
            var query = this.DbSet.Where(x => x.Customer_Id == customerId);

            if (!isShowScrapped)
            {
                query = query.Where(x => x.ScrapDate == null);
            }

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

            if (sortOptions != null && sortOptions.Name != null)
            {
                if (sortOptions.SortBy == SortBy.Ascending)
                {
                    if (sortOptions.Name == ComputerFields.Name)
                    {
                        query = query.OrderBy(x => x.ComputerName);
                    }
                    else if (sortOptions.Name == ComputerFields.Manufacturer)
                    {
                        query = query.OrderBy(x => x.Manufacturer);
                    }
                    else if (sortOptions.Name == ComputerFields.Model)
                    {
                        query = query.OrderBy(x => x.ComputerModel.Name);
                    }
                    else if (sortOptions.Name == ComputerFields.SerialNumber)
                    {
                        query = query.OrderBy(x => x.SerialNumber);
                    }
                    else if (sortOptions.Name == ComputerFields.SerialNumber)
                    {
                        query = query.OrderBy(x => x.SerialNumber);
                    }
                    else if (sortOptions.Name == ComputerFields.BIOSVersion)
                    {
                        query = query.OrderBy(x => x.BIOSVersion);
                    }
                    else if (sortOptions.Name == ComputerFields.BIOSDate)
                    {
                        query = query.OrderBy(x => x.BIOSDate);
                    }
                    else if (sortOptions.Name == ComputerFields.Theftmark)
                    {
                        query = query.OrderBy(x => x.TheftMark);
                    }
                    else if (sortOptions.Name == ComputerFields.CarePackNumber)
                    {
                        query = query.OrderBy(x => x.CarePackNumber);
                    }
                    else if (sortOptions.Name == ComputerFields.ComputerType)
                    {
                        query = query.OrderBy(x => x.ComputerType.Name);
                    }
                }
                else
                {
                    if (sortOptions.Name == ComputerFields.Name)
                    {
                        query = query.OrderByDescending(x => x.ComputerName);
                    }
                    else if (sortOptions.Name == ComputerFields.Manufacturer)
                    {
                        query = query.OrderByDescending(x => x.Manufacturer);
                    }
                    else if (sortOptions.Name == ComputerFields.Model)
                    {
                        query = query.OrderByDescending(x => x.ComputerModel.Name);
                    }
                    else if (sortOptions.Name == ComputerFields.SerialNumber)
                    {
                        query = query.OrderByDescending(x => x.SerialNumber);
                    }
                    else if (sortOptions.Name == ComputerFields.SerialNumber)
                    {
                        query = query.OrderByDescending(x => x.SerialNumber);
                    }
                    else if (sortOptions.Name == ComputerFields.BIOSVersion)
                    {
                        query = query.OrderByDescending(x => x.BIOSVersion);
                    }
                    else if (sortOptions.Name == ComputerFields.BIOSDate)
                    {
                        query = query.OrderByDescending(x => x.BIOSDate);
                    }
                    else if (sortOptions.Name == ComputerFields.Theftmark)
                    {
                        query = query.OrderByDescending(x => x.TheftMark);
                    }
                    else if (sortOptions.Name == ComputerFields.CarePackNumber)
                    {
                        query = query.OrderByDescending(x => x.CarePackNumber);
                    }
                    else if (sortOptions.Name == ComputerFields.ComputerType)
                    {
                        query = query.OrderByDescending(x => x.ComputerType.Name);
                    }
                }
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
                            x.User.UserId,
                            UserDepartmentName = x.User.Department.DepartmentName,
                            UserUnitName = x.User.OU.Name
                        }).ToList().OrderBy(x => x.Entity.ComputerName);

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

        public List<ReportModel> FindConnectedToComputerLocationOverviews(int customerId, int? departmentId, string searchFor)
        {
            var query = this.DbSet.Where(x => x.Customer_Id == customerId);

            if (departmentId.HasValue)
            {
                query = query.Where(x => x.Department_Id == departmentId);
            }

            if (!string.IsNullOrEmpty(searchFor))
            {
                var pharseInLowerCase = searchFor.ToLower();
                query = query.Where(x => x.Location.ToLower().Contains(pharseInLowerCase));
            }

            var anonymus =
                query.Where(x => x.Location != null && x.Location != string.Empty)
                    .Select(x => new { Item = x.Location, Owner = x.ComputerName, ComputerId = x.Id })
                    .ToList();

            var models = anonymus.Select(x => new ReportModel(x.Item, x.Owner, x.ComputerId)).ToList();

            return models;
        }

        public int GetIdByName(string computerName, int customerId)
        {
            var computer = DbSet.FirstOrDefault(x => x.Customer_Id == customerId && x.ComputerName.Equals(computerName));
            return computer?.Id ?? 0;
        }

        private static void Map(Domain.Computers.Computer entity, Computer businessModel)
        {
            entity.ComputerName = businessModel.WorkstationFields.ComputerName ?? string.Empty;
            entity.Manufacturer = businessModel.WorkstationFields.Manufacturer ?? string.Empty;
            entity.ComputerModel_Id = businessModel.WorkstationFields.ComputerModelId;
            entity.SerialNumber = businessModel.WorkstationFields.SerialNumber ?? string.Empty;
            entity.BIOSVersion = businessModel.WorkstationFields.BIOSVersion ?? string.Empty;
            entity.BIOSDate = businessModel.WorkstationFields.BIOSDate;
            entity.TheftMark = businessModel.WorkstationFields.Theftmark ?? string.Empty;
            entity.CarePackNumber = businessModel.WorkstationFields.CarePackNumber ?? string.Empty;
            entity.ComputerType_Id = businessModel.WorkstationFields.ComputerTypeId;
            entity.Location = businessModel.WorkstationFields.Location ?? string.Empty;

            entity.Processor_Id = businessModel.ProccesorFields.ProccesorId;

            entity.Department_Id = businessModel.OrganizationFields.DepartmentId;
            entity.Domain_Id = businessModel.OrganizationFields.DomainId;
            entity.OU_Id = businessModel.OrganizationFields.UnitId;

            entity.OS_Id = businessModel.OperatingSystemFields.OperatingSystemId;
            entity.Version = businessModel.OperatingSystemFields.Version ?? string.Empty;
            entity.SP = businessModel.OperatingSystemFields.ServicePack ?? string.Empty;
            entity.RegistrationCode = businessModel.OperatingSystemFields.RegistrationCode;
            entity.ProductKey = businessModel.OperatingSystemFields.ProductKey;

            entity.RAM_ID = businessModel.MemoryFields.RAMId;

            entity.BarCode = businessModel.InventoryFields.BarCode ?? string.Empty;
            entity.PurchaseDate = businessModel.InventoryFields.PurchaseDate;

            entity.ChassisType = businessModel.ChassisFields.Chassis ?? string.Empty;

            entity.Status = businessModel.StateFields.State;
            entity.Stolen = businessModel.StateFields.IsStolen.ToInt();
            entity.ReplacedWithComputerName = businessModel.StateFields.Replaced ?? string.Empty;
            entity.SendBack = businessModel.StateFields.IsSendBack.ToInt();
            entity.ScrapDate = businessModel.StateFields.ScrapDate;

            entity.SoundCard = businessModel.SoundFields.SoundCard ?? string.Empty;

            entity.Room_Id = businessModel.PlaceFields.RoomId;
            entity.LocationAddress = businessModel.PlaceFields.Address;
            entity.LocationPostalCode = businessModel.PlaceFields.PostalCode;
            entity.LocationPostalAddress = businessModel.PlaceFields.PostalAddress;
            entity.LocationRoom = businessModel.PlaceFields.Location;
            entity.Location2 = businessModel.PlaceFields.Location2;

            entity.Info = businessModel.OtherFields.Info ?? string.Empty;

            entity.VideoCard = businessModel.GraphicsFields.VideoCard ?? string.Empty;

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
            entity.IPAddress = businessModel.CommunicationFields.IPAddress ?? string.Empty;
            entity.MACAddress = businessModel.CommunicationFields.MacAddress ?? string.Empty;
            entity.RAS = businessModel.CommunicationFields.IsRAS.ToInt();
            entity.NovellClient = businessModel.CommunicationFields.NovellClient ?? string.Empty;
        }
    }
}
