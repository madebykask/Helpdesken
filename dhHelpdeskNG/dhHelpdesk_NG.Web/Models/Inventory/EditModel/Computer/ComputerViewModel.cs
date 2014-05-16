namespace DH.Helpdesk.Web.Models.Inventory.EditModel.Computer
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Web.Mvc;

    using DH.Helpdesk.BusinessData.Models.Inventory;
    using DH.Helpdesk.BusinessData.Models.Inventory.Output.Settings.ModelEdit;
    using DH.Helpdesk.BusinessData.Models.Inventory.Output.Settings.ModelEdit.ComputerSettings;
    using DH.Helpdesk.BusinessData.Models.Shared;
    using DH.Helpdesk.BusinessData.Models.Shared.Output;
    using DH.Helpdesk.Common.ValidationAttributes;
    using DH.Helpdesk.Services.Response.Inventory;
    using DH.Helpdesk.Web.Models.Inventory.EditModel;
    using DH.Helpdesk.Web.Models.Inventory.EditModel.Shared;

    public class ComputerViewModel
    {
        public ComputerViewModel()
        {
        }

        public ComputerViewModel(
            int? customerId,
            ConfigurableFieldModel<DateTime> createdDate,
            ConfigurableFieldModel<DateTime> changedDate,
            DateFieldsModel dateFields,
            CommunicationFieldsViewModel communicationFieldsViewModel,
            ContactFieldsModel contactFields,
            ContactInformationFieldsModel contactInformationFields,
            ContractFieldsViewModel contractFieldsViewModel,
            GraphicsFieldsModel graphicsFields,
            OtherFieldsModel otherFields,
            PlaceFieldsViewModel placeFieldsViewModel,
            SoundFieldsModel soundFields,
            StateFieldsViewModel stateFieldsViewModel,
            ChassisFieldsModel chassisFields,
            InventoryFieldsModel inventoryFields,
            MemoryFieldsViewModel memoryFieldsViewModel,
            OperatingSystemFieldsViewModel operatingSystemFieldsViewModel,
            OrganizationFieldsViewModel organizationFieldsViewModel,
            ProccesorFieldsViewModel proccesorFieldsViewModel,
            WorkstationFieldsViewModel workstationFieldsViewModel)
        {
            this.CustomerId = customerId;
            this.CreatedDate = createdDate;
            this.ChangedDate = changedDate;
            this.DateFieldsModel = dateFields;
            this.CommunicationFieldsViewModel = communicationFieldsViewModel;
            this.ContactFieldsModel = contactFields;
            this.ContactInformationFieldsModel = contactInformationFields;
            this.ContractFieldsViewModel = contractFieldsViewModel;
            this.GraphicsFieldsModel = graphicsFields;
            this.OtherFieldsModel = otherFields;
            this.PlaceFieldsViewModel = placeFieldsViewModel;
            this.SoundFieldsModel = soundFields;
            this.StateFieldsViewModel = stateFieldsViewModel;
            this.ChassisFieldsModel = chassisFields;
            this.InventoryFieldsModel = inventoryFields;
            this.MemoryFieldsViewModel = memoryFieldsViewModel;
            this.OperatingSystemFieldsViewModel = operatingSystemFieldsViewModel;
            this.OrganizationFieldsViewModel = organizationFieldsViewModel;
            this.ProccesorFieldsViewModel = proccesorFieldsViewModel;
            this.WorkstationFieldsViewModel = workstationFieldsViewModel;
        }

        [IsId]
        public int Id { get; set; }

        [IsId]
        public int? CustomerId { get; private set; }

        public ConfigurableFieldModel<DateTime> CreatedDate { get; private set; }

        public ConfigurableFieldModel<DateTime> ChangedDate { get; private set; }

        [NotNull]
        public DateFieldsModel DateFieldsModel { get; private set; }

        [NotNull]
        public CommunicationFieldsViewModel CommunicationFieldsViewModel { get; private set; }

        [NotNull]
        public ContactFieldsModel ContactFieldsModel { get; private set; }

        [NotNull]
        public ContactInformationFieldsModel ContactInformationFieldsModel { get; private set; }

        [NotNull]
        public ContractFieldsViewModel ContractFieldsViewModel { get; private set; }

        [NotNull]
        public GraphicsFieldsModel GraphicsFieldsModel { get; private set; }

        [NotNull]
        public OtherFieldsModel OtherFieldsModel { get; private set; }

        [NotNull]
        public PlaceFieldsViewModel PlaceFieldsViewModel { get; private set; }

        [NotNull]
        public SoundFieldsModel SoundFieldsModel { get; private set; }

        [NotNull]
        public StateFieldsViewModel StateFieldsViewModel { get; private set; }

        [NotNull]
        public ChassisFieldsModel ChassisFieldsModel { get; private set; }

        [NotNull]
        public InventoryFieldsModel InventoryFieldsModel { get; private set; }

        [NotNull]
        public MemoryFieldsViewModel MemoryFieldsViewModel { get; private set; }

        [NotNull]
        public OperatingSystemFieldsViewModel OperatingSystemFieldsViewModel { get; private set; }

        [NotNull]
        public OrganizationFieldsViewModel OrganizationFieldsViewModel { get; private set; }

        [NotNull]
        public ProccesorFieldsViewModel ProccesorFieldsViewModel { get; private set; }

        [NotNull]
        public WorkstationFieldsViewModel WorkstationFieldsViewModel { get; private set; }

        public static ComputerViewModel BuildViewModel(
            BusinessData.Models.Inventory.Edit.Computer.Computer model,
            ComputerEditOptionsResponse options,
            ComputerFieldsSettingsForModelEdit settings)
        {
            var createdDate = CreateDateTimeField(
                settings.DateFieldsSettings.CreatedDateFieldSetting,
                model.CreatedDate);
            var changedDate = CreateDateTimeField(
                settings.DateFieldsSettings.ChangedDateFieldSetting,
                model.ChangedDate);

            var name = CreateStringField(
                settings.WorkstationFieldsSettings.ComputerNameFieldSetting,
                model.WorkstationFields.ComputerName);
            var manufacturer = CreateStringField(
                settings.WorkstationFieldsSettings.ManufacturerFieldSetting,
                model.WorkstationFields.Manufacturer);
            var serial = CreateStringField(
                settings.WorkstationFieldsSettings.SerialNumberFieldSetting,
                model.WorkstationFields.SerialNumber);
            var biosVersion = CreateStringField(
                settings.WorkstationFieldsSettings.BIOSVersionFieldSetting,
                model.WorkstationFields.BIOSVersion);
            var biosDate = CreateNullableDateTimeField(
                settings.WorkstationFieldsSettings.BIOSDateFieldSetting,
                model.WorkstationFields.BIOSDate);
            var theftMark = CreateStringField(
                settings.WorkstationFieldsSettings.TheftmarkFieldSetting,
                model.WorkstationFields.Theftmark);
            var carePackNumber = CreateStringField(
                settings.WorkstationFieldsSettings.CarePackNumberFieldSetting,
                model.WorkstationFields.CarePackNumber);
            var location = CreateStringField(
                settings.WorkstationFieldsSettings.LocationFieldSetting,
                model.WorkstationFields.Location);

            var workstationFieldsModel = new WorkstationFieldsModel(
                name,
                manufacturer,
                model.WorkstationFields.ComputerModelId,
                serial,
                biosVersion,
                biosDate,
                theftMark,
                carePackNumber,
                model.WorkstationFields.ComputerTypeId,
                location);
            var computerModels = CreateSelectListField(
                settings.WorkstationFieldsSettings.ComputerModelFieldSetting,
                options.ComputerModels,
                model.WorkstationFields.ComputerModelId.ToString());
            var computerTypes = CreateSelectListField(
                settings.WorkstationFieldsSettings.ComputerTypeFieldSetting,
                options.ComputerTypes,
                model.WorkstationFields.ComputerTypeId.ToString());

            var workstationViewModel = new WorkstationFieldsViewModel(
                workstationFieldsModel,
                computerModels,
                computerTypes);

            var proccesorFieldsModel = new ProccesorFieldsModel(model.ProccesorFields.ProccesorId);
            var processors = CreateSelectListField(
                settings.ProccesorFieldsSettings.ProccesorFieldSetting,
                options.Processors,
                model.ProccesorFields.ProccesorId.ToString());

            var processorViewModel = new ProccesorFieldsViewModel(proccesorFieldsModel, processors);

            var organizationFieldsModel = new OrganizationFieldsModel(
                model.OrganizationFields.DepartmentId,
                model.OrganizationFields.DomainId,
                model.OrganizationFields.UnitId);
            var departments = CreateSelectListField(
                settings.OrganizationFieldsSettings.DepartmentFieldSetting,
                options.Departments,
                model.OrganizationFields.DepartmentId.ToString());
            var domains = CreateSelectListField(
                settings.OrganizationFieldsSettings.DomainFieldSetting,
                options.Domains,
                model.OrganizationFields.DomainId.ToString());
            var ous = CreateSelectListField(
                settings.OrganizationFieldsSettings.UnitFieldSetting,
                options.Units,
                model.OrganizationFields.UnitId.ToString());

            var organizationViewModel = new OrganizationFieldsViewModel(
                organizationFieldsModel,
                departments,
                domains,
                ous);

            var version = CreateStringField(
                settings.OperatingSystemFieldsSettings.VersionFieldSetting,
                model.OperatingSystemFields.Version);
            var servicePack = CreateStringField(
                settings.OperatingSystemFieldsSettings.ServicePackSystemFieldSetting,
                model.OperatingSystemFields.ServicePack);
            var registratinCode =
                CreateStringField(
                    settings.OperatingSystemFieldsSettings.RegistrationCodeSystemFieldSetting,
                    model.OperatingSystemFields.RegistrationCode);
            var productKey = CreateStringField(
                settings.OperatingSystemFieldsSettings.ProductKeyFieldSetting,
                model.OperatingSystemFields.ProductKey);

            var operatingSystemFieldsModel =
                new OperatingSystemFieldsModel(
                    model.OperatingSystemFields.OperatingSystemId,
                    version,
                    servicePack,
                    registratinCode,
                    productKey);
            var operatingSystems =
                CreateSelectListField(
                    settings.OperatingSystemFieldsSettings.OperatingSystemFieldSetting,
                    options.OperatingSystems,
                    model.OperatingSystemFields.OperatingSystemId.ToString());

            var operatingSystemsViewModel = new OperatingSystemFieldsViewModel(
                operatingSystemFieldsModel,
                operatingSystems);

            var memoryFieldsModel = new MemoryFieldsModel(model.MemoryFields.RAMId);
            var memories = CreateSelectListField(
                settings.MemoryFieldsSettings.RAMFieldSetting,
                options.Rams,
                model.MemoryFields.RAMId.ToString());

            var memoryViewModel = new MemoryFieldsViewModel(memoryFieldsModel, memories);

            var barCode = CreateStringField(
                settings.InventoryFieldsSettings.BarCodeFieldSetting,
                model.InventoryFields.BarCode);
            var purchaseDate = CreateNullableDateTimeField(
                settings.InventoryFieldsSettings.PurchaseDateFieldSetting,
                model.InventoryFields.PurchaseDate);

            var inventoryFieldModel = new InventoryFieldsModel(barCode, purchaseDate);

            var chassis = CreateStringField(
                settings.ChassisFieldsSettings.ChassisFieldSetting,
                model.ChassisFields.Chassis);

            var chassisFieldModel = new ChassisFieldsModel(chassis);

            var stolen = CreateBooleanField(settings.StateFieldsSettings.StolenFieldSetting, model.StateFields.IsStolen);
            var replaced = CreateStringField(
                settings.StateFieldsSettings.ReplacedWithFieldSetting,
                model.StateFields.Replaced);
            var sendBack = CreateBooleanField(
                settings.StateFieldsSettings.SendBackFieldSetting,
                model.StateFields.IsSendBack);
            var scrapDate = CreateNullableDateTimeField(
                settings.StateFieldsSettings.ScrapDateFieldSetting,
                model.StateFields.ScrapDate);

            var stateFieldsModel = new StateFieldsModel(model.StateFields.State, stolen, replaced, sendBack, scrapDate);

            var statuses = CreateSelectListField(
                settings.StateFieldsSettings.StateFieldSetting,
                Enum.GetValues(typeof(ComputerStatuses)),
                model.StateFields.State.ToString(CultureInfo.InvariantCulture));

            var stateViewModel = new StateFieldsViewModel(stateFieldsModel, statuses);

            var sound = CreateStringField(
                settings.SoundFieldsSettings.SoundCardFieldSetting,
                model.SoundFields.SoundCard);

            var soundFieldModel = new SoundFieldsModel(sound);

            var address = CreateStringField(settings.PlaceFieldsSettings.AddressFieldSetting, model.PlaceFields.Address);
            var postalCode = CreateStringField(
                settings.PlaceFieldsSettings.PostalCodeFieldSetting,
                model.PlaceFields.PostalCode);
            var postalAddress = CreateStringField(
                settings.PlaceFieldsSettings.PostalAddressFieldSetting,
                model.PlaceFields.PostalAddress);
            var location1 = CreateStringField(
                settings.PlaceFieldsSettings.PlaceFieldSetting,
                model.PlaceFields.Location);
            var location2 = CreateStringField(
                settings.PlaceFieldsSettings.Place2FieldSetting,
                model.PlaceFields.Location2);

            var placeFieldsModel = new PlaceFieldsModel(
                model.PlaceFields.BuildingId,
                model.PlaceFields.FloorId,
                model.PlaceFields.RoomId,
                address,
                postalCode,
                postalAddress,
                location1,
                location2);

            var buildings = CreateSelectList(
                settings.PlaceFieldsSettings.RoomFieldSetting,
                options.Buildings,
                model.PlaceFields.BuildingId.ToString());
            var floors = CreateSelectList(
                settings.PlaceFieldsSettings.RoomFieldSetting,
                options.Floors,
                model.PlaceFields.FloorId.ToString());
            var rooms = CreateSelectListField(
                settings.PlaceFieldsSettings.RoomFieldSetting,
                options.Rooms,
                model.PlaceFields.RoomId.ToString());

            var placeFieldsViewModel = new PlaceFieldsViewModel(placeFieldsModel, buildings, floors, rooms);

            var other = CreateStringField(settings.OtherFieldsSettings.InfoFieldSetting, model.OtherFields.Info);

            var otherFieldModel = new OtherFieldsModel(other);

            var graphics = CreateStringField(
                settings.GraphicsFieldsSettings.VideoCardFieldSetting,
                model.GraphicsFields.VideoCard);

            var graphicsFieldModel = new GraphicsFieldsModel(graphics);

            var contractNumber = CreateStringField(
                settings.ContractFieldsSettings.ContractNumberFieldSetting,
                model.ContractFields.ContractNumber);
            var contractStartDate =
                CreateNullableDateTimeField(
                    settings.ContractFieldsSettings.ContractStartDateFieldSetting,
                    model.ContractFields.ContractStartDate);
            var contractEndDate =
                CreateNullableDateTimeField(
                    settings.ContractFieldsSettings.ContractEndDateFieldSetting,
                    model.ContractFields.ContractEndDate);
            var price = CreateIntegerField(
                settings.ContractFieldsSettings.PurchasePriceFieldSetting,
                model.ContractFields.PurchasePrice);
            var accounting1 = CreateStringField(
                settings.ContractFieldsSettings.AccountingDimension1FieldSetting,
                model.ContractFields.AccountingDimension1);
            var accounting2 = CreateStringField(
                settings.ContractFieldsSettings.AccountingDimension2FieldSetting,
                model.ContractFields.AccountingDimension2);
            var accounting3 = CreateStringField(
                settings.ContractFieldsSettings.AccountingDimension3FieldSetting,
                model.ContractFields.AccountingDimension3);
            var accounting4 = CreateStringField(
                settings.ContractFieldsSettings.AccountingDimension4FieldSetting,
                model.ContractFields.AccountingDimension4);
            var accounting5 = CreateStringField(
                settings.ContractFieldsSettings.AccountingDimension5FieldSetting,
                model.ContractFields.AccountingDimension5);

            var contractFieldsModel = new ContractFieldsModel(
                model.ContractFields.ContractStatusId,
                contractNumber,
                contractStartDate,
                contractEndDate,
                price,
                accounting1,
                accounting2,
                accounting3,
                accounting4,
                accounting5);

            var contractStatuses = CreateSelectListField(
                settings.StateFieldsSettings.StateFieldSetting,
                Enum.GetValues(typeof(ContractStatuses)),
                model.ContractFields.ContractStatusId.ToString());

            var contractViewModel = new ContractFieldsViewModel(contractFieldsModel, contractStatuses);

            var contactName = CreateStringField(
                settings.ContactFieldsSettings.NameFieldSetting,
                model.ContactFields.Name);
            var contactPhone = CreateStringField(
                settings.ContactFieldsSettings.PhoneFieldSetting,
                model.ContactFields.Phone);
            var contactEmail = CreateStringField(
                settings.ContactFieldsSettings.EmailFieldSetting,
                model.ContactFields.Email);

            var contactFieldsModel = new ContactFieldsModel(contactName, contactPhone, contactEmail);

            var ip = CreateStringField(
                settings.CommunicationFieldsSettings.IPAddressFieldSetting,
                model.CommunicationFields.IPAddress);
            var mac = CreateStringField(
                settings.CommunicationFieldsSettings.MacAddressFieldSetting,
                model.CommunicationFields.MacAddress);
            var ras = CreateBooleanField(
                settings.CommunicationFieldsSettings.RASFieldSetting,
                model.CommunicationFields.IsRAS);
            var client = CreateStringField(
                settings.CommunicationFieldsSettings.NovellClientFieldSetting,
                model.CommunicationFields.NovellClient);

            var communicationFieldsModel = new CommunicationFieldsModel(
                model.CommunicationFields.NetworkAdapterId,
                ip,
                mac,
                ras,
                client);

            var adapters = CreateSelectListField(
                settings.CommunicationFieldsSettings.NetworkAdapterFieldSetting,
                options.NetworkAdapters,
                model.CommunicationFields.NetworkAdapterId.ToString());

            var communicationViewModel = new CommunicationFieldsViewModel(communicationFieldsModel, adapters);

            var syncDate = CreateNullableDateTimeField(
                settings.DateFieldsSettings.SyncChangedDateSetting,
                model.DateFields.SynchronizeDate);
            var scanDate = CreateNullableDateTimeField(
                settings.DateFieldsSettings.ScanDateFieldSetting,
                model.DateFields.ScanDate);
            var path = CreateStringField(
                settings.DateFieldsSettings.PathDirectoryFieldSetting,
                model.DateFields.PathDirectory);

            var dateFieldsModel = new DateFieldsModel(syncDate, scanDate, path);

            var userStringId = CreateStringField(
                settings.ContactInformationFieldsSettings.UserIdFieldSetting,
                model.ContactInformationFields.UserStringId);

            var contactInformationFieldsModel = new ContactInformationFieldsModel(
                model.ContactInformationFields.UserId,
                userStringId,
                model.ContactInformationFields.Department,
                model.ContactInformationFields.Unit,
                model.ContactInformationFields.UserName);

            return new ComputerViewModel(
                model.CustomerId,
                createdDate,
                changedDate,
                dateFieldsModel,
                communicationViewModel,
                contactFieldsModel,
                contactInformationFieldsModel,
                contractViewModel,
                graphicsFieldModel,
                otherFieldModel,
                placeFieldsViewModel,
                soundFieldModel,
                stateViewModel,
                chassisFieldModel,
                inventoryFieldModel,
                memoryViewModel,
                operatingSystemsViewModel,
                organizationViewModel,
                processorViewModel,
                workstationViewModel) { Id = model.Id };
        }

        public static ComputerViewModel BuildViewModel(
            ComputerEditOptionsResponse options,
            ComputerFieldsSettingsForModelEdit settings,
            int currentCustomerId)
        {
            var name = CreateStringField(
                settings.WorkstationFieldsSettings.ComputerNameFieldSetting,
                null);
            var manufacturer = CreateStringField(
                settings.WorkstationFieldsSettings.ManufacturerFieldSetting,
                null);
            var serial = CreateStringField(
                settings.WorkstationFieldsSettings.SerialNumberFieldSetting,
                null);
            var biosVersion = CreateStringField(
                settings.WorkstationFieldsSettings.BIOSVersionFieldSetting,
                null);
            var biosDate = CreateNullableDateTimeField(
                settings.WorkstationFieldsSettings.BIOSDateFieldSetting,
                null);
            var theftMark = CreateStringField(
                settings.WorkstationFieldsSettings.TheftmarkFieldSetting,
                null);
            var carePackNumber = CreateStringField(
                settings.WorkstationFieldsSettings.CarePackNumberFieldSetting,
                null);
            var location = CreateStringField(
                settings.WorkstationFieldsSettings.LocationFieldSetting,
                null);

            var workstationFieldsModel = new WorkstationFieldsModel(
                name,
                manufacturer,
                null,
                serial,
                biosVersion,
                biosDate,
                theftMark,
                carePackNumber,
                null,
                location);
            var computerTypes = CreateSelectListField(
                settings.WorkstationFieldsSettings.ComputerTypeFieldSetting,
                options.ComputerTypes,
                null);
            var computerModels = CreateSelectListField(
                settings.WorkstationFieldsSettings.ComputerModelFieldSetting,
                options.ComputerModels,
                null);

            var workstationViewModel = new WorkstationFieldsViewModel(
                workstationFieldsModel,
                computerTypes,
                computerModels);

            var proccesorFieldsModel = new ProccesorFieldsModel(null);
            var processors = CreateSelectListField(
                settings.ProccesorFieldsSettings.ProccesorFieldSetting,
                options.Processors,
                null);

            var processorViewModel = new ProccesorFieldsViewModel(proccesorFieldsModel, processors);

            var organizationFieldsModel = new OrganizationFieldsModel(
                null,
                null,
                null);
            var departments = CreateSelectListField(
                settings.OrganizationFieldsSettings.DepartmentFieldSetting,
                options.Departments,
                null);
            var domains = CreateSelectListField(
                settings.OrganizationFieldsSettings.DomainFieldSetting,
                options.Domains,
                null);
            var ous = CreateSelectListField(
                settings.OrganizationFieldsSettings.UnitFieldSetting,
                options.Units,
                null);

            var organizationViewModel = new OrganizationFieldsViewModel(
                organizationFieldsModel,
                departments,
                domains,
                ous);

            var version = CreateStringField(
                settings.OperatingSystemFieldsSettings.VersionFieldSetting,
                null);
            var servicePack = CreateStringField(
                settings.OperatingSystemFieldsSettings.ServicePackSystemFieldSetting,
                null);
            var registratinCode =
                CreateStringField(
                    settings.OperatingSystemFieldsSettings.RegistrationCodeSystemFieldSetting,
                    null);
            var productKey = CreateStringField(
                settings.OperatingSystemFieldsSettings.ProductKeyFieldSetting,
                null);

            var operatingSystemFieldsModel =
                new OperatingSystemFieldsModel(
                    null,
                    version,
                    servicePack,
                    registratinCode,
                    productKey);
            var operatingSystems =
                CreateSelectListField(
                    settings.OperatingSystemFieldsSettings.OperatingSystemFieldSetting,
                    options.OperatingSystems,
                    null);

            var operatingSystemsViewModel = new OperatingSystemFieldsViewModel(
                operatingSystemFieldsModel,
                operatingSystems);

            var memoryFieldsModel = new MemoryFieldsModel(null);
            var memories = CreateSelectListField(
                settings.MemoryFieldsSettings.RAMFieldSetting,
                options.Rams,
                null);

            var memoryViewModel = new MemoryFieldsViewModel(memoryFieldsModel, memories);

            var barCode = CreateStringField(
                settings.InventoryFieldsSettings.BarCodeFieldSetting,
                null);
            var purchaseDate = CreateNullableDateTimeField(
                settings.InventoryFieldsSettings.PurchaseDateFieldSetting,
                null);

            var inventoryFieldModel = new InventoryFieldsModel(barCode, purchaseDate);

            var chassis = CreateStringField(
                settings.ChassisFieldsSettings.ChassisFieldSetting,
                null);

            var chassisFieldModel = new ChassisFieldsModel(chassis);

            var stolen = CreateBooleanField(settings.StateFieldsSettings.StolenFieldSetting, false);
            var replaced = CreateStringField(
                settings.StateFieldsSettings.ReplacedWithFieldSetting,
                null);
            var sendBack = CreateBooleanField(
                settings.StateFieldsSettings.SendBackFieldSetting,
                false);
            var scrapDate = CreateNullableDateTimeField(
                settings.StateFieldsSettings.ScrapDateFieldSetting,
                null);

            var stateFieldsModel = new StateFieldsModel(0, stolen, replaced, sendBack, scrapDate);

            var statuses = CreateSelectListField(
                settings.StateFieldsSettings.StateFieldSetting,
                Enum.GetValues(typeof(ComputerStatuses)),
                null);

            var stateViewModel = new StateFieldsViewModel(stateFieldsModel, statuses);

            var sound = CreateStringField(
                settings.SoundFieldsSettings.SoundCardFieldSetting,
                null);

            var soundFieldModel = new SoundFieldsModel(sound);

            var address = CreateStringField(settings.PlaceFieldsSettings.AddressFieldSetting, null);
            var postalCode = CreateStringField(
                settings.PlaceFieldsSettings.PostalCodeFieldSetting,
                null);
            var postalAddress = CreateStringField(
                settings.PlaceFieldsSettings.PostalAddressFieldSetting,
                null);
            var location1 = CreateStringField(
                settings.PlaceFieldsSettings.PlaceFieldSetting,
                null);
            var location2 = CreateStringField(
                settings.PlaceFieldsSettings.Place2FieldSetting,
                null);

            var placeFieldsModel = new PlaceFieldsModel(
                null,
                null,
                null,
                address,
                postalCode,
                postalAddress,
                location1,
                location2);

            var buildings = CreateSelectList(
                settings.PlaceFieldsSettings.RoomFieldSetting,
                options.Buildings,
                null);
            var floors = CreateSelectList(
                settings.PlaceFieldsSettings.RoomFieldSetting,
                options.Floors,
                null);
            var rooms = CreateSelectListField(
                settings.PlaceFieldsSettings.RoomFieldSetting,
                options.Rooms,
                null);

            var placeFieldsViewModel = new PlaceFieldsViewModel(placeFieldsModel, buildings, floors, rooms);

            var other = CreateStringField(settings.OtherFieldsSettings.InfoFieldSetting, null);

            var otherFieldModel = new OtherFieldsModel(other);

            var graphics = CreateStringField(
                settings.GraphicsFieldsSettings.VideoCardFieldSetting,
                null);

            var graphicsFieldModel = new GraphicsFieldsModel(graphics);

            var contractNumber = CreateStringField(
                settings.ContractFieldsSettings.ContractNumberFieldSetting,
                null);
            var contractStartDate =
                CreateNullableDateTimeField(
                    settings.ContractFieldsSettings.ContractStartDateFieldSetting,
                    null);
            var contractEndDate =
                CreateNullableDateTimeField(
                    settings.ContractFieldsSettings.ContractEndDateFieldSetting,
                    null);
            var price = CreateIntegerField(
                settings.ContractFieldsSettings.PurchasePriceFieldSetting,
                0);
            var accounting1 = CreateStringField(
                settings.ContractFieldsSettings.AccountingDimension1FieldSetting,
                null);
            var accounting2 = CreateStringField(
                settings.ContractFieldsSettings.AccountingDimension2FieldSetting,
                null);
            var accounting3 = CreateStringField(
                settings.ContractFieldsSettings.AccountingDimension3FieldSetting,
                null);
            var accounting4 = CreateStringField(
                settings.ContractFieldsSettings.AccountingDimension4FieldSetting,
                null);
            var accounting5 = CreateStringField(
                settings.ContractFieldsSettings.AccountingDimension5FieldSetting,
                null);

            var contractFieldsModel = new ContractFieldsModel(
                null,
                contractNumber,
                contractStartDate,
                contractEndDate,
                price,
                accounting1,
                accounting2,
                accounting3,
                accounting4,
                accounting5);

            var contractStatuses = CreateSelectListField(
                settings.StateFieldsSettings.StateFieldSetting,
                Enum.GetValues(typeof(ContractStatuses)),
                null);

            var contractViewModel = new ContractFieldsViewModel(contractFieldsModel, contractStatuses);

            var contactName = CreateStringField(
                settings.ContactFieldsSettings.NameFieldSetting,
                null);
            var contactPhone = CreateStringField(
                settings.ContactFieldsSettings.PhoneFieldSetting,
                null);
            var contactEmail = CreateStringField(
                settings.ContactFieldsSettings.EmailFieldSetting,
                null);

            var contactFieldsModel = new ContactFieldsModel(contactName, contactPhone, contactEmail);

            var ip = CreateStringField(
                settings.CommunicationFieldsSettings.IPAddressFieldSetting,
                null);
            var mac = CreateStringField(
                settings.CommunicationFieldsSettings.MacAddressFieldSetting,
                null);
            var ras = CreateBooleanField(
                settings.CommunicationFieldsSettings.RASFieldSetting,
                false);
            var client = CreateStringField(
                settings.CommunicationFieldsSettings.NovellClientFieldSetting,
                null);

            var communicationFieldsModel = new CommunicationFieldsModel(
                null,
                ip,
                mac,
                ras,
                client);

            var adapters = CreateSelectListField(
                settings.CommunicationFieldsSettings.NetworkAdapterFieldSetting,
                options.NetworkAdapters,
                null);

            var communicationViewModel = new CommunicationFieldsViewModel(communicationFieldsModel, adapters);

            var syncDate = CreateNullableDateTimeField(
                settings.DateFieldsSettings.SyncChangedDateSetting,
                null);
            var scanDate = CreateNullableDateTimeField(
                settings.DateFieldsSettings.ScanDateFieldSetting,
                null);
            var path = CreateStringField(
                settings.DateFieldsSettings.PathDirectoryFieldSetting,
                null);

            var dateFieldsModel = new DateFieldsModel(syncDate, scanDate, path);

            var userStringId = CreateStringField(
                settings.ContactInformationFieldsSettings.UserIdFieldSetting,
                null);

            var contactInformationFieldsModel = new ContactInformationFieldsModel(
                null,
                userStringId,
                null,
                null,
                null);

            return new ComputerViewModel(
                currentCustomerId,
                null,
                null,
                dateFieldsModel,
                communicationViewModel,
                contactFieldsModel,
                contactInformationFieldsModel,
                contractViewModel,
                graphicsFieldModel,
                otherFieldModel,
                placeFieldsViewModel,
                soundFieldModel,
                stateViewModel,
                chassisFieldModel,
                inventoryFieldModel,
                memoryViewModel,
                operatingSystemsViewModel,
                organizationViewModel,
                processorViewModel,
                workstationViewModel);
        }

        public static SelectList CreateSelectList(
            ModelEditFieldSetting setting,
            List<ItemOverview> items,
            string selectedValue)
        {
            if (!setting.IsShow)
            {
                return new SelectList(Enumerable.Empty<SelectListItem>());
            }

            var list = new SelectList(items, "Value", "Name", selectedValue);
            return list;
        }

        public static ConfigurableFieldModel<DateTime?> CreateNullableDateTimeField(
            ModelEditFieldSetting setting,
            DateTime? value)
        {
            return !setting.IsShow
                       ? ConfigurableFieldModel<DateTime?>.CreateUnshowable()
                       : new ConfigurableFieldModel<DateTime?>(
                             setting.Caption,
                             value,
                             setting.IsRequired,
                             setting.IsReadOnly);
        }

        public static ConfigurableFieldModel<DateTime> CreateDateTimeField(ModelEditFieldSetting setting, DateTime value)
        {
            return !setting.IsShow
                       ? ConfigurableFieldModel<DateTime>.CreateUnshowable()
                       : new ConfigurableFieldModel<DateTime>(
                             setting.Caption,
                             value,
                             setting.IsRequired,
                             setting.IsReadOnly);
        }

        public static ConfigurableFieldModel<SelectList> CreateSelectListField(
            ModelEditFieldSetting setting,
            List<ItemOverview> items,
            string selectedValue)
        {
            if (!setting.IsShow)
            {
                return ConfigurableFieldModel<SelectList>.CreateUnshowable();
            }

            var list = new SelectList(items, "Value", "Name", selectedValue);
            return new ConfigurableFieldModel<SelectList>(setting.Caption, list, setting.IsRequired, setting.IsReadOnly);
        }

        public static ConfigurableFieldModel<SelectList> CreateSelectListField(
            ModelEditFieldSetting setting,
            Array items,
            string selectedValue)
        {
            if (!setting.IsShow)
            {
                return ConfigurableFieldModel<SelectList>.CreateUnshowable();
            }

            var list = new SelectList(items, selectedValue);
            return new ConfigurableFieldModel<SelectList>(setting.Caption, list, setting.IsRequired, setting.IsReadOnly);
        }

        public static ConfigurableFieldModel<string> CreateStringField(ModelEditFieldSetting setting, string value)
        {
            return !setting.IsShow
                       ? ConfigurableFieldModel<string>.CreateUnshowable()
                       : new ConfigurableFieldModel<string>(
                             setting.Caption,
                             value,
                             setting.IsRequired,
                             setting.IsReadOnly);
        }

        public static ConfigurableFieldModel<bool> CreateBooleanField(ModelEditFieldSetting setting, bool value)
        {
            return !setting.IsShow
                ? ConfigurableFieldModel<bool>.CreateUnshowable()
                : new ConfigurableFieldModel<bool>(setting.Caption, value, setting.IsRequired, setting.IsReadOnly);
        }

        public static ConfigurableFieldModel<int> CreateIntegerField(ModelEditFieldSetting setting, int value)
        {
            return !setting.IsShow
                ? ConfigurableFieldModel<int>.CreateUnshowable()
                : new ConfigurableFieldModel<int>(setting.Caption, value, setting.IsRequired, setting.IsReadOnly);
        }
    }
}