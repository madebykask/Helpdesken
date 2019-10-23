using DH.Helpdesk.BusinessData.Models.Inventory.Output.Computer;

namespace DH.Helpdesk.Web.Infrastructure.BusinessModelFactories.Inventory.Concrete
{
    using System;

    using DH.Helpdesk.BusinessData.Models;
    using DH.Helpdesk.BusinessData.Models.Inventory.Edit.Computer;
    using DH.Helpdesk.BusinessData.Models.Inventory.Edit.Shared;
    using DH.Helpdesk.Web.Areas.Inventory.Models.EditModel.Computer;

    using PlaceFields = DH.Helpdesk.BusinessData.Models.Inventory.Edit.Computer.PlaceFields;

    public class ComputerBuilder : IComputerBuilder
    {
        public ComputerForUpdate BuildForUpdate(ComputerViewModel model, OperationContext contex)
        {
            var workstation = CreateWorkstation(model.WorkstationFieldsViewModel);
            var chassis = CreateChassis(model.ChassisFieldsModel);
            var inventering = CreateInventering(model.InventoryFieldsModel);
            var operatingSystem = CreateOperatingSystem(model.OperatingSystemFieldsViewModel);
            var processor = CreateProcessor(model.ProccesorFieldsViewModel);
            var memory = CreateMemory(model.MemoryFieldsViewModel);
            var communication = CreateCommunication(model.CommunicationFieldsViewModel);
            var graphics = CreateGraphics(model.GraphicsFieldsModel);
            var sound = CreateSound(model.SoundFieldsModel);
            var contract = CreateContract(model.ContractFieldsViewModel);
            var other = CreateOther(model.OtherFieldsModel);
            var contactInformation = CreateContactInformation(model.ContactInformationFieldsModel);
            var organization = CreateOrganization(model.OrganizationFieldsViewModel);
            var place = CreatePlace(model.PlaceFieldsViewModel);
            var contact = CreateContact(model.ContactFieldsModel);
            var state = CreateState(model.StateFieldsViewModel);

            // var date = CreateDate(model.DateFieldsModel); todo
            var fieldsModel = new ComputerForUpdate(
                model.Id,
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
                contex.UserId,
                contex.DateAndTime);

            return fieldsModel;
        }

        public ComputerForInsert BuildForAdd(ComputerViewModel model, OperationContext context, ComputerFile computerFile)
        {
            var workstation = CreateWorkstation(model.WorkstationFieldsViewModel);
            var chassis = CreateChassis(model.ChassisFieldsModel);
            var inventering = CreateInventering(model.InventoryFieldsModel);

            var operatingSystem = CreateOperatingSystem(
                model.OperatingSystemFieldsViewModel);

            var processor = CreateProcessor(model.ProccesorFieldsViewModel);

            var memory = CreateMemory(model.MemoryFieldsViewModel);

            var communication = CreateCommunication(model.CommunicationFieldsViewModel);

            var graphics = CreateGraphics(model.GraphicsFieldsModel);
            var sound = CreateSound(model.SoundFieldsModel);

            var contract = CreateContract(model.ContractFieldsViewModel);

            var other = CreateOther(model.OtherFieldsModel);
            var contactInformation = CreateContactInformation(model.ContactInformationFieldsModel);

            var organization = CreateOrganization(model.OrganizationFieldsViewModel);

            var place = CreatePlace(model.PlaceFieldsViewModel);

            var contact = CreateContact(model.ContactFieldsModel);

            var state = CreateState(model.StateFieldsViewModel);

            // var date = CreateDate(model.DateFieldsModel);
            var fieldsModel = new ComputerForInsert(
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
                context.CustomerId,
                computerFile,
                context.UserId,
                context.DateAndTime);

            return fieldsModel;
        }

