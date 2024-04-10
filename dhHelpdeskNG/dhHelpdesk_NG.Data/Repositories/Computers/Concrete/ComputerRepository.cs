using System.Data.Entity;
using DH.Helpdesk.Common.Tools;

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
    using DateFields = DH.Helpdesk.BusinessData.Enums.Inventory.Fields.Computer.DateFields;
    using MemoryFields = DH.Helpdesk.BusinessData.Enums.Inventory.Fields.Shared.MemoryFields;
    using ProcessorFields = DH.Helpdesk.BusinessData.Enums.Inventory.Fields.Shared.ProcessorFields;
    using SoundFields = DH.Helpdesk.BusinessData.Enums.Inventory.Fields.Computer.SoundFields;
    using GraphicsFields = DH.Helpdesk.BusinessData.Enums.Inventory.Fields.Computer.GraphicsFields;
    using CommunicationFields = DH.Helpdesk.BusinessData.Enums.Inventory.Fields.Computer.CommunicationFields;
    using ContractFields = DH.Helpdesk.BusinessData.Enums.Inventory.Fields.Computer.ContractFields;
    using OtherFields = DH.Helpdesk.BusinessData.Enums.Inventory.Fields.Computer.OtherFields;
    using ContactInformationFields = DH.Helpdesk.BusinessData.Enums.Inventory.Fields.Computer.ContactInformationFields;
    using StateFields = DH.Helpdesk.BusinessData.Enums.Inventory.Fields.Computer.StateFields;
    using DH.Helpdesk.BusinessData.Enums.Inventory.Fields.Inventory;
    using DH.Helpdesk.BusinessData.Enums.Inventory.Fields.Shared;

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

            //save file
            if (businessModel.File != null)
            {
                entity.ComputerFileName = businessModel.File.FileName;
                entity.ComputerDocument = businessModel.File.Content;
            }
            
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

        public ComputerFile GetFile(int contractId)
        {
            var computerFile =
                DbSet.Where(x => x.Id == contractId).Select(x => new ComputerFile()
                {
                    FileName = x.ComputerFileName,
                    Content = x.ComputerDocument
                }).FirstOrDefault();

            return computerFile;
        }

        public void SaveFile(int id, string fileName, byte[] data)
        {
            var entity = this.DbSet.Find(id);
            if (entity != null)
            {
                entity.ComputerFileName = fileName;
                entity.ComputerDocument = data;

                Commit();
            }
        }

        public void DeleteFile(int id)
        {
            var entity = this.DbSet.Find(id);
            if (entity != null)
            {
                entity.ComputerFileName = null;
                entity.ComputerDocument = null;

                Commit();
            }
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
                                UserRegionName = x.User.Department.Region.Name,
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
                                t.s.UserRegionName,
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
            workstation.ComputerTypeName = anonymus.Entity.ComputerType != null ? anonymus.Entity.ComputerType.ComputerTypeDescription : string.Empty;

            var processor = new BusinessData.Models.Inventory.Edit.Shared.ProcessorFields(anonymus.Entity.Processor_Id);

            var organization =
                new BusinessData.Models.Inventory.Edit.Computer.OrganizationFields(
                    anonymus.Entity.Region_Id,
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
                    anonymus.Entity.AccountingDimension5,
                    anonymus.Entity.ComputerFileName,
                    anonymus.Entity.WarrantyEndDate);

            var contactInfo = new BusinessData.Models.Inventory.Edit.Computer.ContactInformationFields(
                anonymus.UserId,
                anonymus.UserStringId,
                anonymus.UserRegionName,
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


        public List<ComputerOverview> FindOverviews(int customerId,
            int? domainId,
            int? departmentId,
            int? regionId,
            int? ouId,
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
            SortField sortOptions,
            int? recordsCount,
            bool isComputerDepartmentSource)
        {
            var query = (from c in DbContext.Computers
                        from cs in DbContext.ComputerStatuses.Where(css => css.Id == c.Status).DefaultIfEmpty()
                        select new {c, cs })
                .Select(x => new ComputerDto
                {
                    Computer = x.c,
                    StatusName = x.cs != null ? x.cs.Name : "",
                    ComputerTypeName = x.c.ComputerType.Name ?? "",
                    ComputerModelName = x.c.ComputerModel.Name ?? "",
                    OSName = x.c.OS.Name ?? "",
                    ProcessorName = x.c.Processor.Name ?? "",
                    RAMName = x.c.RAM.Name ?? "",
                    NICName = x.c.NIC.Name ?? "",
                    DepartmentName= x.c.Department.DepartmentName ?? "",
                    DomainName = x.c.Domain.Name ?? "",
                    OUName = x.c.OU.Name ?? "",
                    RoomName = x.c.Room.Name ?? "",
                    UserId = x.c.User.UserId ?? "",
                    UserRegionName = x.c.User.Department.Region_Id.HasValue ?  x.c.User.Department.Region.Name : "",
                    UserDepartmentName = x.c.User.Department.DepartmentName ?? "",
                    UserOUName = x.c.User.OU.Name ?? "",
                    ContractStatusName = x.c.ContractStatus_Id.HasValue ? x.c.ContractStatus.Name : "",
                    RegionName =  x.c.Region_Id.HasValue ? x.c.Region.Name : ""
                })
                .Where(x => x.Computer.Customer_Id == customerId);

            if (!isShowScrapped) 
            {
                var compareDate = DateTime.UtcNow.AddDays(-1).GetEndOfDay();
                query = query.Where(x => !x.Computer.ScrapDate.HasValue || x.Computer.ScrapDate.Value > compareDate);
            }

            if (domainId.HasValue)
                query = query.Where(x => x.Computer.Domain_Id == domainId);

            if (regionId.HasValue)
            {
                query = isComputerDepartmentSource
                    ? query.Where(x => x.Computer.Region_Id == regionId)
                    : query.Where(x =>
                        x.Computer.User_Id.HasValue &&
                        x.Computer.User.Department_Id.HasValue &&
                        x.Computer.User.Department.Region_Id == regionId);
            }

            if (departmentId.HasValue)
            {
                query = isComputerDepartmentSource 
                    ? query.Where(x => x.Computer.Department_Id == departmentId)
                    : query.Where(x =>
                    x.Computer.User_Id.HasValue && x.Computer.User.Department_Id.HasValue && x.Computer.User.Department_Id == departmentId);
            }

            if (ouId.HasValue)
            {
                query = isComputerDepartmentSource 
                    ? query.Where(x => x.Computer.OU_Id == ouId)
                    : query.Where(x =>
                        x.Computer.User_Id.HasValue && x.Computer.User.OU_Id == ouId);
            }

            if (computerTypeId.HasValue)
                query = query.Where(x => x.Computer.ComputerType_Id == computerTypeId);

            if (contractStatusId.HasValue)
                query = query.Where(x => x.Computer.ContractStatus_Id == contractStatusId);

            if (contractStartDateFrom.HasValue)
                query = query.Where(x => x.Computer.ContractStartDate >= contractStartDateFrom);

            if (contractStartDateTo.HasValue)
                query = query.Where(x => x.Computer.ContractStartDate <= contractStartDateTo);

            if (contractEndDateFrom.HasValue)
                query = query.Where(x => x.Computer.ContractEndDate >= contractEndDateFrom);

            if (contractEndDateTo.HasValue)
                query = query.Where(x => x.Computer.ContractEndDate <= contractEndDateTo);

            if (scanDateFrom.HasValue)
                query = query.Where(x => x.Computer.ScanDate >= scanDateFrom);

            if (scanDateTo.HasValue)
                query = query.Where(x => x.Computer.ScanDate <= scanDateTo);

            if (scrapDateFrom.HasValue)
                query = query.Where(x => x.Computer.ScrapDate >= scrapDateFrom);

            if (scrapDateTo.HasValue)
                query = query.Where(x => x.Computer.ScrapDate <= scrapDateTo);

            if (!string.IsNullOrEmpty(searchFor))
            {
                var pharseInLowerCase = searchFor.ToLower();

                query =
                    query.Where(
                        c =>
                        c.Computer.ComputerName.ToLower().Contains(pharseInLowerCase)
                        || c.Computer.ComputerModel.Name.ToLower().Contains(pharseInLowerCase)
                        || c.Computer.Manufacturer.ToLower().Contains(pharseInLowerCase)
                        || c.Computer.SerialNumber.ToLower().Contains(pharseInLowerCase)
                        || c.Computer.BIOSVersion.ToLower().Contains(pharseInLowerCase)
                        || c.Computer.TheftMark.ToLower().Contains(pharseInLowerCase)
                        || c.Computer.CarePackNumber.ToLower().Contains(pharseInLowerCase)
                        || c.Computer.ComputerType.Name.ToLower().Contains(pharseInLowerCase)
                        || c.Computer.Location.ToLower().Contains(pharseInLowerCase) 
                        || c.Computer.User.UserId.ToLower().Contains(pharseInLowerCase)
                        || c.Computer.User.FirstName.ToLower().Contains(pharseInLowerCase)
                        || c.Computer.User.SurName.ToLower().Contains(pharseInLowerCase)
                        || c.Computer.NIC.Name.ToLower().Contains(pharseInLowerCase)
						|| c.Computer.BarCode.ToLower().Contains(pharseInLowerCase)
						//|| c.Computer.Building.ToLower().Contains(pharseInLowerCase)
						|| c.Computer.ChassisType.ToLower().Contains(pharseInLowerCase)
						|| c.Computer.ContractNumber.ToLower().Contains(pharseInLowerCase)
						//|| c.Floor.ToLower().Contains(pharseInLowerCase)
						|| c.Computer.IPAddress.ToLower().Contains(pharseInLowerCase)
						|| c.Computer.Location2.ToLower().Contains(pharseInLowerCase)
						|| c.Computer.LocationAddress.ToLower().Contains(pharseInLowerCase)
						|| c.Computer.LocationPostalAddress.ToLower().Contains(pharseInLowerCase)
						|| c.Computer.LocationPostalCode.ToLower().Contains(pharseInLowerCase)
						|| c.Computer.LocationAddress.ToLower().Contains(pharseInLowerCase)
						|| c.Computer.MACAddress.ToLower().Contains(pharseInLowerCase)
						|| c.Computer.NovellClient.ToLower().Contains(pharseInLowerCase)
						|| (c.Computer.OS != null && c.Computer.OS.Name.ToLower().Contains(pharseInLowerCase))
						|| (c.Computer.Processor != null && c.Computer.Processor.Name.ToLower().Contains(pharseInLowerCase))
						|| (c.Computer.RAM != null && c.Computer.RAM.Name.ToLower().Contains(pharseInLowerCase))
						|| c.Computer.ReplacedWithComputerName.ToLower().Contains(pharseInLowerCase)
						|| (c.Computer.Room != null && c.Computer.Room.Name.ToLower().Contains(pharseInLowerCase))
						|| c.Computer.SoundCard.ToLower().Contains(pharseInLowerCase)
						|| c.Computer.SP.ToLower().Contains(pharseInLowerCase)
						|| c.Computer.Info.ToLower().Contains(pharseInLowerCase)
						|| c.Computer.Location.ToLower().Contains(pharseInLowerCase)
						|| c.Computer.LocationRoom.ToLower().Contains(pharseInLowerCase)
						|| (c.Computer.OU != null && c.Computer.OU.Name.ToLower().Contains(pharseInLowerCase))
						|| c.Computer.VideoCard.ToLower().Contains(pharseInLowerCase)
						);
            }

            if (sortOptions != null && sortOptions.Name != null)
            {
                if (sortOptions.SortBy == SortBy.Ascending)
                {
                    if (sortOptions.Name == ComputerFields.Name)
                        query = query.OrderBy(x => x.Computer.ComputerName);
                    else if (sortOptions.Name == ComputerFields.Manufacturer)
                        query = query.OrderBy(x => x.Computer.Manufacturer);
                    else if (sortOptions.Name == ComputerFields.Location)
                        query = query.OrderBy(x => x.Computer.Location);
                    else if (sortOptions.Name == "Organization.Department")
                        query = query.OrderBy(x => x.Computer.Department.DepartmentName);
                    else if (sortOptions.Name == ComputerFields.Model)
                        query = query.OrderBy(x => x.Computer.ComputerModel.Name);
                    else if (sortOptions.Name == ComputerFields.SerialNumber)
                        query = query.OrderBy(x => x.Computer.SerialNumber);
                    else if (sortOptions.Name == CommunicationFields.MacAddress)
                        query = query.OrderBy(x => x.Computer.MACAddress);
                    else if (sortOptions.Name == CommunicationFields.IPAddress)
                        query = query.OrderBy(x => x.Computer.IPAddress);
                    else if (sortOptions.Name == ComputerFields.BIOSVersion)
                        query = query.OrderBy(x => x.Computer.BIOSVersion);
                    else if (sortOptions.Name == ComputerFields.BIOSDate)
                        query = query.OrderBy(x => x.Computer.BIOSDate);
                    else if (sortOptions.Name == ComputerFields.Theftmark)
                        query = query.OrderBy(x => x.Computer.TheftMark);
                    else if (sortOptions.Name == ComputerFields.CarePackNumber)
                        query = query.OrderBy(x => x.Computer.CarePackNumber);
                    else if (sortOptions.Name == ComputerFields.ComputerType)
                        query = query.OrderBy(x => x.Computer.ComputerType.Name);
                    else if (sortOptions.Name == DateFields.CreatedDate)
                        query = query.OrderBy(x => x.Computer.CreatedDate);
                    else if (sortOptions.Name == MemoryFields.RAM)
                        query = query.OrderBy(x => x.Computer.RAM.Name);
                    else if (sortOptions.Name == ProcessorFields.ProccesorName)
                        query = query.OrderBy(x => x.Computer.Processor.Name);
                    else if (sortOptions.Name == SoundFields.SoundCard)
                        query = query.OrderBy(x => x.Computer.SoundCard);
                    else if (sortOptions.Name == GraphicsFields.VideoCard)
                        query = query.OrderBy(x => x.Computer.VideoCard);
                    else if (sortOptions.Name == ContractFields.ContractStatusName)
                        query = query.OrderBy(x => x.Computer.ContractStatus.Name);
                    else if (sortOptions.Name == ContractFields.PurchasePrice)
                        query = query.OrderBy(x => x.Computer.Price);
                    //Nytt 2024-01-25
                    else if (sortOptions.Name == "Organization.Region")
                        query = query.OrderBy(x => x.Computer.Region.Name);
                    else if (sortOptions.Name == "Organization.Unit")
                        query = query.OrderBy(x => x.Computer.OU.Name);
                    else if (sortOptions.Name == "Organization.Domain")
                        query = query.OrderBy(x => x.Computer.Domain.Name);
                    else if (sortOptions.Name == "Place.LocationRoom")
                        query = query.OrderBy(x => x.Computer.LocationRoom);
                    else if (sortOptions.Name == "Place.Location2")
                        query = query.OrderBy(x => x.Computer.Location2);
                    else if (sortOptions.Name == "Place.LocationAddress")
                        query = query.OrderBy(x => x.Computer.LocationAddress);
                    else if (sortOptions.Name == "Place.LocationPostalAddress")
                        query = query.OrderBy(x => x.Computer.LocationPostalAddress);
                    else if (sortOptions.Name == "Contact.Name")
                        query = query.OrderBy(x => x.Computer.ContactName);
                    else if (sortOptions.Name == "Contact.Phone")
                        query = query.OrderBy(x => x.Computer.ContactPhone);
                    else if (sortOptions.Name == "Contact.Email")
                        query = query.OrderBy(x => x.Computer.ContactEmailAddress);
                    else if (sortOptions.Name == CommunicationFields.RAS)
                        query = query.OrderBy(x => x.Computer.RAS);
                    else if (sortOptions.Name == ContractFields.ContractStartDate)
                        query = query.OrderBy(x => x.Computer.ContractStartDate);
                    else if (sortOptions.Name == ContractFields.ContractEndDate)
                        query = query.OrderBy(x => x.Computer.ContractEndDate);
                    else if (sortOptions.Name == ContractFields.WarrantyEndDate)
                        query = query.OrderBy(x => x.Computer.WarrantyEndDate);
                    else if (sortOptions.Name == ContractFields.ContractNumber)
                        query = query.OrderBy(x => x.Computer.ContractNumber);
                    else if (sortOptions.Name == ContractFields.AccountingDimension1)
                        query = query.OrderBy(x => x.Computer.AccountingDimension1);
                    else if (sortOptions.Name == ContractFields.AccountingDimension2)
                        query = query.OrderBy(x => x.Computer.AccountingDimension2);
                    else if (sortOptions.Name == ContractFields.AccountingDimension3)
                        query = query.OrderBy(x => x.Computer.AccountingDimension3);
                    else if (sortOptions.Name == ContractFields.AccountingDimension4)
                        query = query.OrderBy(x => x.Computer.AccountingDimension4);
                    else if (sortOptions.Name == ContractFields.AccountingDimension5)
                        query = query.OrderBy(x => x.Computer.AccountingDimension5);
                    else if (sortOptions.Name == ComputerFields.Location)
                        query = query.OrderBy(x => x.Computer.Location);
                    else if (sortOptions.Name == "Chassis.Type")
                        query = query.OrderBy(x => x.Computer.ChassisType);
                    else if (sortOptions.Name == "Inventory.BarCode")
                        query = query.OrderBy(x => x.Computer.BarCode);
                    else if (sortOptions.Name == CommunicationFields.NovellClient)
                        query = query.OrderBy(x => x.Computer.NovellClient);
                    else if (sortOptions.Name == OperatingSystemFields.Version)
                        query = query.OrderBy(x => x.Computer.Version);
                    else if (sortOptions.Name == OperatingSystemFields.ServicePack)
                        query = query.OrderBy(x => x.Computer.SP);
                    else if (sortOptions.Name == OperatingSystemFields.RegistrationCode)
                        query = query.OrderBy(x => x.Computer.RegistrationCode);
                    else if (sortOptions.Name == OperatingSystemFields.ProductKey)
                        query = query.OrderBy(x => x.Computer.ProductKey);
                    else if (sortOptions.Name == OperatingSystemFields.OperatingSystem)
                        query = query.OrderBy(x => x.Computer.OS.Name);
                    //Slut nytt 2024-01-25
                    else if (sortOptions.Name == "Inventory.PurchaseDate")
                        query = query.OrderBy(x => x.Computer.PurchaseDate);
                    else if (sortOptions.Name == OtherFields.Info)
                        query = query.OrderBy(x => x.Computer.Info);
                    else if (sortOptions.Name == ContactInformationFields.UserId)
                        query = query.OrderBy(x => x.Computer.User.UserId);
                    else if (sortOptions.Name == StateFields.State)
                        query = query.OrderBy(x => x.StatusName);
                    else if (sortOptions.Name == StateFields.ScrapDate)
                        query = query.OrderBy(x => x.Computer.ScrapDate);
                    else if (sortOptions.Name == DateFields.ChangedDate)
                        query = query.OrderBy(x => x.Computer.ChangedDate);
                    else if (sortOptions.Name == DateFields.SynchronizeDate)
                        query = query.OrderBy(x => x.Computer.SyncChangedDate);
                    else if (sortOptions.Name == CommunicationFields.NetworkAdapter)
                        query = query.OrderBy(x => x.Computer.NIC.Name);
                }
                else
                {

                    if (sortOptions.Name == ComputerFields.Name)
                        query = query.OrderByDescending(x => x.Computer.ComputerName);
                    else if (sortOptions.Name == ComputerFields.Manufacturer)
                        query = query.OrderByDescending(x => x.Computer.Manufacturer);
                    else if (sortOptions.Name == ComputerFields.Location)
                        query = query.OrderByDescending(x => x.Computer.Location);
                    else if (sortOptions.Name == "Organization.Department")
                        query = query.OrderByDescending(x => x.Computer.Department.DepartmentName);
                    else if (sortOptions.Name == ComputerFields.Model)
                        query = query.OrderByDescending(x => x.Computer.ComputerModel.Name);
                    else if (sortOptions.Name == ComputerFields.SerialNumber)
                        query = query.OrderByDescending(x => x.Computer.SerialNumber);
                    else if (sortOptions.Name == CommunicationFields.MacAddress)
                        query = query.OrderByDescending(x => x.Computer.MACAddress);
                    else if (sortOptions.Name == CommunicationFields.IPAddress)
                        query = query.OrderByDescending(x => x.Computer.IPAddress);
                    else if (sortOptions.Name == ComputerFields.BIOSVersion)
                        query = query.OrderByDescending(x => x.Computer.BIOSVersion);
                    else if (sortOptions.Name == ComputerFields.BIOSDate)
                        query = query.OrderByDescending(x => x.Computer.BIOSDate);
                    else if (sortOptions.Name == ComputerFields.Theftmark)
                        query = query.OrderByDescending(x => x.Computer.TheftMark);
                    else if (sortOptions.Name == ComputerFields.CarePackNumber)
                        query = query.OrderByDescending(x => x.Computer.CarePackNumber);
                    else if (sortOptions.Name == ComputerFields.ComputerType)
                        query = query.OrderByDescending(x => x.Computer.ComputerType.Name);
                    else if (sortOptions.Name == DateFields.CreatedDate)
                        query = query.OrderByDescending(x => x.Computer.CreatedDate);
                    else if (sortOptions.Name == MemoryFields.RAM)
                        query = query.OrderByDescending(x => x.Computer.RAM.Name);
                    else if (sortOptions.Name == ProcessorFields.ProccesorName)
                        query = query.OrderByDescending(x => x.Computer.Processor.Name);
                    else if (sortOptions.Name == SoundFields.SoundCard)
                        query = query.OrderByDescending(x => x.Computer.SoundCard);
                    else if (sortOptions.Name == GraphicsFields.VideoCard)
                        query = query.OrderByDescending(x => x.Computer.VideoCard);
                    else if (sortOptions.Name == ContractFields.ContractStatusName)
                        query = query.OrderByDescending(x => x.Computer.ContractStatus.Name);
                    else if (sortOptions.Name == ContractFields.PurchasePrice)
                        query = query.OrderByDescending(x => x.Computer.Price);
                    //Nytt 2024-01-25
                    else if (sortOptions.Name == "Organization.Region")
                        query = query.OrderByDescending(x => x.Computer.Region.Name);
                    else if (sortOptions.Name == "Organization.Unit")
                        query = query.OrderByDescending(x => x.Computer.OU.Name);
                    else if (sortOptions.Name == "Organization.Domain")
                        query = query.OrderByDescending(x => x.Computer.Domain.Name);
                    else if (sortOptions.Name == "Place.LocationRoom")
                        query = query.OrderByDescending(x => x.Computer.LocationRoom);
                    else if (sortOptions.Name == "Place.Location2")
                        query = query.OrderByDescending(x => x.Computer.Location2);
                    else if (sortOptions.Name == "Place.LocationAddress")
                        query = query.OrderByDescending(x => x.Computer.LocationAddress);
                    else if (sortOptions.Name == "Place.LocationPostalAddress")
                        query = query.OrderByDescending(x => x.Computer.LocationPostalAddress);
                    else if (sortOptions.Name == "Contact.Name")
                        query = query.OrderByDescending(x => x.Computer.ContactName);
                    else if (sortOptions.Name == "Contact.Phone")
                        query = query.OrderByDescending(x => x.Computer.ContactPhone);
                    else if (sortOptions.Name == "Contact.Email")
                        query = query.OrderByDescending(x => x.Computer.ContactEmailAddress);
                    else if (sortOptions.Name == CommunicationFields.RAS)
                        query = query.OrderByDescending(x => x.Computer.RAS);
                    else if (sortOptions.Name == ContractFields.ContractStartDate)
                        query = query.OrderByDescending(x => x.Computer.ContractStartDate);
                    else if (sortOptions.Name == ContractFields.ContractEndDate)
                        query = query.OrderByDescending(x => x.Computer.ContractEndDate);
                    else if (sortOptions.Name == ContractFields.WarrantyEndDate)
                        query = query.OrderByDescending(x => x.Computer.WarrantyEndDate);
                    else if (sortOptions.Name == ContractFields.AccountingDimension1)
                        query = query.OrderByDescending(x => x.Computer.AccountingDimension1);
                    else if (sortOptions.Name == ContractFields.AccountingDimension2)
                        query = query.OrderByDescending(x => x.Computer.AccountingDimension2);
                    else if (sortOptions.Name == ContractFields.AccountingDimension3)
                        query = query.OrderByDescending(x => x.Computer.AccountingDimension3);
                    else if (sortOptions.Name == ContractFields.AccountingDimension4)
                        query = query.OrderByDescending(x => x.Computer.AccountingDimension4);
                    else if (sortOptions.Name == ContractFields.AccountingDimension5)
                        query = query.OrderByDescending(x => x.Computer.AccountingDimension5);
                    else if (sortOptions.Name == ComputerFields.Location)
                        query = query.OrderByDescending(x => x.Computer.Location);
                    else if (sortOptions.Name == "Chassis.Type")
                        query = query.OrderByDescending(x => x.Computer.ChassisType);
                    else if (sortOptions.Name == "Inventory.BarCode")
                        query = query.OrderByDescending(x => x.Computer.BarCode);
                    else if (sortOptions.Name == CommunicationFields.NovellClient)
                        query = query.OrderByDescending(x => x.Computer.NovellClient);
                    else if (sortOptions.Name == OperatingSystemFields.Version)
                        query = query.OrderByDescending(x => x.Computer.Version);
                    else if (sortOptions.Name == OperatingSystemFields.ServicePack)
                        query = query.OrderByDescending(x => x.Computer.SP);
                    else if (sortOptions.Name == OperatingSystemFields.RegistrationCode)
                        query = query.OrderByDescending(x => x.Computer.RegistrationCode);
                    else if (sortOptions.Name == OperatingSystemFields.ProductKey)
                        query = query.OrderByDescending(x => x.Computer.ProductKey);
                    else if (sortOptions.Name == OperatingSystemFields.OperatingSystem)
                        query = query.OrderByDescending(x => x.Computer.OS.Name);
                    else if (sortOptions.Name == OperatingSystemFields.OperatingSystem)
                        query = query.OrderByDescending(x => x.Computer.OS.Name);
                    //RegistrationCode
                    //Slut nytt 2024-01-25
                    else if (sortOptions.Name == "Inventory.PurchaseDate")
                        query = query.OrderByDescending(x => x.Computer.PurchaseDate);
                    else if (sortOptions.Name == OtherFields.Info)
                        query = query.OrderByDescending(x => x.Computer.Info);
                    else if (sortOptions.Name == ContactInformationFields.UserId)
                        query = query.OrderByDescending(x => x.Computer.User.UserId);
                    else if (sortOptions.Name == StateFields.State)
                        query = query.OrderByDescending(x => x.StatusName);
                    else if (sortOptions.Name == StateFields.ScrapDate)
                        query = query.OrderByDescending(x => x.Computer.ScrapDate);
                    else if (sortOptions.Name == DateFields.ChangedDate)
                        query = query.OrderByDescending(x => x.Computer.ChangedDate);
                    else if (sortOptions.Name == DateFields.SynchronizeDate)
                        query = query.OrderByDescending(x => x.Computer.SyncChangedDate);
                    else if (sortOptions.Name == CommunicationFields.NetworkAdapter)
                        query = query.OrderByDescending(x => x.Computer.NIC.Name);
                }
            }

            if (recordsCount.HasValue)
                query = query.Take(recordsCount.Value);

            var overviews = MapToComputerOverview(query.ToList());
            return overviews;
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

        public int? GetComputerTypeById(int id)
        {
            var computerTypePrice =
                (from c in DbContext.ComputerTypes.Where(i => i.Id == id).Select(i=> i.Price)
                select c).Single() ?? 0;

            return computerTypePrice;
        }

        public bool IsMacAddressUnique(int exceptId, string macAddress, int customerId)
        {
            return !DbSet.Any(w => w.MACAddress.Equals(macAddress, StringComparison.InvariantCultureIgnoreCase) && w.Id != exceptId && w.Customer_Id == customerId);
        }
        public bool IsTheftMarkUnique(int exceptId, string theftMark, int customerId)
        {
            return !DbSet.Any(w => w.TheftMark.Equals(theftMark, StringComparison.InvariantCultureIgnoreCase) && w.Id != exceptId && w.Customer_Id == customerId);
        }
        public bool IsIpAddressUnique(int exceptId, string ipAddress, int customerId)
        {
            return !DbSet.Any(w => w.IPAddress.Equals(ipAddress, StringComparison.InvariantCultureIgnoreCase) && w.Id != exceptId && w.Customer_Id == customerId);
        }
        public bool IsComputerNameUnique(int exceptId, string computerName, int customerId)
        {
            return !DbSet.Any(w => w.ComputerName.Equals(computerName, StringComparison.InvariantCultureIgnoreCase) && w.Id != exceptId && w.Customer_Id == customerId);
        }
        public List<ComputerOverview> GetRelatedOverviews(int customerId, string userId)
        {
            var computers = (from c in DbContext.Computers
                    from cs in DbContext.ComputerStatuses.Where(css => css.Id == c.Status).DefaultIfEmpty()
                    select new {c, cs })
                .Select(x => new ComputerDto
                {
                    Computer = x.c,
                    StatusName = x.cs != null ? x.cs.Name : "",
                    ComputerTypeName = x.c.ComputerType.Name ?? "",
                    ComputerModelName = x.c.ComputerModel.Name ?? "",
                    OSName = x.c.OS.Name ?? "",
                    ProcessorName = x.c.Processor.Name ?? "",
                    RAMName = x.c.RAM.Name ?? "",
                    NICName = x.c.NIC.Name ?? "",
                    DepartmentName= x.c.Department.DepartmentName ?? "",
                    DomainName = x.c.Domain.Name ?? "",
                    OUName = x.c.OU.Name ?? "",
                    RoomName = x.c.Room.Name ?? "",
                    UserId = x.c.User.UserId ?? "",
                    UserDepartmentName = x.c.User.Department.DepartmentName ?? "",
                    UserRegionName = x.c.User.Department.Region_Id.HasValue ?  x.c.User.Department.Region.Name : "",
                    UserOUName = x.c.User.OU.Name ?? "",
                    ContractStatusName = x.c.ContractStatus_Id.HasValue ? x.c.ContractStatus.Name : "",
                    RegionName =  x.c.Region_Id.HasValue ? x.c.Region.Name : ""
                })
                .Where(x => x.Computer.Customer_Id == customerId && x.Computer.User.UserId.Trim().Equals(userId.Trim()))
                .ToList();
            var overviews = MapToComputerOverview(computers);
            return overviews;
        }

        private static List<ComputerOverview> MapToComputerOverview(IList<ComputerDto> computers)
        {
            var overviewAggregates = computers.Select(x =>
                    new ComputerOverview(
                        x.Computer.Id,
                        x.Computer.Customer_Id,
                        x.Computer.CreatedDate,
                        x.Computer.ChangedDate,
                        new BusinessData.Models.Inventory.Output.Computer.WorkstationFields(
                        x.Computer.ComputerName,
                        x.Computer.Manufacturer,
                        x.ComputerModelName,
                        x.Computer.SerialNumber,
                        x.Computer.BIOSVersion,
                        x.Computer.BIOSDate,
                        x.Computer.TheftMark,
                        x.Computer.CarePackNumber,
                        x.ComputerTypeName,
                        x.Computer.Location),
                        new BusinessData.Models.Inventory.Output.Shared.ProcessorFields(x.ProcessorName),
                        new BusinessData.Models.Inventory.Output.Computer.OrganizationFields(
                        x.RegionName,
                        x.DepartmentName,
                        x.DomainName,
                        x.OUName),
                        new BusinessData.Models.Inventory.Output.Shared.OperatingSystemFields(
                        x.OSName,
                        x.Computer.Version,
                        x.Computer.SP,
                        x.Computer.RegistrationCode,
                        x.Computer.ProductKey),
                        new BusinessData.Models.Inventory.Output.Shared.MemoryFields(x.RAMName),
                        new BusinessData.Models.Inventory.Output.Shared.InventoryFields(
                        x.Computer.BarCode,
                        x.Computer.PurchaseDate),
                        new BusinessData.Models.Inventory.Output.Shared.ChassisFields(x.Computer.ChassisType),
                        new BusinessData.Models.Inventory.Output.Computer.StateFields(
                        x.StatusName,
                        x.Computer.Stolen.ToBool(),
                        x.Computer.ReplacedWithComputerName,
                        x.Computer.SendBack.ToBool(),
                        x.Computer.ScrapDate),
                        new BusinessData.Models.Inventory.Output.Computer.SoundFields(x.Computer.SoundCard),
                        new BusinessData.Models.Inventory.Output.Computer.PlaceFields(
                        x.RoomName,
                        x.Computer.LocationAddress,
                        x.Computer.LocationPostalCode,
                        x.Computer.LocationPostalAddress,
                        x.Computer.LocationRoom,
                        x.Computer.Location2),
                        new BusinessData.Models.Inventory.Output.Computer.OtherFields(x.Computer.Info),
                        new BusinessData.Models.Inventory.Output.Computer.GraphicsFields(x.Computer.VideoCard),
                        new BusinessData.Models.Inventory.Output.Computer.ContractFields(
                        x.ContractStatusName,
                        x.Computer.ContractNumber,
                        x.Computer.ContractStartDate,
                        x.Computer.ContractEndDate,
                        x.Computer.Price,
                        x.Computer.AccountingDimension1,
                        x.Computer.AccountingDimension2,
                        x.Computer.AccountingDimension3,
                        x.Computer.AccountingDimension4,
                        x.Computer.AccountingDimension5,
                        x.Computer.WarrantyEndDate),
                        new BusinessData.Models.Inventory.Output.Computer.ContactInformationFields(
                        x.UserId,
                        x.UserRegionName,
                        x.UserDepartmentName,
                        x.UserOUName),
                        new BusinessData.Models.Inventory.Output.Computer.ContactFields(
                        x.Computer.ContactName,
                        x.Computer.ContactPhone,
                        x.Computer.ContactEmailAddress),
                        new BusinessData.Models.Inventory.Output.Computer.CommunicationFields(
                        x.NICName,
                        x.Computer.IPAddress,
                        x.Computer.MACAddress,
                        x.Computer.RAS.ToBool(),
                        x.Computer.NovellClient),
                        new BusinessData.Models.Inventory.Output.Computer.DateFields(
                        x.Computer.SyncChangedDate,
                        x.Computer.ScanDate,
                        x.Computer.LDAPPath))).ToList();

            return overviewAggregates;
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
            entity.Region_Id = businessModel.OrganizationFields.RegionId;
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
            entity.WarrantyEndDate = businessModel.ContractFields.WarrantyEndDate;


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

        private class ComputerDto
        {
            public Domain.Computers.Computer Computer { get; set; }
            public string StatusName { get; set; }
            public string ComputerTypeName { get; set; }
            public string ComputerModelName { get; set; }
            public string OSName { get; set; }
            public string ProcessorName { get; set; }
            public string RAMName { get; set; }
            public string NICName { get; set; }
            public string DepartmentName { get; set; }
            public string DomainName { get; set; }
            public string OUName { get; set; }
            public string RoomName { get; set; }
            public string UserId { get; set; }
            public string UserOUName { get; set; }
            public string ContractStatusName { get; set; }
            public string UserDepartmentName { get; set; }
            public string UserRegionName { get; set; }
            public string RegionName { get; set; }
        }
    }
}
