namespace DH.Helpdesk.Services.BusinessLogic.BusinessModelRestorers.Inventory.Concrete
{
    using DH.Helpdesk.BusinessData.Models.Inventory.Edit.Server;
    using DH.Helpdesk.BusinessData.Models.Inventory.Edit.Shared;
    using DH.Helpdesk.BusinessData.Models.Inventory.Output.Settings.ProcessingSetttings;
    using DH.Helpdesk.BusinessData.Models.Inventory.Output.Settings.ProcessingSetttings.ServerSettings;
    using DH.Helpdesk.BusinessData.Models.Inventory.Output.Settings.ProcessingSetttings.SharedSettings;

    public class ServerRestorer : Restorer, IServerRestorer
    {
        public void Restore(Server server, Server existingServer, ServerFieldsSettingsProcessing settings)
        {
            this.RestoreGeneral(
                server.GeneralFields,
                existingServer.GeneralFields,
                settings.GeneralFieldsSettings);
            this.RestoreChassis(server.ChassisFields, existingServer.ChassisFields, settings.ChassisFieldsSettings);
            this.RestoreInventoring(
                server.InventoryFields,
                existingServer.InventoryFields,
                settings.InventoryFieldsSettings);
            this.RestoreOperatingSystem(
                server.OperatingSystemFields,
                existingServer.OperatingSystemFields,
                settings.OperatingSystemFieldsSettings);
            this.RestoreProcessor(
                server.ProccesorFields,
                existingServer.ProccesorFields,
                settings.ProccesorFieldsSettings);
            this.RestoreMemory(server.MemoryFields, existingServer.MemoryFields, settings.MemoryFieldsSettings);
            this.RestoreStorage(server.StorageFields, existingServer.StorageFields, settings.StorageFieldsSettings);
            this.RestoreCommunication(
                server.CommunicationFields,
                existingServer.CommunicationFields,
                settings.CommunicationFieldsSettings);
            this.RestoreOther(server.OtherFields, existingServer.OtherFields, settings.OtherFieldsSettings);
            this.RestorePlace(server.PlaceFields, existingServer.PlaceFields, settings.PlaceFieldsSettings);
        }

        private void RestoreGeneral(
            GeneralFields updated,
            GeneralFields existing,
            GeneralFieldsSettings updatedSettings)
        {
            this.RestoreFieldIfNeeded(
                updated,
                () => updated.Name,
                existing.Name,
                this.CreateValidationRule(updatedSettings.ServerNameFieldSetting));
            this.RestoreFieldIfNeeded(
                updated,
                () => updated.Manufacturer,
                existing.Manufacturer,
                this.CreateValidationRule(updatedSettings.ManufacturerFieldSetting));
            this.RestoreFieldIfNeeded(
                updated,
                () => updated.Model,
                existing.Model,
                this.CreateValidationRule(updatedSettings.ComputerModelFieldSetting));
            this.RestoreFieldIfNeeded(
                updated,
                () => updated.SerialNumber,
                existing.SerialNumber,
                this.CreateValidationRule(updatedSettings.SerialNumberFieldSetting));
            this.RestoreFieldIfNeeded(
                updated,
                () => updated.Description,
                existing.Description,
                this.CreateValidationRule(updatedSettings.DescriptionFieldSetting));
        }

        private void RestoreChassis(
            ChassisFields updated,
            ChassisFields existing,
            ChassisFieldsSettings updatedSettings)
        {
            this.RestoreFieldIfNeeded(
                updated,
                () => updated.Chassis,
                existing.Chassis,
                this.CreateValidationRule(updatedSettings.ChassisFieldSetting));
        }

        private void RestoreInventoring(
            InventoryFields updated,
            InventoryFields existing,
            InventoryFieldsSettings updatedSettings)
        {
            this.RestoreFieldIfNeeded(
                updated,
                () => updated.BarCode,
                existing.BarCode,
                this.CreateValidationRule(updatedSettings.BarCodeFieldSetting));
            this.RestoreFieldIfNeeded(
                updated,
                () => updated.PurchaseDate,
                existing.PurchaseDate,
                this.CreateValidationRule(updatedSettings.PurchaseDateFieldSetting));
        }

