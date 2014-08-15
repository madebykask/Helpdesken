namespace DH.Helpdesk.Services.BusinessLogic.BusinessModelRestorers.Inventory
{
    using DH.Helpdesk.BusinessData.Models.Inventory.Edit.Server;
    using DH.Helpdesk.BusinessData.Models.Inventory.Edit.Shared;
    using DH.Helpdesk.BusinessData.Models.Inventory.Output.Settings.ProcessingSetttings;
    using DH.Helpdesk.BusinessData.Models.Inventory.Output.Settings.ProcessingSetttings.ServerSettings;
    using DH.Helpdesk.BusinessData.Models.Inventory.Output.Settings.ProcessingSetttings.SharedSettings;

    public interface IServerRestorer
    {
        void Restore(Server computer, Server existingComputer, ServerFieldsSettingsProcessing settings);
    }


    public class ServerRestorer : Restorer, IServerRestorer
    {
        public void Restore(Server computer, Server existingComputer, ServerFieldsSettingsProcessing settings)
        {
            //this.RestoreGeneral(
            //    computer.GeneralFields,
            //    existingComputer.WorkstationFields,
            //    settings.WorkstationFieldsSettings);
            //this.RestoreChassis(computer.ChassisFields, existingComputer.ChassisFields, settings.ChassisFieldsSettings);
            //this.RestoreInventoring(
            //    computer.InventoryFields,
            //    existingComputer.InventoryFields,
            //    settings.InventoryFieldsSettings);
            //this.RestoreOperatingSystem(
            //    computer.OperatingSystemFields,
            //    existingComputer.OperatingSystemFields,
            //    settings.OperatingSystemFieldsSettings);
            //this.RestoreProcessor(
            //    computer.ProccesorFields,
            //    existingComputer.ProccesorFields,
            //    settings.ProccesorFieldsSettings);
            //this.RestoreMemory(computer.MemoryFields, existingComputer.MemoryFields, settings.MemoryFieldsSettings);
            //this.RestoreCommunication(
            //    computer.CommunicationFields,
            //    existingComputer.CommunicationFields,
            //    settings.CommunicationFieldsSettings);
            //this.RestoreGraphics(
            //    computer.GraphicsFields,
            //    existingComputer.GraphicsFields,
            //    settings.GraphicsFieldsSettings);
            //this.RestoreSound(computer.SoundFields, existingComputer.SoundFields, settings.SoundFieldsSettings);
            //this.RestoreContract(
            //    computer.ContractFields,
            //    existingComputer.ContractFields,
            //    settings.ContractFieldsSettings);
            //this.RestoreOther(computer.OtherFields, existingComputer.OtherFields, settings.OtherFieldsSettings);
            //this.RestoreContactInformation(
            //    computer.ContactInformationFields,
            //    existingComputer.ContactInformationFields,
            //    settings.ContactInformationFieldsSettings);
            //this.RestoreOrganization(
            //    computer.OrganizationFields,
            //    existingComputer.OrganizationFields,
            //    settings.OrganizationFieldsSettings);
            //this.RestorePlace(computer.PlaceFields, existingComputer.PlaceFields, settings.PlaceFieldsSettings);
            //this.RestoreContact(computer.ContactFields, existingComputer.ContactFields, settings.ContactFieldsSettings);
            //this.RestoreState(computer.StateFields, existingComputer.StateFields, settings.StateFieldsSettings);
            //this.RestoreDate(computer.DateFields, existingComputer.DateFields, settings.DateFieldsSettings);
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