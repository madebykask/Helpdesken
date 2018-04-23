namespace DH.Helpdesk.Dal.Mappers.Inventory.EntityToBusinessModel.Printer
{
    using DH.Helpdesk.BusinessData.Models.Inventory.Edit.Settings;
    using DH.Helpdesk.BusinessData.Models.Inventory.Edit.Settings.PrinterSettings;
    using DH.Helpdesk.BusinessData.Models.Inventory.Edit.Settings.SharedSettings;
    using DH.Helpdesk.Common.Collections;
    using DH.Helpdesk.Common.Extensions.Integer;
    using DH.Helpdesk.Dal.Enums.Inventory.Printer;
    using DH.Helpdesk.Dal.Enums.Inventory.Shared;
    using DH.Helpdesk.Dal.MapperData.Inventory;

    using CommunicationFields = DH.Helpdesk.Dal.Enums.Inventory.Printer.CommunicationFields;
    using CommunicationFieldsSettings = DH.Helpdesk.BusinessData.Models.Inventory.Edit.Settings.SharedSettings.CommunicationFieldsSettings;
    using OrganizationFields = DH.Helpdesk.Dal.Enums.Inventory.Printer.OrganizationFields;
    using OrganizationFieldsSettings = DH.Helpdesk.BusinessData.Models.Inventory.Edit.Settings.PrinterSettings.OrganizationFieldsSettings;
    using OtherFields = DH.Helpdesk.Dal.Enums.Inventory.Printer.OtherFields;
    using OtherFieldsSettings = DH.Helpdesk.BusinessData.Models.Inventory.Edit.Settings.PrinterSettings.OtherFieldsSettings;
    using PlaceFields = DH.Helpdesk.Dal.Enums.Inventory.Shared.PlaceFields;
    using PlaceFieldsSettings = DH.Helpdesk.BusinessData.Models.Inventory.Edit.Settings.SharedSettings.PlaceFieldsSettings;
    using StateFields = DH.Helpdesk.Dal.Enums.Inventory.Printer.StateFields;
    using StateFieldsSettings = DH.Helpdesk.BusinessData.Models.Inventory.Edit.Settings.PrinterSettings.StateFieldsSettings;

    public sealed class PrinterFieldSettingsToFieldSettingsMapper :
        IEntityToBusinessModelMapper<NamedObjectCollection<FieldSettingMapperData>, PrinterFieldsSettings>
    {
        public PrinterFieldsSettings Map(NamedObjectCollection<FieldSettingMapperData> entity)
        {
            var general = CreateWorkstationSettings(entity);
            var inventory = CreateInventeringSettings(entity);
            var communication = CreateCommunicationSettings(entity);
            var other = CreateOtherSettings(entity);
            var organization = CreateOrganizationSettings(entity);
            var place = CreatePlaceSettings(entity);
            var state = CreateStateSettings(entity);

            var settings = PrinterFieldsSettings.CreateForEdit(
                general,
                inventory,
                communication,
                other,
                organization,
                place,
                state);

            return settings;
        }

        private static GeneralFieldsSettings CreateWorkstationSettings(NamedObjectCollection<FieldSettingMapperData> entity)
        {
            var name = CreateFieldSetting(entity.FindByName(GeneralFields.Name));
            var manufacturer = CreateFieldSetting(entity.FindByName(GeneralFields.Manufacturer));
            var model = CreateFieldSetting(entity.FindByName(GeneralFields.Model));
            var serialNumber = CreateFieldSetting(entity.FindByName(GeneralFields.SerialNumber));

            var settings = new GeneralFieldsSettings(name, manufacturer, model, serialNumber);

            return settings;
        }

        private static InventoryFieldsSettings CreateInventeringSettings(NamedObjectCollection<FieldSettingMapperData> entity)
        {
            var barCode = CreateFieldSetting(entity.FindByName(InventoryFields.BarCode));
            var purchaseDate = CreateFieldSetting(entity.FindByName(InventoryFields.PurchaseDate));

            var settings = new InventoryFieldsSettings(barCode, purchaseDate);

            return settings;
        }

        private static CommunicationFieldsSettings CreateCommunicationSettings(NamedObjectCollection<FieldSettingMapperData> entity)
        {
            var networkAdapter = CreateFieldSetting(entity.FindByName(CommunicationFields.NetworkAdapter));
            var ipAddress = CreateFieldSetting(entity.FindByName(CommunicationFields.IPAddress));
            var macAddress = CreateFieldSetting(entity.FindByName(CommunicationFields.MacAddress));

            var settings = new CommunicationFieldsSettings(
                networkAdapter,
                ipAddress,
                macAddress);

            return settings;
        }

        private static OtherFieldsSettings CreateOtherSettings(NamedObjectCollection<FieldSettingMapperData> entity)
        {
            var numberOfTrays = CreateFieldSetting(entity.FindByName(OtherFields.NumberOfTrays));
            var driver = CreateFieldSetting(entity.FindByName(OtherFields.Driver));
            var info = CreateFieldSetting(entity.FindByName(OtherFields.Info));
            var url = CreateFieldSetting(entity.FindByName(OtherFields.URL));

            var settings = new OtherFieldsSettings(numberOfTrays, driver, info, url);

            return settings;
        }

        private static OrganizationFieldsSettings CreateOrganizationSettings(NamedObjectCollection<FieldSettingMapperData> entity)
        {
            var department = CreateFieldSetting(entity.FindByName(OrganizationFields.Department));
            var unit = CreateFieldSetting(entity.FindByName(OrganizationFields.Unit));

            var settings = new OrganizationFieldsSettings(department, unit);

            return settings;
        }

        private static PlaceFieldsSettings CreatePlaceSettings(NamedObjectCollection<FieldSettingMapperData> entity)
        {
            var room = CreateFieldSetting(entity.FindByName(PlaceFields.Room));
            var location = CreateFieldSetting(entity.FindByName(PlaceFields.Location));

            var settings = new PlaceFieldsSettings(room, location);

            return settings;
        }

        private static StateFieldsSettings CreateStateSettings(NamedObjectCollection<FieldSettingMapperData> entity)
        {
            var createdDate = CreateFieldSetting(entity.FindByName(StateFields.CreatedDate));
            var changedDate = CreateFieldSetting(entity.FindByName(StateFields.ChangedDate));
            var syncDate = CreateFieldSetting(entity.FindByName(StateFields.SyncDate));

            var settings = new StateFieldsSettings(createdDate, changedDate, syncDate);
            return settings;
        }

        private static FieldSetting CreateFieldSetting(FieldSettingMapperData fieldSetting)
        {
            return new FieldSetting(
                fieldSetting.ShowInDetails.ToBool(),
                fieldSetting.ShowInList.ToBool(),
                fieldSetting.Caption,
                fieldSetting.Required.ToBool());
        }
    }
}
