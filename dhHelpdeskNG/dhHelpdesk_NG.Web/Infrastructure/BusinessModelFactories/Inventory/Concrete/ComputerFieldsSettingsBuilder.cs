using DH.Helpdesk.BusinessData.Models;
using DH.Helpdesk.Dal.Enums.Inventory.Computer;

namespace DH.Helpdesk.Web.Infrastructure.BusinessModelFactories.Inventory.Concrete
{
    using System;

    using DH.Helpdesk.BusinessData.Models.Inventory.Edit.Settings.ComputerSettings;
    using DH.Helpdesk.Web.Areas.Inventory.Models.EditModel.Settings.Computer;

    using CommunicationFieldsSettings = DH.Helpdesk.BusinessData.Models.Inventory.Edit.Settings.ComputerSettings.CommunicationFieldsSettings;
    using FieldSetting = DH.Helpdesk.BusinessData.Models.Inventory.Edit.Settings.ComputerSettings.FieldSetting;
    using FieldSettingModel = DH.Helpdesk.Web.Areas.Inventory.Models.EditModel.Settings.Computer.FieldSettingModel;
    using PlaceFieldsSettings = DH.Helpdesk.BusinessData.Models.Inventory.Edit.Settings.ComputerSettings.PlaceFieldsSettings;

    public class ComputerFieldsSettingsBuilder : IComputerFieldsSettingsBuilder
    {
        public ComputerFieldsSettings BuildViewModel(
               ComputerFieldsSettingsViewModel settings,
               int customerId)
        {
            var createdDate = MapFieldSetting(settings.DateFieldsSettingsModel.CreatedDateFieldSettingModel);
            var changedDate = MapFieldSetting(settings.DateFieldsSettingsModel.ChangedDateFieldSettingModel);
            var syncDate = MapFieldSetting(settings.DateFieldsSettingsModel.SyncChangedDateSettingModel);
            var scanDate = MapFieldSetting(settings.DateFieldsSettingsModel.ScanDateFieldSettingModel);
            var pathDate = MapFieldSetting(settings.DateFieldsSettingsModel.PathDirectoryFieldSettingModel);
            var dateFieldsSettings = new DateFieldsSettings(createdDate, changedDate, syncDate, scanDate, pathDate);

            var network = MapFieldSetting(settings.CommunicationFieldsSettingsModel.NetworkAdapterFieldSettingModel);
            var ipaddress = MapFieldSetting(settings.CommunicationFieldsSettingsModel.IPAddressFieldSettingModel);
            var macAddress = MapFieldSetting(settings.CommunicationFieldsSettingsModel.MacAddressFieldSettingModel);
            var ras = MapFieldSetting(settings.CommunicationFieldsSettingsModel.RASFieldSettingModel);
            var novell = MapFieldSetting(settings.CommunicationFieldsSettingsModel.NovellClientFieldSettingModel);
            var communicationFieldsSettings = new CommunicationFieldsSettings(
                network,
                ipaddress,
                macAddress,
                ras,
                novell);

            var name = MapFieldSetting(settings.ContactFieldsSettingsModel.NameFieldSettingModel);
            var phone = MapFieldSetting(settings.ContactFieldsSettingsModel.PhoneFieldSettingModel);
            var email = MapFieldSetting(settings.ContactFieldsSettingsModel.EmailFieldSettingModel);
            var contactFieldsSettings = new ContactFieldsSettings(name, phone, email);

            var user = MapFieldSetting(settings.ContactInformationFieldsSettingsModel.UserIdFieldSettingModel);
            var contactInformationFieldsSettings = new ContactInformationFieldsSettings(user);

            var status = MapFieldSetting(settings.ContractFieldsSettingsModel.ContractStatusFieldSettingModel);
            var number = MapFieldSetting(settings.ContractFieldsSettingsModel.ContractNumberFieldSettingModel);
            var startDate = MapFieldSetting(settings.ContractFieldsSettingsModel.ContractStartDateFieldSettingModel);
            var endDate = MapFieldSetting(settings.ContractFieldsSettingsModel.ContractEndDateFieldSettingModel);
            var price = MapFieldSetting(settings.ContractFieldsSettingsModel.PurchasePriceFieldSettingModel);
            var accounting1 = MapFieldSetting(
                settings.ContractFieldsSettingsModel.AccountingDimension1FieldSettingModel);
            var accounting2 = MapFieldSetting(
                settings.ContractFieldsSettingsModel.AccountingDimension2FieldSettingModel);
            var accounting3 = MapFieldSetting(
                settings.ContractFieldsSettingsModel.AccountingDimension3FieldSettingModel);
            var accounting4 = MapFieldSetting(
                settings.ContractFieldsSettingsModel.AccountingDimension4FieldSettingModel);
            var accounting5 = MapFieldSetting(
                settings.ContractFieldsSettingsModel.AccountingDimension5FieldSettingModel);
            var document = MapFieldSetting(
                settings.ContractFieldsSettingsModel.DocumentsFieldSettingModel);
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
                accounting5,
                document);