        private static WorkstationFields CreateWorkstation(WorkstationFieldsViewModel fieldsModel)
        {
            if (fieldsModel == null || fieldsModel.WorkstationFieldsModel == null)
            {
                return WorkstationFields.CreateDefault();
            }

            var name = ConfigurableFieldModel<string>.GetValueOrDefault(fieldsModel.WorkstationFieldsModel.Name);
            var manufacturer = ConfigurableFieldModel<string>.GetValueOrDefault(fieldsModel.WorkstationFieldsModel.Manufacturer);
            var model = ConfigurableFieldModel<int?>.GetValueOrDefault(fieldsModel.WorkstationFieldsModel.ComputerModelId);
            var serialNumber = ConfigurableFieldModel<string>.GetValueOrDefault(fieldsModel.WorkstationFieldsModel.SerialNumber);
            var biosDate = ConfigurableFieldModel<DateTime?>.GetValueOrDefault(fieldsModel.WorkstationFieldsModel.BIOSDate);
            var biosVersion = ConfigurableFieldModel<string>.GetValueOrDefault(fieldsModel.WorkstationFieldsModel.BIOSVersion);
            var theftMark = ConfigurableFieldModel<string>.GetValueOrDefault(fieldsModel.WorkstationFieldsModel.Theftmark);
            var carepackNumber = ConfigurableFieldModel<string>.GetValueOrDefault(fieldsModel.WorkstationFieldsModel.CarePackNumber);
            var computerType = ConfigurableFieldModel<int?>.GetValueOrDefault(fieldsModel.WorkstationFieldsModel.ComputerTypeId);
            var place = ConfigurableFieldModel<string>.GetValueOrDefault(fieldsModel.WorkstationFieldsModel.Location);

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

        private static OperatingSystemFields CreateOperatingSystem(OperatingSystemFieldsViewModel fieldsModel)
        {
            if (fieldsModel == null || fieldsModel.OperatingSystemFieldsModel == null)
            {
                return OperatingSystemFields.CreateDefault();
            }

            var operatingSystem = ConfigurableFieldModel<int?>.GetValueOrDefault(fieldsModel.OperatingSystemFieldsModel.OperatingSystemId);
            var version = ConfigurableFieldModel<string>.GetValueOrDefault(fieldsModel.OperatingSystemFieldsModel.Version);
            var servicePack = ConfigurableFieldModel<string>.GetValueOrDefault(fieldsModel.OperatingSystemFieldsModel.ServicePack);
            var registrationCode = ConfigurableFieldModel<string>.GetValueOrDefault(fieldsModel.OperatingSystemFieldsModel.RegistrationCode);
            var productKey = ConfigurableFieldModel<string>.GetValueOrDefault(fieldsModel.OperatingSystemFieldsModel.ProductKey);

            var fields = new OperatingSystemFields(
                operatingSystem,
                version,
                servicePack,
                registrationCode,
                productKey);

            return fields;
        }

        private static ProcessorFields CreateProcessor(ProccesorFieldsViewModel fieldsModel)
        {
            if (fieldsModel == null || fieldsModel.ProccesorFieldsModel == null)
            {
                return ProcessorFields.CreateDefault();
            }

            var processor = ConfigurableFieldModel<int?>.GetValueOrDefault(fieldsModel.ProccesorFieldsModel.ProccesorId);

            var fields = new ProcessorFields(processor);

            return fields;
        }

        private static MemoryFields CreateMemory(MemoryFieldsViewModel fieldsModel)
        {
            if (fieldsModel == null || fieldsModel.MemoryFieldsModel == null)
            {
                return MemoryFields.CreateDefault();
            }

            var memory = ConfigurableFieldModel<int?>.GetValueOrDefault(fieldsModel.MemoryFieldsModel.RAMId);

            var fields = new MemoryFields(memory);

            return fields;
        }

        private static CommunicationFields CreateCommunication(CommunicationFieldsViewModel fieldsModel)
        {
            if (fieldsModel == null || fieldsModel.CommunicationFieldsModel == null)
            {
                return CommunicationFields.CreateDefault();
            }

            var networkAdapter = ConfigurableFieldModel<int?>.GetValueOrDefault(fieldsModel.CommunicationFieldsModel.NetworkAdapterId);
            var ipaddress = ConfigurableFieldModel<string>.GetValueOrDefault(fieldsModel.CommunicationFieldsModel.IPAddress);
            var macAddress = ConfigurableFieldModel<string>.GetValueOrDefault(fieldsModel.CommunicationFieldsModel.MacAddress);
            var ras = ConfigurableFieldModel<bool>.GetValueOrDefault(fieldsModel.CommunicationFieldsModel.IsRAS);
            var novellClient = ConfigurableFieldModel<string>.GetValueOrDefault(fieldsModel.CommunicationFieldsModel.NovellClient);

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

        private static ContractFields CreateContract(ContractFieldsViewModel fieldsModel)
        {
            if (fieldsModel == null || fieldsModel.ContractFieldsModel == null)
            {
                return ContractFields.CreateDefault();
            }

            var contractStatusName = ConfigurableFieldModel<int?>.GetValueOrDefault(fieldsModel.ContractFieldsModel.ContractStatusId);
            var contractNumber = ConfigurableFieldModel<string>.GetValueOrDefault(fieldsModel.ContractFieldsModel.ContractNumber);
            var contractStartDate = ConfigurableFieldModel<DateTime?>.GetValueOrDefault(fieldsModel.ContractFieldsModel.ContractStartDate);
            var contractEndDate = ConfigurableFieldModel<DateTime?>.GetValueOrDefault(fieldsModel.ContractFieldsModel.ContractEndDate);
            var purchasePrice = ConfigurableFieldModel<int>.GetValueOrDefault(fieldsModel.ContractFieldsModel.PurchasePrice);
            var accountingDimension1 = ConfigurableFieldModel<string>.GetValueOrDefault(fieldsModel.ContractFieldsModel.AccountingDimension1);
            var accountingDimension2 = ConfigurableFieldModel<string>.GetValueOrDefault(fieldsModel.ContractFieldsModel.AccountingDimension2);
            var accountingDimension3 = ConfigurableFieldModel<string>.GetValueOrDefault(fieldsModel.ContractFieldsModel.AccountingDimension3);
            var accountingDimension4 = ConfigurableFieldModel<string>.GetValueOrDefault(fieldsModel.ContractFieldsModel.AccountingDimension4);
            var accountingDimension5 = ConfigurableFieldModel<string>.GetValueOrDefault(fieldsModel.ContractFieldsModel.AccountingDimension5);

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
                accountingDimension5,
                null);

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

            if (string.IsNullOrEmpty(fieldsModel.UserStringId.Value))
                fieldsModel.UserId = null;
            // todo
            var fields = new ContactInformationFields(fieldsModel.UserId);

            return fields;
        }

        private static OrganizationFields CreateOrganization(OrganizationFieldsViewModel fieldsModel)
        {
            if (fieldsModel == null || fieldsModel.OrganizationFieldsModel == null)
            {
                return OrganizationFields.CreateDefault();
            }

            var region = ConfigurableFieldModel<int?>.GetValueOrDefault(fieldsModel.OrganizationFieldsModel.RegionId);
            var department = ConfigurableFieldModel<int?>.GetValueOrDefault(fieldsModel.OrganizationFieldsModel.DepartmentId);
            var domain = ConfigurableFieldModel<int?>.GetValueOrDefault(fieldsModel.OrganizationFieldsModel.DomainId);
            var unit = ConfigurableFieldModel<int?>.GetValueOrDefault(fieldsModel.OrganizationFieldsModel.UnitId);

            var fields = new OrganizationFields(region, department, domain, unit);

            return fields;
        }

        private static PlaceFields CreatePlace(PlaceFieldsViewModel fieldsModel)
        {
            if (fieldsModel == null || fieldsModel.PlaceFieldsModel == null)
            {
                return PlaceFields.CreateDefault();
            }

            var room = ConfigurableFieldModel<int?>.GetValueOrDefault(fieldsModel.PlaceFieldsModel.RoomId);
            var address = ConfigurableFieldModel<string>.GetValueOrDefault(fieldsModel.PlaceFieldsModel.Address);
            var postalCode = ConfigurableFieldModel<string>.GetValueOrDefault(fieldsModel.PlaceFieldsModel.PostalCode);
            var postalAddress = ConfigurableFieldModel<string>.GetValueOrDefault(fieldsModel.PlaceFieldsModel.PostalAddress);
            var location = ConfigurableFieldModel<string>.GetValueOrDefault(fieldsModel.PlaceFieldsModel.Location);
            var location2 = ConfigurableFieldModel<string>.GetValueOrDefault(fieldsModel.PlaceFieldsModel.Location2);

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

        private static StateFields CreateState(StateFieldsViewModel fieldsModel)
        {
            if (fieldsModel?.StateFieldsModel == null)
            {
                return StateFields.CreateDefault();
            }

            var state = ConfigurableFieldModel<int?>.GetValueOrDefault(fieldsModel.StateFieldsModel.StateId);
            var stolen = ConfigurableFieldModel<bool>.GetValueOrDefault(fieldsModel.StateFieldsModel.IsStolen);
            var replaced = ConfigurableFieldModel<string>.GetValueOrDefault(fieldsModel.StateFieldsModel.Replaced);
            var sendBack = ConfigurableFieldModel<bool>.GetValueOrDefault(fieldsModel.StateFieldsModel.IsSendBack);
            var scrapDate = ConfigurableFieldModel<DateTime?>.GetValueOrDefault(fieldsModel.StateFieldsModel.ScrapDate);

            var fields = new StateFields(state ?? 0, stolen, replaced, sendBack, scrapDate);

            return fields;
        }

        //private static DateFields CreateDate(DateFieldsModel fieldsModel)
        //{
        //    if (fieldsModel == null)
        //    {
        //        return DateFields.CreateDefault();
        //    }

        //    var synchronizeDate = ConfigurableFieldModel<DateTime?>.GetValueOrDefault(fieldsModel.SynchronizeDate);
        //    var scanDate = ConfigurableFieldModel<DateTime?>.GetValueOrDefault(fieldsModel.ScanDate);
        //    var pathDirectory = ConfigurableFieldModel<string>.GetValueOrDefault(fieldsModel.PathDirectory);

        //    var fields = new DateFields(synchronizeDate, scanDate, pathDirectory);

        //    return fields;
        //}
    }
}