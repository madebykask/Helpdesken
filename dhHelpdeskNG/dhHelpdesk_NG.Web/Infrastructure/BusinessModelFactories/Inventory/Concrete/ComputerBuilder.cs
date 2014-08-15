namespace DH.Helpdesk.Web.Infrastructure.BusinessModelFactories.Inventory.Concrete
{
    using System;

    using DH.Helpdesk.BusinessData.Models;
    using DH.Helpdesk.BusinessData.Models.Inventory.Edit.Computer;
    using DH.Helpdesk.BusinessData.Models.Inventory.Edit.Shared;
    using DH.Helpdesk.Web.Models.Inventory.EditModel.Computer;

    using PlaceFields = DH.Helpdesk.BusinessData.Models.Inventory.Edit.Computer.PlaceFields;

    public class ComputerBuilder : IComputerBuilder
    {
        public Computer BuildForUpdate(ComputerViewModel model)
        {
            var workstation = CreateWorkstation(model.WorkstationFieldsViewModel.WorkstationFieldsModel);
            var chassis = CreateChassis(model.ChassisFieldsModel);
            var inventering = CreateInventering(model.InventoryFieldsModel);
            var operatingSystem = CretateOperatingSystem(model.OperatingSystemFieldsViewModel.OperatingSystemFieldsModel);
            var processor = CreateProcessor(model.ProccesorFieldsViewModel.ProccesorFieldsModel);
            var memory = CreateMemory(model.MemoryFieldsViewModel.MemoryFieldsModel);
            var communication = CreateCommunication(model.CommunicationFieldsViewModel.CommunicationFieldsModel);
            var graphics = CreateGraphics(model.GraphicsFieldsModel);
            var sound = CreateSound(model.SoundFieldsModel);
            var contract = CreateContract(model.ContractFieldsViewModel.ContractFieldsModel);
            var other = CreateOther(model.OtherFieldsModel);
            var contactInformation = CreateContactInformation(model.ContactInformationFieldsModel);
            var organization = CreateOrganization(model.OrganizationFieldsViewModel.OrganizationFieldsModel);
            var place = CreatePlace(model.PlaceFieldsViewModel.PlaceFieldsModel);
            var contact = CreateContact(model.ContactFieldsModel);
            var state = CreateState(model.StateFieldsViewModel.StateFieldsModel);
            var date = CreateDate(model.DateFieldsModel);

            var fieldsModel = Computer.CreateUpdated(
                model.Id,
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
                workstation,
                DateTime.Now);

            return fieldsModel;
        }

        public Computer BuildForAdd(ComputerViewModel model, OperationContext context)
        {
            var workstation = CreateWorkstation(model.WorkstationFieldsViewModel.WorkstationFieldsModel);
            var chassis = CreateChassis(model.ChassisFieldsModel);
            var inventering = CreateInventering(model.InventoryFieldsModel);
            var operatingSystem = CretateOperatingSystem(model.OperatingSystemFieldsViewModel.OperatingSystemFieldsModel);
            var processor = CreateProcessor(model.ProccesorFieldsViewModel.ProccesorFieldsModel);
            var memory = CreateMemory(model.MemoryFieldsViewModel.MemoryFieldsModel);
            var communication = CreateCommunication(model.CommunicationFieldsViewModel.CommunicationFieldsModel);
            var graphics = CreateGraphics(model.GraphicsFieldsModel);
            var sound = CreateSound(model.SoundFieldsModel);
            var contract = CreateContract(model.ContractFieldsViewModel.ContractFieldsModel);
            var other = CreateOther(model.OtherFieldsModel);
            var contactInformation = CreateContactInformation(model.ContactInformationFieldsModel);
            var organization = CreateOrganization(model.OrganizationFieldsViewModel.OrganizationFieldsModel);
            var place = CreatePlace(model.PlaceFieldsViewModel.PlaceFieldsModel);
            var contact = CreateContact(model.ContactFieldsModel);
            var state = CreateState(model.StateFieldsViewModel.StateFieldsModel);
            var date = CreateDate(model.DateFieldsModel);

            var fieldsModel = Computer.CreateNew(
                context.CustomerId,
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
                workstation,
                DateTime.Now);

            return fieldsModel;
        }

        private static WorkstationFields CreateWorkstation(WorkstationFieldsModel fieldsModel)
        {
            if (fieldsModel == null)
            {
                return WorkstationFields.CreateDefault();
            }

            var name = ConfigurableFieldModel<string>.GetValueOrDefault(fieldsModel.Name);
            var manufacturer = ConfigurableFieldModel<string>.GetValueOrDefault(fieldsModel.Manufacturer);
            var model = ConfigurableFieldModel<int?>.GetValueOrDefault(fieldsModel.ComputerModelId);
            var serialNumber = ConfigurableFieldModel<string>.GetValueOrDefault(fieldsModel.SerialNumber);
            var biosDate = ConfigurableFieldModel<DateTime?>.GetValueOrDefault(fieldsModel.BIOSDate);
            var biosVersion = ConfigurableFieldModel<string>.GetValueOrDefault(fieldsModel.BIOSVersion);
            var theftMark = ConfigurableFieldModel<string>.GetValueOrDefault(fieldsModel.Theftmark);
            var carepackNumber = ConfigurableFieldModel<string>.GetValueOrDefault(fieldsModel.CarePackNumber);
            var computerType = ConfigurableFieldModel<int?>.GetValueOrDefault(fieldsModel.ComputerTypeId);
            var place = ConfigurableFieldModel<string>.GetValueOrDefault(fieldsModel.Location);

            var fields = new WorkstationFields(
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

            return fields;
        }

        private static ChassisFields CreateChassis(ChassisFieldsModel fieldsModel)
        {
            if (fieldsModel == null)
            {
                return ChassisFields.CreateDefault();
            }

            var chassis = ConfigurableFieldModel<string>.GetValueOrDefault(fieldsModel.Chassis);

            return new ChassisFields(chassis);
        }

        private static InventoryFields CreateInventering(InventoryFieldsModel fieldsModel)
        {
            if (fieldsModel == null)
            {
                return InventoryFields.CreateDefault();
            }

            var barCode = ConfigurableFieldModel<string>.GetValueOrDefault(fieldsModel.BarCode);
            var purchaseDate = ConfigurableFieldModel<DateTime?>.GetValueOrDefault(fieldsModel.PurchaseDate);

            var fields = new InventoryFields(barCode, purchaseDate);

            return fields;
        }

        private static OperatingSystemFields CretateOperatingSystem(OperatingSystemFieldsModel fieldsModel)
        {
            if (fieldsModel == null)
            {
                return OperatingSystemFields.CreateDefault();
            }

            var operatingSystem = ConfigurableFieldModel<int?>.GetValueOrDefault(fieldsModel.OperatingSystemId);
            var version = ConfigurableFieldModel<string>.GetValueOrDefault(fieldsModel.Version);
            var servicePack = ConfigurableFieldModel<string>.GetValueOrDefault(fieldsModel.ServicePack);
            var registrationCode = ConfigurableFieldModel<string>.GetValueOrDefault(fieldsModel.RegistrationCode);
            var productKey = ConfigurableFieldModel<string>.GetValueOrDefault(fieldsModel.ProductKey);

            var fields = new OperatingSystemFields(
                operatingSystem,
                version,
                servicePack,
                registrationCode,
                productKey);

            return fields;
        }

        private static ProcessorFields CreateProcessor(ProccesorFieldsModel fieldsModel)
        {
            if (fieldsModel == null)
            {
                return ProcessorFields.CreateDefault();
            }

            var processor = ConfigurableFieldModel<int?>.GetValueOrDefault(fieldsModel.ProccesorId);

            var fields = new ProcessorFields(processor);

            return fields;
        }

        private static MemoryFields CreateMemory(MemoryFieldsModel fieldsModel)
        {
            if (fieldsModel == null)
            {
                return MemoryFields.CreateDefault();
            }

            var memory = ConfigurableFieldModel<int?>.GetValueOrDefault(fieldsModel.RAMId);

            var fields = new MemoryFields(memory);

            return fields;
        }

        private static CommunicationFields CreateCommunication(CommunicationFieldsModel fieldsModel)
        {
            if (fieldsModel == null)
            {
                return CommunicationFields.CreateDefault();
            }

            var networkAdapter = ConfigurableFieldModel<int?>.GetValueOrDefault(fieldsModel.NetworkAdapterId);
            var ipaddress = ConfigurableFieldModel<string>.GetValueOrDefault(fieldsModel.IPAddress);
            var macAddress = ConfigurableFieldModel<string>.GetValueOrDefault(fieldsModel.MacAddress);
            var ras = ConfigurableFieldModel<bool>.GetValueOrDefault(fieldsModel.IsRAS);
            var novellClient = ConfigurableFieldModel<string>.GetValueOrDefault(fieldsModel.NovellClient);

            var fields = new CommunicationFields(
                networkAdapter,
                ipaddress,
                macAddress,
                ras,
                novellClient);

            return fields;
        }

        private static GraphicsFields CreateGraphics(GraphicsFieldsModel fieldsModel)
        {
            if (fieldsModel == null)
            {
                return GraphicsFields.CreateDefault();
            }

            var graphics = ConfigurableFieldModel<string>.GetValueOrDefault(fieldsModel.VideoCard);

            var fields = new GraphicsFields(graphics);

            return fields;
        }

        private static SoundFields CreateSound(SoundFieldsModel fieldsModel)
        {
            if (fieldsModel == null)
            {
                return SoundFields.CreateDefault();
            }

            var sound = ConfigurableFieldModel<string>.GetValueOrDefault(fieldsModel.SoundCard);

            var fields = new SoundFields(sound);

            return fields;
        }

        private static ContractFields CreateContract(ContractFieldsModel fieldsModel)
        {
            if (fieldsModel == null)
            {
                return ContractFields.CreateDefault();
            }

            var contractStatusName = ConfigurableFieldModel<int?>.GetValueOrDefault(fieldsModel.ContractStatusId);
            var contractNumber = ConfigurableFieldModel<string>.GetValueOrDefault(fieldsModel.ContractNumber);
            var contractStartDate = ConfigurableFieldModel<DateTime?>.GetValueOrDefault(fieldsModel.ContractStartDate);
            var contractEndDate = ConfigurableFieldModel<DateTime?>.GetValueOrDefault(fieldsModel.ContractEndDate);
            var purchasePrice = ConfigurableFieldModel<int>.GetValueOrDefault(fieldsModel.PurchasePrice);
            var accountingDimension1 = ConfigurableFieldModel<string>.GetValueOrDefault(fieldsModel.AccountingDimension1);
            var accountingDimension2 = ConfigurableFieldModel<string>.GetValueOrDefault(fieldsModel.AccountingDimension2);
            var accountingDimension3 = ConfigurableFieldModel<string>.GetValueOrDefault(fieldsModel.AccountingDimension3);
            var accountingDimension4 = ConfigurableFieldModel<string>.GetValueOrDefault(fieldsModel.AccountingDimension4);
            var accountingDimension5 = ConfigurableFieldModel<string>.GetValueOrDefault(fieldsModel.AccountingDimension5);

            var fields = new ContractFields(
                contractStatusName,
                contractNumber,
                contractStartDate,
                contractEndDate,
                purchasePrice,
                accountingDimension1,
                accountingDimension2,
                accountingDimension3,
                accountingDimension4,
                accountingDimension5);

            return fields;
        }

        private static OtherFields CreateOther(OtherFieldsModel fieldsModel)
        {
            if (fieldsModel == null)
            {
                return OtherFields.CreateDefault();
            }

            var info = ConfigurableFieldModel<string>.GetValueOrDefault(fieldsModel.Info);

            var fields = new OtherFields(info);

            return fields;
        }

        private static ContactInformationFields CreateContactInformation(ContactInformationFieldsModel fieldsModel)
        {
            if (fieldsModel == null)
            {
                return ContactInformationFields.CreateDefault();
            }

            // todo
            var fields = new ContactInformationFields(fieldsModel.UserId);

            return fields;
        }

        private static OrganizationFields CreateOrganization(OrganizationFieldsModel fieldsModel)
        {
            if (fieldsModel == null)
            {
                return OrganizationFields.CreateDefault();
            }

            var department = ConfigurableFieldModel<int?>.GetValueOrDefault(fieldsModel.DepartmentId);
            var domain = ConfigurableFieldModel<int?>.GetValueOrDefault(fieldsModel.DomainId);
            var unit = ConfigurableFieldModel<int?>.GetValueOrDefault(fieldsModel.UnitId);

            var fields = new OrganizationFields(department, domain, unit);

            return fields;
        }

        private static PlaceFields CreatePlace(PlaceFieldsModel fieldsModel)
        {
            if (fieldsModel == null)
            {
                return PlaceFields.CreateDefault();
            }

            var room = ConfigurableFieldModel<int?>.GetValueOrDefault(fieldsModel.RoomId);
            var address = ConfigurableFieldModel<string>.GetValueOrDefault(fieldsModel.Address);
            var postalCode = ConfigurableFieldModel<string>.GetValueOrDefault(fieldsModel.PostalCode);
            var postalAddress = ConfigurableFieldModel<string>.GetValueOrDefault(fieldsModel.PostalAddress);
            var location = ConfigurableFieldModel<string>.GetValueOrDefault(fieldsModel.Location);
            var location2 = ConfigurableFieldModel<string>.GetValueOrDefault(fieldsModel.Location2);

            var fields = new PlaceFields(room, address, postalCode, postalAddress, location, location2);

            return fields;
        }

        private static ContactFields CreateContact(ContactFieldsModel fieldsModel)
        {
            if (fieldsModel == null)
            {
                return ContactFields.CreateDefault();
            }

            var name = ConfigurableFieldModel<string>.GetValueOrDefault(fieldsModel.Name);
            var phone = ConfigurableFieldModel<string>.GetValueOrDefault(fieldsModel.Phone);
            var email = ConfigurableFieldModel<string>.GetValueOrDefault(fieldsModel.Email);

            var fields = new ContactFields(name, phone, email);

            return fields;
        }

        private static StateFields CreateState(StateFieldsModel fieldsModel)
        {
            if (fieldsModel == null)
            {
                return StateFields.CreateDefault();
            }

            var state = ConfigurableFieldModel<int?>.GetValueOrDefault(fieldsModel.StateId);
            var stolen = ConfigurableFieldModel<bool>.GetValueOrDefault(fieldsModel.IsStolen);
            var replaced = ConfigurableFieldModel<string>.GetValueOrDefault(fieldsModel.Replaced);
            var sendBack = ConfigurableFieldModel<bool>.GetValueOrDefault(fieldsModel.IsSendBack);
            var scrapDate = ConfigurableFieldModel<DateTime?>.GetValueOrDefault(fieldsModel.ScrapDate);

            var fields = new StateFields(state.HasValue ? state.Value : 0, stolen, replaced, sendBack, scrapDate);

            return fields;
        }

        private static DateFields CreateDate(DateFieldsModel fieldsModel)
        {
            if (fieldsModel == null)
            {
                return DateFields.CreateDefault();
            }

            var synchronizeDate = ConfigurableFieldModel<DateTime?>.GetValueOrDefault(fieldsModel.SynchronizeDate);
            var scanDate = ConfigurableFieldModel<DateTime?>.GetValueOrDefault(fieldsModel.ScanDate);
            var pathDirectory = ConfigurableFieldModel<string>.GetValueOrDefault(fieldsModel.PathDirectory);

            var fields = new DateFields(synchronizeDate, scanDate, pathDirectory);

            return fields;
        }
    }
}