            var video = MapFieldSetting(settings.GraphicsFieldsSettingsModel.VideoCardFieldSettingModel);
            var graphicsFieldsSettings = new GraphicsFieldsSettings(video);

            var info = MapFieldSetting(settings.OtherFieldsSettingsModel.InfoFieldSettingModel);
            var otherFieldsSettings = new OtherFieldsSettings(info);

            var room = MapFieldSetting(settings.PlaceFieldsSettingsModel.RoomFieldSettingModel);
            var building = MapFieldSetting(settings.PlaceFieldsSettingsModel.BuildingFieldSettingModel);
            var floor = MapFieldSetting(settings.PlaceFieldsSettingsModel.FloorFieldSettingModel);
            var address = MapFieldSetting(settings.PlaceFieldsSettingsModel.AddressFieldSettingModel);
            var postalCode = MapFieldSetting(settings.PlaceFieldsSettingsModel.PostalCodeFieldSettingModel);
            var postalAddress = MapFieldSetting(settings.PlaceFieldsSettingsModel.PostalAddressFieldSettingModel);
            var place = MapFieldSetting(settings.PlaceFieldsSettingsModel.PlaceFieldSettingModel);
            var place2 = MapFieldSetting(settings.PlaceFieldsSettingsModel.Place2FieldSettingModel);
            var placeFieldsSettings = new PlaceFieldsSettings(room, building, floor,
                address, postalCode, postalAddress,
                place, place2);

            var sound = MapFieldSetting(settings.SoundFieldsSettingsModel.SoundCardFieldSettingModel);
            var soundFieldsSettings = new SoundFieldsSettings(sound);

            var state = MapFieldSetting(settings.StateFieldsSettingsModel.StateFieldSettingModel);
            var stolen = MapFieldSetting(settings.StateFieldsSettingsModel.StolenFieldSettingModel);
            var replaced = MapFieldSetting(settings.StateFieldsSettingsModel.ReplacedWithFieldSettingModel);
            var sendBack = MapFieldSetting(settings.StateFieldsSettingsModel.SendBackFieldSettingModel);
            var scrapDate = MapFieldSetting(settings.StateFieldsSettingsModel.ScrapDateFieldSettingModel);
            var stateFieldsSettings = new StateFieldsSettings(state, stolen, replaced, sendBack, scrapDate);

            var chassis = MapFieldSetting(settings.ChassisFieldsSettingsModel.ChassisFieldSettingModel);
            var chassisFieldsSettings = new ChassisFieldsSettings(chassis);

            var barCode = MapFieldSetting(settings.InventoryFieldsSettingsModel.BarCodeFieldSettingModel);
            var purchaseDate = MapFieldSetting(settings.InventoryFieldsSettingsModel.PurchaseDateFieldSettingModel);
            var inventoryFieldsSettings = new InventoryFieldsSettings(barCode, purchaseDate);

            var memory = MapFieldSetting(settings.MemoryFieldsSettingsModel.RAMFieldSettingModel);
            var memoryFieldsSettings = new MemoryFieldsSettings(memory);

