namespace DH.Helpdesk.Web.Infrastructure.ModelFactories.Inventory.Concrete
{
    using DH.Helpdesk.BusinessData.Models.Inventory.Edit.Server;
    using DH.Helpdesk.BusinessData.Models.Inventory.Output.Settings.ModelEdit.ServerSettings;
    using DH.Helpdesk.Web.Areas.Inventory.Models.EditModel.Server;
    using DH.Helpdesk.Web.Areas.Inventory.Models.EditModel.Shared;
    using DH.Helpdesk.Web.Areas.Inventory.Models.OptionsAggregates;
    using System;
    using System.Collections.Generic;

    public class ServerViewModelBuilder : IServerViewModelBuilder
    {
        private readonly IConfigurableFieldModelBuilder configurableFieldModelBuilder;

        public ServerViewModelBuilder(IConfigurableFieldModelBuilder configurableFieldModelBuilder)
        {
            this.configurableFieldModelBuilder = configurableFieldModelBuilder;
        }

        public ServerViewModel BuildViewModel(
            ServerForRead model,
            ServerEditOptions options,
            ServerFieldsSettingsForModelEdit settings,
            List<string> fileUploadWhiteList)
        {
            var createdDate =
                this.configurableFieldModelBuilder.CreateDateTimeField(
                    settings.StateFieldsSettings.CreatedDateFieldSetting,
                    model.CreatedDate);
            var changedDate =
                this.configurableFieldModelBuilder.CreateDateTimeField(
                    settings.StateFieldsSettings.ChangedDateFieldSetting,
                    model.ChangedDate);

            var name =
                this.configurableFieldModelBuilder.CreateStringField(
                    settings.GeneralFieldsSettings.ServerNameFieldSetting,
                    model.GeneralFields.Name);
            var manufacturer =
                this.configurableFieldModelBuilder.CreateStringField(
                    settings.GeneralFieldsSettings.ManufacturerFieldSetting,
                    model.GeneralFields.Manufacturer);
            var description =
                this.configurableFieldModelBuilder.CreateStringField(
                    settings.GeneralFieldsSettings.DescriptionFieldSetting,
                    model.GeneralFields.Description);
            var serverModel =
                this.configurableFieldModelBuilder.CreateStringField(
                    settings.GeneralFieldsSettings.ComputerModelFieldSetting,
                    model.GeneralFields.Model);
            var serial =
                this.configurableFieldModelBuilder.CreateStringField(
                    settings.GeneralFieldsSettings.SerialNumberFieldSetting,
                    model.GeneralFields.SerialNumber);

            var generalFieldsModel = new GeneralFieldsModel(name, manufacturer, description, serverModel, serial);

            var processor =
                this.configurableFieldModelBuilder.CreateNullableIntegerField(
                    settings.ProccesorFieldsSettings.ProccesorFieldSetting,
                    model.ProccesorFields.ProccesorId);
            var proccesorFieldsModel = new ProccesorFieldsModel(processor);
            var processors =
                this.configurableFieldModelBuilder.CreateSelectList(
                    settings.ProccesorFieldsSettings.ProccesorFieldSetting,
                    options.Processors,
                    model.ProccesorFields.ProccesorId.ToString());

            var processorViewModel = new ProccesorFieldsViewModel(proccesorFieldsModel, processors);

            var operatingSystem =
                this.configurableFieldModelBuilder.CreateNullableIntegerField(
                    settings.OperatingSystemFieldsSettings.OperatingSystemFieldSetting,
                    model.OperatingSystemFields.OperatingSystemId);
            var version =
                this.configurableFieldModelBuilder.CreateStringField(
                    settings.OperatingSystemFieldsSettings.VersionFieldSetting,
                    model.OperatingSystemFields.Version);
            var servicePack =
                this.configurableFieldModelBuilder.CreateStringField(
                    settings.OperatingSystemFieldsSettings.ServicePackSystemFieldSetting,
                    model.OperatingSystemFields.ServicePack);
            var registratinCode =
                this.configurableFieldModelBuilder.CreateStringField(
                    settings.OperatingSystemFieldsSettings.RegistrationCodeSystemFieldSetting,
                    model.OperatingSystemFields.RegistrationCode);
            var productKey =
                this.configurableFieldModelBuilder.CreateStringField(
                    settings.OperatingSystemFieldsSettings.ProductKeyFieldSetting,
                    model.OperatingSystemFields.ProductKey);

            var operatingSystemFieldsModel = new OperatingSystemFieldsModel(
                operatingSystem,
                version,
                servicePack,
                registratinCode,
                productKey);
            var operatingSystems =
                this.configurableFieldModelBuilder.CreateSelectList(
                    settings.OperatingSystemFieldsSettings.OperatingSystemFieldSetting,
                    options.OperatingSystems,
                    model.OperatingSystemFields.OperatingSystemId.ToString());

            var operatingSystemsViewModel = new OperatingSystemFieldsViewModel(
                operatingSystemFieldsModel,
                operatingSystems);

            var memory =
                this.configurableFieldModelBuilder.CreateNullableIntegerField(
                    settings.MemoryFieldsSettings.RAMFieldSetting,
                    model.MemoryFields.RAMId);
            var memoryFieldsModel = new MemoryFieldsModel(memory);
            var memories =
                this.configurableFieldModelBuilder.CreateSelectList(
                    settings.MemoryFieldsSettings.RAMFieldSetting,
                    options.Rams,
                    model.MemoryFields.RAMId.ToString());

            var memoryViewModel = new MemoryFieldsViewModel(memoryFieldsModel, memories);

            var storage =
                this.configurableFieldModelBuilder.CreateStringField(
                    settings.StorageFieldsSettings.CapasityFieldSetting,
                    model.StorageFields.Capasity);

            var storagiesFieldModel = new StorageFieldsModel(storage);

            var barCode =
                this.configurableFieldModelBuilder.CreateStringField(
                    settings.InventoryFieldsSettings.BarCodeFieldSetting,
                    model.InventoryFields.BarCode);
            var purchaseDate =
                this.configurableFieldModelBuilder.CreateNullableDateTimeField(
                    settings.InventoryFieldsSettings.PurchaseDateFieldSetting,
                    model.InventoryFields.PurchaseDate);

            var inventoryFieldModel = new InventoryFieldsModel(barCode, purchaseDate);

            var chassis =
                this.configurableFieldModelBuilder.CreateStringField(
                    settings.ChassisFieldsSettings.ChassisFieldSetting,
                    model.ChassisFields.Chassis);

            var chassisFieldModel = new ChassisFieldsModel(chassis);

            var location =
                this.configurableFieldModelBuilder.CreateStringField(
                    settings.PlaceFieldsSettings.LocationFieldSetting,
                    model.PlaceFields.Location);

            var room =
                this.configurableFieldModelBuilder.CreateNullableIntegerField(
                    settings.PlaceFieldsSettings.RoomFieldSetting,
                    model.PlaceFields.RoomId);
            var placeFieldsModel = new PlaceFieldsModel(
                model.PlaceFields.BuildingId,
                model.PlaceFields.FloorId,
                room,
                location);

            var buildings =
                this.configurableFieldModelBuilder.CreateSelectList(
                    settings.PlaceFieldsSettings.RoomFieldSetting,
                    options.Buildings,
                    model.PlaceFields.BuildingId.ToString());
            var floors =
                this.configurableFieldModelBuilder.CreateSelectList(
                    settings.PlaceFieldsSettings.RoomFieldSetting,
                    options.Floors,
                    model.PlaceFields.FloorId.ToString());
            var rooms =
                this.configurableFieldModelBuilder.CreateSelectList(
                    settings.PlaceFieldsSettings.RoomFieldSetting,
                    options.Rooms,
                    model.PlaceFields.RoomId.ToString());

            var placeFieldsViewModel = new PlaceFieldsViewModel(placeFieldsModel, buildings, floors, rooms);

            var document =
                this.configurableFieldModelBuilder.CreateStringField(
                    settings.DocumentFieldsSettings.DocumentFieldSetting,
                    model.DocumentFields.Document);

            var documentFieldModel = new DocumentFieldsModel(document);

            var info =
                this.configurableFieldModelBuilder.CreateStringField(
                    settings.OtherFieldsSettings.InfoFieldSetting,
                    model.OtherFields.Info);
            var other =
                this.configurableFieldModelBuilder.CreateStringField(
                    settings.OtherFieldsSettings.OtherFieldSetting,
                    model.OtherFields.Other);
            var url = this.configurableFieldModelBuilder.CreateStringField(
                settings.OtherFieldsSettings.URLFieldSetting,
                model.OtherFields.URL);
            var url2 =
                this.configurableFieldModelBuilder.CreateStringField(
                    settings.OtherFieldsSettings.URL2FieldSetting,
                    model.OtherFields.URL2);
            var owner =
                this.configurableFieldModelBuilder.CreateStringField(
                    settings.OtherFieldsSettings.OwnerFieldSetting,
                    model.OtherFields.Owner);

            var otherFieldModel = new OtherFieldsModel(info, other, url, url2, owner);

            var ip =
                this.configurableFieldModelBuilder.CreateStringField(
                    settings.CommunicationFieldsSettings.IPAddressFieldSetting,
                    model.CommunicationFields.IPAddress);
            var mac =
                this.configurableFieldModelBuilder.CreateStringField(
                    settings.CommunicationFieldsSettings.MacAddressFieldSetting,
                    model.CommunicationFields.MacAddress);

            var communication =
                this.configurableFieldModelBuilder.CreateNullableIntegerField(
                    settings.CommunicationFieldsSettings.NetworkAdapterFieldSetting,
                    model.CommunicationFields.NetworkAdapterId);
            var communicationFieldsModel = new CommunicationFieldsModel(communication, ip, mac);

            var adapters =
                this.configurableFieldModelBuilder.CreateSelectList(
                    settings.CommunicationFieldsSettings.NetworkAdapterFieldSetting,
                    options.NetworkAdapters,
                    model.CommunicationFields.NetworkAdapterId.ToString());

            var communicationViewModel = new CommunicationFieldsViewModel(communicationFieldsModel, adapters);

            var syncDate =
                this.configurableFieldModelBuilder.CreateNullableDateTimeField(
                    settings.StateFieldsSettings.SyncChangeDateFieldSetting,
                    model.StateFields.SyncChangeDate);

            var stateFieldsModel = new StateFieldsModel(syncDate, model.StateFields.ChangedByUserName);

            return new ServerViewModel(
                model.IsOperationObject,
                generalFieldsModel,
                otherFieldModel,
                stateFieldsModel,
                storagiesFieldModel,
                chassisFieldModel,
                inventoryFieldModel,
                memoryViewModel,
                communicationViewModel,
                operatingSystemsViewModel,
                processorViewModel,
                placeFieldsViewModel,
                documentFieldModel,
                fileUploadWhiteList) { Id = model.Id, CreatedDate = createdDate, ChangedDate = changedDate };
        }

