namespace DH.Helpdesk.Dal.Mappers.Inventory.EntityToBusinessModel.Computer
{
    using DH.Helpdesk.BusinessData.Models.Inventory.Output.Settings.ProcessingSetttings.ComputerSettings;
    using DH.Helpdesk.Common.Collections;
    using DH.Helpdesk.Common.Extensions.Integer;
    using DH.Helpdesk.Dal.Enums.Inventory.Computer;
    using DH.Helpdesk.Dal.Enums.Inventory.Shared;
    using DH.Helpdesk.Dal.MapperData.Inventory;

    using ChassisFieldsSettings = DH.Helpdesk.BusinessData.Models.Inventory.Output.Settings.ProcessingSetttings.ComputerSettings.ChassisFieldsSettings;
    using CommunicationFields = DH.Helpdesk.Dal.Enums.Inventory.Computer.CommunicationFields;
    using CommunicationFieldsSettings = DH.Helpdesk.BusinessData.Models.Inventory.Output.Settings.ProcessingSetttings.ComputerSettings.CommunicationFieldsSettings;
    using ContactFieldsSettings = DH.Helpdesk.BusinessData.Models.Inventory.Output.Settings.ProcessingSetttings.ComputerSettings.ContactFieldsSettings;
    using ContactInformationFieldsSettings = DH.Helpdesk.BusinessData.Models.Inventory.Output.Settings.ProcessingSetttings.ComputerSettings.ContactInformationFieldsSettings;
    using ContractFieldsSettings = DH.Helpdesk.BusinessData.Models.Inventory.Output.Settings.ProcessingSetttings.ComputerSettings.ContractFieldsSettings;
    using DateFieldsSettings = DH.Helpdesk.BusinessData.Models.Inventory.Output.Settings.ProcessingSetttings.ComputerSettings.DateFieldsSettings;
    using GraphicsFieldsSettings = DH.Helpdesk.BusinessData.Models.Inventory.Output.Settings.ProcessingSetttings.ComputerSettings.GraphicsFieldsSettings;
    using InventoryFieldsSettings = DH.Helpdesk.BusinessData.Models.Inventory.Output.Settings.ProcessingSetttings.ComputerSettings.InventoryFieldsSettings;
    using MemoryFieldsSettings = DH.Helpdesk.BusinessData.Models.Inventory.Output.Settings.ProcessingSetttings.ComputerSettings.MemoryFieldsSettings;
    using OperatingSystemFieldsSettings = DH.Helpdesk.BusinessData.Models.Inventory.Output.Settings.ProcessingSetttings.ComputerSettings.OperatingSystemFieldsSettings;
    using OrganizationFieldsSettings = DH.Helpdesk.BusinessData.Models.Inventory.Output.Settings.ProcessingSetttings.ComputerSettings.OrganizationFieldsSettings;
    using OtherFields = DH.Helpdesk.Dal.Enums.Inventory.Computer.OtherFields;
    using OtherFieldsSettings = DH.Helpdesk.BusinessData.Models.Inventory.Output.Settings.ProcessingSetttings.ComputerSettings.OtherFieldsSettings;
    using PlaceFields = DH.Helpdesk.Dal.Enums.Inventory.Computer.PlaceFields;
    using PlaceFieldsSettings = DH.Helpdesk.BusinessData.Models.Inventory.Output.Settings.ProcessingSetttings.ComputerSettings.PlaceFieldsSettings;
    using ProcessorFieldsSettings = DH.Helpdesk.BusinessData.Models.Inventory.Output.Settings.ProcessingSetttings.ComputerSettings.ProcessorFieldsSettings;
    using SoundFieldsSettings = DH.Helpdesk.BusinessData.Models.Inventory.Output.Settings.ProcessingSetttings.ComputerSettings.SoundFieldsSettings;
    using StateFields = DH.Helpdesk.Dal.Enums.Inventory.Computer.StateFields;
    using StateFieldsSettings = DH.Helpdesk.BusinessData.Models.Inventory.Output.Settings.ProcessingSetttings.ComputerSettings.StateFieldsSettings;
    using WorkstationFieldsSettings = DH.Helpdesk.BusinessData.Models.Inventory.Output.Settings.ProcessingSetttings.ComputerSettings.WorkstationFieldsSettings;