        private void RestoreOperatingSystem(
            OperatingSystemFields updated,
            OperatingSystemFields existing,
            OperatingSystemFieldsSettings updatedSettings)
        {
            this.RestoreFieldIfNeeded(
                updated,
                () => updated.OperatingSystemId,
                existing.OperatingSystemId,
                this.CreateValidationRule(updatedSettings.OperatingSystemFieldSetting));
            this.RestoreFieldIfNeeded(
                updated,
                () => updated.Version,
                existing.Version,
                this.CreateValidationRule(updatedSettings.VersionFieldSetting));
            this.RestoreFieldIfNeeded(
                updated,
                () => updated.ServicePack,
                existing.ServicePack,
                this.CreateValidationRule(updatedSettings.ServicePackSystemFieldSetting));
            this.RestoreFieldIfNeeded(
                updated,
                () => updated.RegistrationCode,
                existing.RegistrationCode,
                this.CreateValidationRule(updatedSettings.RegistrationCodeSystemFieldSetting));
            this.RestoreFieldIfNeeded(
                updated,
                () => updated.ProductKey,
                existing.ProductKey,
                this.CreateValidationRule(updatedSettings.ProductKeyFieldSetting));
        }

        private void RestoreProcessor(
            ProcessorFields updated,
            ProcessorFields existing,
            ProcessorFieldsSettings updatedSettings)
        {
            this.RestoreFieldIfNeeded(
                updated,
                () => updated.ProccesorId,
                existing.ProccesorId,
                this.CreateValidationRule(updatedSettings.ProccesorFieldSetting));
        }

        private void RestoreMemory(MemoryFields updated, MemoryFields existing, MemoryFieldsSettings updatedSettings)
        {
            this.RestoreFieldIfNeeded(
                updated,
                () => updated.RAMId,
                existing.RAMId,
                this.CreateValidationRule(updatedSettings.RAMFieldSetting));
        }

        private void RestoreStorage(StorageFields updated, StorageFields existing, StorageFieldsSettings updatedSettings)
        {
            this.RestoreFieldIfNeeded(
                updated,
                () => updated.Capasity,
                existing.Capasity,
                this.CreateValidationRule(updatedSettings.CapasityFieldSetting));
        }

        private void RestoreCommunication(
            CommunicationFields updated,
            CommunicationFields existing,
            CommunicationFieldsSettings updatedSettings)
        {
            this.RestoreFieldIfNeeded(
                updated,
                () => updated.NetworkAdapterId,
                existing.NetworkAdapterId,
                this.CreateValidationRule(updatedSettings.NetworkAdapterFieldSetting));
            this.RestoreFieldIfNeeded(
                updated,
                () => updated.IPAddress,
                existing.IPAddress,
                this.CreateValidationRule(updatedSettings.IPAddressFieldSetting));
            this.RestoreFieldIfNeeded(
                updated,
                () => updated.MacAddress,
                existing.MacAddress,
                this.CreateValidationRule(updatedSettings.MacAddressFieldSetting));
        }

        private void RestoreOther(OtherFields updated, OtherFields existing, OtherFieldsSettings updatedSettings)
        {
            this.RestoreFieldIfNeeded(
                updated,
                () => updated.Info,
                existing.Info,
                this.CreateValidationRule(updatedSettings.InfoFieldSetting));
        }

        private void RestorePlace(PlaceFields updated, PlaceFields existing, PlaceFieldsSettings updatedSettings)
        {
            this.RestoreFieldIfNeeded(
                updated,
                () => updated.RoomId,
                existing.RoomId,
                this.CreateValidationRule(updatedSettings.RoomFieldSetting));
            this.RestoreFieldIfNeeded(
                updated,
                () => updated.Location,
                existing.Location,
                this.CreateValidationRule(updatedSettings.LocationFieldSetting));
        }

        private bool CreateValidationRule(ProcessingFieldSetting setting)
        {
            return setting.IsShow;
        }
    }
}