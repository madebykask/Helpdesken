using DH.Helpdesk.BusinessData.Models.Inventory.Output.Computer;

namespace DH.Helpdesk.Web.Infrastructure.BusinessModelFactories.Inventory.Concrete
{
    using System;

    using DH.Helpdesk.BusinessData.Models;
    using DH.Helpdesk.BusinessData.Models.Inventory.Edit.Server;
    using DH.Helpdesk.BusinessData.Models.Inventory.Edit.Shared;
    using DH.Helpdesk.Web.Areas.Inventory.Models.EditModel;
    using DH.Helpdesk.Web.Areas.Inventory.Models.EditModel.Server;
    using DH.Helpdesk.Web.Areas.Inventory.Models.EditModel.Shared;

    public class ServerBuilder : IServerBuilder
    {
        public ServerForUpdate BuildForUpdate(ServerViewModel model, OperationContext context)
        {
            var general = CreateGeneral(model.GeneralFieldsModel);
            var chassis = CreateChassis(model.ChassisFieldsModel);
            var inventering = CreateInventering(model.InventoryFieldsModel);
            var operatingSystem = CretateOperatingSystem(model.OperatingSystemFieldsViewModel);
            var processor = CreateProcessor(model.ProccesorFieldsViewModel);
            var memory = CreateMemory(model.MemoryFieldsViewModel);
            var storage = CreateStorage(model.StorageFieldsModel);
            var communication = CreateCommunication(model.CommunicationFieldsViewModel);
            var other = CreateOther(model.OtherFieldsModel);
            var place = CreatePlace(model.PlaceFieldsViewModel);
            var document = CreateDocument(model.DocumentFieldsModel);

            var fieldsModel = new ServerForUpdate(
                model.Id,
                model.IsOperationObject,
                general,
                other,
                storage,
                chassis,
                inventering,
                operatingSystem,
                memory,
                place,
                document,
                processor,
                communication,
                context.DateAndTime,
                context.UserId);

            return fieldsModel;
        }

        public ServerForInsert BuildForAdd(ServerViewModel model, OperationContext context, ComputerFile computerFile)
        {
            var general = CreateGeneral(model.GeneralFieldsModel);
            var chassis = CreateChassis(model.ChassisFieldsModel);
            var inventering = CreateInventering(model.InventoryFieldsModel);
            var operatingSystem = CretateOperatingSystem(model.OperatingSystemFieldsViewModel);
            var processor = CreateProcessor(model.ProccesorFieldsViewModel);
            var memory = CreateMemory(model.MemoryFieldsViewModel);
            var storage = CreateStorage(model.StorageFieldsModel);
            var communication = CreateCommunication(model.CommunicationFieldsViewModel);
            var other = CreateOther(model.OtherFieldsModel);
            var place = CreatePlace(model.PlaceFieldsViewModel);
            var document = CreateDocument(model.DocumentFieldsModel);

            var fieldsModel = new ServerForInsert(
                model.IsOperationObject,
                general,
                other,
                storage,
                chassis,
                inventering,
                operatingSystem,
                memory,
                place,
                document,
                processor,
                communication,
                context.CustomerId,
                computerFile,
                context.DateAndTime,
                context.UserId);

            return fieldsModel;
        }

        private static GeneralFields CreateGeneral(GeneralFieldsModel fieldsModel)
        {
            if (fieldsModel == null)
            {
                return GeneralFields.CreateDefault();
            }

            var name = ConfigurableFieldModel<string>.GetValueOrDefault(fieldsModel.Name);
            var manufacturer = ConfigurableFieldModel<string>.GetValueOrDefault(fieldsModel.Manufacturer);
            var serialNumber = ConfigurableFieldModel<string>.GetValueOrDefault(fieldsModel.SerialNumber);
            var model = ConfigurableFieldModel<string>.GetValueOrDefault(fieldsModel.Model);
            var description = ConfigurableFieldModel<string>.GetValueOrDefault(fieldsModel.Description);

            var fields = new GeneralFields(name, manufacturer, description, model, serialNumber);

            return fields;
        }

        private static ChassisFields CreateChassis(ChassisFieldsModel fieldsModel)
        {
            if (fieldsModel == null)
            {
                return ChassisFields.CreateDefault();
            }

            var chassis = ConfigurableFieldModel<string>.GetValueOrDefault(fieldsModel.Chassis);

            return new ChassisFields(chassis);
        }

        private static StorageFields CreateStorage(StorageFieldsModel fieldsModel)
        {
            if (fieldsModel == null)
            {
                return StorageFields.CreateDefault();
            }

            var storage = ConfigurableFieldModel<string>.GetValueOrDefault(fieldsModel.Capasity);

            return new StorageFields(storage);
        }

        private static InventoryFields CreateInventering(InventoryFieldsModel fieldsModel)
        {
            if (fieldsModel == null)
            {
                return InventoryFields.CreateDefault();
            }

            var barCode = ConfigurableFieldModel<string>.GetValueOrDefault(fieldsModel.BarCode);
            var purchaseDate = ConfigurableFieldModel<DateTime?>.GetValueOrDefault(fieldsModel.PurchaseDate);

            var fields = new InventoryFields(barCode, purchaseDate);

            return fields;
        }