            var os = MapFieldSetting(settings.OperatingSystemFieldsSettingsModel.OperatingSystemFieldSettingModel);
            var version = MapFieldSetting(settings.OperatingSystemFieldsSettingsModel.VersionFieldSettingModel);
            var servicePack =
                MapFieldSetting(settings.OperatingSystemFieldsSettingsModel.ServicePackSystemFieldSettingModel);
            var code =
                MapFieldSetting(settings.OperatingSystemFieldsSettingsModel.RegistrationCodeSystemFieldSettingModel);
            var key = MapFieldSetting(settings.OperatingSystemFieldsSettingsModel.ProductKeyFieldSettingModel);
            var operatingSystemFieldsSettings = new OperatingSystemFieldsSettings(os, version, servicePack, code, key);

            var region = MapFieldSetting(settings.OrganizationFieldsSettingsModel.RegionFieldSettingModel);
            var department = MapFieldSetting(settings.OrganizationFieldsSettingsModel.DepartmentFieldSettingModel);
            var domain = MapFieldSetting(settings.OrganizationFieldsSettingsModel.DomainFieldSettingModel);
            var unit = MapFieldSetting(settings.OrganizationFieldsSettingsModel.UnitFieldSettingModel);
            var organizationFieldsSettings = new OrganizationFieldsSettings(region, department, domain, unit);

            var proccesor = MapFieldSetting(settings.ProccesorFieldsSettingsModel.ProccesorFieldSettingModel);
            var proccesorFieldsSettings = new ProcessorFieldsSettings(proccesor);

            var computerName = MapFieldSetting(settings.WorkstationFieldsSettingsModel.NameFieldSettingModel);
            var manufacturer = MapFieldSetting(settings.WorkstationFieldsSettingsModel.ManufacturerFieldSettingModel);
            var computerModel = MapFieldSetting(settings.WorkstationFieldsSettingsModel.ModelFieldSettingModel);
            var serialNumber = MapFieldSetting(settings.WorkstationFieldsSettingsModel.SerialNumberFieldSettingModel);
            var biosVersion = MapFieldSetting(settings.WorkstationFieldsSettingsModel.BIOSVersionFieldSettingModel);
            var biosDate = MapFieldSetting(settings.WorkstationFieldsSettingsModel.BIOSDateFieldSettingModel);
            var theftMark = MapFieldSetting(settings.WorkstationFieldsSettingsModel.TheftmarkFieldSettingModel);
            var carePack = MapFieldSetting(settings.WorkstationFieldsSettingsModel.CarePackNumberFieldSettingModel);
            var computerType = MapFieldSetting(settings.WorkstationFieldsSettingsModel.ComputerTypeFieldSettingModel);
            var location = MapFieldSetting(settings.WorkstationFieldsSettingsModel.LocationFieldSettingModel);
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
                settings.LanguageId,
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

        public WorkstationTabsSettings BuildTabsViewModel(
            WorkstationTabsSettingsModel settings,
            int customerId)
        {
            return new WorkstationTabsSettings(
                ModelStates.Updated,
                //settings.TabLanguageId,
                MapTabSetting(WorkstationTabs.Workstations, settings.ComputersTabSettingModel),
                MapTabSetting(WorkstationTabs.Storages, settings.StorageTabSettingModel),
                MapTabSetting(WorkstationTabs.Softwares, settings.SoftwareTabSettingModel),
                MapTabSetting(WorkstationTabs.HotFixes, settings.HotFixesTabSettingModel),
                MapTabSetting(WorkstationTabs.ComputerLogs, settings.ComputerLogsTabSettingModel),
                MapTabSetting(WorkstationTabs.Accessories, settings.AccessoriesTabSettingModel),
                MapTabSetting(WorkstationTabs.RelatedCases, settings.RelatedCasesTabSettingModel)
            );
        }

        private static FieldSetting MapFieldSetting(FieldSettingModel setting)
        {
            var settingModel = new FieldSetting(
                setting.ShowInDetails,
                setting.ShowInList,
                setting.Caption,
                setting.IsRequired,
                setting.IsReadOnly,
                setting.IsCopy);

            return settingModel;
        }

        private static TabSetting MapTabSetting(string tabField, TabSettingModel setting)
        {
            var settingModel = new TabSetting(
                tabField,
                setting.Show,
                setting.Caption);

            return settingModel;
        }
    }
}