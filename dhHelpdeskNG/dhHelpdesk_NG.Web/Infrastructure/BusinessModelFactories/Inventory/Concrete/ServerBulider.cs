namespace DH.Helpdesk.Web.Infrastructure.BusinessModelFactories.Inventory.Concrete
{
    using System;

    using DH.Helpdesk.BusinessData.Models;
    using DH.Helpdesk.BusinessData.Models.Inventory.Edit.Server;
    using DH.Helpdesk.BusinessData.Models.Inventory.Edit.Shared;
    using DH.Helpdesk.Web.Models.Inventory.EditModel;
    using DH.Helpdesk.Web.Models.Inventory.EditModel.Server;
    using DH.Helpdesk.Web.Models.Inventory.EditModel.Shared;

    public class ServerBuilder : IServerBuilder
    {
        public Server BuildForUpdate(ServerViewModel model, OperationContext context)
        {
            var general = CreateGeneral(model.GeneralFieldsModel);
            var chassis = CreateChassis(model.ChassisFieldsModel);
            var inventering = CreateInventering(model.InventoryFieldsModel);
            var operatingSystem = CretateOperatingSystem(model.OperatingSystemFieldsViewModel.OperatingSystemFieldsModel);
            var processor = CreateProcessor(model.ProccesorFieldsViewModel.ProccesorFieldsModel);
            var memory = CreateMemory(model.MemoryFieldsViewModel.MemoryFieldsModel);
            var storage = CreateStorage(model.StorageFieldsModel);
            var communication = CreateCommunication(model.CommunicationFieldsViewModel.CommunicationFieldsModel);
            var other = CreateOther(model.OtherFieldsModel);
            var place = CreatePlace(model.PlaceFieldsViewModel.PlaceFieldsModel);
            var state = CreateState(model.StateFieldsModel);

            var fieldsModel = Server.CreateUpdated(
                model.Id,
                model.IsOperationObject,
                general,
                other,
                state,
                storage,
                chassis,
                inventering,
                operatingSystem,
                memory,
                place,
                processor,
                context.DateAndTime,
                communication);

            return fieldsModel;
        }

        public Server BuildForAdd(ServerViewModel model, OperationContext context)
        {
            var general = CreateGeneral(model.GeneralFieldsModel);
            var chassis = CreateChassis(model.ChassisFieldsModel);
            var inventering = CreateInventering(model.InventoryFieldsModel);
            var operatingSystem = CretateOperatingSystem(model.OperatingSystemFieldsViewModel.OperatingSystemFieldsModel);
            var processor = CreateProcessor(model.ProccesorFieldsViewModel.ProccesorFieldsModel);
            var memory = CreateMemory(model.MemoryFieldsViewModel.MemoryFieldsModel);
            var storage = CreateStorage(model.StorageFieldsModel);
            var communication = CreateCommunication(model.CommunicationFieldsViewModel.CommunicationFieldsModel);
            var other = CreateOther(model.OtherFieldsModel);
            var place = CreatePlace(model.PlaceFieldsViewModel.PlaceFieldsModel);
            var state = CreateState(model.StateFieldsModel);

            var fieldsModel = Server.CreateNew(
                context.CustomerId,
                model.IsOperationObject,
                general,
                other,
                state,
                storage,
                chassis,
                inventering,
                operatingSystem,
                memory,
                place,
                processor,
                context.DateAndTime,
                communication);

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

        private static OperatingSystemFields CretateOperatingSystem(OperatingSystemFieldsModel fieldsModel)
        {
            if (fieldsModel == null)
            {
                return OperatingSystemFields.CreateDefault();
            }

            var operatingSystem = ConfigurableFieldModel<int?>.GetValueOrDefault(fieldsModel.OperatingSystemId);
            var version = ConfigurableFieldModel<string>.GetValueOrDefault(fieldsModel.Version);
            var servicePack = ConfigurableFieldModel<string>.GetValueOrDefault(fieldsModel.ServicePack);
            var registrationCode = ConfigurableFieldModel<string>.GetValueOrDefault(fieldsModel.RegistrationCode);
            var productKey = ConfigurableFieldModel<string>.GetValueOrDefault(fieldsModel.ProductKey);

            var fields = new OperatingSystemFields(
                operatingSystem,
                version,
                servicePack,
                registrationCode,
                productKey);

            return fields;
        }

        private static ProcessorFields CreateProcessor(ProccesorFieldsModel fieldsModel)
        {
            if (fieldsModel == null)
            {
                return ProcessorFields.CreateDefault();
            }

            var processor = ConfigurableFieldModel<int?>.GetValueOrDefault(fieldsModel.ProccesorId);

            var fields = new ProcessorFields(processor);

            return fields;
        }

        private static MemoryFields CreateMemory(MemoryFieldsModel fieldsModel)
        {
            if (fieldsModel == null)
            {
                return MemoryFields.CreateDefault();
            }

            var memory = ConfigurableFieldModel<int?>.GetValueOrDefault(fieldsModel.RAMId);

            var fields = new MemoryFields(memory);

            return fields;
        }

        private static CommunicationFields CreateCommunication(CommunicationFieldsModel fieldsModel)
        {
            if (fieldsModel == null)
            {
                return CommunicationFields.CreateDefault();
            }

            var networkAdapter = ConfigurableFieldModel<int?>.GetValueOrDefault(fieldsModel.NetworkAdapterId);
            var ipaddress = ConfigurableFieldModel<string>.GetValueOrDefault(fieldsModel.IPAddress);
            var macAddress = ConfigurableFieldModel<string>.GetValueOrDefault(fieldsModel.MacAddress);

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

        private static PlaceFields CreatePlace(PlaceFieldsModel fieldsModel)
        {
            if (fieldsModel == null)
            {
                return PlaceFields.CreateDefault();
            }

            var room = ConfigurableFieldModel<int?>.GetValueOrDefault(fieldsModel.RoomId);
            var location = ConfigurableFieldModel<string>.GetValueOrDefault(fieldsModel.Location);

            var fields = new PlaceFields(room, location);

            return fields;
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