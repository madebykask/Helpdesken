namespace DH.Helpdesk.Services.BusinessLogic.BusinessModelValidators.Inventory.Concrete
{
    using DH.Helpdesk.BusinessData.Models.Inventory.Edit.Computer;
    using DH.Helpdesk.BusinessData.Models.Inventory.Edit.Shared;
    using DH.Helpdesk.BusinessData.Models.Inventory.Output.Settings.ProcessingSetttings.ComputerSettings;
    using DH.Helpdesk.Services.BusinessLogic.BusinessModelValidators.Common;

    using PlaceFields = DH.Helpdesk.BusinessData.Models.Inventory.Edit.Computer.PlaceFields;

    public class ComputerValidator : IComputerValidator
    {
        private readonly IElementaryRulesValidator elementaryRulesValidator;

        public ComputerValidator(IElementaryRulesValidator elementaryRulesValidator)
        {
            this.elementaryRulesValidator = elementaryRulesValidator;
        }

        public void Validate(ComputerForUpdate updatedComputer, ComputerForRead existingComputer, ComputerFieldsSettingsProcessing settings)
        {
            this.ValidateWorkstation(updatedComputer.WorkstationFields, existingComputer.WorkstationFields, settings.WorkstationFieldsSettings);
            this.ValidateChassis(updatedComputer.ChassisFields, existingComputer.ChassisFields, settings.ChassisFieldsSettings);
            this.ValidateInventoring(updatedComputer.InventoryFields, existingComputer.InventoryFields, settings.InventoryFieldsSettings);
            this.ValidateOperatingSystem(updatedComputer.OperatingSystemFields, existingComputer.OperatingSystemFields, settings.OperatingSystemFieldsSettings);
            this.ValidateProcessor(updatedComputer.ProccesorFields, existingComputer.ProccesorFields, settings.ProccesorFieldsSettings);
            this.ValidateMemory(updatedComputer.MemoryFields, existingComputer.MemoryFields, settings.MemoryFieldsSettings);
            this.ValidateCommunication(updatedComputer.CommunicationFields, existingComputer.CommunicationFields, settings.CommunicationFieldsSettings);
            this.ValidateGraphics(updatedComputer.GraphicsFields, existingComputer.GraphicsFields, settings.GraphicsFieldsSettings);
            this.ValidateSound(updatedComputer.SoundFields, existingComputer.SoundFields, settings.SoundFieldsSettings);
            this.ValidateContract(updatedComputer.ContractFields, existingComputer.ContractFields, settings.ContractFieldsSettings);
            this.ValidateOther(updatedComputer.OtherFields, existingComputer.OtherFields, settings.OtherFieldsSettings);
            this.ValidateContactInformation(updatedComputer.ContactInformationFields, existingComputer.ContactInformationFields, settings.ContactInformationFieldsSettings);
            this.ValidateOrganization(updatedComputer.OrganizationFields, existingComputer.OrganizationFields, settings.OrganizationFieldsSettings);
            this.ValidatePlace(updatedComputer.PlaceFields, existingComputer.PlaceFields, settings.PlaceFieldsSettings);
            this.ValidateContact(updatedComputer.ContactFields, existingComputer.ContactFields, settings.ContactFieldsSettings);
            this.ValidateState(updatedComputer.StateFields, existingComputer.StateFields, settings.StateFieldsSettings);

            // this.ValidateDate(updatedComputer.DateFields, existingComputer.DateFields, settings.DateFieldsSettings);
        }

        public void Validate(ComputerForInsert newComputer, ComputerFieldsSettingsProcessing settings)
        {
            this.ValidateWorkstation(newComputer.WorkstationFields, settings.WorkstationFieldsSettings);
            this.ValidateChassis(newComputer.ChassisFields, settings.ChassisFieldsSettings);
            this.ValidateInventoring(newComputer.InventoryFields, settings.InventoryFieldsSettings);
            this.ValidateOperatingSystem(newComputer.OperatingSystemFields, settings.OperatingSystemFieldsSettings);
            this.ValidateProcessor(newComputer.ProccesorFields, settings.ProccesorFieldsSettings);
            this.ValidateMemory(newComputer.MemoryFields, settings.MemoryFieldsSettings);
            this.ValidateCommunication(newComputer.CommunicationFields, settings.CommunicationFieldsSettings);
            this.ValidateGraphics(newComputer.GraphicsFields, settings.GraphicsFieldsSettings);
            this.ValidateSound(newComputer.SoundFields, settings.SoundFieldsSettings);
            this.ValidateContract(newComputer.ContractFields, settings.ContractFieldsSettings);
            this.ValidateOther(newComputer.OtherFields, settings.OtherFieldsSettings);
            this.ValidateContactInformation(newComputer.ContactInformationFields, settings.ContactInformationFieldsSettings);
            this.ValidateOrganization(newComputer.OrganizationFields, settings.OrganizationFieldsSettings);
            this.ValidatePlace(newComputer.PlaceFields, settings.PlaceFieldsSettings);
            this.ValidateContact(newComputer.ContactFields, settings.ContactFieldsSettings);
            this.ValidateState(newComputer.StateFields, settings.StateFieldsSettings);
            
            // this.ValidateDate(newComputer.DateFields, settings.DateFieldsSettings);
        }

        private void ValidateWorkstation(
            WorkstationFields updated,
            WorkstationFields existing,
            WorkstationFieldsSettings updatedSettings)
        {
            this.elementaryRulesValidator.ValidateStringField(
                updated.ComputerName,
                existing.ComputerName,
                BusinessData.Enums.Inventory.Fields.Computer.WorkstationFields.Name,
                this.CreateValidationRule(updatedSettings.ComputerNameFieldSetting));
            this.elementaryRulesValidator.ValidateStringField(
                updated.Manufacturer,
                existing.Manufacturer,
                BusinessData.Enums.Inventory.Fields.Computer.WorkstationFields.Manufacturer,
                this.CreateValidationRule(updatedSettings.ManufacturerFieldSetting));
            this.elementaryRulesValidator.ValidateIntegerField(
                updated.ComputerModelId,
                existing.ComputerModelId,
                BusinessData.Enums.Inventory.Fields.Computer.WorkstationFields.Model,
                this.CreateValidationRule(updatedSettings.ComputerModelFieldSetting));
            this.elementaryRulesValidator.ValidateStringField(
                updated.SerialNumber,
                existing.SerialNumber,
                BusinessData.Enums.Inventory.Fields.Computer.WorkstationFields.SerialNumber,
                this.CreateValidationRule(updatedSettings.SerialNumberFieldSetting));
            this.elementaryRulesValidator.ValidateDateTimeField(
                updated.BIOSDate,
                existing.BIOSDate,
                BusinessData.Enums.Inventory.Fields.Computer.WorkstationFields.BIOSDate,
                this.CreateValidationRule(updatedSettings.BIOSDateFieldSetting));
            this.elementaryRulesValidator.ValidateStringField(
                updated.BIOSVersion,
                existing.BIOSVersion,
                BusinessData.Enums.Inventory.Fields.Computer.WorkstationFields.BIOSVersion,
                this.CreateValidationRule(updatedSettings.BIOSVersionFieldSetting));
            this.elementaryRulesValidator.ValidateStringField(
                updated.Theftmark,
                existing.Theftmark,
                BusinessData.Enums.Inventory.Fields.Computer.WorkstationFields.Theftmark,
                this.CreateValidationRule(updatedSettings.TheftmarkFieldSetting));
            this.elementaryRulesValidator.ValidateStringField(
                updated.CarePackNumber,
                existing.CarePackNumber,
                BusinessData.Enums.Inventory.Fields.Computer.WorkstationFields.CarePackNumber,
                this.CreateValidationRule(updatedSettings.CarePackNumberFieldSetting));
            this.elementaryRulesValidator.ValidateIntegerField(
                updated.ComputerTypeId,
                existing.ComputerTypeId,
                BusinessData.Enums.Inventory.Fields.Computer.WorkstationFields.ComputerType,
                this.CreateValidationRule(updatedSettings.ComputerTypeFieldSetting));
            this.elementaryRulesValidator.ValidateStringField(
                updated.Location,
                existing.Location,
                BusinessData.Enums.Inventory.Fields.Computer.WorkstationFields.Location,
                this.CreateValidationRule(updatedSettings.LocationFieldSetting));
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

        private void ValidateMemory(
            MemoryFields updated,
            MemoryFields existing,
            MemoryFieldsSettings updatedSettings)
        {
            this.elementaryRulesValidator.ValidateIntegerField(
                updated.RAMId,
                existing.RAMId,
                BusinessData.Enums.Inventory.Fields.Shared.MemoryFields.RAM,
                this.CreateValidationRule(updatedSettings.RAMFieldSetting));
        }

        private void ValidateCommunication(
            CommunicationFields updated,
            CommunicationFields existing,
            CommunicationFieldsSettings updatedSettings)
        {
            this.elementaryRulesValidator.ValidateIntegerField(
                updated.NetworkAdapterId,
                existing.NetworkAdapterId,
                BusinessData.Enums.Inventory.Fields.Computer.CommunicationFields.NetworkAdapter,
                this.CreateValidationRule(updatedSettings.NetworkAdapterFieldSetting));
            this.elementaryRulesValidator.ValidateStringField(
                updated.IPAddress,
                existing.IPAddress,
                BusinessData.Enums.Inventory.Fields.Computer.CommunicationFields.IPAddress,
                this.CreateValidationRule(updatedSettings.IPAddressFieldSetting));
            this.elementaryRulesValidator.ValidateStringField(
                updated.MacAddress,
                existing.MacAddress,
                BusinessData.Enums.Inventory.Fields.Computer.CommunicationFields.MacAddress,
                this.CreateValidationRule(updatedSettings.MacAddressFieldSetting));
            this.elementaryRulesValidator.ValidateBooleanField(
                updated.IsRAS,
                existing.IsRAS,
                BusinessData.Enums.Inventory.Fields.Computer.CommunicationFields.RAS,
                this.CreateValidationRule(updatedSettings.RASFieldSetting));
            this.elementaryRulesValidator.ValidateStringField(
                updated.NovellClient,
                existing.NovellClient,
                BusinessData.Enums.Inventory.Fields.Computer.CommunicationFields.NovellClient,
                this.CreateValidationRule(updatedSettings.NovellClientFieldSetting));
        }

        private void ValidateGraphics(
            GraphicsFields updated,
            GraphicsFields existing,
            GraphicsFieldsSettings updatedSettings)
        {
            this.elementaryRulesValidator.ValidateStringField(
                updated.VideoCard,
                existing.VideoCard,
                BusinessData.Enums.Inventory.Fields.Computer.GraphicsFields.VideoCard,
                this.CreateValidationRule(updatedSettings.VideoCardFieldSetting));
        }

        private void ValidateSound(
            SoundFields updated,
            SoundFields existing,
            SoundFieldsSettings updatedSettings)
        {
            this.elementaryRulesValidator.ValidateStringField(
                updated.SoundCard,
                existing.SoundCard,
                BusinessData.Enums.Inventory.Fields.Computer.SoundFields.SoundCard,
                this.CreateValidationRule(updatedSettings.SoundCardFieldSetting));
        }

        private void ValidateContract(
            ContractFields updated,
            ContractFields existing,
            ContractFieldsSettings updatedSettings)
        {
            this.elementaryRulesValidator.ValidateIntegerField(
                updated.ContractStatusId,
                existing.ContractStatusId,
                BusinessData.Enums.Inventory.Fields.Computer.ContractFields.ContractStatusName,
                this.CreateValidationRule(updatedSettings.ContractStatusFieldSetting));
            this.elementaryRulesValidator.ValidateStringField(
                updated.ContractNumber,
                existing.ContractNumber,
                BusinessData.Enums.Inventory.Fields.Computer.ContractFields.ContractNumber,
                this.CreateValidationRule(updatedSettings.ContractNumberFieldSetting));
            this.elementaryRulesValidator.ValidateDateTimeField(
                updated.ContractStartDate,
                existing.ContractStartDate,
                BusinessData.Enums.Inventory.Fields.Computer.ContractFields.ContractStartDate,
                this.CreateValidationRule(updatedSettings.ContractStartDateFieldSetting));
            this.elementaryRulesValidator.ValidateDateTimeField(
                updated.ContractEndDate,
                existing.ContractEndDate,
                BusinessData.Enums.Inventory.Fields.Computer.ContractFields.ContractEndDate,
                this.CreateValidationRule(updatedSettings.ContractEndDateFieldSetting));
            this.elementaryRulesValidator.ValidateIntegerField(
                updated.PurchasePrice,
                existing.PurchasePrice,
                BusinessData.Enums.Inventory.Fields.Computer.ContractFields.PurchasePrice,
                this.CreateValidationRule(updatedSettings.PurchasePriceFieldSetting));
            this.elementaryRulesValidator.ValidateStringField(
                updated.AccountingDimension1,
                existing.AccountingDimension1,
                BusinessData.Enums.Inventory.Fields.Computer.ContractFields.AccountingDimension1,
                this.CreateValidationRule(updatedSettings.AccountingDimension1FieldSetting));
            this.elementaryRulesValidator.ValidateStringField(
                updated.AccountingDimension2,
                existing.AccountingDimension2,
                BusinessData.Enums.Inventory.Fields.Computer.ContractFields.AccountingDimension2,
                this.CreateValidationRule(updatedSettings.AccountingDimension2FieldSetting));
            this.elementaryRulesValidator.ValidateStringField(
                updated.AccountingDimension3,
                existing.AccountingDimension3,
                BusinessData.Enums.Inventory.Fields.Computer.ContractFields.AccountingDimension3,
                this.CreateValidationRule(updatedSettings.AccountingDimension3FieldSetting));
            this.elementaryRulesValidator.ValidateStringField(
                updated.AccountingDimension4,
                existing.AccountingDimension4,
                BusinessData.Enums.Inventory.Fields.Computer.ContractFields.AccountingDimension4,
                this.CreateValidationRule(updatedSettings.AccountingDimension4FieldSetting));
            this.elementaryRulesValidator.ValidateStringField(
                updated.AccountingDimension5,
                existing.AccountingDimension5,
                BusinessData.Enums.Inventory.Fields.Computer.ContractFields.AccountingDimension5,
                this.CreateValidationRule(updatedSettings.AccountingDimension5FieldSetting));
        }

        private void ValidateOther(
            OtherFields updated,
            OtherFields existing,
            OtherFieldsSettings updatedSettings)
        {
            this.elementaryRulesValidator.ValidateStringField(
                updated.Info,
                existing.Info,
                BusinessData.Enums.Inventory.Fields.Computer.OtherFields.Info,
                this.CreateValidationRule(updatedSettings.InfoFieldSetting));
        }

        private void ValidateContactInformation(
            ContactInformationFields updated,
            ContactInformationFields existing,
            ContactInformationFieldsSettings updatedSettings)
        {
            this.elementaryRulesValidator.ValidateIntegerField(
                updated.UserId,
                existing.UserId,
                BusinessData.Enums.Inventory.Fields.Computer.ContactInformationFields.UserId,
                this.CreateValidationRule(updatedSettings.UserIdFieldSetting));
        }

        private void ValidateOrganization(
            OrganizationFields updated,
            OrganizationFields existing,
            OrganizationFieldsSettings updatedSettings)
        {
            this.elementaryRulesValidator.ValidateIntegerField(
                updated.RegionId,
                existing.RegionId,
                BusinessData.Enums.Inventory.Fields.Computer.OrganizationFields.Region,
                this.CreateValidationRule(updatedSettings.RegionFieldSetting));
            this.elementaryRulesValidator.ValidateIntegerField(
                updated.DepartmentId,
                existing.DepartmentId,
                BusinessData.Enums.Inventory.Fields.Computer.OrganizationFields.Department,
                this.CreateValidationRule(updatedSettings.DepartmentFieldSetting));
            this.elementaryRulesValidator.ValidateIntegerField(
                updated.DomainId,
                existing.DomainId,
                BusinessData.Enums.Inventory.Fields.Computer.OrganizationFields.Domain,
                this.CreateValidationRule(updatedSettings.DomainFieldSetting));
            this.elementaryRulesValidator.ValidateIntegerField(
                updated.UnitId,
                existing.UnitId,
                BusinessData.Enums.Inventory.Fields.Computer.OrganizationFields.Unit,
                this.CreateValidationRule(updatedSettings.UnitFieldSetting));
        }

        private void ValidatePlace(
            PlaceFields updated,
            PlaceFields existing,
            PlaceFieldsSettings updatedSettings)
        {
            this.elementaryRulesValidator.ValidateIntegerField(
                updated.RoomId,
                existing.RoomId,
                BusinessData.Enums.Inventory.Fields.Computer.PlaceFields.Room,
                this.CreateValidationRule(updatedSettings.RoomFieldSetting));
            this.elementaryRulesValidator.ValidateStringField(
                updated.Address,
                existing.Address,
                BusinessData.Enums.Inventory.Fields.Computer.PlaceFields.Address,
                this.CreateValidationRule(updatedSettings.AddressFieldSetting));
            this.elementaryRulesValidator.ValidateStringField(
                updated.PostalCode,
                existing.PostalCode,
                BusinessData.Enums.Inventory.Fields.Computer.PlaceFields.PostalCode,
                this.CreateValidationRule(updatedSettings.PostalAddressFieldSetting));
            this.elementaryRulesValidator.ValidateStringField(
                updated.PostalAddress,
                existing.PostalAddress,
                BusinessData.Enums.Inventory.Fields.Computer.PlaceFields.PostalAddress,
                this.CreateValidationRule(updatedSettings.PostalAddressFieldSetting));
            this.elementaryRulesValidator.ValidateStringField(
                updated.Location,
                existing.Location,
                BusinessData.Enums.Inventory.Fields.Computer.PlaceFields.Location,
                this.CreateValidationRule(updatedSettings.PlaceFieldSetting));
            this.elementaryRulesValidator.ValidateStringField(
                updated.Location2,
                existing.Location2,
                BusinessData.Enums.Inventory.Fields.Computer.PlaceFields.Location2,
                this.CreateValidationRule(updatedSettings.Place2FieldSetting));
        }

        private void ValidateContact(
            ContactFields updated,
            ContactFields existing,
            ContactFieldsSettings updatedSettings)
        {
            this.elementaryRulesValidator.ValidateStringField(
                updated.Name,
                existing.Name,
                BusinessData.Enums.Inventory.Fields.Computer.ContactFields.Name,
                this.CreateValidationRule(updatedSettings.NameFieldSetting));
            this.elementaryRulesValidator.ValidateStringField(
                updated.Phone,
                existing.Phone,
                BusinessData.Enums.Inventory.Fields.Computer.ContactFields.Phone,
                this.CreateValidationRule(updatedSettings.PhoneFieldSetting));
            this.elementaryRulesValidator.ValidateStringField(
                updated.Email,
                existing.Email,
                BusinessData.Enums.Inventory.Fields.Computer.ContactFields.Email,
                this.CreateValidationRule(updatedSettings.EmailFieldSetting));
        }

        private void ValidateState(
            StateFields updated,
            StateFields existing,
            StateFieldsSettings updatedSettings)
        {
            this.elementaryRulesValidator.ValidateIntegerField(
                updated.State,
                existing.State,
                BusinessData.Enums.Inventory.Fields.Computer.StateFields.State,
                this.CreateValidationRule(updatedSettings.StateFieldSetting));
            this.elementaryRulesValidator.ValidateBooleanField(
                updated.IsStolen,
                existing.IsStolen,
                BusinessData.Enums.Inventory.Fields.Computer.StateFields.Stolen,
                this.CreateValidationRule(updatedSettings.StolenFieldSetting));
            this.elementaryRulesValidator.ValidateStringField(
                updated.Replaced,
                existing.Replaced,
                BusinessData.Enums.Inventory.Fields.Computer.StateFields.Replaced,
                this.CreateValidationRule(updatedSettings.ReplacedWithFieldSetting));
            this.elementaryRulesValidator.ValidateBooleanField(
                updated.IsSendBack,
                existing.IsSendBack,
                BusinessData.Enums.Inventory.Fields.Computer.StateFields.SendBack,
                this.CreateValidationRule(updatedSettings.SendBackFieldSetting));
            this.elementaryRulesValidator.ValidateDateTimeField(
                updated.ScrapDate,
                existing.ScrapDate,
                BusinessData.Enums.Inventory.Fields.Computer.StateFields.ScrapDate,
                this.CreateValidationRule(updatedSettings.ScrapDateFieldSetting));
        }

        private void ValidateDate(
            DateFields updated,
            DateFields existing,
            DateFieldsSettings updatedSettings)
        {
            this.elementaryRulesValidator.ValidateDateTimeField(
                updated.SynchronizeDate,
                existing.SynchronizeDate,
                BusinessData.Enums.Inventory.Fields.Computer.DateFields.SynchronizeDate,
                this.CreateValidationRule(updatedSettings.SyncChangedDateSetting));
            this.elementaryRulesValidator.ValidateDateTimeField(
                updated.ScanDate,
                existing.ScanDate,
                BusinessData.Enums.Inventory.Fields.Computer.DateFields.ScanDate,
                this.CreateValidationRule(updatedSettings.ScanDateFieldSetting));
            this.elementaryRulesValidator.ValidateStringField(
                updated.PathDirectory,
                existing.PathDirectory,
                BusinessData.Enums.Inventory.Fields.Computer.DateFields.PathDirectory,
                this.CreateValidationRule(updatedSettings.PathDirectoryFieldSetting));
        }

        private void ValidateWorkstation(
            WorkstationFields model,
            WorkstationFieldsSettings modelSettings)
        {
            this.elementaryRulesValidator.ValidateStringField(
                model.ComputerName,
                BusinessData.Enums.Inventory.Fields.Computer.WorkstationFields.Name,
                this.CreateValidationRule(modelSettings.ComputerNameFieldSetting));
            this.elementaryRulesValidator.ValidateStringField(
                model.Manufacturer,
                BusinessData.Enums.Inventory.Fields.Computer.WorkstationFields.Manufacturer,
                this.CreateValidationRule(modelSettings.ManufacturerFieldSetting));
            this.elementaryRulesValidator.ValidateIntegerField(
                model.ComputerModelId,
                BusinessData.Enums.Inventory.Fields.Computer.WorkstationFields.Model,
                this.CreateValidationRule(modelSettings.ComputerModelFieldSetting));
            this.elementaryRulesValidator.ValidateStringField(
                model.SerialNumber,
                BusinessData.Enums.Inventory.Fields.Computer.WorkstationFields.SerialNumber,
                this.CreateValidationRule(modelSettings.SerialNumberFieldSetting));
            this.elementaryRulesValidator.ValidateDateTimeField(
                model.BIOSDate,
                BusinessData.Enums.Inventory.Fields.Computer.WorkstationFields.BIOSDate,
                this.CreateValidationRule(modelSettings.BIOSDateFieldSetting));
            this.elementaryRulesValidator.ValidateStringField(
                model.BIOSVersion,
                BusinessData.Enums.Inventory.Fields.Computer.WorkstationFields.BIOSVersion,
                this.CreateValidationRule(modelSettings.BIOSVersionFieldSetting));
            this.elementaryRulesValidator.ValidateStringField(
                model.Theftmark,
                BusinessData.Enums.Inventory.Fields.Computer.WorkstationFields.Theftmark,
                this.CreateValidationRule(modelSettings.TheftmarkFieldSetting));
            this.elementaryRulesValidator.ValidateStringField(
                model.CarePackNumber,
                BusinessData.Enums.Inventory.Fields.Computer.WorkstationFields.CarePackNumber,
                this.CreateValidationRule(modelSettings.CarePackNumberFieldSetting));
            this.elementaryRulesValidator.ValidateIntegerField(
                model.ComputerTypeId,
                BusinessData.Enums.Inventory.Fields.Computer.WorkstationFields.ComputerType,
                this.CreateValidationRule(modelSettings.ComputerTypeFieldSetting));
            this.elementaryRulesValidator.ValidateStringField(
                model.Location,
                BusinessData.Enums.Inventory.Fields.Computer.WorkstationFields.Location,
                this.CreateValidationRule(modelSettings.LocationFieldSetting));
        }

        private void ValidateChassis(
            ChassisFields model,
            ChassisFieldsSettings modelSettings)
        {
            this.elementaryRulesValidator.ValidateStringField(
                model.Chassis,
                BusinessData.Enums.Inventory.Fields.Shared.ChassisFields.Chassis,
                this.CreateValidationRule(modelSettings.ChassisFieldSetting));
        }

        private void ValidateInventoring(
            InventoryFields model,
            InventoryFieldsSettings modelSettings)
        {
            this.elementaryRulesValidator.ValidateStringField(
                model.BarCode,
                BusinessData.Enums.Inventory.Fields.Shared.InventoryFields.BarCode,
                this.CreateValidationRule(modelSettings.BarCodeFieldSetting));
            this.elementaryRulesValidator.ValidateDateTimeField(
                model.PurchaseDate,
                BusinessData.Enums.Inventory.Fields.Shared.InventoryFields.PurchaseDate,
                this.CreateValidationRule(modelSettings.PurchaseDateFieldSetting));
        }

        private void ValidateOperatingSystem(
            OperatingSystemFields model,
            OperatingSystemFieldsSettings modelSettings)
        {
            this.elementaryRulesValidator.ValidateIntegerField(
                model.OperatingSystemId,
                BusinessData.Enums.Inventory.Fields.Shared.OperatingSystemFields.OperatingSystem,
                this.CreateValidationRule(modelSettings.OperatingSystemFieldSetting));
            this.elementaryRulesValidator.ValidateStringField(
                model.Version,
                BusinessData.Enums.Inventory.Fields.Shared.OperatingSystemFields.Version,
                this.CreateValidationRule(modelSettings.VersionFieldSetting));
            this.elementaryRulesValidator.ValidateStringField(
                model.ServicePack,
                BusinessData.Enums.Inventory.Fields.Shared.OperatingSystemFields.ServicePack,
                this.CreateValidationRule(modelSettings.ServicePackSystemFieldSetting));
            this.elementaryRulesValidator.ValidateStringField(
                model.RegistrationCode,
                BusinessData.Enums.Inventory.Fields.Shared.OperatingSystemFields.RegistrationCode,
                this.CreateValidationRule(modelSettings.RegistrationCodeSystemFieldSetting));
            this.elementaryRulesValidator.ValidateStringField(
                model.ProductKey,
                BusinessData.Enums.Inventory.Fields.Shared.OperatingSystemFields.ProductKey,
                this.CreateValidationRule(modelSettings.ProductKeyFieldSetting));
        }

        private void ValidateProcessor(
            ProcessorFields model,
            ProcessorFieldsSettings modelSettings)
        {
            this.elementaryRulesValidator.ValidateIntegerField(
                model.ProccesorId,
                BusinessData.Enums.Inventory.Fields.Shared.ProcessorFields.ProccesorName,
                this.CreateValidationRule(modelSettings.ProccesorFieldSetting));
        }

        private void ValidateMemory(
            MemoryFields model,
            MemoryFieldsSettings modelSettings)
        {
            this.elementaryRulesValidator.ValidateIntegerField(
                model.RAMId,
                BusinessData.Enums.Inventory.Fields.Shared.MemoryFields.RAM,
                this.CreateValidationRule(modelSettings.RAMFieldSetting));
        }

        private void ValidateCommunication(
            CommunicationFields model,
            CommunicationFieldsSettings modelSettings)
        {
            this.elementaryRulesValidator.ValidateIntegerField(
                model.NetworkAdapterId,
                BusinessData.Enums.Inventory.Fields.Computer.CommunicationFields.NetworkAdapter,
                this.CreateValidationRule(modelSettings.NetworkAdapterFieldSetting));
            this.elementaryRulesValidator.ValidateStringField(
                model.IPAddress,
                BusinessData.Enums.Inventory.Fields.Computer.CommunicationFields.IPAddress,
                this.CreateValidationRule(modelSettings.IPAddressFieldSetting));
            this.elementaryRulesValidator.ValidateStringField(
                model.MacAddress,
                BusinessData.Enums.Inventory.Fields.Computer.CommunicationFields.MacAddress,
                this.CreateValidationRule(modelSettings.MacAddressFieldSetting));
            this.elementaryRulesValidator.ValidateBooleanField(
                model.IsRAS,
                false,
                BusinessData.Enums.Inventory.Fields.Computer.CommunicationFields.RAS,
                this.CreateValidationRule(modelSettings.RASFieldSetting));
            this.elementaryRulesValidator.ValidateStringField(
                model.NovellClient,
                BusinessData.Enums.Inventory.Fields.Computer.CommunicationFields.NovellClient,
                this.CreateValidationRule(modelSettings.NovellClientFieldSetting));
        }

        private void ValidateGraphics(
            GraphicsFields model,
            GraphicsFieldsSettings modelSettings)
        {
            this.elementaryRulesValidator.ValidateStringField(
                model.VideoCard,
                BusinessData.Enums.Inventory.Fields.Computer.GraphicsFields.VideoCard,
                this.CreateValidationRule(modelSettings.VideoCardFieldSetting));
        }

        private void ValidateSound(
            SoundFields model,
            SoundFieldsSettings modelSettings)
        {
            this.elementaryRulesValidator.ValidateStringField(
                model.SoundCard,
                BusinessData.Enums.Inventory.Fields.Computer.SoundFields.SoundCard,
                this.CreateValidationRule(modelSettings.SoundCardFieldSetting));
        }

        private void ValidateContract(
            ContractFields model,
            ContractFieldsSettings modelSettings)
        {
            this.elementaryRulesValidator.ValidateIntegerField(
                model.ContractStatusId,
                BusinessData.Enums.Inventory.Fields.Computer.ContractFields.ContractStatusName,
                this.CreateValidationRule(modelSettings.ContractStatusFieldSetting));
            this.elementaryRulesValidator.ValidateStringField(
                model.ContractNumber,
                BusinessData.Enums.Inventory.Fields.Computer.ContractFields.ContractNumber,
                this.CreateValidationRule(modelSettings.ContractNumberFieldSetting));
            this.elementaryRulesValidator.ValidateDateTimeField(
                model.ContractStartDate,
                BusinessData.Enums.Inventory.Fields.Computer.ContractFields.ContractStartDate,
                this.CreateValidationRule(modelSettings.ContractStartDateFieldSetting));
            this.elementaryRulesValidator.ValidateDateTimeField(
                model.ContractEndDate,
                BusinessData.Enums.Inventory.Fields.Computer.ContractFields.ContractEndDate,
                this.CreateValidationRule(modelSettings.ContractEndDateFieldSetting));
            this.elementaryRulesValidator.ValidateIntegerField(
                model.PurchasePrice,
                BusinessData.Enums.Inventory.Fields.Computer.ContractFields.PurchasePrice,
                this.CreateValidationRule(modelSettings.PurchasePriceFieldSetting));
            this.elementaryRulesValidator.ValidateStringField(
                model.AccountingDimension1,
                BusinessData.Enums.Inventory.Fields.Computer.ContractFields.AccountingDimension1,
                this.CreateValidationRule(modelSettings.AccountingDimension1FieldSetting));
            this.elementaryRulesValidator.ValidateStringField(
                model.AccountingDimension2,
                BusinessData.Enums.Inventory.Fields.Computer.ContractFields.AccountingDimension2,
                this.CreateValidationRule(modelSettings.AccountingDimension2FieldSetting));
            this.elementaryRulesValidator.ValidateStringField(
                model.AccountingDimension3,
                BusinessData.Enums.Inventory.Fields.Computer.ContractFields.AccountingDimension3,
                this.CreateValidationRule(modelSettings.AccountingDimension3FieldSetting));
            this.elementaryRulesValidator.ValidateStringField(
                model.AccountingDimension4,
                BusinessData.Enums.Inventory.Fields.Computer.ContractFields.AccountingDimension4,
                this.CreateValidationRule(modelSettings.AccountingDimension4FieldSetting));
            this.elementaryRulesValidator.ValidateStringField(
                model.AccountingDimension5,
                BusinessData.Enums.Inventory.Fields.Computer.ContractFields.AccountingDimension5,
                this.CreateValidationRule(modelSettings.AccountingDimension5FieldSetting));
        }

        private void ValidateOther(
            OtherFields model,
            OtherFieldsSettings modelSettings)
        {
            this.elementaryRulesValidator.ValidateStringField(
                model.Info,
                BusinessData.Enums.Inventory.Fields.Computer.OtherFields.Info,
                this.CreateValidationRule(modelSettings.InfoFieldSetting));
        }

        private void ValidateContactInformation(
            ContactInformationFields model,
            ContactInformationFieldsSettings modelSettings)
        {
            this.elementaryRulesValidator.ValidateIntegerField(
                model.UserId,
                BusinessData.Enums.Inventory.Fields.Computer.ContactInformationFields.UserId,
                this.CreateValidationRule(modelSettings.UserIdFieldSetting));
        }

        private void ValidateOrganization(
            OrganizationFields model,
            OrganizationFieldsSettings modelSettings)
        {
            this.elementaryRulesValidator.ValidateIntegerField(
                model.RegionId,
                BusinessData.Enums.Inventory.Fields.Computer.OrganizationFields.Region,
                this.CreateValidationRule(modelSettings.RegionFieldSetting));
            this.elementaryRulesValidator.ValidateIntegerField(
                model.DepartmentId,
                BusinessData.Enums.Inventory.Fields.Computer.OrganizationFields.Department,
                this.CreateValidationRule(modelSettings.DepartmentFieldSetting));
            this.elementaryRulesValidator.ValidateIntegerField(
                model.DomainId,
                BusinessData.Enums.Inventory.Fields.Computer.OrganizationFields.Domain,
                this.CreateValidationRule(modelSettings.DomainFieldSetting));
            this.elementaryRulesValidator.ValidateIntegerField(
                model.UnitId,
                BusinessData.Enums.Inventory.Fields.Computer.OrganizationFields.Unit,
                this.CreateValidationRule(modelSettings.UnitFieldSetting));
        }

        private void ValidatePlace(
            PlaceFields model,
            PlaceFieldsSettings modelSettings)
        {
            this.elementaryRulesValidator.ValidateIntegerField(
                model.RoomId,
                BusinessData.Enums.Inventory.Fields.Computer.PlaceFields.Room,
                this.CreateValidationRule(modelSettings.RoomFieldSetting));
            this.elementaryRulesValidator.ValidateStringField(
                model.Address,
                BusinessData.Enums.Inventory.Fields.Computer.PlaceFields.Address,
                this.CreateValidationRule(modelSettings.AddressFieldSetting));
            this.elementaryRulesValidator.ValidateStringField(
                model.PostalCode,
                BusinessData.Enums.Inventory.Fields.Computer.PlaceFields.PostalCode,
                this.CreateValidationRule(modelSettings.PostalAddressFieldSetting));
            this.elementaryRulesValidator.ValidateStringField(
                model.PostalAddress,
                BusinessData.Enums.Inventory.Fields.Computer.PlaceFields.PostalAddress,
                this.CreateValidationRule(modelSettings.PostalAddressFieldSetting));
            this.elementaryRulesValidator.ValidateStringField(
                model.Location,
                BusinessData.Enums.Inventory.Fields.Computer.PlaceFields.Location,
                this.CreateValidationRule(modelSettings.PlaceFieldSetting));
            this.elementaryRulesValidator.ValidateStringField(
                model.Location2,
                BusinessData.Enums.Inventory.Fields.Computer.PlaceFields.Location2,
                this.CreateValidationRule(modelSettings.Place2FieldSetting));
        }

        private void ValidateContact(
            ContactFields model,
            ContactFieldsSettings modelSettings)
        {
            this.elementaryRulesValidator.ValidateStringField(
                model.Name,
                BusinessData.Enums.Inventory.Fields.Computer.ContactFields.Name,
                this.CreateValidationRule(modelSettings.NameFieldSetting));
            this.elementaryRulesValidator.ValidateStringField(
                model.Phone,
                BusinessData.Enums.Inventory.Fields.Computer.ContactFields.Phone,
                this.CreateValidationRule(modelSettings.PhoneFieldSetting));
            this.elementaryRulesValidator.ValidateStringField(
                model.Email,
                BusinessData.Enums.Inventory.Fields.Computer.ContactFields.Email,
                this.CreateValidationRule(modelSettings.EmailFieldSetting));
        }

        private void ValidateState(
            StateFields model,
            StateFieldsSettings modelSettings)
        {
            this.elementaryRulesValidator.ValidateIntegerField(
                model.State,
                BusinessData.Enums.Inventory.Fields.Computer.StateFields.State,
                this.CreateValidationRule(modelSettings.StateFieldSetting));
            this.elementaryRulesValidator.ValidateBooleanField(
                model.IsStolen,
                false,
                BusinessData.Enums.Inventory.Fields.Computer.StateFields.Stolen,
                this.CreateValidationRule(modelSettings.StolenFieldSetting));
            this.elementaryRulesValidator.ValidateStringField(
                model.Replaced,
                BusinessData.Enums.Inventory.Fields.Computer.StateFields.Replaced,
                this.CreateValidationRule(modelSettings.ReplacedWithFieldSetting));
            this.elementaryRulesValidator.ValidateBooleanField(
                model.IsSendBack,
                false,
                BusinessData.Enums.Inventory.Fields.Computer.StateFields.SendBack,
                this.CreateValidationRule(modelSettings.SendBackFieldSetting));
            this.elementaryRulesValidator.ValidateDateTimeField(
                model.ScrapDate,
                BusinessData.Enums.Inventory.Fields.Computer.StateFields.ScrapDate,
                this.CreateValidationRule(modelSettings.ScrapDateFieldSetting));
        }

        private void ValidateDate(
            DateFields model,
            DateFieldsSettings modelSettings)
        {
            this.elementaryRulesValidator.ValidateDateTimeField(
                model.SynchronizeDate,
                BusinessData.Enums.Inventory.Fields.Computer.DateFields.SynchronizeDate,
                this.CreateValidationRule(modelSettings.SyncChangedDateSetting));
            this.elementaryRulesValidator.ValidateDateTimeField(
                model.ScanDate,
                BusinessData.Enums.Inventory.Fields.Computer.DateFields.ScanDate,
                this.CreateValidationRule(modelSettings.ScanDateFieldSetting));
            this.elementaryRulesValidator.ValidateStringField(
                model.PathDirectory,
                BusinessData.Enums.Inventory.Fields.Computer.DateFields.PathDirectory,
                this.CreateValidationRule(modelSettings.PathDirectoryFieldSetting));
        }

        private ElementaryValidationRule CreateValidationRule(ProcessingFieldSetting setting)
        {
            return new ElementaryValidationRule(!setting.IsShow || setting.IsReadOnly, setting.IsRequired);
        }
    }
}