namespace DH.Helpdesk.Web.Infrastructure.BusinessModelFactories.Inventory.Concrete
{
    using System;

    using DH.Helpdesk.BusinessData.Models.Inventory.Edit.Settings.ComputerSettings;
    using DH.Helpdesk.BusinessData.Models.Inventory.Edit.Settings.SharedSettings;
    using DH.Helpdesk.Web.Models.Inventory.EditModel.Settings.Computer;

    using CommunicationFieldsSettings = DH.Helpdesk.BusinessData.Models.Inventory.Edit.Settings.ComputerSettings.CommunicationFieldsSettings;
    using PlaceFieldsSettings = DH.Helpdesk.BusinessData.Models.Inventory.Edit.Settings.ComputerSettings.PlaceFieldsSettings;

    public class ComputerFieldsSettingsBuilder : IComputerFieldsSettingsBuilder
    {
        private readonly IFieldSettingBuilder settingBuilder;

        public ComputerFieldsSettingsBuilder(IFieldSettingBuilder settingBuilder)
        {
            this.settingBuilder = settingBuilder;
        }

        public ComputerFieldsSettings BuildViewModel(
            ComputerFieldsSettingsViewModel settings,
            int customerId,
            int languageId)
        {
            var createdDate = this.settingBuilder.MapFieldSetting(settings.DateFieldsSettingsModel.CreatedDateFieldSettingModel);
            var changedDate = this.settingBuilder.MapFieldSetting(settings.DateFieldsSettingsModel.ChangedDateFieldSettingModel);
            var syncDate = this.settingBuilder.MapFieldSetting(settings.DateFieldsSettingsModel.SyncChangedDateSettingModel);
            var scanDate = this.settingBuilder.MapFieldSetting(settings.DateFieldsSettingsModel.ScanDateFieldSettingModel);
            var pathDate = this.settingBuilder.MapFieldSetting(settings.DateFieldsSettingsModel.PathDirectoryFieldSettingModel);
            var dateFieldsSettings = new DateFieldsSettings(createdDate, changedDate, syncDate, scanDate, pathDate);

            var network = this.settingBuilder.MapFieldSetting(settings.CommunicationFieldsSettingsModel.NetworkAdapterFieldSettingModel);
            var ipaddress = this.settingBuilder.MapFieldSetting(settings.CommunicationFieldsSettingsModel.IPAddressFieldSettingModel);
            var macAddress = this.settingBuilder.MapFieldSetting(settings.CommunicationFieldsSettingsModel.MacAddressFieldSettingModel);
            var ras = this.settingBuilder.MapFieldSetting(settings.CommunicationFieldsSettingsModel.RASFieldSettingModel);
            var novell = this.settingBuilder.MapFieldSetting(settings.CommunicationFieldsSettingsModel.NovellClientFieldSettingModel);
            var communicationFieldsSettings = new CommunicationFieldsSettings(
                network,
                ipaddress,
                macAddress,
                ras,
                novell);

            var name = this.settingBuilder.MapFieldSetting(settings.ContactFieldsSettingsModel.NameFieldSettingModel);
            var phone = this.settingBuilder.MapFieldSetting(settings.ContactFieldsSettingsModel.PhoneFieldSettingModel);
            var email = this.settingBuilder.MapFieldSetting(settings.ContactFieldsSettingsModel.EmailFieldSettingModel);
            var contactFieldsSettings = new ContactFieldsSettings(name, phone, email);

            var user = this.settingBuilder.MapFieldSetting(settings.ContactInformationFieldsSettingsModel.UserIdFieldSettingModel);
            var contactInformationFieldsSettings = new ContactInformationFieldsSettings(user);

            var status = this.settingBuilder.MapFieldSetting(settings.ContractFieldsSettingsModel.ContractStatusFieldSettingModel);
            var number = this.settingBuilder.MapFieldSetting(settings.ContractFieldsSettingsModel.ContractNumberFieldSettingModel);
            var startDate = this.settingBuilder.MapFieldSetting(settings.ContractFieldsSettingsModel.ContractStartDateFieldSettingModel);
            var endDate = this.settingBuilder.MapFieldSetting(settings.ContractFieldsSettingsModel.ContractEndDateFieldSettingModel);
            var price = this.settingBuilder.MapFieldSetting(settings.ContractFieldsSettingsModel.PurchasePriceFieldSettingModel);
            var accounting1 = this.settingBuilder.MapFieldSetting(
                settings.ContractFieldsSettingsModel.AccountingDimension1FieldSettingModel);
            var accounting2 = this.settingBuilder.MapFieldSetting(
                settings.ContractFieldsSettingsModel.AccountingDimension2FieldSettingModel);
            var accounting3 = this.settingBuilder.MapFieldSetting(
                settings.ContractFieldsSettingsModel.AccountingDimension3FieldSettingModel);
            var accounting4 = this.settingBuilder.MapFieldSetting(
                settings.ContractFieldsSettingsModel.AccountingDimension4FieldSettingModel);
            var accounting5 = this.settingBuilder.MapFieldSetting(
                settings.ContractFieldsSettingsModel.AccountingDimension5FieldSettingModel);
            var contractFieldsSettings = new ContractFieldsSettings(
                status,
                number,
                startDate,
                endDate,
                price,
                accounting1,
                accounting2,
                accounting3,
                accounting4,
                accounting5); // todo

            var video = this.settingBuilder.MapFieldSetting(settings.GraphicsFieldsSettingsModel.VideoCardFieldSettingModel);
            var graphicsFieldsSettings = new GraphicsFieldsSettings(video);

            var info = this.settingBuilder.MapFieldSetting(settings.OtherFieldsSettingsModel.InfoFieldSettingModel);
            var otherFieldsSettings = new OtherFieldsSettings(info);

            var room = this.settingBuilder.MapFieldSetting(settings.PlaceFieldsSettingsModel.RoomFieldSettingModel);
            var address = this.settingBuilder.MapFieldSetting(settings.PlaceFieldsSettingsModel.AddressFieldSettingModel);
            var postalCode = this.settingBuilder.MapFieldSetting(settings.PlaceFieldsSettingsModel.PostalCodeFieldSettingModel);
            var postalAddress = this.settingBuilder.MapFieldSetting(settings.PlaceFieldsSettingsModel.PostalAddressFieldSettingModel);
            var place = this.settingBuilder.MapFieldSetting(settings.PlaceFieldsSettingsModel.PlaceFieldSettingModel);
            var place2 = this.settingBuilder.MapFieldSetting(settings.PlaceFieldsSettingsModel.Place2FieldSettingModel);
            var placeFieldsSettings = new PlaceFieldsSettings(room, address, postalCode, postalAddress, place, place2);

            var sound = this.settingBuilder.MapFieldSetting(settings.SoundFieldsSettingsModel.SoundCardFieldSettingModel);
            var soundFieldsSettings = new SoundFieldsSettings(sound);

            var state = this.settingBuilder.MapFieldSetting(settings.StateFieldsSettingsModel.StateFieldSettingModel);
            var stolen = this.settingBuilder.MapFieldSetting(settings.StateFieldsSettingsModel.StolenFieldSettingModel);
            var replaced = this.settingBuilder.MapFieldSetting(settings.StateFieldsSettingsModel.ReplacedWithFieldSettingModel);
            var sendBack = this.settingBuilder.MapFieldSetting(settings.StateFieldsSettingsModel.SendBackFieldSettingModel);
            var scrapDate = this.settingBuilder.MapFieldSetting(settings.StateFieldsSettingsModel.ScrapDateFieldSettingModel);
            var stateFieldsSettings = new StateFieldsSettings(state, stolen, replaced, sendBack, scrapDate);

            var chassis = this.settingBuilder.MapFieldSetting(settings.ChassisFieldsSettingsModel.ChassisFieldSettingModel);
            var chassisFieldsSettings = new ChassisFieldsSettings(chassis);

            var barCode = this.settingBuilder.MapFieldSetting(settings.InventoryFieldsSettingsModel.BarCodeFieldSettingModel);
            var purchaseDate = this.settingBuilder.MapFieldSetting(settings.InventoryFieldsSettingsModel.PurchaseDateFieldSettingModel);
            var inventoryFieldsSettings = new InventoryFieldsSettings(barCode, purchaseDate);

            var memory = this.settingBuilder.MapFieldSetting(settings.MemoryFieldsSettingsModel.RAMFieldSettingModel);
            var memoryFieldsSettings = new MemoryFieldsSettings(memory);

            var os = this.settingBuilder.MapFieldSetting(settings.OperatingSystemFieldsSettingsModel.OperatingSystemFieldSettingModel);
            var version = this.settingBuilder.MapFieldSetting(settings.OperatingSystemFieldsSettingsModel.VersionFieldSettingModel);
            var servicePack =
                this.settingBuilder.MapFieldSetting(settings.OperatingSystemFieldsSettingsModel.ServicePackSystemFieldSettingModel);
            var code =
                this.settingBuilder.MapFieldSetting(settings.OperatingSystemFieldsSettingsModel.RegistrationCodeSystemFieldSettingModel);
            var key = this.settingBuilder.MapFieldSetting(settings.OperatingSystemFieldsSettingsModel.ProductKeyFieldSettingModel);
            var operatingSystemFieldsSettings = new OperatingSystemFieldsSettings(os, version, servicePack, code, key);

            var department = this.settingBuilder.MapFieldSetting(settings.OrganizationFieldsSettingsModel.DepartmentFieldSettingModel);
            var domain = this.settingBuilder.MapFieldSetting(settings.OrganizationFieldsSettingsModel.DomainFieldSettingModel);
            var unit = this.settingBuilder.MapFieldSetting(settings.OrganizationFieldsSettingsModel.UnitFieldSettingModel);
            var organizationFieldsSettings = new OrganizationFieldsSettings(department, domain, unit);

            var proccesor = this.settingBuilder.MapFieldSetting(settings.ProccesorFieldsSettingsModel.ProccesorFieldSettingModel);
            var proccesorFieldsSettings = new ProcessorFieldsSettings(proccesor);

            var computerName = this.settingBuilder.MapFieldSetting(settings.WorkstationFieldsSettingsModel.NameFieldSettingModel);
            var manufacturer = this.settingBuilder.MapFieldSetting(settings.WorkstationFieldsSettingsModel.ManufacturerFieldSettingModel);
            var computerModel = this.settingBuilder.MapFieldSetting(settings.WorkstationFieldsSettingsModel.ModelFieldSettingModel);
            var serialNumber = this.settingBuilder.MapFieldSetting(settings.WorkstationFieldsSettingsModel.SerialNumberFieldSettingModel);
            var biosVersion = this.settingBuilder.MapFieldSetting(settings.WorkstationFieldsSettingsModel.BIOSVersionFieldSettingModel);
            var biosDate = this.settingBuilder.MapFieldSetting(settings.WorkstationFieldsSettingsModel.BIOSDateFieldSettingModel);
            var theftMark = this.settingBuilder.MapFieldSetting(settings.WorkstationFieldsSettingsModel.TheftmarkFieldSettingModel);
            var carePack = this.settingBuilder.MapFieldSetting(settings.WorkstationFieldsSettingsModel.CarePackNumberFieldSettingModel);
            var computerType = this.settingBuilder.MapFieldSetting(settings.WorkstationFieldsSettingsModel.ComputerTypeFieldSettingModel);
            var location = this.settingBuilder.MapFieldSetting(settings.WorkstationFieldsSettingsModel.LocationFieldSettingModel);
            var workstationFieldsSettings = new WorkstationFieldsSettings(
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

            var businessModel = ComputerFieldsSettings.CreateUpdated(
                customerId,
                languageId,
                DateTime.Now,
                dateFieldsSettings,
                communicationFieldsSettings,
                contactFieldsSettings,
                contactInformationFieldsSettings,
                contractFieldsSettings,
                graphicsFieldsSettings,
                otherFieldsSettings,
                placeFieldsSettings,
                soundFieldsSettings,
                stateFieldsSettings,
                chassisFieldsSettings,
                inventoryFieldsSettings,
                memoryFieldsSettings,
                operatingSystemFieldsSettings,
                organizationFieldsSettings,
                proccesorFieldsSettings,
                workstationFieldsSettings);

            return businessModel;
        }
   }
}