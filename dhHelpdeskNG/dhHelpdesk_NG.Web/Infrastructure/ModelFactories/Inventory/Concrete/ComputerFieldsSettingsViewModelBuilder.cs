namespace DH.Helpdesk.Web.Infrastructure.ModelFactories.Inventory.Concrete
{
    using DH.Helpdesk.BusinessData.Models.Inventory.Edit.Settings.ComputerSettings;
    using DH.Helpdesk.Web.Models.Inventory.EditModel.Settings;
    using DH.Helpdesk.Web.Models.Inventory.EditModel.Settings.Computer;
    using DH.Helpdesk.Web.Models.Inventory.EditModel.Settings.Shared;

    using PlaceFieldsSettingsModel = DH.Helpdesk.Web.Models.Inventory.EditModel.Settings.Computer.PlaceFieldsSettingsModel;

    public class ComputerFieldsSettingsViewModelBuilder : IComputerFieldsSettingsViewModelBuilder
    {
        private readonly IFieldSettingModelBuilder settingModelBuilder;

        public ComputerFieldsSettingsViewModelBuilder(IFieldSettingModelBuilder settingModelBuilder)
        {
            this.settingModelBuilder = settingModelBuilder;
        }

        public ComputerFieldsSettingsViewModel BuildViewModel(ComputerFieldsSettings settings)
        {
            var createdDate = this.settingModelBuilder.MapFieldSetting(settings.DateFieldsSettings.CreatedDateFieldSetting);
            var changedDate = this.settingModelBuilder.MapFieldSetting(settings.DateFieldsSettings.ChangedDateFieldSetting);
            var syncDate = this.settingModelBuilder.MapFieldSetting(settings.DateFieldsSettings.SyncChangedDateSetting);
            var scanDate = this.settingModelBuilder.MapFieldSetting(settings.DateFieldsSettings.ScanDateFieldSetting);
            var pathDate = this.settingModelBuilder.MapFieldSetting(settings.DateFieldsSettings.PathDirectoryFieldSetting);
            var dateFieldsSettingsModel = new DateFieldsSettingsModel(
                createdDate,
                changedDate,
                syncDate,
                scanDate,
                pathDate);

            var network = this.settingModelBuilder.MapFieldSetting(settings.CommunicationFieldsSettings.NetworkAdapterFieldSetting);
            var ipaddress = this.settingModelBuilder.MapFieldSetting(settings.CommunicationFieldsSettings.IPAddressFieldSetting);
            var macAddress = this.settingModelBuilder.MapFieldSetting(settings.CommunicationFieldsSettings.MacAddressFieldSetting);
            var ras = this.settingModelBuilder.MapFieldSetting(settings.CommunicationFieldsSettings.RASFieldSetting);
            var novell = this.settingModelBuilder.MapFieldSetting(settings.CommunicationFieldsSettings.NovellClientFieldSetting);
            var communicationFieldsSettingsModel = new CommunicationFieldsSettingsModel(
                network,
                ipaddress,
                macAddress,
                ras,
                novell);

            var name = this.settingModelBuilder.MapFieldSetting(settings.ContactFieldsSettings.NameFieldSetting);
            var phone = this.settingModelBuilder.MapFieldSetting(settings.ContactFieldsSettings.PhoneFieldSetting);
            var email = this.settingModelBuilder.MapFieldSetting(settings.ContactFieldsSettings.EmailFieldSetting);
            var contactFieldsSettingsModel = new ContactFieldsSettingsModel(name, phone, email);

            var user = this.settingModelBuilder.MapFieldSetting(settings.ContactInformationFieldsSettings.UserIdFieldSetting);
            var contactInformationFieldsSettingsModel = new ContactInformationFieldsSettingsModel(user);

            var status = this.settingModelBuilder.MapFieldSetting(settings.ContractFieldsSettings.ContractStatusFieldSetting);
            var number = this.settingModelBuilder.MapFieldSetting(settings.ContractFieldsSettings.ContractNumberFieldSetting);
            var startDate = this.settingModelBuilder.MapFieldSetting(settings.ContractFieldsSettings.ContractStartDateFieldSetting);
            var endDate = this.settingModelBuilder.MapFieldSetting(settings.ContractFieldsSettings.ContractEndDateFieldSetting);
            var price = this.settingModelBuilder.MapFieldSetting(settings.ContractFieldsSettings.PurchasePriceFieldSetting);
            var accounting1 = this.settingModelBuilder.MapFieldSetting(settings.ContractFieldsSettings.AccountingDimension1FieldSetting);
            var accounting2 = this.settingModelBuilder.MapFieldSetting(settings.ContractFieldsSettings.AccountingDimension2FieldSetting);
            var accounting3 = this.settingModelBuilder.MapFieldSetting(settings.ContractFieldsSettings.AccountingDimension3FieldSetting);
            var accounting4 = this.settingModelBuilder.MapFieldSetting(settings.ContractFieldsSettings.AccountingDimension4FieldSetting);
            var accounting5 = this.settingModelBuilder.MapFieldSetting(settings.ContractFieldsSettings.AccountingDimension5FieldSetting);
            var contractFieldsSettingsModel = new ContractFieldsSettingsModel(
                status,
                number,
                startDate,
                endDate,
                price,
                accounting1,
                accounting2,
                accounting3,
                accounting4,
                accounting5,
                new FieldSettingModel()); // todo

            var video = this.settingModelBuilder.MapFieldSetting(settings.GraphicsFieldsSettings.VideoCardFieldSetting);
            var graphicsFieldsSettingsModel = new GraphicsFieldsSettingsModel(video);

            var info = this.settingModelBuilder.MapFieldSetting(settings.OtherFieldsSettings.InfoFieldSetting);
            var otherFieldsSettingsModel = new OtherFieldsSettingsModel(info);

            var room = this.settingModelBuilder.MapFieldSetting(settings.PlaceFieldsSettings.RoomFieldSetting);
            var address = this.settingModelBuilder.MapFieldSetting(settings.PlaceFieldsSettings.AddressFieldSetting);
            var postalCode = this.settingModelBuilder.MapFieldSetting(settings.PlaceFieldsSettings.PostalCodeFieldSetting);
            var postalAddress = this.settingModelBuilder.MapFieldSetting(settings.PlaceFieldsSettings.PostalAddressFieldSetting);
            var place = this.settingModelBuilder.MapFieldSetting(settings.PlaceFieldsSettings.PlaceFieldSetting);
            var place2 = this.settingModelBuilder.MapFieldSetting(settings.PlaceFieldsSettings.Place2FieldSetting);
            var placeFieldsSettingsModel = new PlaceFieldsSettingsModel(
                room,
                address,
                postalCode,
                postalAddress,
                place,
                place2);

            var sound = this.settingModelBuilder.MapFieldSetting(settings.SoundFieldsSettings.SoundCardFieldSetting);
            var soundFieldsSettingsModel = new SoundFieldsSettingsModel(sound);

            var state = this.settingModelBuilder.MapFieldSetting(settings.StateFieldsSettings.StateFieldSetting);
            var stolen = this.settingModelBuilder.MapFieldSetting(settings.StateFieldsSettings.StolenFieldSetting);
            var replaced = this.settingModelBuilder.MapFieldSetting(settings.StateFieldsSettings.ReplacedWithFieldSetting);
            var sendBack = this.settingModelBuilder.MapFieldSetting(settings.StateFieldsSettings.SendBackFieldSetting);
            var scrapDate = this.settingModelBuilder.MapFieldSetting(settings.StateFieldsSettings.ScrapDateFieldSetting);
            var stateFieldsSettingsModel = new StateFieldsSettingsModel(state, stolen, replaced, sendBack, scrapDate);

            var chassis = this.settingModelBuilder.MapFieldSetting(settings.ChassisFieldsSettings.ChassisFieldSetting);
            var chassisFieldsSettingsModel = new ChassisFieldsSettingsModel(chassis);

            var barCode = this.settingModelBuilder.MapFieldSetting(settings.InventoryFieldsSettings.BarCodeFieldSetting);
            var purchaseDate = this.settingModelBuilder.MapFieldSetting(settings.InventoryFieldsSettings.PurchaseDateFieldSetting);
            var inventoryFieldsSettingsModel = new InventoryFieldsSettingsModel(barCode, purchaseDate);

            var memory = this.settingModelBuilder.MapFieldSetting(settings.MemoryFieldsSettings.RAMFieldSetting);
            var memoryFieldsSettingsModel = new MemoryFieldsSettingsModel(memory);

            var os = this.settingModelBuilder.MapFieldSetting(settings.OperatingSystemFieldsSettings.OperatingSystemFieldSetting);
            var version = this.settingModelBuilder.MapFieldSetting(settings.OperatingSystemFieldsSettings.VersionFieldSetting);
            var servicePack = this.settingModelBuilder.MapFieldSetting(settings.OperatingSystemFieldsSettings.ServicePackSystemFieldSetting);
            var code = this.settingModelBuilder.MapFieldSetting(settings.OperatingSystemFieldsSettings.RegistrationCodeSystemFieldSetting);
            var key = this.settingModelBuilder.MapFieldSetting(settings.OperatingSystemFieldsSettings.ProductKeyFieldSetting);
            var operatingSystemFieldsSettingsModel = new OperatingSystemFieldsSettingsModel(
                os,
                version,
                servicePack,
                code,
                key);

            var department = this.settingModelBuilder.MapFieldSetting(settings.OrganizationFieldsSettings.DepartmentFieldSetting);
            var domain = this.settingModelBuilder.MapFieldSetting(settings.OrganizationFieldsSettings.DomainFieldSetting);
            var unit = this.settingModelBuilder.MapFieldSetting(settings.OrganizationFieldsSettings.UnitFieldSetting);
            var organizationFieldsSettingsModel = new OrganizationFieldsSettingsModel(department, domain, unit);

            var proccesor = this.settingModelBuilder.MapFieldSetting(settings.ProccesorFieldsSettings.ProccesorFieldSetting);
            var proccesorFieldsSettingsModel = new ProccesorFieldsSettingsModel(proccesor);

            var computerName = this.settingModelBuilder.MapFieldSetting(settings.WorkstationFieldsSettings.ComputerNameFieldSetting);
            var manufacturer = this.settingModelBuilder.MapFieldSetting(settings.WorkstationFieldsSettings.ManufacturerFieldSetting);
            var computerModel = this.settingModelBuilder.MapFieldSetting(settings.WorkstationFieldsSettings.ComputerModelFieldSetting);
            var serialNumber = this.settingModelBuilder.MapFieldSetting(settings.WorkstationFieldsSettings.SerialNumberFieldSetting);
            var biosVersion = this.settingModelBuilder.MapFieldSetting(settings.WorkstationFieldsSettings.BIOSVersionFieldSetting);
            var biosDate = this.settingModelBuilder.MapFieldSetting(settings.WorkstationFieldsSettings.BIOSDateFieldSetting);
            var theftMark = this.settingModelBuilder.MapFieldSetting(settings.WorkstationFieldsSettings.TheftmarkFieldSetting);
            var carePack = this.settingModelBuilder.MapFieldSetting(settings.WorkstationFieldsSettings.CarePackNumberFieldSetting);
            var computerType = this.settingModelBuilder.MapFieldSetting(settings.WorkstationFieldsSettings.ComputerTypeFieldSetting);
            var location = this.settingModelBuilder.MapFieldSetting(settings.WorkstationFieldsSettings.LocationFieldSetting);
            var workstationFieldsSettingsModel = new WorkstationFieldsSettingsModel(
                computerName,
                manufacturer,
                computerModel,
                serialNumber,
                biosVersion,
                biosDate,
                theftMark,
                carePack,
                computerType,
                location);

            var viewModel = new ComputerFieldsSettingsViewModel(
                dateFieldsSettingsModel,
                communicationFieldsSettingsModel,
                contactFieldsSettingsModel,
                contactInformationFieldsSettingsModel,
                contractFieldsSettingsModel,
                graphicsFieldsSettingsModel,
                otherFieldsSettingsModel,
                placeFieldsSettingsModel,
                soundFieldsSettingsModel,
                stateFieldsSettingsModel,
                chassisFieldsSettingsModel,
                inventoryFieldsSettingsModel,
                memoryFieldsSettingsModel,
                operatingSystemFieldsSettingsModel,
                organizationFieldsSettingsModel,
                proccesorFieldsSettingsModel,
                workstationFieldsSettingsModel);

            return viewModel;
        }
    }
}