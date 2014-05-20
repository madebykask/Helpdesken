namespace DH.Helpdesk.Web.Infrastructure.ModelFactories.Inventory.Concrete
{
    using DH.Helpdesk.BusinessData.Models.Inventory.Edit.Printer;
    using DH.Helpdesk.BusinessData.Models.Inventory.Output.Settings.ModelEdit.PrinterSettings;
    using DH.Helpdesk.Services.Response.Inventory;
    using DH.Helpdesk.Web.Models.Inventory.EditModel.Printer;
    using DH.Helpdesk.Web.Models.Inventory.EditModel.Shared;

    public class PrinterViewModelBuilder : IPrinterViewModelBuilder
    {
        private readonly IConfigurableFieldModelBuilder configurableFieldModelBuilder;

        public PrinterViewModelBuilder(IConfigurableFieldModelBuilder configurableFieldModelBuilder)
        {
            this.configurableFieldModelBuilder = configurableFieldModelBuilder;
        }

        public PrinterViewModel BuildViewModel(
            Printer model,
            PrinterEditOptionsResponse options,
            PrinterFieldsSettingsForModelEdit settings)
        {
            var createdDate =
                this.configurableFieldModelBuilder.CreateDateTimeField(
                    settings.StateFieldsSettings.CreatedDateFieldSetting,
                    model.CreatedDate);
            var changedDate =
                this.configurableFieldModelBuilder.CreateDateTimeField(
                    settings.StateFieldsSettings.ChangedDateFieldSetting,
                    model.ChangedDate);

            var name =
                this.configurableFieldModelBuilder.CreateStringField(
                    settings.GeneralFieldsSettings.NameFieldSetting,
                    model.GeneralFields.Name);
            var manufacturer =
                this.configurableFieldModelBuilder.CreateStringField(
                    settings.GeneralFieldsSettings.ManufacturerFieldSetting,
                    model.GeneralFields.Manufacturer);
            var description =
                this.configurableFieldModelBuilder.CreateStringField(
                    settings.GeneralFieldsSettings.ModelFieldSetting,
                    model.GeneralFields.Model);
            var serial =
                this.configurableFieldModelBuilder.CreateStringField(
                    settings.GeneralFieldsSettings.SerialNumberFieldSetting,
                    model.GeneralFields.SerialNumber);

            var generalFieldsModel = new GeneralFieldsModel(
                name,
                manufacturer,
                description,
                serial);

            var barCode =
                this.configurableFieldModelBuilder.CreateStringField(
                    settings.InventoryFieldsSettings.BarCodeFieldSetting,
                    model.InventoryFields.BarCode);
            var purchaseDate =
                this.configurableFieldModelBuilder.CreateNullableDateTimeField(
                    settings.InventoryFieldsSettings.PurchaseDateFieldSetting,
                    model.InventoryFields.PurchaseDate);

            var inventoryFieldModel = new InventoryFieldsModel(barCode, purchaseDate);

            var location =
                this.configurableFieldModelBuilder.CreateStringField(
                    settings.PlaceFieldsSettings.LocationFieldSetting,
                    model.PlaceFields.Location);

            var placeFieldsModel = new PlaceFieldsModel(
                model.PlaceFields.BuildingId,
                model.PlaceFields.FloorId,
                model.PlaceFields.RoomId,
                location);

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

            var trays = this.configurableFieldModelBuilder.CreateStringField(
                settings.OtherFieldsSettings.NumberOfTraysFieldSetting,
                model.OtherFields.NumberOfTrays);
            var driver = this.configurableFieldModelBuilder.CreateStringField(
                settings.OtherFieldsSettings.DriverFieldSetting,
                model.OtherFields.Driver);
            var info = this.configurableFieldModelBuilder.CreateStringField(
                settings.OtherFieldsSettings.InfoFieldSetting,
                model.OtherFields.Info);
            var url = this.configurableFieldModelBuilder.CreateStringField(
                settings.OtherFieldsSettings.URLFieldSetting,
                model.OtherFields.URL);

            var otherFieldModel = new OtherFieldsModel(trays, driver, info, url);

            var adapter =
                this.configurableFieldModelBuilder.CreateStringField(
                    settings.CommunicationFieldsSettings.NetworkAdapterFieldSetting,
                    model.CommunicationFields.NetworkAdapterName);

            var ip =
                this.configurableFieldModelBuilder.CreateStringField(
                    settings.CommunicationFieldsSettings.IPAddressFieldSetting,
                    model.CommunicationFields.IPAddress);
            var mac =
                this.configurableFieldModelBuilder.CreateStringField(
                    settings.CommunicationFieldsSettings.MacAddressFieldSetting,
                    model.CommunicationFields.MacAddress);

            var communicationFieldsModel = new CommunicationFieldsModel(
                adapter,
                ip,
                mac);

            var ou =
                this.configurableFieldModelBuilder.CreateStringField(
                    settings.OrganizationFieldsSettings.UnitFieldSetting,
                    model.OrganizationFields.UnitId);

            var organizationFieldsModel = new OrganizationFieldsModel(model.OrganizationFields.DepartmentId, ou);

            var departments =
                this.configurableFieldModelBuilder.CreateSelectListField(
                    settings.OrganizationFieldsSettings.DepartmentFieldSetting,
                    options.Departments,
                    model.OrganizationFields.DepartmentId.ToString());

            var organizationViewModel = new OrganizationFieldsViewModel(organizationFieldsModel, departments);

            return new PrinterViewModel(
                generalFieldsModel,
                inventoryFieldModel,
                communicationFieldsModel,
                otherFieldModel,
                organizationViewModel,
                placeFieldsViewModel) { Id = model.Id, CreatedDate = createdDate, ChangedDate = changedDate };
        }

        public PrinterViewModel BuildViewModel(
            PrinterEditOptionsResponse options,
            PrinterFieldsSettingsForModelEdit settings,
            int currentCustomerId)
        {
            var name =
                this.configurableFieldModelBuilder.CreateStringField(
                    settings.GeneralFieldsSettings.NameFieldSetting,
                    null);
            var manufacturer =
                this.configurableFieldModelBuilder.CreateStringField(
                    settings.GeneralFieldsSettings.ManufacturerFieldSetting,
                    null);
            var description =
                this.configurableFieldModelBuilder.CreateStringField(
                    settings.GeneralFieldsSettings.ModelFieldSetting,
                    null);
            var serial =
                this.configurableFieldModelBuilder.CreateStringField(
                    settings.GeneralFieldsSettings.SerialNumberFieldSetting,
                    null);

            var generalFieldsModel = new GeneralFieldsModel(
                name,
                manufacturer,
                description,
                serial);

            var barCode =
                this.configurableFieldModelBuilder.CreateStringField(
                    settings.InventoryFieldsSettings.BarCodeFieldSetting,
                    null);
            var purchaseDate =
                this.configurableFieldModelBuilder.CreateNullableDateTimeField(
                    settings.InventoryFieldsSettings.PurchaseDateFieldSetting,
                    null);

            var inventoryFieldModel = new InventoryFieldsModel(barCode, purchaseDate);

            var location =
                this.configurableFieldModelBuilder.CreateStringField(
                    settings.PlaceFieldsSettings.LocationFieldSetting,
                    null);

            var placeFieldsModel = new PlaceFieldsModel(
                null,
                null,
                null,
                location);

            var buildings = this.configurableFieldModelBuilder.CreateSelectList(
                settings.PlaceFieldsSettings.RoomFieldSetting,
                options.Buildings,
                null);
            var floors = this.configurableFieldModelBuilder.CreateSelectList(
                settings.PlaceFieldsSettings.RoomFieldSetting,
                options.Floors,
                null);
            var rooms =
                this.configurableFieldModelBuilder.CreateSelectListField(
                    settings.PlaceFieldsSettings.RoomFieldSetting,
                    options.Rooms,
                    null);

            var placeFieldsViewModel = new PlaceFieldsViewModel(placeFieldsModel, buildings, floors, rooms);

            var trays = this.configurableFieldModelBuilder.CreateStringField(
                settings.OtherFieldsSettings.NumberOfTraysFieldSetting,
                null);
            var driver = this.configurableFieldModelBuilder.CreateStringField(
                settings.OtherFieldsSettings.DriverFieldSetting,
                null);
            var info = this.configurableFieldModelBuilder.CreateStringField(
                settings.OtherFieldsSettings.InfoFieldSetting,
                null);
            var url = this.configurableFieldModelBuilder.CreateStringField(
                settings.OtherFieldsSettings.URLFieldSetting,
                null);

            var otherFieldModel = new OtherFieldsModel(trays, driver, info, url);

            var adapter =
                this.configurableFieldModelBuilder.CreateStringField(
                    settings.CommunicationFieldsSettings.NetworkAdapterFieldSetting,
                    null);

            var ip =
                this.configurableFieldModelBuilder.CreateStringField(
                    settings.CommunicationFieldsSettings.IPAddressFieldSetting,
                    null);
            var mac =
                this.configurableFieldModelBuilder.CreateStringField(
                    settings.CommunicationFieldsSettings.MacAddressFieldSetting,
                    null);

            var communicationFieldsModel = new CommunicationFieldsModel(
                adapter,
                ip,
                mac);

            var ou =
                this.configurableFieldModelBuilder.CreateStringField(
                    settings.OrganizationFieldsSettings.UnitFieldSetting,
                    null);

            var organizationFieldsModel = new OrganizationFieldsModel(null, ou);

            var departments =
                this.configurableFieldModelBuilder.CreateSelectListField(
                    settings.OrganizationFieldsSettings.DepartmentFieldSetting,
                    options.Departments,
                    null);

            var organizationViewModel = new OrganizationFieldsViewModel(organizationFieldsModel, departments);

            return new PrinterViewModel(
                generalFieldsModel,
                inventoryFieldModel,
                communicationFieldsModel,
                otherFieldModel,
                organizationViewModel,
                placeFieldsViewModel) { CustomerId = currentCustomerId };
        }
    }
}