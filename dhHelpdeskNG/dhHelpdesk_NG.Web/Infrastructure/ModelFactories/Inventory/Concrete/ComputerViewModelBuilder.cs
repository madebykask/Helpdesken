namespace DH.Helpdesk.Web.Infrastructure.ModelFactories.Inventory.Concrete
{
    using System;
    using System.Globalization;

    using DH.Helpdesk.BusinessData.Models.Inventory;
    using DH.Helpdesk.BusinessData.Models.Inventory.Output.Settings.ModelEdit.ComputerSettings;
    using DH.Helpdesk.Services.Response.Inventory;
    using DH.Helpdesk.Web.Models.Inventory.EditModel.Computer;
    using DH.Helpdesk.Web.Models.Inventory.EditModel.Shared;

    using PlaceFieldsModel = DH.Helpdesk.Web.Models.Inventory.EditModel.Computer.PlaceFieldsModel;
    using PlaceFieldsViewModel = DH.Helpdesk.Web.Models.Inventory.EditModel.Computer.PlaceFieldsViewModel;

    public class ComputerViewModelBuilder : IComputerViewModelBuilder
    {
        private readonly IConfigurableFieldModelBuilder configurableFieldModelBuilder;

        public ComputerViewModelBuilder(IConfigurableFieldModelBuilder configurableFieldModelBuilder)
        {
            this.configurableFieldModelBuilder = configurableFieldModelBuilder;
        }

        public ComputerViewModel BuildViewModel(
            BusinessData.Models.Inventory.Edit.Computer.Computer model,
            ComputerEditOptionsResponse options,
            ComputerFieldsSettingsForModelEdit settings)
        {
            var createdDate =
                this.configurableFieldModelBuilder.CreateDateTimeField(
                    settings.DateFieldsSettings.CreatedDateFieldSetting,
                    model.CreatedDate);
            var changedDate =
                this.configurableFieldModelBuilder.CreateDateTimeField(
                    settings.DateFieldsSettings.ChangedDateFieldSetting,
                    model.ChangedDate);

            var name =
                this.configurableFieldModelBuilder.CreateStringField(
                    settings.WorkstationFieldsSettings.ComputerNameFieldSetting,
                    model.WorkstationFields.ComputerName);
            var manufacturer =
                this.configurableFieldModelBuilder.CreateStringField(
                    settings.WorkstationFieldsSettings.ManufacturerFieldSetting,
                    model.WorkstationFields.Manufacturer);
            var serial =
                this.configurableFieldModelBuilder.CreateStringField(
                    settings.WorkstationFieldsSettings.SerialNumberFieldSetting,
                    model.WorkstationFields.SerialNumber);
            var biosVersion =
                this.configurableFieldModelBuilder.CreateStringField(
                    settings.WorkstationFieldsSettings.BIOSVersionFieldSetting,
                    model.WorkstationFields.BIOSVersion);
            var biosDate =
                this.configurableFieldModelBuilder.CreateNullableDateTimeField(
                    settings.WorkstationFieldsSettings.BIOSDateFieldSetting,
                    model.WorkstationFields.BIOSDate);
            var theftMark =
                this.configurableFieldModelBuilder.CreateStringField(
                    settings.WorkstationFieldsSettings.TheftmarkFieldSetting,
                    model.WorkstationFields.Theftmark);
            var carePackNumber =
                this.configurableFieldModelBuilder.CreateStringField(
                    settings.WorkstationFieldsSettings.CarePackNumberFieldSetting,
                    model.WorkstationFields.CarePackNumber);
            var location =
                this.configurableFieldModelBuilder.CreateStringField(
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
            var computerModels =
                this.configurableFieldModelBuilder.CreateSelectListField(
                    settings.WorkstationFieldsSettings.ComputerModelFieldSetting,
                    options.ComputerModels,
                    model.WorkstationFields.ComputerModelId.ToString());
            var computerTypes =
                this.configurableFieldModelBuilder.CreateSelectListField(
                    settings.WorkstationFieldsSettings.ComputerTypeFieldSetting,
                    options.ComputerTypes,
                    model.WorkstationFields.ComputerTypeId.ToString());

            var workstationViewModel = new WorkstationFieldsViewModel(
                workstationFieldsModel,
                computerModels,
                computerTypes);

            var proccesorFieldsModel = new ProccesorFieldsModel(model.ProccesorFields.ProccesorId);
            var processors =
                this.configurableFieldModelBuilder.CreateSelectListField(
                    settings.ProccesorFieldsSettings.ProccesorFieldSetting,
                    options.Processors,
                    model.ProccesorFields.ProccesorId.ToString());

            var processorViewModel = new ProccesorFieldsViewModel(proccesorFieldsModel, processors);

            var organizationFieldsModel = new OrganizationFieldsModel(
                model.OrganizationFields.DepartmentId,
                model.OrganizationFields.DomainId,
                model.OrganizationFields.UnitId);
            var departments =
                this.configurableFieldModelBuilder.CreateSelectListField(
                    settings.OrganizationFieldsSettings.DepartmentFieldSetting,
                    options.Departments,
                    model.OrganizationFields.DepartmentId.ToString());
            var domains =
                this.configurableFieldModelBuilder.CreateSelectListField(
                    settings.OrganizationFieldsSettings.DomainFieldSetting,
                    options.Domains,
                    model.OrganizationFields.DomainId.ToString());
            var ous =
                this.configurableFieldModelBuilder.CreateSelectListField(
                    settings.OrganizationFieldsSettings.UnitFieldSetting,
                    options.Units,
                    model.OrganizationFields.UnitId.ToString());

            var organizationViewModel = new OrganizationFieldsViewModel(
                organizationFieldsModel,
                departments,
                domains,
                ous);

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

            var operatingSystemFieldsModel =
                new OperatingSystemFieldsModel(
                    model.OperatingSystemFields.OperatingSystemId,
                    version,
                    servicePack,
                    registratinCode,
                    productKey);
            var operatingSystems =
                this.configurableFieldModelBuilder.CreateSelectListField(
                    settings.OperatingSystemFieldsSettings.OperatingSystemFieldSetting,
                    options.OperatingSystems,
                    model.OperatingSystemFields.OperatingSystemId.ToString());

            var operatingSystemsViewModel = new OperatingSystemFieldsViewModel(
                operatingSystemFieldsModel,
                operatingSystems);

            var memoryFieldsModel = new MemoryFieldsModel(model.MemoryFields.RAMId);
            var memories =
                this.configurableFieldModelBuilder.CreateSelectListField(
                    settings.MemoryFieldsSettings.RAMFieldSetting,
                    options.Rams,
                    model.MemoryFields.RAMId.ToString());

            var memoryViewModel = new MemoryFieldsViewModel(memoryFieldsModel, memories);

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

            var stolen =
                this.configurableFieldModelBuilder.CreateBooleanField(
                    settings.StateFieldsSettings.StolenFieldSetting,
                    model.StateFields.IsStolen);
            var replaced =
                this.configurableFieldModelBuilder.CreateStringField(
                    settings.StateFieldsSettings.ReplacedWithFieldSetting,
                    model.StateFields.Replaced);
            var sendBack =
                this.configurableFieldModelBuilder.CreateBooleanField(
                    settings.StateFieldsSettings.SendBackFieldSetting,
                    model.StateFields.IsSendBack);
            var scrapDate =
                this.configurableFieldModelBuilder.CreateNullableDateTimeField(
                    settings.StateFieldsSettings.ScrapDateFieldSetting,
                    model.StateFields.ScrapDate);

            var stateFieldsModel = new StateFieldsModel(model.StateFields.State, stolen, replaced, sendBack, scrapDate);

            var statuses =
                this.configurableFieldModelBuilder.CreateSelectListField(
                    settings.StateFieldsSettings.StateFieldSetting,
                    Enum.GetValues(typeof(ComputerStatuses)),
                    model.StateFields.State.ToString(CultureInfo.InvariantCulture));

            var stateViewModel = new StateFieldsViewModel(stateFieldsModel, statuses);

            var sound =
                this.configurableFieldModelBuilder.CreateStringField(
                    settings.SoundFieldsSettings.SoundCardFieldSetting,
                    model.SoundFields.SoundCard);

            var soundFieldModel = new SoundFieldsModel(sound);

            var address =
                this.configurableFieldModelBuilder.CreateStringField(
                    settings.PlaceFieldsSettings.AddressFieldSetting,
                    model.PlaceFields.Address);
            var postalCode =
                this.configurableFieldModelBuilder.CreateStringField(
                    settings.PlaceFieldsSettings.PostalCodeFieldSetting,
                    model.PlaceFields.PostalCode);
            var postalAddress =
                this.configurableFieldModelBuilder.CreateStringField(
                    settings.PlaceFieldsSettings.PostalAddressFieldSetting,
                    model.PlaceFields.PostalAddress);
            var location1 =
                this.configurableFieldModelBuilder.CreateStringField(
                    settings.PlaceFieldsSettings.PlaceFieldSetting,
                    model.PlaceFields.Location);
            var location2 =
                this.configurableFieldModelBuilder.CreateStringField(
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

            var buildings = this.configurableFieldModelBuilder.CreateSelectList(
                settings.PlaceFieldsSettings.RoomFieldSetting,
                options.Buildings,
                model.PlaceFields.BuildingId.ToString());
            var floors = this.configurableFieldModelBuilder.CreateSelectList(
                settings.PlaceFieldsSettings.RoomFieldSetting,
                options.Floors,
                model.PlaceFields.FloorId.ToString());
            var rooms =
                this.configurableFieldModelBuilder.CreateSelectListField(
                    settings.PlaceFieldsSettings.RoomFieldSetting,
                    options.Rooms,
                    model.PlaceFields.RoomId.ToString());

            var placeFieldsViewModel = new PlaceFieldsViewModel(placeFieldsModel, buildings, floors, rooms);

            var other = this.configurableFieldModelBuilder.CreateStringField(
                settings.OtherFieldsSettings.InfoFieldSetting,
                model.OtherFields.Info);

            var otherFieldModel = new OtherFieldsModel(other);

            var graphics =
                this.configurableFieldModelBuilder.CreateStringField(
                    settings.GraphicsFieldsSettings.VideoCardFieldSetting,
                    model.GraphicsFields.VideoCard);

            var graphicsFieldModel = new GraphicsFieldsModel(graphics);

            var contractNumber =
                this.configurableFieldModelBuilder.CreateStringField(
                    settings.ContractFieldsSettings.ContractNumberFieldSetting,
                    model.ContractFields.ContractNumber);
            var contractStartDate =
                this.configurableFieldModelBuilder.CreateNullableDateTimeField(
                    settings.ContractFieldsSettings.ContractStartDateFieldSetting,
                    model.ContractFields.ContractStartDate);
            var contractEndDate =
                this.configurableFieldModelBuilder.CreateNullableDateTimeField(
                    settings.ContractFieldsSettings.ContractEndDateFieldSetting,
                    model.ContractFields.ContractEndDate);
            var price =
                this.configurableFieldModelBuilder.CreateIntegerField(
                    settings.ContractFieldsSettings.PurchasePriceFieldSetting,
                    model.ContractFields.PurchasePrice);
            var accounting1 =
                this.configurableFieldModelBuilder.CreateStringField(
                    settings.ContractFieldsSettings.AccountingDimension1FieldSetting,
                    model.ContractFields.AccountingDimension1);
            var accounting2 =
                this.configurableFieldModelBuilder.CreateStringField(
                    settings.ContractFieldsSettings.AccountingDimension2FieldSetting,
                    model.ContractFields.AccountingDimension2);
            var accounting3 =
                this.configurableFieldModelBuilder.CreateStringField(
                    settings.ContractFieldsSettings.AccountingDimension3FieldSetting,
                    model.ContractFields.AccountingDimension3);
            var accounting4 =
                this.configurableFieldModelBuilder.CreateStringField(
                    settings.ContractFieldsSettings.AccountingDimension4FieldSetting,
                    model.ContractFields.AccountingDimension4);
            var accounting5 =
                this.configurableFieldModelBuilder.CreateStringField(
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

            var contractStatuses =
                this.configurableFieldModelBuilder.CreateSelectListField(
                    settings.StateFieldsSettings.StateFieldSetting,
                    Enum.GetValues(typeof(ContractStatuses)),
                    model.ContractFields.ContractStatusId.ToString());

            var contractViewModel = new ContractFieldsViewModel(contractFieldsModel, contractStatuses);

            var contactName =
                this.configurableFieldModelBuilder.CreateStringField(
                    settings.ContactFieldsSettings.NameFieldSetting,
                    model.ContactFields.Name);
            var contactPhone =
                this.configurableFieldModelBuilder.CreateStringField(
                    settings.ContactFieldsSettings.PhoneFieldSetting,
                    model.ContactFields.Phone);
            var contactEmail =
                this.configurableFieldModelBuilder.CreateStringField(
                    settings.ContactFieldsSettings.EmailFieldSetting,
                    model.ContactFields.Email);

            var contactFieldsModel = new ContactFieldsModel(contactName, contactPhone, contactEmail);

            var ip =
                this.configurableFieldModelBuilder.CreateStringField(
                    settings.CommunicationFieldsSettings.IPAddressFieldSetting,
                    model.CommunicationFields.IPAddress);
            var mac =
                this.configurableFieldModelBuilder.CreateStringField(
                    settings.CommunicationFieldsSettings.MacAddressFieldSetting,
                    model.CommunicationFields.MacAddress);
            var ras =
                this.configurableFieldModelBuilder.CreateBooleanField(
                    settings.CommunicationFieldsSettings.RASFieldSetting,
                    model.CommunicationFields.IsRAS);
            var client =
                this.configurableFieldModelBuilder.CreateStringField(
                    settings.CommunicationFieldsSettings.NovellClientFieldSetting,
                    model.CommunicationFields.NovellClient);

            var communicationFieldsModel = new CommunicationFieldsModel(
                model.CommunicationFields.NetworkAdapterId,
                ip,
                mac,
                ras,
                client);

            var adapters =
                this.configurableFieldModelBuilder.CreateSelectListField(
                    settings.CommunicationFieldsSettings.NetworkAdapterFieldSetting,
                    options.NetworkAdapters,
                    model.CommunicationFields.NetworkAdapterId.ToString());

            var communicationViewModel = new CommunicationFieldsViewModel(communicationFieldsModel, adapters);

            var syncDate =
                this.configurableFieldModelBuilder.CreateNullableDateTimeField(
                    settings.DateFieldsSettings.SyncChangedDateSetting,
                    model.DateFields.SynchronizeDate);
            var scanDate =
                this.configurableFieldModelBuilder.CreateNullableDateTimeField(
                    settings.DateFieldsSettings.ScanDateFieldSetting,
                    model.DateFields.ScanDate);
            var path =
                this.configurableFieldModelBuilder.CreateStringField(
                    settings.DateFieldsSettings.PathDirectoryFieldSetting,
                    model.DateFields.PathDirectory);

            var dateFieldsModel = new DateFieldsModel(syncDate, scanDate, path);

            var userStringId =
                this.configurableFieldModelBuilder.CreateStringField(
                    settings.ContactInformationFieldsSettings.UserIdFieldSetting,
                    model.ContactInformationFields.UserStringId);

            var contactInformationFieldsModel = new ContactInformationFieldsModel(
                model.ContactInformationFields.UserId,
                userStringId,
                model.ContactInformationFields.Department,
                model.ContactInformationFields.Unit,
                model.ContactInformationFields.UserName);

            return new ComputerViewModel(
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

        public ComputerViewModel BuildViewModel(
            ComputerEditOptionsResponse options,
            ComputerFieldsSettingsForModelEdit settings,
            int currentCustomerId)
        {
            var name = this.configurableFieldModelBuilder.CreateStringField(
                settings.WorkstationFieldsSettings.ComputerNameFieldSetting,
                null);
            var manufacturer = this.configurableFieldModelBuilder.CreateStringField(
                settings.WorkstationFieldsSettings.ManufacturerFieldSetting,
                null);
            var serial = this.configurableFieldModelBuilder.CreateStringField(
                settings.WorkstationFieldsSettings.SerialNumberFieldSetting,
                null);
            var biosVersion = this.configurableFieldModelBuilder.CreateStringField(
                settings.WorkstationFieldsSettings.BIOSVersionFieldSetting,
                null);
            var biosDate = this.configurableFieldModelBuilder.CreateNullableDateTimeField(
                settings.WorkstationFieldsSettings.BIOSDateFieldSetting,
                null);
            var theftMark = this.configurableFieldModelBuilder.CreateStringField(
                settings.WorkstationFieldsSettings.TheftmarkFieldSetting,
                null);
            var carePackNumber = this.configurableFieldModelBuilder.CreateStringField(
                settings.WorkstationFieldsSettings.CarePackNumberFieldSetting,
                null);
            var location = this.configurableFieldModelBuilder.CreateStringField(
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
            var computerTypes = this.configurableFieldModelBuilder.CreateSelectListField(
                settings.WorkstationFieldsSettings.ComputerTypeFieldSetting,
                options.ComputerTypes,
                null);
            var computerModels = this.configurableFieldModelBuilder.CreateSelectListField(
                settings.WorkstationFieldsSettings.ComputerModelFieldSetting,
                options.ComputerModels,
                null);

            var workstationViewModel = new WorkstationFieldsViewModel(
                workstationFieldsModel,
                computerTypes,
                computerModels);

            var proccesorFieldsModel = new ProccesorFieldsModel(null);
            var processors = this.configurableFieldModelBuilder.CreateSelectListField(
                settings.ProccesorFieldsSettings.ProccesorFieldSetting,
                options.Processors,
                null);

            var processorViewModel = new ProccesorFieldsViewModel(proccesorFieldsModel, processors);

            var organizationFieldsModel = new OrganizationFieldsModel(
                null,
                null,
                null);
            var departments = this.configurableFieldModelBuilder.CreateSelectListField(
                settings.OrganizationFieldsSettings.DepartmentFieldSetting,
                options.Departments,
                null);
            var domains = this.configurableFieldModelBuilder.CreateSelectListField(
                settings.OrganizationFieldsSettings.DomainFieldSetting,
                options.Domains,
                null);
            var ous = this.configurableFieldModelBuilder.CreateSelectListField(
                settings.OrganizationFieldsSettings.UnitFieldSetting,
                options.Units,
                null);

            var organizationViewModel = new OrganizationFieldsViewModel(
                organizationFieldsModel,
                departments,
                domains,
                ous);

            var version = this.configurableFieldModelBuilder.CreateStringField(
                settings.OperatingSystemFieldsSettings.VersionFieldSetting,
                null);
            var servicePack = this.configurableFieldModelBuilder.CreateStringField(
                settings.OperatingSystemFieldsSettings.ServicePackSystemFieldSetting,
                null);
            var registratinCode =
                this.configurableFieldModelBuilder.CreateStringField(
                    settings.OperatingSystemFieldsSettings.RegistrationCodeSystemFieldSetting,
                    null);
            var productKey = this.configurableFieldModelBuilder.CreateStringField(
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
                this.configurableFieldModelBuilder.CreateSelectListField(
                    settings.OperatingSystemFieldsSettings.OperatingSystemFieldSetting,
                    options.OperatingSystems,
                    null);

            var operatingSystemsViewModel = new OperatingSystemFieldsViewModel(
                operatingSystemFieldsModel,
                operatingSystems);

            var memoryFieldsModel = new MemoryFieldsModel(null);
            var memories = this.configurableFieldModelBuilder.CreateSelectListField(
                settings.MemoryFieldsSettings.RAMFieldSetting,
                options.Rams,
                null);

            var memoryViewModel = new MemoryFieldsViewModel(memoryFieldsModel, memories);

            var barCode = this.configurableFieldModelBuilder.CreateStringField(
                settings.InventoryFieldsSettings.BarCodeFieldSetting,
                null);
            var purchaseDate = this.configurableFieldModelBuilder.CreateNullableDateTimeField(
                settings.InventoryFieldsSettings.PurchaseDateFieldSetting,
                null);

            var inventoryFieldModel = new InventoryFieldsModel(barCode, purchaseDate);

            var chassis = this.configurableFieldModelBuilder.CreateStringField(
                settings.ChassisFieldsSettings.ChassisFieldSetting,
                null);

            var chassisFieldModel = new ChassisFieldsModel(chassis);

            var stolen = this.configurableFieldModelBuilder.CreateBooleanField(settings.StateFieldsSettings.StolenFieldSetting, false);
            var replaced = this.configurableFieldModelBuilder.CreateStringField(
                settings.StateFieldsSettings.ReplacedWithFieldSetting,
                null);
            var sendBack = this.configurableFieldModelBuilder.CreateBooleanField(
                settings.StateFieldsSettings.SendBackFieldSetting,
                false);
            var scrapDate = this.configurableFieldModelBuilder.CreateNullableDateTimeField(
                settings.StateFieldsSettings.ScrapDateFieldSetting,
                null);

            var stateFieldsModel = new StateFieldsModel(0, stolen, replaced, sendBack, scrapDate);

            var statuses = this.configurableFieldModelBuilder.CreateSelectListField(
                settings.StateFieldsSettings.StateFieldSetting,
                Enum.GetValues(typeof(ComputerStatuses)),
                null);

            var stateViewModel = new StateFieldsViewModel(stateFieldsModel, statuses);

            var sound = this.configurableFieldModelBuilder.CreateStringField(
                settings.SoundFieldsSettings.SoundCardFieldSetting,
                null);

            var soundFieldModel = new SoundFieldsModel(sound);

            var address = this.configurableFieldModelBuilder.CreateStringField(settings.PlaceFieldsSettings.AddressFieldSetting, null);
            var postalCode = this.configurableFieldModelBuilder.CreateStringField(
                settings.PlaceFieldsSettings.PostalCodeFieldSetting,
                null);
            var postalAddress = this.configurableFieldModelBuilder.CreateStringField(
                settings.PlaceFieldsSettings.PostalAddressFieldSetting,
                null);
            var location1 = this.configurableFieldModelBuilder.CreateStringField(
                settings.PlaceFieldsSettings.PlaceFieldSetting,
                null);
            var location2 = this.configurableFieldModelBuilder.CreateStringField(
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

            var buildings = this.configurableFieldModelBuilder.CreateSelectList(
                settings.PlaceFieldsSettings.RoomFieldSetting,
                options.Buildings,
                null);
            var floors = this.configurableFieldModelBuilder.CreateSelectList(
                settings.PlaceFieldsSettings.RoomFieldSetting,
                options.Floors,
                null);
            var rooms = this.configurableFieldModelBuilder.CreateSelectListField(
                settings.PlaceFieldsSettings.RoomFieldSetting,
                options.Rooms,
                null);

            var placeFieldsViewModel = new PlaceFieldsViewModel(placeFieldsModel, buildings, floors, rooms);

            var other = this.configurableFieldModelBuilder.CreateStringField(settings.OtherFieldsSettings.InfoFieldSetting, null);

            var otherFieldModel = new OtherFieldsModel(other);

            var graphics = this.configurableFieldModelBuilder.CreateStringField(
                settings.GraphicsFieldsSettings.VideoCardFieldSetting,
                null);

            var graphicsFieldModel = new GraphicsFieldsModel(graphics);

            var contractNumber = this.configurableFieldModelBuilder.CreateStringField(
                settings.ContractFieldsSettings.ContractNumberFieldSetting,
                null);
            var contractStartDate =
                this.configurableFieldModelBuilder.CreateNullableDateTimeField(
                    settings.ContractFieldsSettings.ContractStartDateFieldSetting,
                    null);
            var contractEndDate =
                this.configurableFieldModelBuilder.CreateNullableDateTimeField(
                    settings.ContractFieldsSettings.ContractEndDateFieldSetting,
                    null);
            var price = this.configurableFieldModelBuilder.CreateIntegerField(
                settings.ContractFieldsSettings.PurchasePriceFieldSetting,
                0);
            var accounting1 = this.configurableFieldModelBuilder.CreateStringField(
                settings.ContractFieldsSettings.AccountingDimension1FieldSetting,
                null);
            var accounting2 = this.configurableFieldModelBuilder.CreateStringField(
                settings.ContractFieldsSettings.AccountingDimension2FieldSetting,
                null);
            var accounting3 = this.configurableFieldModelBuilder.CreateStringField(
                settings.ContractFieldsSettings.AccountingDimension3FieldSetting,
                null);
            var accounting4 = this.configurableFieldModelBuilder.CreateStringField(
                settings.ContractFieldsSettings.AccountingDimension4FieldSetting,
                null);
            var accounting5 = this.configurableFieldModelBuilder.CreateStringField(
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

            var contractStatuses = this.configurableFieldModelBuilder.CreateSelectListField(
                settings.StateFieldsSettings.StateFieldSetting,
                Enum.GetValues(typeof(ContractStatuses)),
                null);

            var contractViewModel = new ContractFieldsViewModel(contractFieldsModel, contractStatuses);

            var contactName = this.configurableFieldModelBuilder.CreateStringField(
                settings.ContactFieldsSettings.NameFieldSetting,
                null);
            var contactPhone = this.configurableFieldModelBuilder.CreateStringField(
                settings.ContactFieldsSettings.PhoneFieldSetting,
                null);
            var contactEmail = this.configurableFieldModelBuilder.CreateStringField(
                settings.ContactFieldsSettings.EmailFieldSetting,
                null);

            var contactFieldsModel = new ContactFieldsModel(contactName, contactPhone, contactEmail);

            var ip = this.configurableFieldModelBuilder.CreateStringField(
                settings.CommunicationFieldsSettings.IPAddressFieldSetting,
                null);
            var mac = this.configurableFieldModelBuilder.CreateStringField(
                settings.CommunicationFieldsSettings.MacAddressFieldSetting,
                null);
            var ras = this.configurableFieldModelBuilder.CreateBooleanField(
                settings.CommunicationFieldsSettings.RASFieldSetting,
                false);
            var client = this.configurableFieldModelBuilder.CreateStringField(
                settings.CommunicationFieldsSettings.NovellClientFieldSetting,
                null);

            var communicationFieldsModel = new CommunicationFieldsModel(
                null,
                ip,
                mac,
                ras,
                client);

            var adapters = this.configurableFieldModelBuilder.CreateSelectListField(
                settings.CommunicationFieldsSettings.NetworkAdapterFieldSetting,
                options.NetworkAdapters,
                null);

            var communicationViewModel = new CommunicationFieldsViewModel(communicationFieldsModel, adapters);

            var syncDate = this.configurableFieldModelBuilder.CreateNullableDateTimeField(
                settings.DateFieldsSettings.SyncChangedDateSetting,
                null);
            var scanDate = this.configurableFieldModelBuilder.CreateNullableDateTimeField(
                settings.DateFieldsSettings.ScanDateFieldSetting,
                null);
            var path = this.configurableFieldModelBuilder.CreateStringField(
                settings.DateFieldsSettings.PathDirectoryFieldSetting,
                null);

            var dateFieldsModel = new DateFieldsModel(syncDate, scanDate, path);

            var userStringId = this.configurableFieldModelBuilder.CreateStringField(
                settings.ContactInformationFieldsSettings.UserIdFieldSetting,
                null);

            var contactInformationFieldsModel = new ContactInformationFieldsModel(
                null,
                userStringId,
                null,
                null,
                null);

            return new ComputerViewModel(
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
                workstationViewModel) { CustomerId = currentCustomerId };
        }
    }
}