    public sealed class ComputerSettingsToComputerProcessingSettingsMapper :
        IEntityToBusinessModelMapper<NamedObjectCollection<FieldProcessingSettingMapperData>, ComputerFieldsSettingsProcessing>
    {
        public ComputerFieldsSettingsProcessing Map(NamedObjectCollection<FieldProcessingSettingMapperData> entity)
        {
            var workstation = CreateWorkstationSettings(entity);
            var chassis = CreateChassisSettings(entity);
            var inventering = CreateInventeringSettings(entity);
            var operatingSystem = CretateOperatingSystemSettings(entity);
            var processor = CreateProcessorSettings(entity);
            var memory = CreateMemorySettings(entity);
            var communication = CreateCommunicationSettings(entity);
            var graphics = CreateGraphicsSettings(entity);
            var sound = CreateSoundSettings(entity);
            var contract = CreateContractSettings(entity);
            var other = CreateOtherSettings(entity);
            var contactInformation = CreateContactInformationSettings(entity);
            var organization = CreateOrganizationSettings(entity);
            var place = CreatePlaceSettings(entity);
            var contact = CreateContactSettings(entity);
            var state = CreateStateSettings(entity);
            var date = CreateDateSettings(entity);

            var settings = new ComputerFieldsSettingsProcessing(
                date,
                communication,
                contact,
                contactInformation,
                contract,
                graphics,
                other,
                place,
                sound,
                state,
                chassis,
                inventering,
                memory,
                operatingSystem,
                organization,
                processor,
                workstation);

            return settings;
        }

        private static WorkstationFieldsSettings CreateWorkstationSettings(NamedObjectCollection<FieldProcessingSettingMapperData> entity)
        {
            var name = CreateFieldSetting(entity.FindByName(WorkstationFields.Name));
            var manufacturer = CreateFieldSetting(entity.FindByName(WorkstationFields.Manufacturer));
            var model = CreateFieldSetting(entity.FindByName(WorkstationFields.Model));
            var serialNumber = CreateFieldSetting(entity.FindByName(WorkstationFields.SerialNumber));
            var biosDate = CreateFieldSetting(entity.FindByName(WorkstationFields.BIOSDate));
            var biosVersion = CreateFieldSetting(entity.FindByName(WorkstationFields.BIOSVersion));
            var theftMark = CreateFieldSetting(entity.FindByName(WorkstationFields.Theftmark));
            var carepackNumber = CreateFieldSetting(entity.FindByName(WorkstationFields.CarePackNumber));
            var computerType = CreateFieldSetting(entity.FindByName(WorkstationFields.ComputerType));
            var place = CreateFieldSetting(entity.FindByName(WorkstationFields.Location));

            var settings = new WorkstationFieldsSettings(
                name,
                manufacturer,
                model,
                serialNumber,
                biosVersion,
                biosDate,
                theftMark,
                carepackNumber,
                computerType,
                place);

            return settings;
        }

        private static ChassisFieldsSettings CreateChassisSettings(NamedObjectCollection<FieldProcessingSettingMapperData> entity)
        {
            var chassis = CreateFieldSetting(entity.FindByName(ChassisFields.Chassis));

            var settings = new ChassisFieldsSettings(chassis);

            return settings;
        }

        private static InventoryFieldsSettings CreateInventeringSettings(NamedObjectCollection<FieldProcessingSettingMapperData> entity)
        {
            var barCode = CreateFieldSetting(entity.FindByName(InventoryFields.BarCode));
            var purchaseDate = CreateFieldSetting(entity.FindByName(InventoryFields.PurchaseDate));

            var settings = new InventoryFieldsSettings(barCode, purchaseDate);

            return settings;
        }

        private static OperatingSystemFieldsSettings CretateOperatingSystemSettings(NamedObjectCollection<FieldProcessingSettingMapperData> entity)
        {
            var operatingSystem = CreateFieldSetting(entity.FindByName(OperatingSystemFields.OS));
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

        private static ProcessorFieldsSettings CreateProcessorSettings(NamedObjectCollection<FieldProcessingSettingMapperData> entity)
        {
            var processor = CreateFieldSetting(entity.FindByName(ProcessorFields.ProccesorName));

            var settings = new ProcessorFieldsSettings(processor);

            return settings;
        }

        private static MemoryFieldsSettings CreateMemorySettings(NamedObjectCollection<FieldProcessingSettingMapperData> entity)
        {
            var memory = CreateFieldSetting(entity.FindByName(MemoryFields.RAM));

            var settings = new MemoryFieldsSettings(memory);

            return settings;
        }

        private static CommunicationFieldsSettings CreateCommunicationSettings(NamedObjectCollection<FieldProcessingSettingMapperData> entity)
        {
            var networkAdapter = CreateFieldSetting(entity.FindByName(CommunicationFields.NetworkAdapter));
            var ipAddress = CreateFieldSetting(entity.FindByName(CommunicationFields.IPAddress));
            var macAddress = CreateFieldSetting(entity.FindByName(CommunicationFields.MacAddress));
            var ras = CreateFieldSetting(entity.FindByName(CommunicationFields.RAS));
            var novellClient = CreateFieldSetting(entity.FindByName(CommunicationFields.NovellClient));

            var settings = new CommunicationFieldsSettings(
                networkAdapter,
                ipAddress,
                macAddress,
                ras,
                novellClient);

            return settings;
        }

        private static GraphicsFieldsSettings CreateGraphicsSettings(NamedObjectCollection<FieldProcessingSettingMapperData> entity)
        {
            var graphics = CreateFieldSetting(entity.FindByName(GraphicsFields.VideoCard));

            var settings = new GraphicsFieldsSettings(graphics);

            return settings;
        }

        private static SoundFieldsSettings CreateSoundSettings(NamedObjectCollection<FieldProcessingSettingMapperData> entity)
        {
            var sound = CreateFieldSetting(entity.FindByName(SoundFields.SoundCard));

            var settings = new SoundFieldsSettings(sound);

            return settings;
        }

        private static ContractFieldsSettings CreateContractSettings(NamedObjectCollection<FieldProcessingSettingMapperData> entity)
        {
            var contractStatusName = CreateFieldSetting(entity.FindByName(ContractFields.ContractStatusName));
            var contractNumber = CreateFieldSetting(entity.FindByName(ContractFields.ContractNumber));
            var contractStartDate = CreateFieldSetting(entity.FindByName(ContractFields.ContractStartDate));
            var contractEndDate = CreateFieldSetting(entity.FindByName(ContractFields.ContractEndDate));
            var purchasePrice = CreateFieldSetting(entity.FindByName(ContractFields.PurchasePrice));
            var purchaseDate = CreateFieldSetting(entity.FindByName(InventoryFields.PurchaseDate)); // todo should be removed from computer contract models
            var accountingDimension1 = CreateFieldSetting(entity.FindByName(ContractFields.AccountingDimension1));
            var accountingDimension2 = CreateFieldSetting(entity.FindByName(ContractFields.AccountingDimension2));
            var accountingDimension3 = CreateFieldSetting(entity.FindByName(ContractFields.AccountingDimension3));
            var accountingDimension4 = CreateFieldSetting(entity.FindByName(ContractFields.AccountingDimension4));
            var accountingDimension5 = CreateFieldSetting(entity.FindByName(ContractFields.AccountingDimension5));
            var warrantyEndDate = CreateFieldSetting(entity.FindByName(ContractFields.WarrantyEndDate));

            var settings = new ContractFieldsSettings(
                contractStatusName,
                contractNumber,
                contractStartDate,
                contractEndDate,
                purchasePrice,
                purchaseDate,
                accountingDimension1,
                accountingDimension2,
                accountingDimension3,
                accountingDimension4,
                accountingDimension5,
                warrantyEndDate);

            return settings;
        }

        private static OtherFieldsSettings CreateOtherSettings(NamedObjectCollection<FieldProcessingSettingMapperData> entity)
        {
            var info = CreateFieldSetting(entity.FindByName(OtherFields.Info));

            var settings = new OtherFieldsSettings(info);

            return settings;
        }

        private static ContactInformationFieldsSettings CreateContactInformationSettings(NamedObjectCollection<FieldProcessingSettingMapperData> entity)
        {
            var userId = CreateFieldSetting(entity.FindByName(ContactInformationFields.UserId));

            var settings = new ContactInformationFieldsSettings(userId);

            return settings;
        }

        private static OrganizationFieldsSettings CreateOrganizationSettings(NamedObjectCollection<FieldProcessingSettingMapperData> entity)
        {
            var region = CreateFieldSetting(entity.FindByName(OrganizationFields.Region));
            var department = CreateFieldSetting(entity.FindByName(OrganizationFields.Department));
            var domain = CreateFieldSetting(entity.FindByName(OrganizationFields.Domain));
            var unit = CreateFieldSetting(entity.FindByName(OrganizationFields.Unit));

            var settings = new OrganizationFieldsSettings(region, department, domain, unit);

            return settings;
        }

        private static PlaceFieldsSettings CreatePlaceSettings(NamedObjectCollection<FieldProcessingSettingMapperData> entity)
        {
            var room = CreateFieldSetting(entity.FindByName(PlaceFields.Room));
            var building = CreateFieldSetting(entity.FindByName(PlaceFields.Building));
            var floor = CreateFieldSetting(entity.FindByName(PlaceFields.Floor));
            var address = CreateFieldSetting(entity.FindByName(PlaceFields.Address));
            var postalCode = CreateFieldSetting(entity.FindByName(PlaceFields.PostalCode));
            var postalAddress = CreateFieldSetting(entity.FindByName(PlaceFields.PostalAddress));
            var location = CreateFieldSetting(entity.FindByName(PlaceFields.Location));
            var location2 = CreateFieldSetting(entity.FindByName(PlaceFields.Location2));

            var settings = new PlaceFieldsSettings(room, building, floor,
                address, postalCode, 
                postalAddress, location, location2);

            return settings;
        }

        private static ContactFieldsSettings CreateContactSettings(NamedObjectCollection<FieldProcessingSettingMapperData> entity)
        {
            var name = CreateFieldSetting(entity.FindByName(ContactFields.Name));
            var phone = CreateFieldSetting(entity.FindByName(ContactFields.Phone));
            var email = CreateFieldSetting(entity.FindByName(ContactFields.Email));

            var settings = new ContactFieldsSettings(name, phone, email);

            return settings;
        }

        private static StateFieldsSettings CreateStateSettings(NamedObjectCollection<FieldProcessingSettingMapperData> entity)
        {
            var state = CreateFieldSetting(entity.FindByName(StateFields.State));
            var stolen = CreateFieldSetting(entity.FindByName(StateFields.Stolen));
            var replaced = CreateFieldSetting(entity.FindByName(StateFields.Replaced));
            var sendBack = CreateFieldSetting(entity.FindByName(StateFields.SendBack));
            var scrapDate = CreateFieldSetting(entity.FindByName(StateFields.ScrapDate));

            var settings = new StateFieldsSettings(state, stolen, replaced, sendBack, scrapDate);

            return settings;
        }

        private static DateFieldsSettings CreateDateSettings(NamedObjectCollection<FieldProcessingSettingMapperData> entity)
        {
            var createdDate = CreateFieldSetting(entity.FindByName(DateFields.CreatedDate));
            var changedDate = CreateFieldSetting(entity.FindByName(DateFields.ChangedDate));
            var synchronizeDate = CreateFieldSetting(entity.FindByName(DateFields.SynchronizeDate));
            var scanDate = CreateFieldSetting(entity.FindByName(DateFields.ScanDate));
            var pathDirectory = CreateFieldSetting(entity.FindByName(DateFields.PathDirectory));

            var settings = new DateFieldsSettings(createdDate, changedDate, synchronizeDate, scanDate, pathDirectory);

            return settings;
        }

        private static ProcessingFieldSetting CreateFieldSetting(FieldProcessingSettingMapperData fieldSetting)
        {
            return new ProcessingFieldSetting(
                fieldSetting.Show.ToBool(),
                fieldSetting.Required.ToBool(),
                fieldSetting.ReadOnly.ToBool(),
                fieldSetting.Copy.ToBool());
        }
    }
}