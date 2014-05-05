namespace DH.Helpdesk.Web.Models.Inventory.EditModel.Computer
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Web.Mvc;

    using DH.Helpdesk.BusinessData.Models.Common.Output;
    using DH.Helpdesk.BusinessData.Models.Inventory;
    using DH.Helpdesk.BusinessData.Models.Inventory.Output.Settings.ModelEdit;
    using DH.Helpdesk.BusinessData.Models.Inventory.Output.Settings.ModelEdit.ComputerSettings;
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
            int id,
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
            this.Id = id;
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
        public int Id { get; private set; }

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

        public static ComputerViewModel BuildViewModel(ComputerEditAggregate editAggregate, ComputerFieldsSettingsForModelEdit settings)
        {
            var createdDate = CreateDateTimeField(settings.DateFieldsSettings.CreatedDateFieldSetting, editAggregate.Computer.CreatedDate);
            var changedDate = CreateDateTimeField(settings.DateFieldsSettings.ChangedDateFieldSetting, editAggregate.Computer.ChangedDate);

            var name = CreateStringField(
                settings.WorkstationFieldsSettings.ComputerNameFieldSetting,
                editAggregate.Computer.WorkstationFields.ComputerName);
            var manufacturer = CreateStringField(
                settings.WorkstationFieldsSettings.ManufacturerFieldSetting,
                editAggregate.Computer.WorkstationFields.Manufacturer);
            var serial = CreateStringField(
                settings.WorkstationFieldsSettings.SerialNumberFieldSetting,
                editAggregate.Computer.WorkstationFields.SerialNumber);
            var biosVersion = CreateStringField(
                settings.WorkstationFieldsSettings.BIOSVersionFieldSetting,
                editAggregate.Computer.WorkstationFields.BIOSVersion);
            var biosDate = CreateNullableDateTimeField(
                settings.WorkstationFieldsSettings.BIOSDateFieldSetting,
                editAggregate.Computer.WorkstationFields.BIOSDate);
            var theftMark = CreateStringField(
                settings.WorkstationFieldsSettings.TheftmarkFieldSetting,
                editAggregate.Computer.WorkstationFields.Theftmark);
            var carePackNumber =
                CreateStringField(
                    settings.WorkstationFieldsSettings.CarePackNumberFieldSetting,
                    editAggregate.Computer.WorkstationFields.CarePackNumber);
            var location = CreateStringField(
                settings.WorkstationFieldsSettings.LocationFieldSetting,
                editAggregate.Computer.WorkstationFields.Location);

            var workstationFieldsModel = new WorkstationFieldsModel(
                name,
                manufacturer,
                editAggregate.Computer.WorkstationFields.ComputerModelId,
                serial,
                biosVersion,
                biosDate,
                theftMark,
                carePackNumber,
                editAggregate.Computer.WorkstationFields.ComputerTypeId,
                location);
            var computerTypes =
                CreateSelectListField(
                    settings.WorkstationFieldsSettings.ComputerTypeFieldSetting,
                    editAggregate.ComputerTypes,
                    editAggregate.Computer.WorkstationFields.ComputerTypeId.ToString());
            var computerModels =
                CreateSelectListField(
                    settings.WorkstationFieldsSettings.ComputerModelFieldSetting,
                    editAggregate.ComputerModels,
                    editAggregate.Computer.WorkstationFields.ComputerModelId.ToString());

            var workstationViewModel = new WorkstationFieldsViewModel(
                workstationFieldsModel,
                computerTypes,
                computerModels);

            var proccesorFieldsModel = new ProccesorFieldsModel(editAggregate.Computer.ProccesorFields.ProccesorId);
            var processors = CreateSelectListField(
                settings.ProccesorFieldsSettings.ProccesorFieldSetting,
                editAggregate.Processors,
                editAggregate.Computer.ProccesorFields.ProccesorId.ToString());

            var processorViewModel = new ProccesorFieldsViewModel(proccesorFieldsModel, processors);

            var organizationFieldsModel = new OrganizationFieldsModel(
                editAggregate.Computer.OrganizationFields.DepartmentId,
                editAggregate.Computer.OrganizationFields.DomainId,
                editAggregate.Computer.OrganizationFields.UnitId);
            var departments = CreateSelectListField(
                settings.OrganizationFieldsSettings.DepartmentFieldSetting,
                editAggregate.Departments,
                editAggregate.Computer.OrganizationFields.DepartmentId.ToString());
            var domains = CreateSelectListField(
                settings.OrganizationFieldsSettings.DomainFieldSetting,
                editAggregate.Domains,
                editAggregate.Computer.OrganizationFields.DomainId.ToString());
            var ous = CreateSelectListField(
                settings.OrganizationFieldsSettings.UnitFieldSetting,
                editAggregate.Units,
                editAggregate.Computer.OrganizationFields.UnitId.ToString());

            var organizationViewModel = new OrganizationFieldsViewModel(
                organizationFieldsModel,
                departments,
                domains,
                ous);

            var version = CreateStringField(
                settings.OperatingSystemFieldsSettings.VersionFieldSetting,
                editAggregate.Computer.OperatingSystemFields.Version);
            var servicePack = CreateStringField(
                settings.OperatingSystemFieldsSettings.ServicePackSystemFieldSetting,
                editAggregate.Computer.OperatingSystemFields.ServicePack);
            var registratinCode = CreateStringField(
                settings.OperatingSystemFieldsSettings.RegistrationCodeSystemFieldSetting,
                editAggregate.Computer.OperatingSystemFields.RegistrationCode);
            var productKey = CreateStringField(
                settings.OperatingSystemFieldsSettings.ProductKeyFieldSetting,
                editAggregate.Computer.OperatingSystemFields.ProductKey);

            var operatingSystemFieldsModel =
                new OperatingSystemFieldsModel(
                    editAggregate.Computer.OperatingSystemFields.OperatingSystemId,
                    version,
                    servicePack,
                    registratinCode,
                    productKey);
            var operatingSystems = CreateSelectListField(
                settings.OperatingSystemFieldsSettings.OperatingSystemFieldSetting,
                editAggregate.OperatingSystems,
                editAggregate.Computer.OperatingSystemFields.OperatingSystemId.ToString());

            var operatingSystemsViewModel = new OperatingSystemFieldsViewModel(operatingSystemFieldsModel, operatingSystems);

            var memoryFieldsModel = new MemoryFieldsModel(editAggregate.Computer.MemoryFields.RAMId);
            var memories = CreateSelectListField(
                settings.MemoryFieldsSettings.RAMFieldSetting,
                editAggregate.Rams,
                editAggregate.Computer.MemoryFields.RAMId.ToString());

            var memoryViewModel = new MemoryFieldsViewModel(memoryFieldsModel, memories);

            var barCode = CreateStringField(
                settings.InventoryFieldsSettings.BarCodeFieldSetting,
                editAggregate.Computer.InventoryFields.BarCode);
            var purchaseDate = CreateNullableDateTimeField(
                settings.InventoryFieldsSettings.PurchaseDateFieldSetting,
                editAggregate.Computer.InventoryFields.PurchaseDate);

            var inventoryFieldModel = new InventoryFieldsModel(barCode, purchaseDate);

            var chassis = CreateStringField(
                settings.ChassisFieldsSettings.ChassisFieldSetting,
                editAggregate.Computer.ChassisFields.Chassis);

            var chassisFieldModel = new ChassisFieldsModel(chassis);

            var stolen = CreateBooleanField(
                settings.StateFieldsSettings.StolenFieldSetting,
                editAggregate.Computer.StateFields.IsStolen);
            var replaced = CreateStringField(
                settings.StateFieldsSettings.ReplacedWithFieldSetting,
                editAggregate.Computer.StateFields.Replaced);
            var sendBack = CreateBooleanField(
                settings.StateFieldsSettings.SendBackFieldSetting,
                editAggregate.Computer.StateFields.IsSendBack);
            var scrapDate = CreateNullableDateTimeField(
                settings.StateFieldsSettings.ScrapDateFieldSetting,
                editAggregate.Computer.StateFields.ScrapDate);

            var stateFieldsModel = new StateFieldsModel(
                editAggregate.Computer.StateFields.State,
                stolen,
                replaced,
                sendBack,
                scrapDate);

            var statuses = CreateSelectListField(
                settings.StateFieldsSettings.StateFieldSetting,
                Enum.GetValues(typeof(ComputerStatuses)),
                editAggregate.Computer.StateFields.State.ToString(CultureInfo.InvariantCulture));

            var stateViewModel = new StateFieldsViewModel(stateFieldsModel, statuses);

            var sound = CreateStringField(
                settings.SoundFieldsSettings.SoundCardFieldSetting,
                editAggregate.Computer.SoundFields.SoundCard);

            var soundFieldModel = new SoundFieldsModel(sound);

            var address = CreateStringField(
                settings.PlaceFieldsSettings.AddressFieldSetting,
                editAggregate.Computer.PlaceFields.Address);
            var postalCode = CreateStringField(
                settings.PlaceFieldsSettings.PostalCodeFieldSetting,
                editAggregate.Computer.PlaceFields.PostalCode);
            var postalAddress = CreateStringField(
                settings.PlaceFieldsSettings.PostalAddressFieldSetting,
                editAggregate.Computer.PlaceFields.PostalAddress);
            var location1 = CreateStringField(
                settings.PlaceFieldsSettings.PlaceFieldSetting,
                editAggregate.Computer.PlaceFields.Location);
            var location2 = CreateStringField(
                settings.PlaceFieldsSettings.Place2FieldSetting,
                editAggregate.Computer.PlaceFields.Location2);

            var placeFieldsModel = new PlaceFieldsModel(
                editAggregate.Computer.PlaceFields.BuildingId,
                editAggregate.Computer.PlaceFields.FloorId,
                editAggregate.Computer.PlaceFields.RoomId,
                address,
                postalCode,
                postalAddress,
                location1,
                location2);

            var buildings = CreateSelectList(
                settings.PlaceFieldsSettings.RoomFieldSetting,
                editAggregate.Buildings,
                editAggregate.Computer.PlaceFields.BuildingId.ToString());
            var floors = CreateSelectList(
                settings.PlaceFieldsSettings.RoomFieldSetting,
                editAggregate.Floors,
                editAggregate.Computer.PlaceFields.FloorId.ToString());
            var rooms = CreateSelectListField(
                settings.PlaceFieldsSettings.RoomFieldSetting,
                editAggregate.Rooms,
                editAggregate.Computer.PlaceFields.RoomId.ToString());

            var placeFieldsViewModel = new PlaceFieldsViewModel(placeFieldsModel, buildings, floors, rooms);

            var other = CreateStringField(
                settings.OtherFieldsSettings.InfoFieldSetting,
                editAggregate.Computer.OtherFields.Info);

            var otherFieldModel = new OtherFieldsModel(other);

            var graphics = CreateStringField(
                settings.GraphicsFieldsSettings.VideoCardFieldSetting,
                editAggregate.Computer.GraphicsFields.VideoCard);

            var graphicsFieldModel = new GraphicsFieldsModel(graphics);

            var contractNumber = CreateStringField(
                settings.ContractFieldsSettings.ContractNumberFieldSetting,
                editAggregate.Computer.ContractFields.ContractNumber);
            var contractStartDate = CreateNullableDateTimeField(
                settings.ContractFieldsSettings.ContractStartDateFieldSetting,
                editAggregate.Computer.ContractFields.ContractStartDate);
            var contractEndDate = CreateNullableDateTimeField(
                settings.ContractFieldsSettings.ContractEndDateFieldSetting,
                editAggregate.Computer.ContractFields.ContractEndDate);
            var price = CreateIntegerField(
                settings.ContractFieldsSettings.PurchasePriceFieldSetting,
                editAggregate.Computer.ContractFields.PurchasePrice);
            var accounting1 = CreateStringField(
                settings.ContractFieldsSettings.AccountingDimension1FieldSetting,
                editAggregate.Computer.ContractFields.AccountingDimension1);
            var accounting2 = CreateStringField(
                settings.ContractFieldsSettings.AccountingDimension2FieldSetting,
                editAggregate.Computer.ContractFields.AccountingDimension2);
            var accounting3 = CreateStringField(
                settings.ContractFieldsSettings.AccountingDimension3FieldSetting,
                editAggregate.Computer.ContractFields.AccountingDimension3);
            var accounting4 = CreateStringField(
                settings.ContractFieldsSettings.AccountingDimension4FieldSetting,
                editAggregate.Computer.ContractFields.AccountingDimension4);
            var accounting5 = CreateStringField(
                settings.ContractFieldsSettings.AccountingDimension5FieldSetting,
                editAggregate.Computer.ContractFields.AccountingDimension5);

            var contractFieldsModel = new ContractFieldsModel(
                editAggregate.Computer.ContractFields.ContractStatusId,
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
                editAggregate.Computer.ContractFields.ContractStatusId.ToString());

            var contractViewModel = new ContractFieldsViewModel(contractFieldsModel, contractStatuses);

            var contactName = CreateStringField(
                settings.ContactFieldsSettings.NameFieldSetting,
                editAggregate.Computer.ContactFields.Name);
            var contactPhone = CreateStringField(
                settings.ContactFieldsSettings.PhoneFieldSetting,
                editAggregate.Computer.ContactFields.Phone);
            var contactEmail = CreateStringField(
                settings.ContactFieldsSettings.EmailFieldSetting,
                editAggregate.Computer.ContactFields.Email);

            var contactFieldsModel = new ContactFieldsModel(contactName, contactPhone, contactEmail);

            var ip = CreateStringField(
                settings.CommunicationFieldsSettings.IPAddressFieldSetting,
                editAggregate.Computer.CommunicationFields.IPAddress);
            var mac = CreateStringField(
                settings.CommunicationFieldsSettings.MacAddressFieldSetting,
                editAggregate.Computer.CommunicationFields.MacAddress);
            var ras = CreateBooleanField(
                settings.CommunicationFieldsSettings.RASFieldSetting,
                editAggregate.Computer.CommunicationFields.IsRAS);
            var client = CreateStringField(
                settings.CommunicationFieldsSettings.NovellClientFieldSetting,
                editAggregate.Computer.CommunicationFields.NovellClient);

            var communicationFieldsModel =
                new CommunicationFieldsModel(
                    editAggregate.Computer.CommunicationFields.NetworkAdapterId,
                    ip,
                    mac,
                    ras,
                    client);

            var adapters = CreateSelectListField(
                settings.CommunicationFieldsSettings.NetworkAdapterFieldSetting,
                editAggregate.NetworkAdapters,
                editAggregate.Computer.CommunicationFields.NetworkAdapterId.ToString());

            var communicationViewModel = new CommunicationFieldsViewModel(communicationFieldsModel, adapters);

            var syncDate = CreateNullableDateTimeField(
                settings.DateFieldsSettings.SyncChangedDateSetting,
                editAggregate.Computer.DateFields.SynchronizeDate);
            var scanDate = CreateNullableDateTimeField(
                settings.DateFieldsSettings.ScanDateFieldSetting,
                editAggregate.Computer.DateFields.ScanDate);
            var path = CreateStringField(
                settings.DateFieldsSettings.PathDirectoryFieldSetting,
                editAggregate.Computer.DateFields.PathDirectory);

            var dateFieldsModel = new DateFieldsModel(syncDate, scanDate, path);

            var userStringId = CreateStringField(
                settings.ContactInformationFieldsSettings.UserIdFieldSetting,
                editAggregate.Computer.ContactInformationFields.UserStringId);

            var contactInformationFieldsModel =
                new ContactInformationFieldsModel(
                    editAggregate.Computer.ContactInformationFields.UserId,
                    userStringId,
                    editAggregate.Computer.ContactInformationFields.Department,
                    editAggregate.Computer.ContactInformationFields.Unit,
                    editAggregate.Computer.ContactInformationFields.UserName);

            return new ComputerViewModel(
                editAggregate.Computer.Id,
                editAggregate.Computer.CustomerId,
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