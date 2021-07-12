namespace DH.Helpdesk.Web.Infrastructure.ModelFactories.Inventory.Concrete
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;

    using DH.Helpdesk.BusinessData.Models.Inventory.Edit.Settings.ComputerSettings;
    using DH.Helpdesk.BusinessData.Models.Shared;
    using DH.Helpdesk.Web.Areas.Inventory.Models.EditModel.Settings.Computer;

    using FieldSettingModel = DH.Helpdesk.Web.Areas.Inventory.Models.EditModel.Settings.Computer.FieldSettingModel;
    using PlaceFieldsSettingsModel = DH.Helpdesk.Web.Areas.Inventory.Models.EditModel.Settings.Computer.PlaceFieldsSettingsModel;

    public class ComputerFieldsSettingsViewModelBuilder : IComputerFieldsSettingsViewModelBuilder
    {
        public ComputerFieldsSettingsViewModel BuildViewModel(
            ComputerFieldsSettings settings,
            WorkstationTabsSettings tabsSettings,
            List<ItemOverview> langauges,
            int langaugeId)
            //int tabLanguageId)
        {
            var createdDate = MapFieldSetting(settings.DateFieldsSettings.CreatedDateFieldSetting);
            var changedDate = MapFieldSetting(settings.DateFieldsSettings.ChangedDateFieldSetting);
            var syncDate = MapFieldSetting(settings.DateFieldsSettings.SyncChangedDateSetting);
            var scanDate = MapFieldSetting(settings.DateFieldsSettings.ScanDateFieldSetting);
            var pathDate = MapFieldSetting(settings.DateFieldsSettings.PathDirectoryFieldSetting);
            var dateFieldsSettingsModel = new DateFieldsSettingsModel(
                createdDate,
                changedDate,
                syncDate,
                scanDate,
                pathDate);

            var network = MapFieldSetting(settings.CommunicationFieldsSettings.NetworkAdapterFieldSetting);
            var ipaddress = MapFieldSetting(settings.CommunicationFieldsSettings.IPAddressFieldSetting);
            var macAddress = MapFieldSetting(settings.CommunicationFieldsSettings.MacAddressFieldSetting);
            var ras = MapFieldSetting(settings.CommunicationFieldsSettings.RASFieldSetting);
            var novell = MapFieldSetting(settings.CommunicationFieldsSettings.NovellClientFieldSetting);
            var communicationFieldsSettingsModel = new CommunicationFieldsSettingsModel(
                network,
                ipaddress,
                macAddress,
                ras,
                novell);

            var name = MapFieldSetting(settings.ContactFieldsSettings.NameFieldSetting);
            var phone = MapFieldSetting(settings.ContactFieldsSettings.PhoneFieldSetting);
            var email = MapFieldSetting(settings.ContactFieldsSettings.EmailFieldSetting);
            var contactFieldsSettingsModel = new ContactFieldsSettingsModel(name, phone, email);

            var user = MapFieldSetting(settings.ContactInformationFieldsSettings.UserIdFieldSetting);
            var contactInformationFieldsSettingsModel = new ContactInformationFieldsSettingsModel(user);

            var status = MapFieldSetting(settings.ContractFieldsSettings.ContractStatusFieldSetting);
            var number = MapFieldSetting(settings.ContractFieldsSettings.ContractNumberFieldSetting);
            var startDate = MapFieldSetting(settings.ContractFieldsSettings.ContractStartDateFieldSetting);
            var endDate = MapFieldSetting(settings.ContractFieldsSettings.ContractEndDateFieldSetting);
            var price = MapFieldSetting(settings.ContractFieldsSettings.PurchasePriceFieldSetting);
            var accounting1 = MapFieldSetting(settings.ContractFieldsSettings.AccountingDimension1FieldSetting);
            var accounting2 = MapFieldSetting(settings.ContractFieldsSettings.AccountingDimension2FieldSetting);
            var accounting3 = MapFieldSetting(settings.ContractFieldsSettings.AccountingDimension3FieldSetting);
            var accounting4 = MapFieldSetting(settings.ContractFieldsSettings.AccountingDimension4FieldSetting);
            var accounting5 = MapFieldSetting(settings.ContractFieldsSettings.AccountingDimension5FieldSetting);
            var document = MapFieldSetting(settings.ContractFieldsSettings.DocumentFieldSetting);
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
                document);

            var video = MapFieldSetting(settings.GraphicsFieldsSettings.VideoCardFieldSetting);
            var graphicsFieldsSettingsModel = new GraphicsFieldsSettingsModel(video);

            var info = MapFieldSetting(settings.OtherFieldsSettings.InfoFieldSetting);
            var otherFieldsSettingsModel = new OtherFieldsSettingsModel(info);

            var room = MapFieldSetting(settings.PlaceFieldsSettings.RoomFieldSetting);
            var address = MapFieldSetting(settings.PlaceFieldsSettings.AddressFieldSetting);
            var postalCode = MapFieldSetting(settings.PlaceFieldsSettings.PostalCodeFieldSetting);
            var postalAddress = MapFieldSetting(settings.PlaceFieldsSettings.PostalAddressFieldSetting);
            var place = MapFieldSetting(settings.PlaceFieldsSettings.PlaceFieldSetting);
            var place2 = MapFieldSetting(settings.PlaceFieldsSettings.Place2FieldSetting);
            var placeFieldsSettingsModel = new PlaceFieldsSettingsModel(
                room,
                address,
                postalCode,
                postalAddress,
                place,
                place2);

            var sound = MapFieldSetting(settings.SoundFieldsSettings.SoundCardFieldSetting);
            var soundFieldsSettingsModel = new SoundFieldsSettingsModel(sound);

            var state = MapFieldSetting(settings.StateFieldsSettings.StateFieldSetting);
            var stolen = MapFieldSetting(settings.StateFieldsSettings.StolenFieldSetting);
            var replaced = MapFieldSetting(settings.StateFieldsSettings.ReplacedWithFieldSetting);
            var sendBack = MapFieldSetting(settings.StateFieldsSettings.SendBackFieldSetting);
            var scrapDate = MapFieldSetting(settings.StateFieldsSettings.ScrapDateFieldSetting);
            var stateFieldsSettingsModel = new StateFieldsSettingsModel(state, stolen, replaced, sendBack, scrapDate);

            var chassis = MapFieldSetting(settings.ChassisFieldsSettings.ChassisFieldSetting);
            var chassisFieldsSettingsModel = new ChassisFieldsSettingsModel(chassis);

            var barCode = MapFieldSetting(settings.InventoryFieldsSettings.BarCodeFieldSetting);
            var purchaseDate = MapFieldSetting(settings.InventoryFieldsSettings.PurchaseDateFieldSetting);
            var inventoryFieldsSettingsModel = new InventoryFieldsSettingsModel(barCode, purchaseDate);

            var memory = MapFieldSetting(settings.MemoryFieldsSettings.RAMFieldSetting);
            var memoryFieldsSettingsModel = new MemoryFieldsSettingsModel(memory);

            var os = MapFieldSetting(settings.OperatingSystemFieldsSettings.OperatingSystemFieldSetting);
            var version = MapFieldSetting(settings.OperatingSystemFieldsSettings.VersionFieldSetting);
            var servicePack = MapFieldSetting(settings.OperatingSystemFieldsSettings.ServicePackSystemFieldSetting);
            var code = MapFieldSetting(settings.OperatingSystemFieldsSettings.RegistrationCodeSystemFieldSetting);
            var key = MapFieldSetting(settings.OperatingSystemFieldsSettings.ProductKeyFieldSetting);
            var operatingSystemFieldsSettingsModel = new OperatingSystemFieldsSettingsModel(
                os,
                version,
                servicePack,
                code,
                key);

            var region = MapFieldSetting(settings.OrganizationFieldsSettings.RegionFieldSetting);
            var department = MapFieldSetting(settings.OrganizationFieldsSettings.DepartmentFieldSetting);
            var domain = MapFieldSetting(settings.OrganizationFieldsSettings.DomainFieldSetting);
            var unit = MapFieldSetting(settings.OrganizationFieldsSettings.UnitFieldSetting);
            var organizationFieldsSettingsModel = new OrganizationFieldsSettingsModel(region, department, domain, unit);

            var proccesor = MapFieldSetting(settings.ProccesorFieldsSettings.ProccesorFieldSetting);
            var proccesorFieldsSettingsModel = new ProccesorFieldsSettingsModel(proccesor);

            var computerName = MapFieldSetting(settings.WorkstationFieldsSettings.ComputerNameFieldSetting);
            var manufacturer = MapFieldSetting(settings.WorkstationFieldsSettings.ManufacturerFieldSetting);
            var computerModel = MapFieldSetting(settings.WorkstationFieldsSettings.ComputerModelFieldSetting);
            var serialNumber = MapFieldSetting(settings.WorkstationFieldsSettings.SerialNumberFieldSetting);
            var biosVersion = MapFieldSetting(settings.WorkstationFieldsSettings.BIOSVersionFieldSetting);
            var biosDate = MapFieldSetting(settings.WorkstationFieldsSettings.BIOSDateFieldSetting);
            var theftMark = MapFieldSetting(settings.WorkstationFieldsSettings.TheftmarkFieldSetting);
            var carePack = MapFieldSetting(settings.WorkstationFieldsSettings.CarePackNumberFieldSetting);
            var computerType = MapFieldSetting(settings.WorkstationFieldsSettings.ComputerTypeFieldSetting);
            var location = MapFieldSetting(settings.WorkstationFieldsSettings.LocationFieldSetting);
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

            var workstationTabsSettingsModel = new WorkstationTabsSettingsModel(
                //tabLanguageId,
                MapTabSetting(tabsSettings.ComputersTabSetting),
                MapTabSetting(tabsSettings.StorageTabSetting),
                MapTabSetting(tabsSettings.SoftwareTabSetting),
                MapTabSetting(tabsSettings.HotFixesTabSetting),
                MapTabSetting(tabsSettings.ComputerLogsTabSetting),
                MapTabSetting(tabsSettings.AccessoriesTabSetting),
                MapTabSetting(tabsSettings.RelatedCasesTabSetting)
            );

            var localizedLanguages =
                langauges.Select(l => new ItemOverview(Translation.Get(l.Name), l.Value)).ToList();

            var langaugesSelectList = new SelectList(localizedLanguages, "Value", "Name");

            var viewModel = new ComputerFieldsSettingsViewModel(
                langaugeId,
                langaugesSelectList,
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
                workstationFieldsSettingsModel, 
                workstationTabsSettingsModel);

            return viewModel;
        }

        private static FieldSettingModel MapFieldSetting(FieldSetting setting)
        {
            var settingModel = new FieldSettingModel(
                setting.ShowInDetails,
                setting.ShowInList,
                setting.Caption,
                setting.IsRequired,
                setting.IsReadOnly,
                setting.IsCopy);

            return settingModel;
        }

        private static TabSettingModel MapTabSetting(TabSetting setting)
        {
            var settingModel = new TabSettingModel(
                setting.Show,
                setting.Caption);

            return settingModel;
        }
    }
}