namespace DH.Helpdesk.Web.Infrastructure.BusinessModelFactories.Inventory.Concrete
{
    using System;

    using DH.Helpdesk.BusinessData.Models;
    using DH.Helpdesk.BusinessData.Models.Inventory.Edit.Printer;
    using DH.Helpdesk.BusinessData.Models.Inventory.Edit.Shared;
    using DH.Helpdesk.Web.Areas.Inventory.Models.EditModel;
    using DH.Helpdesk.Web.Areas.Inventory.Models.EditModel.Printer;
    using DH.Helpdesk.Web.Areas.Inventory.Models.EditModel.Shared;

    public class PrinterBuilder : IPrinterBuilder
    {
        public PrinterForUpdate BuildForUpdate(PrinterViewModel model, OperationContext context)
        {
            var general = CreateGeneral(model.GeneralFieldsModel);
            var inventering = CreateInventering(model.InventoryFieldsModel);
            var communication = CreateCommunication(model.CommunicationFieldsModel);
            var other = CreateOther(model.OtherFieldsModel);
            var place = CreatePlace(model.PlaceFieldsViewModel);
            var organzation = CreateOrganization(model.OrganizationFieldsViewModel);

            var fieldsModel = new PrinterForUpdate(
                model.Id,
                inventering,
                general,
                communication,
                other,
                organzation,
                place,
                context.DateAndTime,
                context.UserId);

            return fieldsModel;
        }

        public PrinterForInsert BuildForAdd(PrinterViewModel model, OperationContext context)
        {
            var general = CreateGeneral(model.GeneralFieldsModel);
            var inventering = CreateInventering(model.InventoryFieldsModel);
            var communication = CreateCommunication(model.CommunicationFieldsModel);
            var other = CreateOther(model.OtherFieldsModel);
            var place = CreatePlace(model.PlaceFieldsViewModel);
            var organzation = CreateOrganization(model.OrganizationFieldsViewModel);

            var fieldsModel = new PrinterForInsert(
                inventering,
                general,
                communication,
                other,
                organzation,
                place,
                context.CustomerId,
                context.DateAndTime,
                context.UserId);

            return fieldsModel;
        }

        private static GeneralFields CreateGeneral(GeneralFieldsModel fieldsModel)
        {
            if (fieldsModel == null)
            {
                return GeneralFields.CreateDefault();
            }

            var name = ConfigurableFieldModel<string>.GetValueOrDefault(fieldsModel.Name);
            var manufacturer = ConfigurableFieldModel<string>.GetValueOrDefault(fieldsModel.Manufacturer);
            var serialNumber = ConfigurableFieldModel<string>.GetValueOrDefault(fieldsModel.SerialNumber);
            var model = ConfigurableFieldModel<string>.GetValueOrDefault(fieldsModel.Model);

            var fields = new GeneralFields(name, manufacturer, model, serialNumber);

            return fields;
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

        private static CommunicationFields CreateCommunication(CommunicationFieldsModel fieldsModel)
        {
            if (fieldsModel == null)
            {
                return CommunicationFields.CreateDefault();
            }

            var networkAdapter = ConfigurableFieldModel<string>.GetValueOrDefault(fieldsModel.NetworkAdapterName);
            var ipaddress = ConfigurableFieldModel<string>.GetValueOrDefault(fieldsModel.IPAddress);
            var macAddress = ConfigurableFieldModel<string>.GetValueOrDefault(fieldsModel.MacAddress);

            var fields = new CommunicationFields(
                networkAdapter,
                ipaddress,
                macAddress);

            return fields;
        }

        private static OtherFields CreateOther(OtherFieldsModel fieldsModel)
        {
            if (fieldsModel == null)
            {
                return OtherFields.CreateDefault();
            }

            var info = ConfigurableFieldModel<string>.GetValueOrDefault(fieldsModel.Info);
            var driver = ConfigurableFieldModel<string>.GetValueOrDefault(fieldsModel.Driver);
            var url = ConfigurableFieldModel<string>.GetValueOrDefault(fieldsModel.URL);
            var trays = ConfigurableFieldModel<string>.GetValueOrDefault(fieldsModel.NumberOfTrays);

            var fields = new OtherFields(trays, driver, info, url);

            return fields;
        }

        private static PlaceFields CreatePlace(PlaceFieldsViewModel fieldsModel)
        {
            if (fieldsModel == null || fieldsModel.PlaceFieldsModel == null)
            {
                return PlaceFields.CreateDefault();
            }

            var room = ConfigurableFieldModel<int?>.GetValueOrDefault(fieldsModel.PlaceFieldsModel.RoomId);
            var location = ConfigurableFieldModel<string>.GetValueOrDefault(fieldsModel.PlaceFieldsModel.Location);

            var fields = new PlaceFields(room, location);

            return fields;
        }

        private static OrganizationFields CreateOrganization(OrganizationFieldsViewModel fieldsModel)
        {
            if (fieldsModel == null || fieldsModel.OrganizationFieldsModel == null)
            {
                return OrganizationFields.CreateDefault();
            }

            var department = ConfigurableFieldModel<int?>.GetValueOrDefault(fieldsModel.OrganizationFieldsModel.DepartmentId);
            var domain = ConfigurableFieldModel<string>.GetValueOrDefault(fieldsModel.OrganizationFieldsModel.UnitId);

            var fields = new OrganizationFields(department, domain);

            return fields;
        }
    }
}