        private static OperatingSystemFields CretateOperatingSystem(OperatingSystemFieldsViewModel fieldsModel)
        {
            if (fieldsModel == null || fieldsModel.OperatingSystemFieldsModel == null)
            {
                return OperatingSystemFields.CreateDefault();
            }

            var operatingSystem = ConfigurableFieldModel<int?>.GetValueOrDefault(fieldsModel.OperatingSystemFieldsModel.OperatingSystemId);
            var version = ConfigurableFieldModel<string>.GetValueOrDefault(fieldsModel.OperatingSystemFieldsModel.Version);
            var servicePack = ConfigurableFieldModel<string>.GetValueOrDefault(fieldsModel.OperatingSystemFieldsModel.ServicePack);
            var registrationCode = ConfigurableFieldModel<string>.GetValueOrDefault(fieldsModel.OperatingSystemFieldsModel.RegistrationCode);
            var productKey = ConfigurableFieldModel<string>.GetValueOrDefault(fieldsModel.OperatingSystemFieldsModel.ProductKey);

            var fields = new OperatingSystemFields(
                operatingSystem,
                version,
                servicePack,
                registrationCode,
                productKey);

            return fields;
        }

        private static ProcessorFields CreateProcessor(ProccesorFieldsViewModel fieldsModel)
        {
            if (fieldsModel == null || fieldsModel.ProccesorFieldsModel == null)
            {
                return ProcessorFields.CreateDefault();
            }

            var processor = ConfigurableFieldModel<int?>.GetValueOrDefault(fieldsModel.ProccesorFieldsModel.ProccesorId);

            var fields = new ProcessorFields(processor);

            return fields;
        }

        private static MemoryFields CreateMemory(MemoryFieldsViewModel fieldsModel)
        {
            if (fieldsModel == null || fieldsModel.MemoryFieldsModel == null)
            {
                return MemoryFields.CreateDefault();
            }

            var memory = ConfigurableFieldModel<int?>.GetValueOrDefault(fieldsModel.MemoryFieldsModel.RAMId);

            var fields = new MemoryFields(memory);

            return fields;
        }

        private static CommunicationFields CreateCommunication(CommunicationFieldsViewModel fieldsModel)
        {
            if (fieldsModel == null || fieldsModel.CommunicationFieldsModel == null)
            {
                return CommunicationFields.CreateDefault();
            }

            var networkAdapter = ConfigurableFieldModel<int?>.GetValueOrDefault(fieldsModel.CommunicationFieldsModel.NetworkAdapterId);
            var ipaddress = ConfigurableFieldModel<string>.GetValueOrDefault(fieldsModel.CommunicationFieldsModel.IPAddress);
            var macAddress = ConfigurableFieldModel<string>.GetValueOrDefault(fieldsModel.CommunicationFieldsModel.MacAddress);

            var fields = new CommunicationFields(
                networkAdapter,
                ipaddress,
                macAddress);

            return fields;
        }

        private static OtherFields CreateOther(OtherFieldsModel fieldsModel)
        {
            if (fieldsModel == null)
            {
                return OtherFields.CreateDefault();
            }

            var info = ConfigurableFieldModel<string>.GetValueOrDefault(fieldsModel.Info);
            var other = ConfigurableFieldModel<string>.GetValueOrDefault(fieldsModel.Other);
            var url = ConfigurableFieldModel<string>.GetValueOrDefault(fieldsModel.URL);
            var url2 = ConfigurableFieldModel<string>.GetValueOrDefault(fieldsModel.URL2);
            var owner = ConfigurableFieldModel<string>.GetValueOrDefault(fieldsModel.Owner);

            var fields = new OtherFields(info, other, url, url2, owner);

            return fields;
        }

        private static PlaceFields CreatePlace(PlaceFieldsViewModel fieldsModel)
        {
            if (fieldsModel == null || fieldsModel.PlaceFieldsModel == null)
            {
                return PlaceFields.CreateDefault();
            }

            var room = ConfigurableFieldModel<int?>.GetValueOrDefault(fieldsModel.PlaceFieldsModel.RoomId);
            var location = ConfigurableFieldModel<string>.GetValueOrDefault(fieldsModel.PlaceFieldsModel.Location);

            var fields = new PlaceFields(room, location);

            return fields;
        }

        private static DocumentFields CreateDocument(DocumentFieldsModel fieldsModel)
        {
            if (fieldsModel == null)
            {
                return DocumentFields.CreateDefault();
            }

            var documnet = ConfigurableFieldModel<string>.GetValueOrDefault(fieldsModel.Document);

            return new DocumentFields(documnet);
        }

        private static StateFields CreateState(StateFieldsModel fieldsModel)
        {
            if (fieldsModel == null)
            {
                return StateFields.CreateDefault();
            }

            var state = ConfigurableFieldModel<DateTime?>.GetValueOrDefault(fieldsModel.SyncChangeDate);

            var fields = new StateFields(state);

            return fields;
        }
    }
}