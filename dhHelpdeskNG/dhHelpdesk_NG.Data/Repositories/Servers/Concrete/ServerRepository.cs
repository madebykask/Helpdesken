namespace DH.Helpdesk.Dal.Repositories.Servers.Concrete
{
    using System.Collections.Generic;
    using System.Linq;

    using DH.Helpdesk.BusinessData.Models.Inventory;
    using DH.Helpdesk.BusinessData.Models.Inventory.Edit.Server;
    using DH.Helpdesk.BusinessData.Models.Inventory.Output.Server;
    using DH.Helpdesk.Common.Types;
    using DH.Helpdesk.Dal.Dal;
    using DH.Helpdesk.Dal.Infrastructure;

    public class ServerRepository : Repository<Domain.Servers.Server>, IServerRepository
    {
        public ServerRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }

        public void Add(Server businessModel)
        {
            var entity = new Domain.Servers.Server();
            Map(entity, businessModel);
            entity.Customer_Id = businessModel.CustomerId;
            entity.CreatedDate = businessModel.CreatedDate;

            this.DbSet.Add(entity);
            this.InitializeAfterCommit(businessModel, entity);
        }

        public void Update(Server businessModel)
        {
            var entity = this.DbSet.Find(businessModel.Id);
            Map(entity, businessModel);
            entity.ChangedDate = businessModel.ChangedDate;
        }

        public Server FindById(int id)
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

            var serverAggregate = Server.CreateForEdit(
                anonymus.Entity.Id,
                new BusinessData.Models.Inventory.Edit.Server.GeneralFields(
                    anonymus.Entity.ServerName,
                    anonymus.Entity.Manufacturer,
                    anonymus.Entity.ServerDescription,
                    anonymus.Entity.ServerModel,
                    anonymus.Entity.SerialNumber),
                new BusinessData.Models.Inventory.Edit.Server.OtherFields(
                    anonymus.Entity.Info,
                    anonymus.Entity.Miscellaneous,
                    anonymus.Entity.URL,
                    anonymus.Entity.URL2,
                    anonymus.Entity.Owner),
                new BusinessData.Models.Inventory.Edit.Server.StateFields(
                    anonymus.Entity.SyncChangedDate,
                    !string.IsNullOrWhiteSpace(anonymus.UserFirstName) ? new UserName(anonymus.UserFirstName, anonymus.UserSurName) : null),
                new BusinessData.Models.Inventory.Edit.Server.StorageFields(anonymus.Entity.Harddrive),
                new BusinessData.Models.Inventory.Edit.Shared.ChassisFields(anonymus.Entity.ChassisType),
                new BusinessData.Models.Inventory.Edit.Shared.InventoryFields(
                    anonymus.Entity.BarCode,
                    anonymus.Entity.PurchaseDate),
                new BusinessData.Models.Inventory.Edit.Shared.OperatingSystemFields(
                    anonymus.Entity.OperatingSystem_Id,
                    anonymus.Entity.Version,
                    anonymus.Entity.SP,
                    anonymus.Entity.RegistrationCode,
                    anonymus.Entity.ProductKey),
                new BusinessData.Models.Inventory.Edit.Shared.MemoryFields(anonymus.Entity.RAM_Id),
                new BusinessData.Models.Inventory.Edit.Shared.PlaceFields(
                    anonymus.BuildingId,
                    anonymus.FloorId,
                    anonymus.Entity.Room_Id,
                    anonymus.Entity.Location),
                new BusinessData.Models.Inventory.Edit.Shared.ProcessorFields(anonymus.Entity.Processor_Id),
                anonymus.Entity.CreatedDate,
                anonymus.Entity.ChangedDate,
                new BusinessData.Models.Inventory.Edit.Server.CommunicationFields(
                    anonymus.Entity.NIC_Id,
                    anonymus.Entity.IPAddress,
                    anonymus.Entity.MACAddress));

            return serverAggregate;
        }

        public List<ServerOverview> FindOverviews(int customerId, string searchFor)
        {
            var query = this.DbSet.Where(x => x.Customer_Id == customerId);

            if (!string.IsNullOrEmpty(searchFor))
            {
                var pharseInLowerCase = searchFor.ToLower();

                query =
                    query.Where(
                        c =>
                        c.ServerName.ToLower().Contains(pharseInLowerCase)
                        || c.ServerModel.ToLower().Contains(pharseInLowerCase)
                        || c.ServerDescription.ToLower().Contains(pharseInLowerCase)
                        || c.Manufacturer.ToLower().Contains(pharseInLowerCase)
                        || c.SerialNumber.ToLower().Contains(pharseInLowerCase));
            }

            var anonymus =
                query.Select(
                    x =>
                    new
                        {
                            Entity = x,
                            OperatingName = x.OperatingSystem.Name,
                            ProcessorName = x.Processor.Name,
                            RamName = x.RAM.Name,
                            NetworkAdapterName = x.NIC.Name,
                            RoomName = x.Room.Name
                        }).ToList();

            var overviewAggregates =
                anonymus.Select(
                    x =>
                    new ServerOverview(
                        x.Entity.Id,
                        x.Entity.Customer_Id,
                        x.Entity.CreatedDate,
                        x.Entity.ChangedDate,
                        new BusinessData.Models.Inventory.Output.Server.GeneralFields(
                        x.Entity.ServerName,
                        x.Entity.Manufacturer,
                        x.Entity.ServerDescription,
                        x.Entity.ServerModel,
                        x.Entity.SerialNumber),
                        new BusinessData.Models.Inventory.Output.Server.OtherFields(
                        x.Entity.Info,
                        x.Entity.Miscellaneous,
                        x.Entity.URL,
                        x.Entity.URL2,
                        x.Entity.Owner),
                        new BusinessData.Models.Inventory.Output.Server.StateFields(x.Entity.SyncChangedDate),
                        new BusinessData.Models.Inventory.Output.Server.StorageFields(x.Entity.Harddrive),
                        new BusinessData.Models.Inventory.Output.Shared.ChassisFields(x.Entity.ChassisType),
                        new BusinessData.Models.Inventory.Output.Shared.InventoryFields(
                        x.Entity.BarCode,
                        x.Entity.PurchaseDate),
                        new BusinessData.Models.Inventory.Output.Shared.MemoryFields(x.RamName),
                        new BusinessData.Models.Inventory.Output.Shared.OperatingSystemFields(
                        x.OperatingName,
                        x.Entity.Version,
                        x.Entity.SP,
                        x.Entity.RegistrationCode,
                        x.Entity.ProductKey),
                        new BusinessData.Models.Inventory.Output.Shared.ProcessorFields(x.ProcessorName),
                        new BusinessData.Models.Inventory.Output.Shared.PlaceFields(x.RoomName, x.Entity.Location),
                        new BusinessData.Models.Inventory.Output.Server.CommunicationFields(
                        x.NetworkAdapterName,
                        x.Entity.IPAddress,
                        x.Entity.MACAddress))).ToList();

            return overviewAggregates;
        }

        public void RemoveReferenceOnNic(int id)
        {
            var models =
                this.DbSet.Where(x => x.NIC_Id == id).ToList();

            foreach (var item in models)
            {
                item.NIC_Id = null;
            }
        }

        public void RemoveReferenceOnRam(int id)
        {
            var models =
                this.DbSet.Where(x => x.RAM_Id == id).ToList();

            foreach (var item in models)
            {
                item.RAM_Id = null;
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
                this.DbSet.Where(x => x.OperatingSystem_Id == id).ToList();

            foreach (var item in models)
            {
                item.OperatingSystem_Id = null;
            }
        }

        public int GetServerCount(int customerId)
        {
            var result = this.DbSet.Count(x => x.Customer_Id == customerId);

            return result;
        }

        public List<ReportModel> FindConnectedToServerLocationOverviews(int customerId, string searchFor)
        {
            var query = this.DbSet.Where(x => x.Customer_Id == customerId);

            if (!string.IsNullOrEmpty(searchFor))
            {
                var pharseInLowerCase = searchFor.ToLower();
                query = query.Where(x => x.Location.ToLower().Contains(pharseInLowerCase));
            }

            var anonymus =
                query.Where(x => string.IsNullOrWhiteSpace(x.Location))
                    .Select(x => new { Item = x.Location, Owner = x.ServerName })
                    .ToList();

            var models = anonymus.Select(x => new ReportModel(x.Item, x.Owner)).ToList();

            return models;
        }

        private static void Map(Domain.Servers.Server entity, Server businessModel)
        {
            entity.ServerName = businessModel.GeneralFields.Name;
            entity.Manufacturer = businessModel.GeneralFields.Manufacturer;
            entity.ServerDescription = businessModel.GeneralFields.Description;
            entity.ServerModel = businessModel.GeneralFields.Model;
            entity.SerialNumber = businessModel.GeneralFields.SerialNumber;

            entity.ChassisType = businessModel.ChassisFields.Chassis;

            entity.BarCode = businessModel.InventoryFields.BarCode;
            entity.PurchaseDate = businessModel.InventoryFields.PurchaseDate;

            entity.OperatingSystem_Id = businessModel.OperatingSystemFields.OperatingSystemId;
            entity.Version = businessModel.OperatingSystemFields.Version;
            entity.SP = businessModel.OperatingSystemFields.ServicePack;
            entity.RegistrationCode = businessModel.OperatingSystemFields.RegistrationCode;
            entity.ProductKey = businessModel.OperatingSystemFields.ProductKey;

            entity.Processor_Id = businessModel.ProccesorFields.ProccesorId;

            entity.RAM_Id = businessModel.MemoryFields.RAMId;

            entity.Harddrive = businessModel.StorageFields.Capasity;

            entity.NIC_Id = businessModel.CommunicationFields.NetworkAdapterId;
            entity.IPAddress = businessModel.CommunicationFields.IPAddress;
            entity.MACAddress = businessModel.CommunicationFields.MacAddress;

            entity.Info = businessModel.OtherFields.Info;
            entity.Miscellaneous = businessModel.OtherFields.Other;
            entity.URL = businessModel.OtherFields.URL;
            entity.URL2 = businessModel.OtherFields.URL2;
            entity.Owner = businessModel.OtherFields.Owner;

            entity.Room_Id = businessModel.PlaceFields.RoomId;
            entity.Location = businessModel.PlaceFields.Location;

            entity.SyncChangedDate = businessModel.StateFields.SyncChangeDate;
        }
    }
}