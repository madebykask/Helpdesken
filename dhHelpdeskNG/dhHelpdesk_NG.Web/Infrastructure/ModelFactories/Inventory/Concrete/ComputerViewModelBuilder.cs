namespace DH.Helpdesk.Web.Infrastructure.ModelFactories.Inventory.Concrete
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Web.Mvc;

    using DH.Helpdesk.BusinessData.Models.Inventory;
    using DH.Helpdesk.BusinessData.Models.Inventory.Output.Settings.ModelEdit.ComputerSettings;
    using DH.Helpdesk.BusinessData.Models.Shared;
    using DH.Helpdesk.Web.Areas.Inventory.Models.EditModel.Computer;
    using DH.Helpdesk.Web.Areas.Inventory.Models.OptionsAggregates;
    using DH.Helpdesk.Web.Infrastructure.Extensions;

    using ChassisFieldsModel = DH.Helpdesk.Web.Areas.Inventory.Models.EditModel.Computer.ChassisFieldsModel;
    using InventoryFieldsModel = DH.Helpdesk.Web.Areas.Inventory.Models.EditModel.Computer.InventoryFieldsModel;
    using MemoryFieldsModel = DH.Helpdesk.Web.Areas.Inventory.Models.EditModel.Computer.MemoryFieldsModel;
    using MemoryFieldsViewModel = DH.Helpdesk.Web.Areas.Inventory.Models.EditModel.Computer.MemoryFieldsViewModel;
    using OperatingSystemFieldsModel = DH.Helpdesk.Web.Areas.Inventory.Models.EditModel.Computer.OperatingSystemFieldsModel;
    using OperatingSystemFieldsViewModel = DH.Helpdesk.Web.Areas.Inventory.Models.EditModel.Computer.OperatingSystemFieldsViewModel;
    using PlaceFieldsModel = DH.Helpdesk.Web.Areas.Inventory.Models.EditModel.Computer.PlaceFieldsModel;
    using PlaceFieldsViewModel = DH.Helpdesk.Web.Areas.Inventory.Models.EditModel.Computer.PlaceFieldsViewModel;
    using ProccesorFieldsModel = DH.Helpdesk.Web.Areas.Inventory.Models.EditModel.Computer.ProccesorFieldsModel;
    using ProccesorFieldsViewModel = DH.Helpdesk.Web.Areas.Inventory.Models.EditModel.Computer.ProccesorFieldsViewModel;

    public class ComputerViewModelBuilder : IComputerViewModelBuilder
    {
        public ComputerViewModel BuildViewModel(
            BusinessData.Models.Inventory.Edit.Computer.ComputerForRead model,
            ComputerEditOptions options,
            ComputerFieldsSettingsForModelEdit settings,
			List<string> fileUploadWhiteList)
        {
            var createdDate =
                CreateDateTimeField(settings.DateFieldsSettings.CreatedDateFieldSetting, model.CreatedDate);

            var changedDate =
                CreateDateTimeField(settings.DateFieldsSettings.ChangedDateFieldSetting, model.ChangedDate);

            #region Workstation Fields

            var name =
                CreateStringField(settings.WorkstationFieldsSettings.ComputerNameFieldSetting, model.WorkstationFields.ComputerName);

            var manufacturer =
                CreateStringField(settings.WorkstationFieldsSettings.ManufacturerFieldSetting, model.WorkstationFields.Manufacturer);

            var computerModel =
                CreateNullableIntegerField(
                    settings.WorkstationFieldsSettings.ComputerModelFieldSetting,
                    model.WorkstationFields.ComputerModelId);
            var serial =
                CreateStringField(
                    settings.WorkstationFieldsSettings.SerialNumberFieldSetting,
                    model.WorkstationFields.SerialNumber);
            var biosVersion =
                CreateStringField(
                    settings.WorkstationFieldsSettings.BIOSVersionFieldSetting,
                    model.WorkstationFields.BIOSVersion);
            var biosDate =
                CreateNullableDateTimeField(
                    settings.WorkstationFieldsSettings.BIOSDateFieldSetting,
                    model.WorkstationFields.BIOSDate);
            var theftMark =
                CreateStringField(
                    settings.WorkstationFieldsSettings.TheftmarkFieldSetting,
                    model.WorkstationFields.Theftmark);
            var carePackNumber =
                CreateStringField(
                    settings.WorkstationFieldsSettings.CarePackNumberFieldSetting,
                    model.WorkstationFields.CarePackNumber);
            var computerType = CreateNullableIntegerField(
                settings.WorkstationFieldsSettings.ComputerTypeFieldSetting,
                model.WorkstationFields.ComputerTypeId);
            var location =
                CreateStringField(
                    settings.WorkstationFieldsSettings.LocationFieldSetting,
                    model.WorkstationFields.Location);

            var workstationFieldsModel = new WorkstationFieldsModel(
                name,
                manufacturer,
                computerModel,
                serial,
                biosVersion,
                biosDate,
                theftMark,
                carePackNumber,
                computerType,
                location);

            var computerModels =
                CreateSelectListField(
                    settings.WorkstationFieldsSettings.ComputerModelFieldSetting,
                    options.ComputerModels,
                    model.WorkstationFields.ComputerModelId.ToString());

            var computerTypes =
                CreateSelectListField(
                    settings.WorkstationFieldsSettings.ComputerTypeFieldSetting,
                    options.ComputerTypes,
                    model.WorkstationFields.ComputerTypeId.ToString());

            var workstationViewModel = new WorkstationFieldsViewModel(
                workstationFieldsModel,
                computerModels,
                computerTypes);

            #endregion

            #region Processor Fields

            var processor = 
                CreateNullableIntegerField(settings.ProccesorFieldsSettings.ProccesorFieldSetting,model.ProccesorFields.ProccesorId);
            var proccesorFieldsModel = new ProccesorFieldsModel(processor);
            var processors =
                CreateSelectListField(
                    settings.ProccesorFieldsSettings.ProccesorFieldSetting,
                    options.Processors,
                    model.ProccesorFields.ProccesorId.ToString());

            var processorViewModel = new ProccesorFieldsViewModel(proccesorFieldsModel, processors);

            #endregion

            #region Organisation Fields

            var department = CreateNullableIntegerField(
                settings.OrganizationFieldsSettings.DepartmentFieldSetting,
                model.OrganizationFields.DepartmentId);
            var region = CreateNullableIntegerField(
                settings.OrganizationFieldsSettings.RegionFieldSetting,
                model.OrganizationFields.RegionId);

            var domain = CreateNullableIntegerField(
                settings.OrganizationFieldsSettings.DomainFieldSetting,
                model.OrganizationFields.DomainId);
            var unit = CreateNullableIntegerField(
                settings.OrganizationFieldsSettings.UnitFieldSetting,
                model.OrganizationFields.UnitId);

            var organizationFieldsModel = new OrganizationFieldsModel(
                region,
                department,
                domain,
                unit);
            var regions =
                CreateSelectListField(
                    settings.OrganizationFieldsSettings.RegionFieldSetting,
                    options.Regions,
                    model.OrganizationFields.RegionId.ToString());
            var departments =
                CreateSelectListField(
                    settings.OrganizationFieldsSettings.DepartmentFieldSetting,
                    options.Departments,
                    model.OrganizationFields.DepartmentId.ToString());
            var domains =
                CreateSelectListField(
                    settings.OrganizationFieldsSettings.DomainFieldSetting,
                    options.Domains,
                    model.OrganizationFields.DomainId.ToString());
            var ous =
                CreateSelectListField(
                    settings.OrganizationFieldsSettings.UnitFieldSetting,
                    options.Units,
                    model.OrganizationFields.UnitId.ToString());

            var organizationViewModel = new OrganizationFieldsViewModel(
                organizationFieldsModel,
                regions,
                departments,
                domains,
                ous);

            #endregion

            #region OS Fields

            var operatingSystem = CreateNullableIntegerField(
                settings.OperatingSystemFieldsSettings.OperatingSystemFieldSetting,
                model.OperatingSystemFields.OperatingSystemId);
            var version =
                CreateStringField(
                    settings.OperatingSystemFieldsSettings.VersionFieldSetting,
                    model.OperatingSystemFields.Version);
            var servicePack =
                CreateStringField(
                    settings.OperatingSystemFieldsSettings.ServicePackSystemFieldSetting,
                    model.OperatingSystemFields.ServicePack);
            var registratinCode =
                CreateStringField(
                    settings.OperatingSystemFieldsSettings.RegistrationCodeSystemFieldSetting,
                    model.OperatingSystemFields.RegistrationCode);
            var productKey =
                CreateStringField(
                    settings.OperatingSystemFieldsSettings.ProductKeyFieldSetting,
                    model.OperatingSystemFields.ProductKey);

            var operatingSystemFieldsModel =
                new OperatingSystemFieldsModel(
                    operatingSystem,
                    version,
                    servicePack,
                    registratinCode,
                    productKey);
            var operatingSystems =
                CreateSelectListField(
                    settings.OperatingSystemFieldsSettings.OperatingSystemFieldSetting,
                    options.OperatingSystems,
                    model.OperatingSystemFields.OperatingSystemId.ToString());

            var operatingSystemsViewModel = new OperatingSystemFieldsViewModel(operatingSystemFieldsModel, operatingSystems);

            #endregion

            #region Memory Fields

            var memory =
                CreateNullableIntegerField(settings.MemoryFieldsSettings.RAMFieldSetting, model.MemoryFields.RAMId);

            var memoryFieldsModel = new MemoryFieldsModel(memory);

            var memories =
                CreateSelectListField(settings.MemoryFieldsSettings.RAMFieldSetting, options.Rams, model.MemoryFields.RAMId.ToString());

            var memoryViewModel = new MemoryFieldsViewModel(memoryFieldsModel, memories);

            #endregion

            #region Inventory Fields

            var barCode =
                CreateStringField(
                    settings.InventoryFieldsSettings.BarCodeFieldSetting,
                    model.InventoryFields.BarCode);

            var inventoryFieldModel = new InventoryFieldsModel(barCode);

            #endregion

            var chassis =
                CreateStringField(
                    settings.ChassisFieldsSettings.ChassisFieldSetting,
                    model.ChassisFields.Chassis);

            var chassisFieldModel = new ChassisFieldsModel(chassis);

            #region State Fields

            var state = CreateNullableIntegerField(
                settings.StateFieldsSettings.StateFieldSetting,
                model.StateFields.State == 0 ? null : (int?)model.StateFields.State);
            var stolen =
                CreateBooleanField(
                    settings.StateFieldsSettings.StolenFieldSetting,
                    model.StateFields.IsStolen);
            var replaced =
                CreateStringField(
                    settings.StateFieldsSettings.ReplacedWithFieldSetting,
                    model.StateFields.Replaced);
            var sendBack =
                CreateBooleanField(
                    settings.StateFieldsSettings.SendBackFieldSetting,
                    model.StateFields.IsSendBack);
            var scrapDate =
                CreateNullableDateTimeField(
                    settings.StateFieldsSettings.ScrapDateFieldSetting,
                    model.StateFields.ScrapDate);

            var stateFieldsModel = new StateFieldsModel(state, stolen, replaced, sendBack, scrapDate);

            var statuses =
                CreateSelectListField(
                    settings.StateFieldsSettings.StateFieldSetting,
                    options.ComputerStatuses,
                    model.StateFields.State.ToString(CultureInfo.InvariantCulture));

            var stateViewModel = new StateFieldsViewModel(stateFieldsModel, statuses);

            #endregion

            var sound =
                CreateStringField(
                    settings.SoundFieldsSettings.SoundCardFieldSetting,
                    model.SoundFields.SoundCard);

            var soundFieldModel = new SoundFieldsModel(sound);

            #region Place fields

            var room = CreateNullableIntegerField(
                settings.PlaceFieldsSettings.RoomFieldSetting,
                model.PlaceFields.RoomId);
            var building = CreateNullableIntegerField(
                settings.PlaceFieldsSettings.BuildingFieldSetting,
                model.PlaceFields.BuildingId);
            var floor = CreateNullableIntegerField(
                settings.PlaceFieldsSettings.FloorFieldSetting,
                model.PlaceFields.FloorId);
            var address =
                CreateStringField(
                    settings.PlaceFieldsSettings.AddressFieldSetting,
                    model.PlaceFields.Address);
            var postalCode =
                CreateStringField(
                    settings.PlaceFieldsSettings.PostalCodeFieldSetting,
                    model.PlaceFields.PostalCode);
            var postalAddress =
                CreateStringField(
                    settings.PlaceFieldsSettings.PostalAddressFieldSetting,
                    model.PlaceFields.PostalAddress);
            var location1 =
                CreateStringField(
                    settings.PlaceFieldsSettings.PlaceFieldSetting,
                    model.PlaceFields.Location);
            var location2 =
                CreateStringField(
                    settings.PlaceFieldsSettings.Place2FieldSetting,
                    model.PlaceFields.Location2);

            var placeFieldsModel = new PlaceFieldsModel(
                building,
                floor,
                room,
                address,
                postalCode,
                postalAddress,
                location1,
                location2);

            var buildings = CreateSelectList(
                settings.PlaceFieldsSettings.BuildingFieldSetting,
                options.Buildings,
                model.PlaceFields.BuildingId.ToString());
            var floors = CreateSelectList(
                settings.PlaceFieldsSettings.FloorFieldSetting,
                options.Floors,
                model.PlaceFields.FloorId.ToString());
            var rooms =
                CreateSelectListField(
                    settings.PlaceFieldsSettings.RoomFieldSetting,
                    options.Rooms,
                    model.PlaceFields.RoomId.ToString());

            var placeFieldsViewModel = new PlaceFieldsViewModel(placeFieldsModel, buildings, floors, rooms);

            #endregion

            var other = CreateStringField(
                settings.OtherFieldsSettings.InfoFieldSetting,
                model.OtherFields.Info);

            var otherFieldModel = new OtherFieldsModel(other);

            var graphics =
                CreateStringField(
                    settings.GraphicsFieldsSettings.VideoCardFieldSetting,
                    model.GraphicsFields.VideoCard);

            var graphicsFieldModel = new GraphicsFieldsModel(graphics);

            #region  Contract Fields

            var contractStatus = CreateNullableIntegerField(
                settings.ContractFieldsSettings.ContractStatusFieldSetting,
                model.ContractFields.ContractStatusId);
            var contractNumber =
                CreateStringField(
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
            var price =
                CreateIntegerField(
                    settings.ContractFieldsSettings.PurchasePriceFieldSetting,
                    model.ContractFields.PurchasePrice);
            var accounting1 =
                CreateStringField(
                    settings.ContractFieldsSettings.AccountingDimension1FieldSetting,
                    model.ContractFields.AccountingDimension1);
            var accounting2 =
                CreateStringField(
                    settings.ContractFieldsSettings.AccountingDimension2FieldSetting,
                    model.ContractFields.AccountingDimension2);
            var accounting3 =
                CreateStringField(
                    settings.ContractFieldsSettings.AccountingDimension3FieldSetting,
                    model.ContractFields.AccountingDimension3);
            var accounting4 =
                CreateStringField(
                    settings.ContractFieldsSettings.AccountingDimension4FieldSetting,
                    model.ContractFields.AccountingDimension4);
            var accounting5 =
                CreateStringField(
                    settings.ContractFieldsSettings.AccountingDimension5FieldSetting,
                    model.ContractFields.AccountingDimension5);
            var purchaseDate =
                CreateNullableDateTimeField(
                    settings.InventoryFieldsSettings.PurchaseDateFieldSetting,
                    model.InventoryFields.PurchaseDate);

            var warrantyEndDate =
                CreateNullableDateTimeField(
                    settings.ContractFieldsSettings.WarrantyEndDateFieldSettings,
                    model.ContractFields.WarrantyEndDate);


            var document = CreateStringField(settings.ContractFieldsSettings.DocumentFieldSetting, model.ContractFields.Document);

            var contractFieldsModel = new ContractFieldsModel(
                contractStatus,
                contractNumber,
                contractStartDate,
                contractEndDate,
                price,
                accounting1,
                accounting2,
                accounting3,
                accounting4,
                accounting5, 
                document,
                purchaseDate,
                warrantyEndDate);

            var contractStatuses =
                CreateSelectListField(
                    settings.ContractFieldsSettings.ContractStatusFieldSetting,
                    options.ComputerContractStatuses,
                    model.ContractFields.ContractStatusId.ToString());

            var contractViewModel = new ContractFieldsViewModel(contractFieldsModel, contractStatuses);

            #endregion

            #region Contact Fields

            var contactName =
                CreateStringField(
                    settings.ContactFieldsSettings.NameFieldSetting,
                    model.ContactFields.Name);
            var contactPhone =
                CreateStringField(
                    settings.ContactFieldsSettings.PhoneFieldSetting,
                    model.ContactFields.Phone);
            var contactEmail =
                CreateStringField(
                    settings.ContactFieldsSettings.EmailFieldSetting,
                    model.ContactFields.Email);

            var contactFieldsModel = new ContactFieldsModel(contactName, contactPhone, contactEmail);

            #endregion

            #region Communication Fields

            var ip =
                CreateStringField(
                    settings.CommunicationFieldsSettings.IPAddressFieldSetting,
                    model.CommunicationFields.IPAddress);
            var mac =
                CreateStringField(
                    settings.CommunicationFieldsSettings.MacAddressFieldSetting,
                    model.CommunicationFields.MacAddress);
            var ras =
                CreateBooleanField(
                    settings.CommunicationFieldsSettings.RASFieldSetting,
                    model.CommunicationFields.IsRAS);
            var client =
                CreateStringField(
                    settings.CommunicationFieldsSettings.NovellClientFieldSetting,
                    model.CommunicationFields.NovellClient);

            var communication = CreateNullableIntegerField(
                settings.CommunicationFieldsSettings.NetworkAdapterFieldSetting,
                model.CommunicationFields.NetworkAdapterId);
            var communicationFieldsModel = new CommunicationFieldsModel(
                communication,
                ip,
                mac,
                ras,
                client);

            var adapters =
                CreateSelectListField(
                    settings.CommunicationFieldsSettings.NetworkAdapterFieldSetting,
                    options.NetworkAdapters,
                    model.CommunicationFields.NetworkAdapterId.ToString());

            var communicationViewModel = new CommunicationFieldsViewModel(communicationFieldsModel, adapters);

            #endregion

            var syncDate =
                CreateNullableDateTimeField(
                    settings.DateFieldsSettings.SyncChangedDateSetting,
                    model.DateFields.SynchronizeDate);
            var scanDate =
                CreateNullableDateTimeField(
                    settings.DateFieldsSettings.ScanDateFieldSetting,
                    model.DateFields.ScanDate);
            var path =
                CreateStringField(
                    settings.DateFieldsSettings.PathDirectoryFieldSetting,
                    model.DateFields.PathDirectory);

            var dateFieldsModel = new DateFieldsModel(syncDate, scanDate, path);

            var contactInformationFieldsModel =
                new ContactInformationFieldsModel(
                    model.ContactInformationFields.UserId,
                    CreateStringField(settings.ContactInformationFieldsSettings.UserIdFieldSetting, model.ContactInformationFields.UserStringId),
                    CreateStringField(settings.ContactInformationFieldsSettings.FirstNameFieldSetting, model.ContactInformationFields.UserName?.FirstName),
                    CreateStringField(settings.ContactInformationFieldsSettings.LastNameFieldSetting, model.ContactInformationFields.UserName?.LastName),
                    CreateStringField(settings.ContactInformationFieldsSettings.RegionFieldSetting, model.ContactInformationFields.Region),
                    CreateStringField(settings.ContactInformationFieldsSettings.DepartmentFieldSetting, model.ContactInformationFields.Department),
                    CreateStringField(settings.ContactInformationFieldsSettings.UnitFieldSetting, model.ContactInformationFields.Unit));

            return new ComputerViewModel(
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
                workstationViewModel,
				fileUploadWhiteList)
                       {
                           Id = model.Id,
                           CreatedDate = createdDate,
                           ChangedDate = changedDate,
                           ChangedByUserName = model.DateFields.ChangedByUserName
                       };
        }

        public ComputerViewModel BuildViewModel(ComputerEditOptions options, ComputerFieldsSettingsForModelEdit settings, int currentCustomerId, List<string> fileUploadWhiteList)
        {
            var name = CreateStringField(settings.WorkstationFieldsSettings.ComputerNameFieldSetting, null);
            var manufacturer = CreateStringField(settings.WorkstationFieldsSettings.ManufacturerFieldSetting, null);
            var computerModel = CreateNullableIntegerField(settings.WorkstationFieldsSettings.ComputerModelFieldSetting, null);
            var serial = CreateStringField(settings.WorkstationFieldsSettings.SerialNumberFieldSetting, null);
            var biosVersion = CreateStringField(settings.WorkstationFieldsSettings.BIOSVersionFieldSetting, null);
            var biosDate = CreateNullableDateTimeField(settings.WorkstationFieldsSettings.BIOSDateFieldSetting, null);
            var theftMark = CreateStringField(settings.WorkstationFieldsSettings.TheftmarkFieldSetting, null);
            var carePackNumber = CreateStringField(settings.WorkstationFieldsSettings.CarePackNumberFieldSetting, null);
            var computerType = CreateNullableIntegerField(settings.WorkstationFieldsSettings.ComputerTypeFieldSetting, null);
            var location = CreateStringField(settings.WorkstationFieldsSettings.LocationFieldSetting, null);

            var workstationFieldsModel = new WorkstationFieldsModel(
                name,
                manufacturer,
                computerModel,
                serial,
                biosVersion,
                biosDate,
                theftMark,
                carePackNumber,
                computerType,
                location);
            var computerModels = CreateSelectListField(
                settings.WorkstationFieldsSettings.ComputerModelFieldSetting,
                options.ComputerModels,
                null);
            var computerTypes = CreateSelectListField(
                settings.WorkstationFieldsSettings.ComputerTypeFieldSetting,
                options.ComputerTypes,
                null);

            var workstationViewModel = new WorkstationFieldsViewModel(
                workstationFieldsModel,
                computerModels,
                computerTypes);

            var processor = CreateNullableIntegerField(settings.ProccesorFieldsSettings.ProccesorFieldSetting, null);
            var proccesorFieldsModel = new ProccesorFieldsModel(processor);
            var processors = CreateSelectListField(
                settings.ProccesorFieldsSettings.ProccesorFieldSetting,
                options.Processors,
                null);

            var processorViewModel = new ProccesorFieldsViewModel(proccesorFieldsModel, processors);

            var region = CreateNullableIntegerField(
                settings.OrganizationFieldsSettings.RegionFieldSetting,
                null);
            var department = CreateNullableIntegerField(
                settings.OrganizationFieldsSettings.DepartmentFieldSetting,
                null);
            var domain = CreateNullableIntegerField(settings.OrganizationFieldsSettings.DomainFieldSetting, null);
            var unit = CreateNullableIntegerField(settings.OrganizationFieldsSettings.UnitFieldSetting, null);

            var organizationFieldsModel = new OrganizationFieldsModel(region,department, domain, unit);
            var regions =
                CreateSelectListField(
                    settings.OrganizationFieldsSettings.RegionFieldSetting,
                    options.Regions,
                    null);
            var departments = CreateSelectListField(
                settings.OrganizationFieldsSettings.DepartmentFieldSetting,
                options.Departments,
                null);
            var domains = CreateSelectListField(
                settings.OrganizationFieldsSettings.DomainFieldSetting,
                options.Domains,
                null);
            var ous = CreateSelectListField(settings.OrganizationFieldsSettings.UnitFieldSetting, options.Units, null);

            var organizationViewModel = new OrganizationFieldsViewModel(
                organizationFieldsModel,
                regions,
                departments,
                domains,
                ous);

            var operatingSystem =
                CreateNullableIntegerField(settings.OperatingSystemFieldsSettings.OperatingSystemFieldSetting, null);
            var version = CreateStringField(settings.OperatingSystemFieldsSettings.VersionFieldSetting, null);
            var servicePack = CreateStringField(
                settings.OperatingSystemFieldsSettings.ServicePackSystemFieldSetting,
                null);
            var registratinCode =
                CreateStringField(settings.OperatingSystemFieldsSettings.RegistrationCodeSystemFieldSetting, null);
            var productKey = CreateStringField(settings.OperatingSystemFieldsSettings.ProductKeyFieldSetting, null);

            var operatingSystemFieldsModel = new OperatingSystemFieldsModel(
                operatingSystem,
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

            var memory = CreateNullableIntegerField(settings.MemoryFieldsSettings.RAMFieldSetting, null);
            var memoryFieldsModel = new MemoryFieldsModel(memory);
            var memories = CreateSelectListField(settings.MemoryFieldsSettings.RAMFieldSetting, options.Rams, null);

            var memoryViewModel = new MemoryFieldsViewModel(memoryFieldsModel, memories);

            var barCode = CreateStringField(settings.InventoryFieldsSettings.BarCodeFieldSetting, null);

            var inventoryFieldModel = new InventoryFieldsModel(barCode);

            var chassis = CreateStringField(settings.ChassisFieldsSettings.ChassisFieldSetting, null);

            var chassisFieldModel = new ChassisFieldsModel(chassis);

            var state = CreateNullableIntegerField(settings.StateFieldsSettings.StateFieldSetting, 0);
            var stolen = CreateBooleanField(settings.StateFieldsSettings.StolenFieldSetting, false);
            var replaced = CreateStringField(settings.StateFieldsSettings.ReplacedWithFieldSetting, null);
            var sendBack = CreateBooleanField(settings.StateFieldsSettings.SendBackFieldSetting, false);
            var scrapDate = CreateNullableDateTimeField(settings.StateFieldsSettings.ScrapDateFieldSetting, null);

            var stateFieldsModel = new StateFieldsModel(state, stolen, replaced, sendBack, scrapDate);

            var statuses = CreateSelectListField(
                settings.StateFieldsSettings.StateFieldSetting,
                options.ComputerStatuses,
                null);

            var stateViewModel = new StateFieldsViewModel(stateFieldsModel, statuses);

            var sound = CreateStringField(settings.SoundFieldsSettings.SoundCardFieldSetting, null);

            var soundFieldModel = new SoundFieldsModel(sound);

            var room = CreateNullableIntegerField(settings.PlaceFieldsSettings.RoomFieldSetting, null);
            var building = CreateNullableIntegerField(settings.PlaceFieldsSettings.BuildingFieldSetting, null);
            var floor = CreateNullableIntegerField(settings.PlaceFieldsSettings.FloorFieldSetting, null);
            var address = CreateStringField(settings.PlaceFieldsSettings.AddressFieldSetting, null);
            var postalCode = CreateStringField(settings.PlaceFieldsSettings.PostalCodeFieldSetting, null);
            var postalAddress = CreateStringField(settings.PlaceFieldsSettings.PostalAddressFieldSetting, null);
            var location1 = CreateStringField(settings.PlaceFieldsSettings.PlaceFieldSetting, null);
            var location2 = CreateStringField(settings.PlaceFieldsSettings.Place2FieldSetting, null);

            var placeFieldsModel = new PlaceFieldsModel(
                building,
                floor,
                room,
                address,
                postalCode,
                postalAddress,
                location1,
                location2);

            var buildings = CreateSelectList(settings.PlaceFieldsSettings.BuildingFieldSetting, options.Buildings, null);
            var floors = CreateSelectList(settings.PlaceFieldsSettings.FloorFieldSetting, options.Floors, null);
            var rooms = CreateSelectListField(settings.PlaceFieldsSettings.RoomFieldSetting, options.Rooms, null);

            var placeFieldsViewModel = new PlaceFieldsViewModel(placeFieldsModel, buildings, floors, rooms);

            var other = CreateStringField(settings.OtherFieldsSettings.InfoFieldSetting, null);
            var otherFieldModel = new OtherFieldsModel(other);

            var graphics = CreateStringField(settings.GraphicsFieldsSettings.VideoCardFieldSetting, null);

            var graphicsFieldModel = new GraphicsFieldsModel(graphics);

            var contractStatus = CreateNullableIntegerField(settings.ContractFieldsSettings.ContractStatusFieldSetting, null);
            var contractNumber = CreateStringField(settings.ContractFieldsSettings.ContractNumberFieldSetting, null);
            var contractStartDate = CreateNullableDateTimeField(settings.ContractFieldsSettings.ContractStartDateFieldSetting, null);
            var contractEndDate = CreateNullableDateTimeField(settings.ContractFieldsSettings.ContractEndDateFieldSetting, null);
            var price = CreateIntegerField(settings.ContractFieldsSettings.PurchasePriceFieldSetting, 0);
            var accounting1 = CreateStringField(settings.ContractFieldsSettings.AccountingDimension1FieldSetting, null);
            var accounting2 = CreateStringField(settings.ContractFieldsSettings.AccountingDimension2FieldSetting, null);
            var accounting3 = CreateStringField(settings.ContractFieldsSettings.AccountingDimension3FieldSetting, null);
            var accounting4 = CreateStringField(settings.ContractFieldsSettings.AccountingDimension4FieldSetting, null);
            var accounting5 = CreateStringField(settings.ContractFieldsSettings.AccountingDimension5FieldSetting, null);
            var document = CreateStringField(settings.ContractFieldsSettings.DocumentFieldSetting, null);
            var purchaseDate = CreateNullableDateTimeField(settings.InventoryFieldsSettings.PurchaseDateFieldSetting, null);
            var warrantyEndDate = CreateNullableDateTimeField(settings.ContractFieldsSettings.WarrantyEndDateFieldSettings, null);

            var contractFieldsModel = new ContractFieldsModel(
                contractStatus,
                contractNumber,
                contractStartDate,
                contractEndDate,
                price,
                accounting1,
                accounting2,
                accounting3,
                accounting4,
                accounting5,
                document,
                purchaseDate,
                warrantyEndDate);

            var contractStatuses = CreateSelectListField(
                settings.ContractFieldsSettings.ContractStatusFieldSetting,
                options.ComputerContractStatuses,
                null);

            var contractViewModel = new ContractFieldsViewModel(contractFieldsModel, contractStatuses);

            var contactName = CreateStringField(settings.ContactFieldsSettings.NameFieldSetting, null);
            var contactPhone = CreateStringField(settings.ContactFieldsSettings.PhoneFieldSetting, null);
            var contactEmail = CreateStringField(settings.ContactFieldsSettings.EmailFieldSetting, null);

            var contactFieldsModel = new ContactFieldsModel(contactName, contactPhone, contactEmail);

            var ip = CreateStringField(settings.CommunicationFieldsSettings.IPAddressFieldSetting, null);
            var mac = CreateStringField(settings.CommunicationFieldsSettings.MacAddressFieldSetting, null);
            var ras = CreateBooleanField(settings.CommunicationFieldsSettings.RASFieldSetting, false);
            var client = CreateStringField(settings.CommunicationFieldsSettings.NovellClientFieldSetting, null);

            var communication =
                CreateNullableIntegerField(settings.CommunicationFieldsSettings.NetworkAdapterFieldSetting, null);
            var communicationFieldsModel = new CommunicationFieldsModel(communication, ip, mac, ras, client);

            var adapters = CreateSelectListField(
                settings.CommunicationFieldsSettings.NetworkAdapterFieldSetting,
                options.NetworkAdapters,
                null);

            var communicationViewModel = new CommunicationFieldsViewModel(communicationFieldsModel, adapters);

            var syncDate = CreateNullableDateTimeField(settings.DateFieldsSettings.SyncChangedDateSetting, null);
            var scanDate = CreateNullableDateTimeField(settings.DateFieldsSettings.ScanDateFieldSetting, null);
            var path = CreateStringField(settings.DateFieldsSettings.PathDirectoryFieldSetting, null);

            var dateFieldsModel = new DateFieldsModel(syncDate, scanDate, path);

            var contactInformationFieldsModel =
                new ContactInformationFieldsModel(
                    null, //userId
                    CreateStringField(settings.ContactInformationFieldsSettings.UserIdFieldSetting, null),
                    CreateStringField(settings.ContactInformationFieldsSettings.FirstNameFieldSetting, null),
                    CreateStringField(settings.ContactInformationFieldsSettings.LastNameFieldSetting, null),
                    CreateStringField(settings.ContactInformationFieldsSettings.RegionFieldSetting, null),
                    CreateStringField(settings.ContactInformationFieldsSettings.DepartmentFieldSetting, null),
                    CreateStringField(settings.ContactInformationFieldsSettings.UnitFieldSetting, null));

            return new ComputerViewModel(
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
                workstationViewModel,
				fileUploadWhiteList)
                       {
                           CustomerId = currentCustomerId,
                           DocumentFileKey = Guid.NewGuid().ToString()
                       };
        }

        private static SelectList CreateSelectList(
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

        private static ConfigurableFieldModel<DateTime?> CreateNullableDateTimeField(
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

        private static ConfigurableFieldModel<DateTime> CreateDateTimeField(ModelEditFieldSetting setting, DateTime value)
        {
            return !setting.IsShow
                       ? ConfigurableFieldModel<DateTime>.CreateUnshowable()
                       : new ConfigurableFieldModel<DateTime>(
                             setting.Caption,
                             value,
                             setting.IsRequired,
                             setting.IsReadOnly);
        }

        private static SelectList CreateSelectListField(
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

        private static SelectList CreateSelectListField(
            ModelEditFieldSetting setting,
            Enum items,
            string selectedValue)
        {
            if (!setting.IsShow)
            {
                return new SelectList(Enumerable.Empty<SelectListItem>());
            }

            var list = items.ToSelectList(selectedValue);
            return list;
        }

        private static ConfigurableFieldModel<string> CreateStringField(ModelEditFieldSetting setting, string value)
        {
            return !setting.IsShow
                       ? ConfigurableFieldModel<string>.CreateUnshowable()
                       : new ConfigurableFieldModel<string>(
                             setting.Caption,
                             value,
                             setting.IsRequired,
                             setting.IsReadOnly);
        }

        private static ConfigurableFieldModel<bool> CreateBooleanField(ModelEditFieldSetting setting, bool value)
        {
            return !setting.IsShow
                       ? ConfigurableFieldModel<bool>.CreateUnshowable()
                       : new ConfigurableFieldModel<bool>(setting.Caption, value, setting.IsRequired, setting.IsReadOnly);
        }

        private static ConfigurableFieldModel<int> CreateIntegerField(ModelEditFieldSetting setting, int value)
        {
            return !setting.IsShow
                       ? ConfigurableFieldModel<int>.CreateUnshowable()
                       : new ConfigurableFieldModel<int>(setting.Caption, value, setting.IsRequired, setting.IsReadOnly);
        }

        private static ConfigurableFieldModel<int?> CreateNullableIntegerField(ModelEditFieldSetting setting, int? value)
        {
            return !setting.IsShow
                       ? ConfigurableFieldModel<int?>.CreateUnshowable()
                       : new ConfigurableFieldModel<int?>(setting.Caption, value, setting.IsRequired, setting.IsReadOnly);
        }
    }
}