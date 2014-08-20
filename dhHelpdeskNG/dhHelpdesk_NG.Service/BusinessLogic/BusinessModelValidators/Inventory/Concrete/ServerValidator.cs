namespace DH.Helpdesk.Services.BusinessLogic.BusinessModelValidators.Inventory.Concrete
{
    using DH.Helpdesk.BusinessData.Models.Inventory.Edit.Server;
    using DH.Helpdesk.BusinessData.Models.Inventory.Edit.Shared;
    using DH.Helpdesk.BusinessData.Models.Inventory.Output.Settings.ProcessingSetttings;
    using DH.Helpdesk.BusinessData.Models.Inventory.Output.Settings.ProcessingSetttings.ServerSettings;
    using DH.Helpdesk.BusinessData.Models.Inventory.Output.Settings.ProcessingSetttings.SharedSettings;
    using DH.Helpdesk.Services.BusinessLogic.BusinessModelValidators.Common;

    public class ServerValidator : IServerValidator
    {
        private readonly IElementaryRulesValidator elementaryRulesValidator;

        public ServerValidator(IElementaryRulesValidator elementaryRulesValidator)
        {
            this.elementaryRulesValidator = elementaryRulesValidator;
        }

        public void Validate(Server updatedServer, Server existingServer, ServerFieldsSettingsProcessing settings)
        {
            this.ValidateGeneral(updatedServer.GeneralFields, existingServer.GeneralFields, settings.GeneralFieldsSettings);
            this.ValidateChassis(updatedServer.ChassisFields, existingServer.ChassisFields, settings.ChassisFieldsSettings);
            this.ValidateInventoring(updatedServer.InventoryFields, existingServer.InventoryFields, settings.InventoryFieldsSettings);
            this.ValidateOperatingSystem(updatedServer.OperatingSystemFields, existingServer.OperatingSystemFields, settings.OperatingSystemFieldsSettings);
            this.ValidateProcessor(updatedServer.ProccesorFields, existingServer.ProccesorFields, settings.ProccesorFieldsSettings);
            this.ValidateMemory(updatedServer.MemoryFields, existingServer.MemoryFields, settings.MemoryFieldsSettings);
            this.ValidateStorage(updatedServer.StorageFields, existingServer.StorageFields, settings.StorageFieldsSettings);
            this.ValidateCommunication(updatedServer.CommunicationFields, existingServer.CommunicationFields, settings.CommunicationFieldsSettings);
            this.ValidateOther(updatedServer.OtherFields, existingServer.OtherFields, settings.OtherFieldsSettings);
            this.ValidatePlace(updatedServer.PlaceFields, existingServer.PlaceFields, settings.PlaceFieldsSettings);
        }

        public void Validate(Server newServer, ServerFieldsSettingsProcessing settings)
        {
            this.ValidateGeneral(newServer.GeneralFields, settings.GeneralFieldsSettings);
            this.ValidateChassis(newServer.ChassisFields, settings.ChassisFieldsSettings);
            this.ValidateInventoring(newServer.InventoryFields, settings.InventoryFieldsSettings);
            this.ValidateOperatingSystem(newServer.OperatingSystemFields, settings.OperatingSystemFieldsSettings);
            this.ValidateProcessor(newServer.ProccesorFields, settings.ProccesorFieldsSettings);
            this.ValidateMemory(newServer.MemoryFields, settings.MemoryFieldsSettings);
            this.ValidateStorage(newServer.StorageFields, settings.StorageFieldsSettings);
            this.ValidateCommunication(newServer.CommunicationFields, settings.CommunicationFieldsSettings);
            this.ValidateOther(newServer.OtherFields, settings.OtherFieldsSettings);
            this.ValidatePlace(newServer.PlaceFields, settings.PlaceFieldsSettings);
        }

        private void ValidateGeneral(
            GeneralFields updated,
            GeneralFields existing,
            GeneralFieldsSettings updatedSettings)
        {
            this.elementaryRulesValidator.ValidateStringField(
                updated.Name,
                existing.Name,
                BusinessData.Enums.Inventory.Fields.Server.GeneralFields.Name,
                this.CreateValidationRule(updatedSettings.ServerNameFieldSetting));
            this.elementaryRulesValidator.ValidateStringField(
                updated.Manufacturer,
                existing.Manufacturer,
                BusinessData.Enums.Inventory.Fields.Server.GeneralFields.Manufacturer,
                this.CreateValidationRule(updatedSettings.ManufacturerFieldSetting));
            this.elementaryRulesValidator.ValidateStringField(
                updated.Model,
                existing.Model,
                BusinessData.Enums.Inventory.Fields.Server.GeneralFields.Model,
                this.CreateValidationRule(updatedSettings.ComputerModelFieldSetting));
            this.elementaryRulesValidator.ValidateStringField(
                updated.SerialNumber,
                existing.SerialNumber,
                BusinessData.Enums.Inventory.Fields.Server.GeneralFields.SerialNumber,
                this.CreateValidationRule(updatedSettings.SerialNumberFieldSetting));
            this.elementaryRulesValidator.ValidateStringField(
                updated.Description,
                existing.Description,
                BusinessData.Enums.Inventory.Fields.Server.GeneralFields.Description,
                this.CreateValidationRule(updatedSettings.DescriptionFieldSetting));
        }

        private void ValidateChassis(
            ChassisFields updated,
            ChassisFields existing,
            ChassisFieldsSettings updatedSettings)
        {
            this.elementaryRulesValidator.ValidateStringField(
                updated.Chassis,
                existing.Chassis,
                BusinessData.Enums.Inventory.Fields.Shared.ChassisFields.Chassis,
                this.CreateValidationRule(updatedSettings.ChassisFieldSetting));
        }

        private void ValidateInventoring(
            InventoryFields updated,
            InventoryFields existing,
            InventoryFieldsSettings updatedSettings)
        {
            this.elementaryRulesValidator.ValidateStringField(
                updated.BarCode,
                existing.BarCode,
                BusinessData.Enums.Inventory.Fields.Shared.InventoryFields.BarCode,
                this.CreateValidationRule(updatedSettings.BarCodeFieldSetting));
            this.elementaryRulesValidator.ValidateDateTimeField(
                updated.PurchaseDate,
                existing.PurchaseDate,
                BusinessData.Enums.Inventory.Fields.Shared.InventoryFields.PurchaseDate,
                this.CreateValidationRule(updatedSettings.PurchaseDateFieldSetting));
        }

        private void ValidateOperatingSystem(
            OperatingSystemFields updated,
            OperatingSystemFields existing,
            OperatingSystemFieldsSettings updatedSettings)
        {
            this.elementaryRulesValidator.ValidateIntegerField(
                updated.OperatingSystemId,
                existing.OperatingSystemId,
                BusinessData.Enums.Inventory.Fields.Shared.OperatingSystemFields.OperatingSystem,
                this.CreateValidationRule(updatedSettings.OperatingSystemFieldSetting));
            this.elementaryRulesValidator.ValidateStringField(
                updated.Version,
                existing.Version,
                BusinessData.Enums.Inventory.Fields.Shared.OperatingSystemFields.Version,
                this.CreateValidationRule(updatedSettings.VersionFieldSetting));
            this.elementaryRulesValidator.ValidateStringField(
                updated.ServicePack,
                existing.ServicePack,
                BusinessData.Enums.Inventory.Fields.Shared.OperatingSystemFields.ServicePack,
                this.CreateValidationRule(updatedSettings.ServicePackSystemFieldSetting));
            this.elementaryRulesValidator.ValidateStringField(
                updated.RegistrationCode,
                existing.RegistrationCode,
                BusinessData.Enums.Inventory.Fields.Shared.OperatingSystemFields.RegistrationCode,
                this.CreateValidationRule(updatedSettings.RegistrationCodeSystemFieldSetting));
            this.elementaryRulesValidator.ValidateStringField(
                updated.ProductKey,
                existing.ProductKey,
                BusinessData.Enums.Inventory.Fields.Shared.OperatingSystemFields.ProductKey,
                this.CreateValidationRule(updatedSettings.ProductKeyFieldSetting));
        }

        private void ValidateProcessor(
            ProcessorFields updated,
            ProcessorFields existing,
            ProcessorFieldsSettings updatedSettings)
        {
            this.elementaryRulesValidator.ValidateIntegerField(
                updated.ProccesorId,
                existing.ProccesorId,
                BusinessData.Enums.Inventory.Fields.Shared.ProcessorFields.ProccesorName,
                this.CreateValidationRule(updatedSettings.ProccesorFieldSetting));
        }

        private void ValidateMemory(MemoryFields updated, MemoryFields existing, MemoryFieldsSettings updatedSettings)
        {
            this.elementaryRulesValidator.ValidateIntegerField(
                updated.RAMId,
                existing.RAMId,
                BusinessData.Enums.Inventory.Fields.Shared.MemoryFields.RAM,
                this.CreateValidationRule(updatedSettings.RAMFieldSetting));
        }

        private void ValidateStorage(StorageFields updated, StorageFields existing, StorageFieldsSettings updatedSettings)
        {
            this.elementaryRulesValidator.ValidateStringField(
                updated.Capasity,
                existing.Capasity,
                BusinessData.Enums.Inventory.Fields.Server.StorageFields.Capasity,
                this.CreateValidationRule(updatedSettings.CapasityFieldSetting));
        }

        private void ValidateCommunication(
            CommunicationFields updated,
            CommunicationFields existing,
            CommunicationFieldsSettings updatedSettings)
        {
            this.elementaryRulesValidator.ValidateIntegerField(
                updated.NetworkAdapterId,
                existing.NetworkAdapterId,
                BusinessData.Enums.Inventory.Fields.Server.CommunicationFields.NetworkAdapter,
                this.CreateValidationRule(updatedSettings.NetworkAdapterFieldSetting));
            this.elementaryRulesValidator.ValidateStringField(
                updated.IPAddress,
                existing.IPAddress,
                BusinessData.Enums.Inventory.Fields.Server.CommunicationFields.IPAddress,
                this.CreateValidationRule(updatedSettings.IPAddressFieldSetting));
            this.elementaryRulesValidator.ValidateStringField(
                updated.MacAddress,
                existing.MacAddress,
                BusinessData.Enums.Inventory.Fields.Server.CommunicationFields.MacAddress,
                this.CreateValidationRule(updatedSettings.MacAddressFieldSetting));
        }

        private void ValidateOther(OtherFields updated, OtherFields existing, OtherFieldsSettings updatedSettings)
        {
            this.elementaryRulesValidator.ValidateStringField(
                updated.Info,
                existing.Info,
                BusinessData.Enums.Inventory.Fields.Server.OtherFields.Info,
                this.CreateValidationRule(updatedSettings.InfoFieldSetting));
            this.elementaryRulesValidator.ValidateStringField(
                updated.Other,
                existing.Other,
                BusinessData.Enums.Inventory.Fields.Server.OtherFields.Other,
                this.CreateValidationRule(updatedSettings.OtherFieldSetting));
            this.elementaryRulesValidator.ValidateStringField(
                updated.URL,
                existing.URL,
                BusinessData.Enums.Inventory.Fields.Server.OtherFields.URL,
                this.CreateValidationRule(updatedSettings.URLFieldSetting));
            this.elementaryRulesValidator.ValidateStringField(
                updated.URL2,
                existing.URL2,
                BusinessData.Enums.Inventory.Fields.Server.OtherFields.URL2,
                this.CreateValidationRule(updatedSettings.URL2FieldSetting));
            this.elementaryRulesValidator.ValidateStringField(
                updated.Owner,
                existing.Owner,
                BusinessData.Enums.Inventory.Fields.Server.OtherFields.Owner,
                this.CreateValidationRule(updatedSettings.OwnerFieldSetting));
        }

        private void ValidatePlace(PlaceFields updated, PlaceFields existing, PlaceFieldsSettings updatedSettings)
        {
            this.elementaryRulesValidator.ValidateIntegerField(
                updated.RoomId,
                existing.RoomId,
                BusinessData.Enums.Inventory.Fields.Shared.PlaceFields.Room,
                this.CreateValidationRule(updatedSettings.RoomFieldSetting));
            this.elementaryRulesValidator.ValidateStringField(
                updated.Location,
                existing.Location,
                BusinessData.Enums.Inventory.Fields.Shared.PlaceFields.Location,
                this.CreateValidationRule(updatedSettings.LocationFieldSetting));
        }

        private void ValidateGeneral(GeneralFields updated, GeneralFieldsSettings updatedSettings)
        {
            this.elementaryRulesValidator.ValidateStringField(
                updated.Name,
                BusinessData.Enums.Inventory.Fields.Server.GeneralFields.Name,
                this.CreateValidationRule(updatedSettings.ServerNameFieldSetting));
            this.elementaryRulesValidator.ValidateStringField(
                updated.Manufacturer,
                BusinessData.Enums.Inventory.Fields.Server.GeneralFields.Manufacturer,
                this.CreateValidationRule(updatedSettings.ManufacturerFieldSetting));
            this.elementaryRulesValidator.ValidateStringField(
                updated.Model,
                BusinessData.Enums.Inventory.Fields.Server.GeneralFields.Model,
                this.CreateValidationRule(updatedSettings.ComputerModelFieldSetting));
            this.elementaryRulesValidator.ValidateStringField(
                updated.SerialNumber,
                BusinessData.Enums.Inventory.Fields.Server.GeneralFields.SerialNumber,
                this.CreateValidationRule(updatedSettings.SerialNumberFieldSetting));
            this.elementaryRulesValidator.ValidateStringField(
                updated.Description,
                BusinessData.Enums.Inventory.Fields.Server.GeneralFields.Description,
                this.CreateValidationRule(updatedSettings.DescriptionFieldSetting));
        }

        private void ValidateChassis(ChassisFields updated, ChassisFieldsSettings updatedSettings)
        {
            this.elementaryRulesValidator.ValidateStringField(
                updated.Chassis,
                BusinessData.Enums.Inventory.Fields.Shared.ChassisFields.Chassis,
                this.CreateValidationRule(updatedSettings.ChassisFieldSetting));
        }

        private void ValidateInventoring(InventoryFields updated, InventoryFieldsSettings updatedSettings)
        {
            this.elementaryRulesValidator.ValidateStringField(
                updated.BarCode,
                BusinessData.Enums.Inventory.Fields.Shared.InventoryFields.BarCode,
                this.CreateValidationRule(updatedSettings.BarCodeFieldSetting));
            this.elementaryRulesValidator.ValidateDateTimeField(
                updated.PurchaseDate,
                BusinessData.Enums.Inventory.Fields.Shared.InventoryFields.PurchaseDate,
                this.CreateValidationRule(updatedSettings.PurchaseDateFieldSetting));
        }

        private void ValidateOperatingSystem(
            OperatingSystemFields updated,
            OperatingSystemFieldsSettings updatedSettings)
        {
            this.elementaryRulesValidator.ValidateIntegerField(
                updated.OperatingSystemId,
                BusinessData.Enums.Inventory.Fields.Shared.OperatingSystemFields.OperatingSystem,
                this.CreateValidationRule(updatedSettings.OperatingSystemFieldSetting));
            this.elementaryRulesValidator.ValidateStringField(
                updated.Version,
                BusinessData.Enums.Inventory.Fields.Shared.OperatingSystemFields.Version,
                this.CreateValidationRule(updatedSettings.VersionFieldSetting));
            this.elementaryRulesValidator.ValidateStringField(
                updated.ServicePack,
                BusinessData.Enums.Inventory.Fields.Shared.OperatingSystemFields.ServicePack,
                this.CreateValidationRule(updatedSettings.ServicePackSystemFieldSetting));
            this.elementaryRulesValidator.ValidateStringField(
                updated.RegistrationCode,
                BusinessData.Enums.Inventory.Fields.Shared.OperatingSystemFields.RegistrationCode,
                this.CreateValidationRule(updatedSettings.RegistrationCodeSystemFieldSetting));
            this.elementaryRulesValidator.ValidateStringField(
                updated.ProductKey,
                BusinessData.Enums.Inventory.Fields.Shared.OperatingSystemFields.ProductKey,
                this.CreateValidationRule(updatedSettings.ProductKeyFieldSetting));
        }

        private void ValidateProcessor(ProcessorFields updated, ProcessorFieldsSettings updatedSettings)
        {
            this.elementaryRulesValidator.ValidateIntegerField(
                updated.ProccesorId,
                BusinessData.Enums.Inventory.Fields.Shared.ProcessorFields.ProccesorName,
                this.CreateValidationRule(updatedSettings.ProccesorFieldSetting));
        }

        private void ValidateMemory(MemoryFields updated, MemoryFieldsSettings updatedSettings)
        {
            this.elementaryRulesValidator.ValidateIntegerField(
                updated.RAMId,
                BusinessData.Enums.Inventory.Fields.Shared.MemoryFields.RAM,
                this.CreateValidationRule(updatedSettings.RAMFieldSetting));
        }

        private void ValidateStorage(StorageFields updated, StorageFieldsSettings updatedSettings)
        {
            this.elementaryRulesValidator.ValidateStringField(
                updated.Capasity,
                BusinessData.Enums.Inventory.Fields.Server.StorageFields.Capasity,
                this.CreateValidationRule(updatedSettings.CapasityFieldSetting));
        }

        private void ValidateCommunication(CommunicationFields updated, CommunicationFieldsSettings updatedSettings)
        {
            this.elementaryRulesValidator.ValidateIntegerField(
                updated.NetworkAdapterId,
                BusinessData.Enums.Inventory.Fields.Server.CommunicationFields.NetworkAdapter,
                this.CreateValidationRule(updatedSettings.NetworkAdapterFieldSetting));
            this.elementaryRulesValidator.ValidateStringField(
                updated.IPAddress,
                BusinessData.Enums.Inventory.Fields.Server.CommunicationFields.IPAddress,
                this.CreateValidationRule(updatedSettings.IPAddressFieldSetting));
            this.elementaryRulesValidator.ValidateStringField(
                updated.MacAddress,
                BusinessData.Enums.Inventory.Fields.Server.CommunicationFields.MacAddress,
                this.CreateValidationRule(updatedSettings.MacAddressFieldSetting));
        }

        private void ValidateOther(OtherFields updated, OtherFieldsSettings updatedSettings)
        {
            this.elementaryRulesValidator.ValidateStringField(
                updated.Info,
                BusinessData.Enums.Inventory.Fields.Server.OtherFields.Info,
                this.CreateValidationRule(updatedSettings.InfoFieldSetting));
            this.elementaryRulesValidator.ValidateStringField(
                updated.Other,
                BusinessData.Enums.Inventory.Fields.Server.OtherFields.Other,
                this.CreateValidationRule(updatedSettings.OtherFieldSetting));
            this.elementaryRulesValidator.ValidateStringField(
                updated.URL,
                BusinessData.Enums.Inventory.Fields.Server.OtherFields.URL,
                this.CreateValidationRule(updatedSettings.URLFieldSetting));
            this.elementaryRulesValidator.ValidateStringField(
                updated.URL2,
                BusinessData.Enums.Inventory.Fields.Server.OtherFields.URL2,
                this.CreateValidationRule(updatedSettings.URL2FieldSetting));
            this.elementaryRulesValidator.ValidateStringField(
                updated.Owner,
                BusinessData.Enums.Inventory.Fields.Server.OtherFields.Owner,
                this.CreateValidationRule(updatedSettings.OwnerFieldSetting));
        }

        private void ValidatePlace(PlaceFields updated, PlaceFieldsSettings updatedSettings)
        {
            this.elementaryRulesValidator.ValidateIntegerField(
                updated.RoomId,
                BusinessData.Enums.Inventory.Fields.Shared.PlaceFields.Room,
                this.CreateValidationRule(updatedSettings.RoomFieldSetting));
            this.elementaryRulesValidator.ValidateStringField(
                updated.Location,
                BusinessData.Enums.Inventory.Fields.Shared.PlaceFields.Location,
                this.CreateValidationRule(updatedSettings.LocationFieldSetting));
        }

        private ElementaryValidationRule CreateValidationRule(ProcessingFieldSetting setting)
        {
            return new ElementaryValidationRule(!setting.IsShow, setting.IsRequired);
        }
    }
}