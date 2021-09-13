namespace DH.Helpdesk.Services.BusinessLogic.BusinessModelRestorers.Inventory.Concrete
{
    using DH.Helpdesk.BusinessData.Models.Inventory.Edit.Computer;
    using DH.Helpdesk.BusinessData.Models.Inventory.Edit.Shared;
    using DH.Helpdesk.BusinessData.Models.Inventory.Output.Settings.ProcessingSetttings.ComputerSettings;

    using PlaceFields = DH.Helpdesk.BusinessData.Models.Inventory.Edit.Computer.PlaceFields;

    public class ComputerRestorer : Restorer, IComputerRestorer
    {
        public void Restore(ComputerForUpdate computer, ComputerForRead existingComputer, ComputerFieldsSettingsProcessing settings)
        {
            this.RestoreWorkstation(
                computer.WorkstationFields,
                existingComputer.WorkstationFields,
                settings.WorkstationFieldsSettings);
            this.RestoreChassis(computer.ChassisFields, existingComputer.ChassisFields, settings.ChassisFieldsSettings);
            this.RestoreInventoring(
                computer.InventoryFields,
                existingComputer.InventoryFields,
                settings.InventoryFieldsSettings);
            this.RestoreOperatingSystem(
                computer.OperatingSystemFields,
                existingComputer.OperatingSystemFields,
                settings.OperatingSystemFieldsSettings);
            this.RestoreProcessor(
                computer.ProccesorFields,
                existingComputer.ProccesorFields,
                settings.ProccesorFieldsSettings);
            this.RestoreMemory(computer.MemoryFields, existingComputer.MemoryFields, settings.MemoryFieldsSettings);
            this.RestoreCommunication(
                computer.CommunicationFields,
                existingComputer.CommunicationFields,
                settings.CommunicationFieldsSettings);
            this.RestoreGraphics(
                computer.GraphicsFields,
                existingComputer.GraphicsFields,
                settings.GraphicsFieldsSettings);
            this.RestoreSound(computer.SoundFields, existingComputer.SoundFields, settings.SoundFieldsSettings);
            this.RestoreContract(
                computer.ContractFields,
                existingComputer.ContractFields,
                settings.ContractFieldsSettings);
            this.RestoreOther(computer.OtherFields, existingComputer.OtherFields, settings.OtherFieldsSettings);
            this.RestoreContactInformation(
                computer.ContactInformationFields,
                existingComputer.ContactInformationFields,
                settings.ContactInformationFieldsSettings);
            this.RestoreOrganization(
                computer.OrganizationFields,
                existingComputer.OrganizationFields,
                settings.OrganizationFieldsSettings);
            this.RestorePlace(computer.PlaceFields, existingComputer.PlaceFields, settings.PlaceFieldsSettings);
            this.RestoreContact(computer.ContactFields, existingComputer.ContactFields, settings.ContactFieldsSettings);
            this.RestoreState(computer.StateFields, existingComputer.StateFields, settings.StateFieldsSettings);
            
            // this.RestoreDate(computer.DateFields, existingComputer.DateFields, settings.DateFieldsSettings);
        }

        private void RestoreWorkstation(
            WorkstationFields updated,
            WorkstationFields existing,
            WorkstationFieldsSettings updatedSettings)
        {
            this.RestoreFieldIfNeeded(
                updated,
                () => updated.ComputerName,
                existing.ComputerName,
                this.CreateValidationRule(updatedSettings.ComputerNameFieldSetting));
            this.RestoreFieldIfNeeded(
                updated,
                () => updated.Manufacturer,
                existing.Manufacturer,
                this.CreateValidationRule(updatedSettings.ManufacturerFieldSetting));
            this.RestoreFieldIfNeeded(
                updated,
                () => updated.ComputerModelId,
                existing.ComputerModelId,
                this.CreateValidationRule(updatedSettings.ComputerModelFieldSetting));
            this.RestoreFieldIfNeeded(
                updated,
                () => updated.SerialNumber,
                existing.SerialNumber,
                this.CreateValidationRule(updatedSettings.SerialNumberFieldSetting));
            this.RestoreFieldIfNeeded(
                updated,
                () => updated.BIOSDate,
                existing.BIOSDate,
                this.CreateValidationRule(updatedSettings.BIOSDateFieldSetting));
            this.RestoreFieldIfNeeded(
                updated,
                () => updated.BIOSVersion,
                existing.BIOSVersion,
                this.CreateValidationRule(updatedSettings.BIOSVersionFieldSetting));
            this.RestoreFieldIfNeeded(
                updated,
                () => updated.Theftmark,
                existing.Theftmark,
                this.CreateValidationRule(updatedSettings.TheftmarkFieldSetting));
            this.RestoreFieldIfNeeded(
                updated,
                () => updated.CarePackNumber,
                existing.CarePackNumber,
                this.CreateValidationRule(updatedSettings.CarePackNumberFieldSetting));
            this.RestoreFieldIfNeeded(
                updated,
                () => updated.ComputerTypeId,
                existing.ComputerTypeId,
                this.CreateValidationRule(updatedSettings.ComputerTypeFieldSetting));
            this.RestoreFieldIfNeeded(
                updated,
                () => updated.Location,
                existing.Location,
                this.CreateValidationRule(updatedSettings.LocationFieldSetting));
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
            this.RestoreFieldIfNeeded(
                updated,
                () => updated.IsRAS,
                existing.IsRAS,
                this.CreateValidationRule(updatedSettings.RASFieldSetting));
            this.RestoreFieldIfNeeded(
                updated,
                () => updated.NovellClient,
                existing.NovellClient,
                this.CreateValidationRule(updatedSettings.NovellClientFieldSetting));
        }

        private void RestoreGraphics(
            GraphicsFields updated,
            GraphicsFields existing,
            GraphicsFieldsSettings updatedSettings)
        {
            this.RestoreFieldIfNeeded(
                updated,
                () => updated.VideoCard,
                existing.VideoCard,
                this.CreateValidationRule(updatedSettings.VideoCardFieldSetting));
        }

        private void RestoreSound(SoundFields updated, SoundFields existing, SoundFieldsSettings updatedSettings)
        {
            this.RestoreFieldIfNeeded(
                updated,
                () => updated.SoundCard,
                existing.SoundCard,
                this.CreateValidationRule(updatedSettings.SoundCardFieldSetting));
        }

        private void RestoreContract(
            ContractFields updated,
            ContractFields existing,
            ContractFieldsSettings updatedSettings)
        {
            this.RestoreFieldIfNeeded(
                updated,
                () => updated.ContractStatusId,
                existing.ContractStatusId,
                this.CreateValidationRule(updatedSettings.ContractStatusFieldSetting));
            this.RestoreFieldIfNeeded(
                updated,
                () => updated.ContractNumber,
                existing.ContractNumber,
                this.CreateValidationRule(updatedSettings.ContractNumberFieldSetting));
            this.RestoreFieldIfNeeded(
                updated,
                () => updated.ContractStartDate,
                existing.ContractStartDate,
                this.CreateValidationRule(updatedSettings.ContractStartDateFieldSetting));
            this.RestoreFieldIfNeeded(
                updated,
                () => updated.ContractEndDate,
                existing.ContractEndDate,
                this.CreateValidationRule(updatedSettings.ContractEndDateFieldSetting));
            this.RestoreFieldIfNeeded(
                updated,
                () => updated.PurchasePrice,
                existing.PurchasePrice,
                this.CreateValidationRule(updatedSettings.PurchasePriceFieldSetting));
            this.RestoreFieldIfNeeded(
                updated,
                () => updated.AccountingDimension1,
                existing.AccountingDimension1,
                this.CreateValidationRule(updatedSettings.AccountingDimension1FieldSetting));
            this.RestoreFieldIfNeeded(
                updated,
                () => updated.AccountingDimension2,
                existing.AccountingDimension2,
                this.CreateValidationRule(updatedSettings.AccountingDimension2FieldSetting));
            this.RestoreFieldIfNeeded(
                updated,
                () => updated.AccountingDimension3,
                existing.AccountingDimension3,
                this.CreateValidationRule(updatedSettings.AccountingDimension3FieldSetting));
            this.RestoreFieldIfNeeded(
                updated,
                () => updated.AccountingDimension4,
                existing.AccountingDimension4,
                this.CreateValidationRule(updatedSettings.AccountingDimension4FieldSetting));
            this.RestoreFieldIfNeeded(
                updated,
                () => updated.AccountingDimension5,
                existing.AccountingDimension5,
                this.CreateValidationRule(updatedSettings.AccountingDimension5FieldSetting));
        }

        private void RestoreOther(OtherFields updated, OtherFields existing, OtherFieldsSettings updatedSettings)
        {
            this.RestoreFieldIfNeeded(
                updated,
                () => updated.Info,
                existing.Info,
                this.CreateValidationRule(updatedSettings.InfoFieldSetting));
        }

        private void RestoreContactInformation(
            ContactInformationFields updated,
            ContactInformationFields existing,
            ContactInformationFieldsSettings updatedSettings)
        {
            this.RestoreFieldIfNeeded(
                updated,
                () => updated.UserId,
                existing.UserId,
                this.CreateValidationRule(updatedSettings.UserIdFieldSetting));
        }

        private void RestoreOrganization(
            OrganizationFields updated,
            OrganizationFields existing,
            OrganizationFieldsSettings updatedSettings)
        {
            this.RestoreFieldIfNeeded(
                updated,
                () => updated.RegionId,
                existing.RegionId,
                this.CreateValidationRule(updatedSettings.RegionFieldSetting));
            this.RestoreFieldIfNeeded(
                updated,
                () => updated.DepartmentId,
                existing.DepartmentId,
                this.CreateValidationRule(updatedSettings.DepartmentFieldSetting));
            this.RestoreFieldIfNeeded(
                updated,
                () => updated.DomainId,
                existing.DomainId,
                this.CreateValidationRule(updatedSettings.DomainFieldSetting));
            this.RestoreFieldIfNeeded(
                updated,
                () => updated.UnitId,
                existing.UnitId,
                this.CreateValidationRule(updatedSettings.UnitFieldSetting));
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
                () => updated.BuildingId,
                existing.BuildingId,
                this.CreateValidationRule(updatedSettings.BuildingFieldSetting));
            this.RestoreFieldIfNeeded(
                updated,
                () => updated.FloorId,
                existing.FloorId,
                this.CreateValidationRule(updatedSettings.FloorFieldSetting));
            this.RestoreFieldIfNeeded(
                updated,
                () => updated.Address,
                existing.Address,
                this.CreateValidationRule(updatedSettings.AddressFieldSetting));
            this.RestoreFieldIfNeeded(
                updated,
                () => updated.PostalCode,
                existing.PostalCode,
                this.CreateValidationRule(updatedSettings.PostalAddressFieldSetting));
            this.RestoreFieldIfNeeded(
                updated,
                () => updated.PostalAddress,
                existing.PostalAddress,
                this.CreateValidationRule(updatedSettings.PostalAddressFieldSetting));
            this.RestoreFieldIfNeeded(
                updated,
                () => updated.Location,
                existing.Location,
                this.CreateValidationRule(updatedSettings.PlaceFieldSetting));
            this.RestoreFieldIfNeeded(
                updated,
                () => updated.Location2,
                existing.Location2,
                this.CreateValidationRule(updatedSettings.Place2FieldSetting));
        }

        private void RestoreContact(
            ContactFields updated,
            ContactFields existing,
            ContactFieldsSettings updatedSettings)
        {
            this.RestoreFieldIfNeeded(
                updated,
                () => updated.Name,
                existing.Name,
                this.CreateValidationRule(updatedSettings.NameFieldSetting));
            this.RestoreFieldIfNeeded(
                updated,
                () => updated.Phone,
                existing.Phone,
                this.CreateValidationRule(updatedSettings.PhoneFieldSetting));
            this.RestoreFieldIfNeeded(
                updated,
                () => updated.Email,
                existing.Email,
                this.CreateValidationRule(updatedSettings.EmailFieldSetting));
        }

        private void RestoreState(StateFields updated, StateFields existing, StateFieldsSettings updatedSettings)
        {
            this.RestoreFieldIfNeeded(
                updated,
                () => updated.State,
                existing.State,
                this.CreateValidationRule(updatedSettings.StateFieldSetting));
            this.RestoreFieldIfNeeded(
                updated,
                () => updated.IsStolen,
                existing.IsStolen,
                this.CreateValidationRule(updatedSettings.StolenFieldSetting));
            this.RestoreFieldIfNeeded(
                updated,
                () => updated.Replaced,
                existing.Replaced,
                this.CreateValidationRule(updatedSettings.ReplacedWithFieldSetting));
            this.RestoreFieldIfNeeded(
                updated,
                () => updated.IsSendBack,
                existing.IsSendBack,
                this.CreateValidationRule(updatedSettings.SendBackFieldSetting));
            this.RestoreFieldIfNeeded(
                updated,
                () => updated.ScrapDate,
                existing.ScrapDate,
                this.CreateValidationRule(updatedSettings.ScrapDateFieldSetting));
        }

        private void RestoreDate(DateFields updated, DateFields existing, DateFieldsSettings updatedSettings)
        {
            this.RestoreFieldIfNeeded(
                updated,
                () => updated.SynchronizeDate,
                existing.SynchronizeDate,
                this.CreateValidationRule(updatedSettings.SyncChangedDateSetting));
            this.RestoreFieldIfNeeded(
                updated,
                () => updated.ScanDate,
                existing.ScanDate,
                this.CreateValidationRule(updatedSettings.ScanDateFieldSetting));
            this.RestoreFieldIfNeeded(
                updated,
                () => updated.PathDirectory,
                existing.PathDirectory,
                this.CreateValidationRule(updatedSettings.PathDirectoryFieldSetting));
        }

        private bool CreateValidationRule(ProcessingFieldSetting setting)
        {
            return setting.IsShow && !setting.IsReadOnly;
        }
    }
}