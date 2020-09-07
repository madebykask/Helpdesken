namespace DH.Helpdesk.Dal.Mappers.Inventory.EntityToBusinessModel.Server
{
    using DH.Helpdesk.BusinessData.Models.Inventory.Output.Settings.ModelEdit;
    using DH.Helpdesk.BusinessData.Models.Inventory.Output.Settings.ModelEdit.ServerSettings;
    using DH.Helpdesk.BusinessData.Models.Inventory.Output.Settings.ModelEdit.SharedSettings;
    using DH.Helpdesk.Common.Collections;
    using DH.Helpdesk.Common.Extensions.Integer;
    using DH.Helpdesk.Dal.Enums.Inventory.Server;
    using DH.Helpdesk.Dal.Enums.Inventory.Shared;
    using DH.Helpdesk.Dal.MapperData.Inventory;

    public sealed class ServerFieldSettingsToServerEditSettingsMapper :
        IEntityToBusinessModelMapper<NamedObjectCollection<FieldSettingMapperDataForModelEdit>, ServerFieldsSettingsForModelEdit>
    {
        public ServerFieldsSettingsForModelEdit Map(NamedObjectCollection<FieldSettingMapperDataForModelEdit> entity)
        {
            var general = CreateGeneralSettings(entity);
            var chassis = CreateChassisSettings(entity);
            var inventory = CreateInventeringSettings(entity);
            var os = CretateOperatingSystemSettings(entity);
            var processor = CreateProcessorSettings(entity);
            var memory = CreateMemorySettings(entity);
            var storage = CreateStorageSettings(entity);
            var communication = CreateCommunicationSettings(entity);
            var other = CreateOtherSettings(entity);
            var place = CreatePlaceSettings(entity);
            var document = CreateDocumentSettings(entity);
            var state = CreateStateSettings(entity);

            var settings = new ServerFieldsSettingsForModelEdit(
                general,
                other,
                state,
                storage,
                chassis,
                inventory,
                memory,
                os,
                processor,
                place,
                document,
                communication);

            return settings;
        }

        private static GeneralFieldsSettings CreateGeneralSettings(NamedObjectCollection<FieldSettingMapperDataForModelEdit> entity)
        {
            var name = CreateFieldSetting(entity.FindByName(GeneralFields.Name));
            var manufacturer = CreateFieldSetting(entity.FindByName(GeneralFields.Manufacturer));
            var description = CreateFieldSetting(entity.FindByName(GeneralFields.Description));
            var model = CreateFieldSetting(entity.FindByName(GeneralFields.Model));
            var serialNumber = CreateFieldSetting(entity.FindByName(GeneralFields.SerialNumber));

            var settings = new GeneralFieldsSettings(name, manufacturer, description, model, serialNumber);

            return settings;
        }

        private static ChassisFieldsSettings CreateChassisSettings(NamedObjectCollection<FieldSettingMapperDataForModelEdit> entity)
        {
            var chassis = CreateFieldSetting(entity.FindByName(ChassisFields.Chassis));

            var settings = new ChassisFieldsSettings(chassis);

            return settings;
        }

        private static InventoryFieldsSettings CreateInventeringSettings(NamedObjectCollection<FieldSettingMapperDataForModelEdit> entity)
        {
            var barCode = CreateFieldSetting(entity.FindByName(InventoryFields.BarCode));
            var purchaseDate = CreateFieldSetting(entity.FindByName(InventoryFields.PurchaseDate));

            var settings = new InventoryFieldsSettings(barCode, purchaseDate);

            return settings;
        }

        private static OperatingSystemFieldsSettings CretateOperatingSystemSettings(NamedObjectCollection<FieldSettingMapperDataForModelEdit> entity)
        {
            var operatingSystem = CreateFieldSetting(entity.FindByName(OperatingSystemFields.OperatingSystem));
            var version = CreateFieldSetting(entity.FindByName(OperatingSystemFields.Version));
            var servicePack = CreateFieldSetting(entity.FindByName(OperatingSystemFields.ServicePack));
            var registrationCode = CreateFieldSetting(entity.FindByName(OperatingSystemFields.RegistrationCode));
            var productKey = CreateFieldSetting(entity.FindByName(OperatingSystemFields.ProductKey));

            var settings = new OperatingSystemFieldsSettings(
                operatingSystem,
                version,
                servicePack,
                registrationCode,
                productKey);

            return settings;
        }

        private static ProcessorFieldsSettings CreateProcessorSettings(NamedObjectCollection<FieldSettingMapperDataForModelEdit> entity)
        {
            var processor = CreateFieldSetting(entity.FindByName(ProcessorFields.ProccesorName));

            var settings = new ProcessorFieldsSettings(processor);

            return settings;
        }

        private static MemoryFieldsSettings CreateMemorySettings(NamedObjectCollection<FieldSettingMapperDataForModelEdit> entity)
        {
            var memory = CreateFieldSetting(entity.FindByName(MemoryFields.RAM));

            var settings = new MemoryFieldsSettings(memory);

            return settings;
        }

        private static StorageFieldsSettings CreateStorageSettings(NamedObjectCollection<FieldSettingMapperDataForModelEdit> entity)
        {
            var memory = CreateFieldSetting(entity.FindByName(StorageFields.Capasity));

            var settings = new StorageFieldsSettings(memory);

            return settings;
        }

        private static CommunicationFieldsSettings CreateCommunicationSettings(NamedObjectCollection<FieldSettingMapperDataForModelEdit> entity)
        {
            var networkAdapter = CreateFieldSetting(entity.FindByName(CommunicationFields.NetworkAdapter));
            var ipAddress = CreateFieldSetting(entity.FindByName(CommunicationFields.IPAddress));
            var macAddress = CreateFieldSetting(entity.FindByName(CommunicationFields.MacAddress));

            var settings = new CommunicationFieldsSettings(
                networkAdapter,
                ipAddress,
                macAddress);

            return settings;
        }

        private static OtherFieldsSettings CreateOtherSettings(NamedObjectCollection<FieldSettingMapperDataForModelEdit> entity)
        {
            var info = CreateFieldSetting(entity.FindByName(OtherFields.Info));
            var driver = CreateFieldSetting(entity.FindByName(OtherFields.Other));
            var url = CreateFieldSetting(entity.FindByName(OtherFields.URL));
            var url2 = CreateFieldSetting(entity.FindByName(OtherFields.URL2));
            var owner = CreateFieldSetting(entity.FindByName(OtherFields.Owner));

            var settings = new OtherFieldsSettings(info, driver, url, url2, owner);

            return settings;
        }

        private static PlaceFieldsSettings CreatePlaceSettings(NamedObjectCollection<FieldSettingMapperDataForModelEdit> entity)
        {
            var room = CreateFieldSetting(entity.FindByName(PlaceFields.Room));
            var location = CreateFieldSetting(entity.FindByName(PlaceFields.Location));

            var settings = new PlaceFieldsSettings(room, location);

            return settings;
        }

        private static DocumentFieldsSettings CreateDocumentSettings(NamedObjectCollection<FieldSettingMapperDataForModelEdit> entity)
        {
            var document = CreateFieldSetting(entity.FindByName(DocumentFields.Document));

            var settings = new DocumentFieldsSettings(document);

            return settings;
        }

        private static StateFieldsSettings CreateStateSettings(NamedObjectCollection<FieldSettingMapperDataForModelEdit> entity)
        {
            var createdDate = CreateFieldSetting(entity.FindByName(StateFields.CreatedDate));
            var changedDate = CreateFieldSetting(entity.FindByName(StateFields.ChangedDate));
            var syncDate = CreateFieldSetting(entity.FindByName(StateFields.SyncChangeDate));

            var settings = new StateFieldsSettings(createdDate, changedDate, syncDate);

            return settings;
        }

        private static ModelEditFieldSetting CreateFieldSetting(FieldSettingMapperDataForModelEdit fieldSetting)
        {
            return new ModelEditFieldSetting(fieldSetting.Show.ToBool(), fieldSetting.Caption, fieldSetting.Required.ToBool(), fieldSetting.ReadOnly.ToBool());
        }
    }
}