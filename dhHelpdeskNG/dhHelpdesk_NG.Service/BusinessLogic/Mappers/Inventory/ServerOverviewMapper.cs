namespace DH.Helpdesk.Services.BusinessLogic.Mappers.Inventory
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Linq.Expressions;

    using DH.Helpdesk.BusinessData.Models.Inventory.Output.Server;
    using DH.Helpdesk.BusinessData.Models.Inventory.Output.Shared;
    using DH.Helpdesk.Dal.NewInfrastructure;
    using DH.Helpdesk.Domain;
    using DH.Helpdesk.Domain.Servers;

    public static class ServerOverviewMapper
    {
        public static ServerOverview[] MapToFullOverviews(this IQueryable<Server> query)
        {
            var overviewAggregates =
                query.AsEnumerable().Select(
                    server =>
                        {
                            GeneralFields generalFields = CreateGeneralFields(server);
                            OtherFields otherFields = CreateOtherFileds(server);
                            StateFields stateFields = new StateFields(server.SyncChangedDate);
                            StorageFields storageFields = new StorageFields(server.Harddrive);
                            ChassisFields chassisFields = new ChassisFields(server.ChassisType);
                            InventoryFields inventoryFields = new InventoryFields(server.BarCode, server.PurchaseDate);
                            MemoryFields memoryFields = new MemoryFields(server.RAM != null && server.RAM.Name != null ? server.RAM.Name : string.Empty);
                            OperatingSystemFields operatingSystemFields =
                                new OperatingSystemFields(
                                    server.OperatingSystem != null && server.OperatingSystem.Name != null
                                        ? server.OperatingSystem.Name
                                        : string.Empty,
                                    server.Version,
                                    server.SP,
                                    server.RegistrationCode,
                                    server.ProductKey);
                            ProcessorFields proccesorFields = new ProcessorFields(server.Processor != null && server.Processor.Name != null? server.Processor.Name : string.Empty);
                            PlaceFields placeFields =
                                new PlaceFields(
                                    server.Room != null && server.Room.Name != null ? server.Room.Name : string.Empty,
                                    server.Location);
                            CommunicationFields communicationFields =
                                new CommunicationFields(
                                    server.NIC != null && server.NIC.Name != null? server.NIC.Name : string.Empty,
                                    server.IPAddress,
                                    server.MACAddress);

                            return new ServerOverview(
                                server.Id,
                                server.Customer_Id,
                                server.CreatedDate,
                                server.ChangedDate,
                                generalFields,
                                otherFields,
                                stateFields,
                                storageFields,
                                chassisFields,
                                inventoryFields,
                                memoryFields,
                                operatingSystemFields,
                                proccesorFields,
                                placeFields,
                                communicationFields);
                        }).ToArray();

            return overviewAggregates;
        }

        #region Create fields
 
        private static GeneralFields CreateGeneralFields(Server entity)
        {
            return new GeneralFields(
                entity.ServerName,
                entity.Manufacturer,
                entity.ServerDescription,
                entity.ServerModel,
                entity.SerialNumber);
        }

        private static OtherFields CreateOtherFileds(Server entity)
        {
            return new OtherFields(
                entity.Info,
                entity.Miscellaneous,
                entity.URL,
                entity.URL2,
                entity.Owner);
        }

        #endregion
    }
}