        public ServerViewModel BuildViewModel(
            ServerEditOptions options,
            ServerFieldsSettingsForModelEdit settings,
            int currentCustomerId,
            List<string> fileUploadWhiteList)
        {
            var name =
                this.configurableFieldModelBuilder.CreateStringField(
                    settings.GeneralFieldsSettings.ServerNameFieldSetting,
                    null);
            var manufacturer =
                this.configurableFieldModelBuilder.CreateStringField(
                    settings.GeneralFieldsSettings.ManufacturerFieldSetting,
                    null);
            var description =
                this.configurableFieldModelBuilder.CreateStringField(
                    settings.GeneralFieldsSettings.DescriptionFieldSetting,
                    null);
            var serverModel =
                this.configurableFieldModelBuilder.CreateStringField(
                    settings.GeneralFieldsSettings.ComputerModelFieldSetting,
                    null);
            var serial =
                this.configurableFieldModelBuilder.CreateStringField(
                    settings.GeneralFieldsSettings.SerialNumberFieldSetting,
                    null);

            var generalFieldsModel = new GeneralFieldsModel(
                name,
                manufacturer,
                description,
                serverModel,
                serial);

            var processor =
                this.configurableFieldModelBuilder.CreateNullableIntegerField(
                    settings.ProccesorFieldsSettings.ProccesorFieldSetting,
                    null);
            var proccesorFieldsModel = new ProccesorFieldsModel(processor);
            var processors =
                this.configurableFieldModelBuilder.CreateSelectList(
                    settings.ProccesorFieldsSettings.ProccesorFieldSetting,
                    options.Processors,
                    null);

            var processorViewModel = new ProccesorFieldsViewModel(proccesorFieldsModel, processors);

            var operatingSystem =
                this.configurableFieldModelBuilder.CreateNullableIntegerField(
                    settings.OperatingSystemFieldsSettings.OperatingSystemFieldSetting,
                    null);
            var version =
                this.configurableFieldModelBuilder.CreateStringField(
                    settings.OperatingSystemFieldsSettings.VersionFieldSetting,
                    null);
            var servicePack =
                this.configurableFieldModelBuilder.CreateStringField(
                    settings.OperatingSystemFieldsSettings.ServicePackSystemFieldSetting,
                    null);
            var registratinCode =
                this.configurableFieldModelBuilder.CreateStringField(
                    settings.OperatingSystemFieldsSettings.RegistrationCodeSystemFieldSetting,
                    null);
            var productKey =
                this.configurableFieldModelBuilder.CreateStringField(
                    settings.OperatingSystemFieldsSettings.ProductKeyFieldSetting,
                    null);

            var operatingSystemFieldsModel =
                new OperatingSystemFieldsModel(
                    operatingSystem,
                    version,
                    servicePack,
                    registratinCode,
                    productKey);
            var operatingSystems =
                this.configurableFieldModelBuilder.CreateSelectList(
                    settings.OperatingSystemFieldsSettings.OperatingSystemFieldSetting,
                    options.OperatingSystems,
                    null);

            var operatingSystemsViewModel = new OperatingSystemFieldsViewModel(
                operatingSystemFieldsModel,
                operatingSystems);

            var memory =
                this.configurableFieldModelBuilder.CreateNullableIntegerField(
                    settings.MemoryFieldsSettings.RAMFieldSetting,
                    null);
            var memoryFieldsModel = new MemoryFieldsModel(memory);
            var memories =
                this.configurableFieldModelBuilder.CreateSelectList(
                    settings.MemoryFieldsSettings.RAMFieldSetting,
                    options.Rams,
                    null);

            var memoryViewModel = new MemoryFieldsViewModel(memoryFieldsModel, memories);

            var storage =
                this.configurableFieldModelBuilder.CreateStringField(
                    settings.StorageFieldsSettings.CapasityFieldSetting,
                    null);

            var storagiesFieldModel = new StorageFieldsModel(storage);

            var barCode =
                this.configurableFieldModelBuilder.CreateStringField(
                    settings.InventoryFieldsSettings.BarCodeFieldSetting,
                    null);
            var purchaseDate =
                this.configurableFieldModelBuilder.CreateNullableDateTimeField(
                    settings.InventoryFieldsSettings.PurchaseDateFieldSetting,
                    null);

            var inventoryFieldModel = new InventoryFieldsModel(barCode, purchaseDate);

            var chassis =
                this.configurableFieldModelBuilder.CreateStringField(
                    settings.ChassisFieldsSettings.ChassisFieldSetting,
                    null);

            var chassisFieldModel = new ChassisFieldsModel(chassis);

            var location =
                this.configurableFieldModelBuilder.CreateStringField(
                    settings.PlaceFieldsSettings.LocationFieldSetting,
                    null);
            var room =
                this.configurableFieldModelBuilder.CreateNullableIntegerField(
                    settings.PlaceFieldsSettings.RoomFieldSetting,
                    null);
            var placeFieldsModel = new PlaceFieldsModel(
                null,
                null,
                room,
                location);

            var buildings = this.configurableFieldModelBuilder.CreateSelectList(
                settings.PlaceFieldsSettings.RoomFieldSetting,
                options.Buildings,
                null);
            var floors = this.configurableFieldModelBuilder.CreateSelectList(
                settings.PlaceFieldsSettings.RoomFieldSetting,
                options.Floors,
                null);
            var rooms =
                this.configurableFieldModelBuilder.CreateSelectList(
                    settings.PlaceFieldsSettings.RoomFieldSetting,
                    options.Rooms,
                    null);

            var placeFieldsViewModel = new PlaceFieldsViewModel(placeFieldsModel, buildings, floors, rooms);

            var document =
               this.configurableFieldModelBuilder.CreateStringField(
                   settings.DocumentFieldsSettings.DocumentFieldSetting,
                   null);

            var documentFieldsModel = new DocumentFieldsModel(document);

            var info = this.configurableFieldModelBuilder.CreateStringField(
                settings.OtherFieldsSettings.InfoFieldSetting,
                null);
            var other = this.configurableFieldModelBuilder.CreateStringField(
                settings.OtherFieldsSettings.OtherFieldSetting,
                null);
            var url = this.configurableFieldModelBuilder.CreateStringField(
                settings.OtherFieldsSettings.URLFieldSetting,
                null);
            var url2 = this.configurableFieldModelBuilder.CreateStringField(
                settings.OtherFieldsSettings.URL2FieldSetting,
                null);
            var owner = this.configurableFieldModelBuilder.CreateStringField(
                settings.OtherFieldsSettings.OwnerFieldSetting,
                null);

            var otherFieldModel = new OtherFieldsModel(info, other, url, url2, owner);

            var ip =
                this.configurableFieldModelBuilder.CreateStringField(
                    settings.CommunicationFieldsSettings.IPAddressFieldSetting,
                    null);
            var mac =
                this.configurableFieldModelBuilder.CreateStringField(
                    settings.CommunicationFieldsSettings.MacAddressFieldSetting,
                    null);
            var communication =
                this.configurableFieldModelBuilder.CreateNullableIntegerField(
                    settings.CommunicationFieldsSettings.NetworkAdapterFieldSetting,
                    null);

            var communicationFieldsModel = new CommunicationFieldsModel(
                communication,
                ip,
                mac);

            var adapters =
                this.configurableFieldModelBuilder.CreateSelectList(
                    settings.CommunicationFieldsSettings.NetworkAdapterFieldSetting,
                    options.NetworkAdapters,
                    null);

            var communicationViewModel = new CommunicationFieldsViewModel(communicationFieldsModel, adapters);

            var syncDate =
                this.configurableFieldModelBuilder.CreateNullableDateTimeField(
                    settings.StateFieldsSettings.SyncChangeDateFieldSetting,
                    null);

            var stateFieldsModel = new StateFieldsModel(syncDate, null);

            return new ServerViewModel(
                false,
                generalFieldsModel,
                otherFieldModel,
                stateFieldsModel,
                storagiesFieldModel,
                chassisFieldModel,
                inventoryFieldModel,
                memoryViewModel,
                communicationViewModel,
                operatingSystemsViewModel,
                processorViewModel,
                placeFieldsViewModel,
                documentFieldsModel,
                fileUploadWhiteList) { CustomerId = currentCustomerId, DocumentFileKey = Guid.NewGuid().ToString() };
        }
    }
}