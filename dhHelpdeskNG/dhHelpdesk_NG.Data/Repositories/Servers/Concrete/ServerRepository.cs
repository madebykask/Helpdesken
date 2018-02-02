namespace DH.Helpdesk.Dal.Repositories.Servers.Concrete
{
    using System.Collections.Generic;
    using System.Linq;
    using DH.Helpdesk.BusinessData.Models.Inventory;
    using DH.Helpdesk.BusinessData.Models.Inventory.Edit.Server;
    using DH.Helpdesk.Common.Types;
    using DH.Helpdesk.Dal.Dal;
    using DH.Helpdesk.Dal.Infrastructure;
   
    public class ServerRepository : Repository<Helpdesk.Domain.Servers.Server>, IServerRepository
    {
        public ServerRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }

        public void Add(ServerForInsert businessModel)
        {
            var entity = new Helpdesk.Domain.Servers.Server();
            Map(entity, businessModel);
            entity.Customer_Id = businessModel.CustomerId;
            entity.CreatedDate = businessModel.CreatedDate;
            entity.ChangedByUser_Id = businessModel.ChangedByUserId;

            entity.ChangedDate = businessModel.CreatedDate; // todo

            this.DbSet.Add(entity);
            this.InitializeAfterCommit(businessModel, entity);
        }

        public void Update(ServerForUpdate businessModel)
        {
            var entity = this.DbSet.Find(businessModel.Id);
            Map(entity, businessModel);
            entity.ChangedDate = businessModel.ChangedDate;
            entity.ChangedByUser_Id = businessModel.ChangedByUserId;
        }

        public string FindOperationObjectName(int id)
        {
            var anonymus =
                this.DbSet.Where(x => x.Id == id)
                    .GroupJoin(
                        this.DbContext.OperationObjects,
                        s => s.ServerName,
                        u => u.Name,
                        (s, res) => new { s, res })
                    .SelectMany(t => t.res.DefaultIfEmpty(), (t, k) => new { OperationObjectName = k.Name })
                    .Single();

            return anonymus.OperationObjectName;
        }

        public ServerForRead FindById(int id)
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
                    .GroupJoin(
                        this.DbContext.OperationObjects,
                        s => s.Entity.ServerName,
                        u => u.Name,
                        (s, res) => new { s, res })
                    .SelectMany(
                        t => t.res.DefaultIfEmpty(),
                        (t, k) =>
                        new
                            {
                                t.s.Entity,
                                t.s.BuildingId,
                                t.s.FloorId,
                                t.s.UserFirstName,
                                t.s.UserSurName,
                                OperationObjectName = k.Name
                            })
                    .Single();

            var serverAggregate = new ServerForRead(
                anonymus.Entity.Id,
                anonymus.OperationObjectName != null,
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
                new BusinessData.Models.Inventory.Edit.Server.CommunicationFields(
                    anonymus.Entity.NIC_Id,
                    anonymus.Entity.IPAddress,
                    anonymus.Entity.MACAddress),
                new BusinessData.Models.Inventory.Edit.Server.StateFields(
                    anonymus.Entity.SyncChangedDate,
                    !string.IsNullOrWhiteSpace(anonymus.UserFirstName)
                        ? new UserName(anonymus.UserFirstName, anonymus.UserSurName)
                        : null),
                anonymus.Entity.CreatedDate,
                anonymus.Entity.ChangedDate);

            return serverAggregate;
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
                query.Where(x => x.Location != null && x.Location != string.Empty)
                    .Select(x => new { Item = x.Location, Owner = x.ServerName, ServerId = x.Id })
                    .ToList();

            var models = anonymus.Select(x => new ReportModel(x.Item, x.Owner, x.ServerId)).ToList();

            return models;
        }

        public int GetIdByName(string serverName, int customerId)
        {
            var server = DbSet.FirstOrDefault(x => x.Customer_Id == customerId && x.ServerName.Equals(serverName));
            return server?.Id ?? 0;
        }

        private static void Map(Helpdesk.Domain.Servers.Server entity, Server businessModel)
        {
            entity.ServerName = businessModel.GeneralFields.Name ?? string.Empty;
            entity.Manufacturer = businessModel.GeneralFields.Manufacturer ?? string.Empty;
            entity.ServerDescription = businessModel.GeneralFields.Description ?? string.Empty;
            entity.ServerModel = businessModel.GeneralFields.Model ?? string.Empty;
            entity.SerialNumber = businessModel.GeneralFields.SerialNumber ?? string.Empty;

            entity.ChassisType = businessModel.ChassisFields.Chassis ?? string.Empty;

            entity.BarCode = businessModel.InventoryFields.BarCode ?? string.Empty;
            entity.PurchaseDate = businessModel.InventoryFields.PurchaseDate;

            entity.OperatingSystem_Id = businessModel.OperatingSystemFields.OperatingSystemId;
            entity.Version = businessModel.OperatingSystemFields.Version ?? string.Empty;
            entity.SP = businessModel.OperatingSystemFields.ServicePack ?? string.Empty;
            entity.RegistrationCode = businessModel.OperatingSystemFields.RegistrationCode ?? string.Empty;
            entity.ProductKey = businessModel.OperatingSystemFields.ProductKey ?? string.Empty;

            entity.Processor_Id = businessModel.ProccesorFields.ProccesorId;

            entity.RAM_Id = businessModel.MemoryFields.RAMId;

            entity.Harddrive = businessModel.StorageFields.Capasity ?? string.Empty;

            entity.NIC_Id = businessModel.CommunicationFields.NetworkAdapterId;
            entity.IPAddress = businessModel.CommunicationFields.IPAddress ?? string.Empty;
            entity.MACAddress = businessModel.CommunicationFields.MacAddress ?? string.Empty;

            entity.Info = businessModel.OtherFields.Info ?? string.Empty;
            entity.Miscellaneous = businessModel.OtherFields.Other ?? string.Empty;
            entity.URL = businessModel.OtherFields.URL ?? string.Empty;
            entity.URL2 = businessModel.OtherFields.URL2 ?? string.Empty;
            entity.Owner = businessModel.OtherFields.Owner ?? string.Empty;

            entity.Room_Id = businessModel.PlaceFields.RoomId;
            entity.Location = businessModel.PlaceFields.Location ?? string.Empty;
        }
    }
}