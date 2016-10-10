namespace DH.Helpdesk.Web.Areas.Orders.Infrastructure.ModelFactories.Concrete
{
    using System.Collections.Generic;
    using System.Linq;

    using DH.Helpdesk.BusinessData.Enums.Orders.FieldNames;
    using DH.Helpdesk.BusinessData.Models.Orders.Index;
    using DH.Helpdesk.BusinessData.Models.Orders.Index.FieldSettingsOverview;
    using DH.Helpdesk.BusinessData.Models.Orders.Index.OrderOverview;
    using DH.Helpdesk.BusinessData.Models.Shared.Input;
    using DH.Helpdesk.Web.Areas.Orders.Models.Index;
    using DH.Helpdesk.Web.Infrastructure.Tools;
    using DH.Helpdesk.Web.Models.Shared;

    public sealed class OrdersModelFactory : IOrdersModelFactory
    {
        public OrdersIndexModel GetIndexModel(OrdersFilterData data, OrdersFilterModel filter)
        {
            var orderTypes = WebMvcHelper.CreateListField(data.OrderTypes, filter.OrderTypeId, true);
            var administrators = WebMvcHelper.CreateMultiSelectField(data.Administrators, filter.AdministratiorIds);
            var statuses = WebMvcHelper.CreateMultiSelectField(data.OrderStatuses, filter.StatusIds);
            var orderTypesForCreateOrder = WebMvcHelper.CreateListField(data.OrderTypes, filter.OrderTypeId, false);

            SortFieldModel sortField = null;

            if (filter.SortField != null)
            {
                sortField = new SortFieldModel { Name = filter.SortField.Name, SortBy = filter.SortField.SortBy };
            }

            return new OrdersIndexModel(
                                    orderTypes, 
                                    administrators, 
                                    filter.StartDate,
                                    filter.EndDate,
                                    statuses,
                                    filter.Text,
                                    filter.RecordsOnPage,
                                    sortField,
                                    orderTypesForCreateOrder);
        }

        public OrdersGridModel Create(SearchResponse response, SortField sortField)
        {
            var headers = new List<GridColumnHeaderModel>();

            CreateGeneralHeaders(response.OverviewSettings.General, headers);
            CreateLogHeaders(response.OverviewSettings.Log, headers);
            CreateOrdererHeaders(response.OverviewSettings.Orderer, headers);
            CreateOrderHeaders(response.OverviewSettings.Order, headers);
            CreateReceiverHeaders(response.OverviewSettings.Receiver, headers);
            CreateSupplierHeaders(response.OverviewSettings.Supplier, headers);
            CreateDeliveryHeaders(response.OverviewSettings.Delivery, headers);
            CreateOtherHeaders(response.OverviewSettings.Other, headers);
            CreateProgramHeaders(response.OverviewSettings.Program, headers);
            CreateUserHeaders(response.OverviewSettings.User, headers);

            var orderOverviews =
                response.SearchResult.Orders.Select(o => CreateFullValues(response.OverviewSettings, o)).ToList();

            return new OrdersGridModel(headers, orderOverviews, response.SearchResult.OrdersFound, sortField);
        }
       
        #region Create headers

        private static void CreateDeliveryHeaders(DeliveryFieldSettingsOverview settings, List<GridColumnHeaderModel> headers)
        {
            FieldSettingsHelper.CreateHeaderIfNeeded(settings.DeliveryDate, DeliveryFieldNames.DeliveryDate, headers);
            FieldSettingsHelper.CreateHeaderIfNeeded(settings.InstallDate, DeliveryFieldNames.InstallDate, headers);
            FieldSettingsHelper.CreateHeaderIfNeeded(settings.DeliveryDepartment, DeliveryFieldNames.DeliveryDepartment, headers);
            FieldSettingsHelper.CreateHeaderIfNeeded(settings.DeliveryOu, DeliveryFieldNames.DeliveryOu, headers);
            FieldSettingsHelper.CreateHeaderIfNeeded(settings.DeliveryAddress, DeliveryFieldNames.DeliveryAddress, headers);
            FieldSettingsHelper.CreateHeaderIfNeeded(settings.DeliveryPostalCode, DeliveryFieldNames.DeliveryPostalCode, headers);
            FieldSettingsHelper.CreateHeaderIfNeeded(settings.DeliveryPostalAddress, DeliveryFieldNames.DeliveryPostalAddress, headers);
            FieldSettingsHelper.CreateHeaderIfNeeded(settings.DeliveryLocation, DeliveryFieldNames.DeliveryLocation, headers);
            FieldSettingsHelper.CreateHeaderIfNeeded(settings.DeliveryInfo1, DeliveryFieldNames.DeliveryInfo1, headers);
            FieldSettingsHelper.CreateHeaderIfNeeded(settings.DeliveryInfo2, DeliveryFieldNames.DeliveryInfo2, headers);
            FieldSettingsHelper.CreateHeaderIfNeeded(settings.DeliveryInfo3, DeliveryFieldNames.DeliveryInfo3, headers);
            FieldSettingsHelper.CreateHeaderIfNeeded(settings.DeliveryOuId, DeliveryFieldNames.DeliveryOuId, headers);
        }

        private static void CreateGeneralHeaders(GeneralFieldSettingsOverview settings, List<GridColumnHeaderModel> headers)
        {
            FieldSettingsHelper.CreateHeaderIfNeeded(settings.OrderNumber, GeneralFieldNames.OrderNumber, headers);
            FieldSettingsHelper.CreateHeaderIfNeeded(settings.Customer, GeneralFieldNames.Customer, headers);
            FieldSettingsHelper.CreateHeaderIfNeeded(settings.Administrator, GeneralFieldNames.Administrator, headers);
            FieldSettingsHelper.CreateHeaderIfNeeded(settings.Domain, GeneralFieldNames.Domain, headers);
            FieldSettingsHelper.CreateHeaderIfNeeded(settings.OrderDate, GeneralFieldNames.OrderDate, headers);
        }

        private static void CreateLogHeaders(LogFieldSettingsOverview settings, List<GridColumnHeaderModel> headers)
        {
            FieldSettingsHelper.CreateHeaderIfNeeded(settings.Log, LogFieldNames.Log, headers);
        }

        private static void CreateOrdererHeaders(OrdererFieldSettingsOverview settings, List<GridColumnHeaderModel> headers)
        {
            FieldSettingsHelper.CreateHeaderIfNeeded(settings.OrdererId, OrdererFieldNames.OrdererId, headers);
            FieldSettingsHelper.CreateHeaderIfNeeded(settings.OrdererName, OrdererFieldNames.OrdererName, headers);
            FieldSettingsHelper.CreateHeaderIfNeeded(settings.OrdererLocation, OrdererFieldNames.OrdererLocation, headers);
            FieldSettingsHelper.CreateHeaderIfNeeded(settings.OrdererEmail, OrdererFieldNames.OrdererEmail, headers);
            FieldSettingsHelper.CreateHeaderIfNeeded(settings.OrdererPhone, OrdererFieldNames.OrdererPhone, headers);
            FieldSettingsHelper.CreateHeaderIfNeeded(settings.OrdererCode, OrdererFieldNames.OrdererCode, headers);
            FieldSettingsHelper.CreateHeaderIfNeeded(settings.Department, OrdererFieldNames.Department, headers);
            FieldSettingsHelper.CreateHeaderIfNeeded(settings.Unit, OrdererFieldNames.Unit, headers);
            FieldSettingsHelper.CreateHeaderIfNeeded(settings.OrdererAddress, OrdererFieldNames.OrdererAddress, headers);
            FieldSettingsHelper.CreateHeaderIfNeeded(settings.OrdererInvoiceAddress, OrdererFieldNames.OrdererInvoiceAddress, headers);
            FieldSettingsHelper.CreateHeaderIfNeeded(settings.OrdererReferenceNumber, OrdererFieldNames.OrdererReferenceNumber, headers);
            FieldSettingsHelper.CreateHeaderIfNeeded(settings.AccountingDimension1, OrdererFieldNames.AccountingDimension1, headers);
            FieldSettingsHelper.CreateHeaderIfNeeded(settings.AccountingDimension2, OrdererFieldNames.AccountingDimension2, headers);
            FieldSettingsHelper.CreateHeaderIfNeeded(settings.AccountingDimension3, OrdererFieldNames.AccountingDimension3, headers);
            FieldSettingsHelper.CreateHeaderIfNeeded(settings.AccountingDimension4, OrdererFieldNames.AccountingDimension4, headers);
            FieldSettingsHelper.CreateHeaderIfNeeded(settings.AccountingDimension5, OrdererFieldNames.AccountingDimension5, headers);
        }

        private static void CreateOrderHeaders(OrderFieldSettingsOverview settings, List<GridColumnHeaderModel> headers)
        {
            FieldSettingsHelper.CreateHeaderIfNeeded(settings.Property, OrderFieldNames.Property, headers);
            FieldSettingsHelper.CreateHeaderIfNeeded(settings.OrderRow1, OrderFieldNames.OrderRow1, headers);
            FieldSettingsHelper.CreateHeaderIfNeeded(settings.OrderRow2, OrderFieldNames.OrderRow2, headers);
            FieldSettingsHelper.CreateHeaderIfNeeded(settings.OrderRow3, OrderFieldNames.OrderRow3, headers);
            FieldSettingsHelper.CreateHeaderIfNeeded(settings.OrderRow4, OrderFieldNames.OrderRow4, headers);
            FieldSettingsHelper.CreateHeaderIfNeeded(settings.OrderRow5, OrderFieldNames.OrderRow5, headers);
            FieldSettingsHelper.CreateHeaderIfNeeded(settings.OrderRow6, OrderFieldNames.OrderRow6, headers);
            FieldSettingsHelper.CreateHeaderIfNeeded(settings.OrderRow7, OrderFieldNames.OrderRow7, headers);
            FieldSettingsHelper.CreateHeaderIfNeeded(settings.OrderRow8, OrderFieldNames.OrderRow8, headers);
            FieldSettingsHelper.CreateHeaderIfNeeded(settings.Configuration, OrderFieldNames.Configuration, headers);
            FieldSettingsHelper.CreateHeaderIfNeeded(settings.OrderInfo, OrderFieldNames.OrderInfo, headers);
            FieldSettingsHelper.CreateHeaderIfNeeded(settings.OrderInfo2, OrderFieldNames.OrderInfo2, headers);
        }

        private static void CreateOtherHeaders(OtherFieldSettingsOverview settings, List<GridColumnHeaderModel> headers)
        {
            FieldSettingsHelper.CreateHeaderIfNeeded(settings.FileName, OtherFieldNames.FileName, headers);
            FieldSettingsHelper.CreateHeaderIfNeeded(settings.CaseNumber, OtherFieldNames.CaseNumber, headers);
            FieldSettingsHelper.CreateHeaderIfNeeded(settings.Info, OtherFieldNames.Info, headers);
            FieldSettingsHelper.CreateHeaderIfNeeded(settings.Status, OtherFieldNames.Status, headers);
        }

        private static void CreateProgramHeaders(ProgramFieldSettingsOverview settings, List<GridColumnHeaderModel> headers)
        {
            FieldSettingsHelper.CreateHeaderIfNeeded(settings.Program, ProgramFieldNames.Program, headers);
        }

        private static void CreateReceiverHeaders(ReceiverFieldSettingsOverview settings, List<GridColumnHeaderModel> headers)
        {
            FieldSettingsHelper.CreateHeaderIfNeeded(settings.ReceiverId, ReceiverFieldNames.ReceiverId, headers);
            FieldSettingsHelper.CreateHeaderIfNeeded(settings.ReceiverName, ReceiverFieldNames.ReceiverName, headers);
            FieldSettingsHelper.CreateHeaderIfNeeded(settings.ReceiverEmail, ReceiverFieldNames.ReceiverEmail, headers);
            FieldSettingsHelper.CreateHeaderIfNeeded(settings.ReceiverPhone, ReceiverFieldNames.ReceiverPhone, headers);
            FieldSettingsHelper.CreateHeaderIfNeeded(settings.ReceiverLocation, ReceiverFieldNames.ReceiverLocation, headers);
            FieldSettingsHelper.CreateHeaderIfNeeded(settings.MarkOfGoods, ReceiverFieldNames.MarkOfGoods, headers);
        }

        private static void CreateSupplierHeaders(SupplierFieldSettingsOverview settings, List<GridColumnHeaderModel> headers)
        {
            FieldSettingsHelper.CreateHeaderIfNeeded(settings.SupplierOrderNumber, SupplierFieldNames.SupplierOrderNumber, headers);
            FieldSettingsHelper.CreateHeaderIfNeeded(settings.SupplierOrderDate, SupplierFieldNames.SupplierOrderDate, headers);
            FieldSettingsHelper.CreateHeaderIfNeeded(settings.SupplierOrderInfo, SupplierFieldNames.SupplierOrderInfo, headers);
        }

        private static void CreateUserHeaders(UserFieldSettingsOverview settings, List<GridColumnHeaderModel> headers)
        {
            FieldSettingsHelper.CreateHeaderIfNeeded(settings.UserId, UserFieldNames.UserId, headers);
            FieldSettingsHelper.CreateHeaderIfNeeded(settings.UserFirstName, UserFieldNames.UserFirstName, headers);
            FieldSettingsHelper.CreateHeaderIfNeeded(settings.UserLastName, UserFieldNames.UserLastName, headers);
        }        

        #endregion

        #region Create values

        private static OrderOverviewModel CreateFullValues(FullFieldSettingsOverview settings, FullOrderOverview order)
        {
            var values = new List<NewGridRowCellValueModel>();

            CreateGeneralValues(settings.General, order.General, values);
            CreateLogValues(settings.Log, order.Log, values);
            CreateOrdererValues(settings.Orderer, order.Orderer, values);
            CreateOrderValues(settings.Order, order.Order, values);
            CreateReceiverValues(settings.Receiver, order.Receiver, values);
            CreateSupplierValues(settings.Supplier, order.Supplier, values);
            CreateDeliveryValues(settings.Delivery, order.Delivery, values);
            CreateOtherValues(settings.Other, order.Other, values);
            CreateProgramValues(settings.Program, order.Program, values);
            CreateUserValues(settings.User, order.User, values);

            return new OrderOverviewModel(order.Id, values);
        }

        private static void CreateDeliveryValues(
            DeliveryFieldSettingsOverview settings,
            DeliveryOverview fields,
            List<NewGridRowCellValueModel> values)
        {
            FieldSettingsHelper.CreateValueIfNeeded(settings.DeliveryDate, DeliveryFieldNames.DeliveryDate, fields.DeliveryDate, values);
            FieldSettingsHelper.CreateValueIfNeeded(settings.InstallDate, DeliveryFieldNames.InstallDate, fields.InstallDate, values);
            FieldSettingsHelper.CreateValueIfNeeded(settings.DeliveryDepartment, DeliveryFieldNames.DeliveryDepartment, fields.DeliveryDepartment, values);
            FieldSettingsHelper.CreateValueIfNeeded(settings.DeliveryOu, DeliveryFieldNames.DeliveryOu, fields.DeliveryOu, values);
            FieldSettingsHelper.CreateValueIfNeeded(settings.DeliveryAddress, DeliveryFieldNames.DeliveryAddress, fields.DeliveryAddress, values);
            FieldSettingsHelper.CreateValueIfNeeded(settings.DeliveryPostalCode, DeliveryFieldNames.DeliveryPostalCode, fields.DeliveryPostalCode, values);
            FieldSettingsHelper.CreateValueIfNeeded(settings.DeliveryPostalAddress, DeliveryFieldNames.DeliveryPostalAddress, fields.DeliveryPostalAddress, values);
            FieldSettingsHelper.CreateValueIfNeeded(settings.DeliveryLocation, DeliveryFieldNames.DeliveryLocation, fields.DeliveryLocation, values);
            FieldSettingsHelper.CreateValueIfNeeded(settings.DeliveryInfo1, DeliveryFieldNames.DeliveryInfo1, fields.DeliveryInfo1, values);
            FieldSettingsHelper.CreateValueIfNeeded(settings.DeliveryInfo2, DeliveryFieldNames.DeliveryInfo2, fields.DeliveryInfo2, values);
            FieldSettingsHelper.CreateValueIfNeeded(settings.DeliveryInfo3, DeliveryFieldNames.DeliveryInfo3, fields.DeliveryInfo3, values);
            FieldSettingsHelper.CreateValueIfNeeded(settings.DeliveryOuId, DeliveryFieldNames.DeliveryOuId, fields.DeliveryOuId, values);
        }

        private static void CreateGeneralValues(
            GeneralFieldSettingsOverview settings,
            GeneralOverview fields,
            List<NewGridRowCellValueModel> values)
        {
            FieldSettingsHelper.CreateValueIfNeeded(settings.OrderNumber, GeneralFieldNames.OrderNumber, fields.OrderNumber, values);   
            FieldSettingsHelper.CreateValueIfNeeded(settings.Customer, GeneralFieldNames.Customer, fields.Customer, values);   
            FieldSettingsHelper.CreateValueIfNeeded(settings.Administrator, GeneralFieldNames.Administrator, fields.Administrator, values);   
            FieldSettingsHelper.CreateValueIfNeeded(settings.Domain, GeneralFieldNames.Domain, fields.Domain, values);
            FieldSettingsHelper.CreateValueIfNeeded(settings.OrderDate, GeneralFieldNames.OrderDate, fields.OrderDate, values);   
        }

        private static void CreateLogValues(
            LogFieldSettingsOverview settings,
            LogOverview fields,
            List<NewGridRowCellValueModel> values)
        {
            FieldSettingsHelper.CreateValueIfNeeded(settings.Log, LogFieldNames.Log, fields.Logs, values);
        }

        private static void CreateOrdererValues(
            OrdererFieldSettingsOverview settings,
            OrdererOverview fields,
            List<NewGridRowCellValueModel> values)
        {
            FieldSettingsHelper.CreateValueIfNeeded(settings.OrdererId, OrdererFieldNames.OrdererId, fields.OrdererId, values);
            FieldSettingsHelper.CreateValueIfNeeded(settings.OrdererName, OrdererFieldNames.OrdererName, fields.OrdererName, values);
            FieldSettingsHelper.CreateValueIfNeeded(settings.OrdererLocation, OrdererFieldNames.OrdererLocation, fields.OrdererLocation, values);
            FieldSettingsHelper.CreateValueIfNeeded(settings.OrdererEmail, OrdererFieldNames.OrdererEmail, fields.OrdererEmail, values);
            FieldSettingsHelper.CreateValueIfNeeded(settings.OrdererPhone, OrdererFieldNames.OrdererPhone, fields.OrdererPhone, values);
            FieldSettingsHelper.CreateValueIfNeeded(settings.OrdererCode, OrdererFieldNames.OrdererCode, fields.OrdererCode, values);
            FieldSettingsHelper.CreateValueIfNeeded(settings.Department, OrdererFieldNames.Department, fields.Department, values);
            FieldSettingsHelper.CreateValueIfNeeded(settings.Unit, OrdererFieldNames.Unit, fields.Unit, values);
            FieldSettingsHelper.CreateValueIfNeeded(settings.OrdererAddress, OrdererFieldNames.OrdererAddress, fields.OrdererAddress, values);
            FieldSettingsHelper.CreateValueIfNeeded(settings.OrdererInvoiceAddress, OrdererFieldNames.OrdererInvoiceAddress, fields.OrdererInvoiceAddress, values);
            FieldSettingsHelper.CreateValueIfNeeded(settings.OrdererReferenceNumber, OrdererFieldNames.OrdererReferenceNumber, fields.OrdererReferenceNumber, values);
            FieldSettingsHelper.CreateValueIfNeeded(settings.AccountingDimension1, OrdererFieldNames.AccountingDimension1, fields.AccountingDimension1, values);
            FieldSettingsHelper.CreateValueIfNeeded(settings.AccountingDimension2, OrdererFieldNames.AccountingDimension2, fields.AccountingDimension2, values);
            FieldSettingsHelper.CreateValueIfNeeded(settings.AccountingDimension3, OrdererFieldNames.AccountingDimension3, fields.AccountingDimension3, values);
            FieldSettingsHelper.CreateValueIfNeeded(settings.AccountingDimension4, OrdererFieldNames.AccountingDimension4, fields.AccountingDimension4, values);
            FieldSettingsHelper.CreateValueIfNeeded(settings.AccountingDimension5, OrdererFieldNames.AccountingDimension5, fields.AccountingDimension5, values);
        }

        private static void CreateOrderValues(
            OrderFieldSettingsOverview settings,
            OrderOverview fields,
            List<NewGridRowCellValueModel> values)
        {
            FieldSettingsHelper.CreateValueIfNeeded(settings.Property, OrderFieldNames.Property, fields.Property, values);
            FieldSettingsHelper.CreateValueIfNeeded(settings.OrderRow1, OrderFieldNames.OrderRow1, fields.OrderRow1, values);
            FieldSettingsHelper.CreateValueIfNeeded(settings.OrderRow2, OrderFieldNames.OrderRow2, fields.OrderRow2, values);
            FieldSettingsHelper.CreateValueIfNeeded(settings.OrderRow3, OrderFieldNames.OrderRow3, fields.OrderRow3, values);
            FieldSettingsHelper.CreateValueIfNeeded(settings.OrderRow4, OrderFieldNames.OrderRow4, fields.OrderRow4, values);
            FieldSettingsHelper.CreateValueIfNeeded(settings.OrderRow5, OrderFieldNames.OrderRow5, fields.OrderRow5, values);
            FieldSettingsHelper.CreateValueIfNeeded(settings.OrderRow6, OrderFieldNames.OrderRow6, fields.OrderRow6, values);
            FieldSettingsHelper.CreateValueIfNeeded(settings.OrderRow7, OrderFieldNames.OrderRow7, fields.OrderRow7, values);
            FieldSettingsHelper.CreateValueIfNeeded(settings.OrderRow8, OrderFieldNames.OrderRow8, fields.OrderRow8, values);
            FieldSettingsHelper.CreateValueIfNeeded(settings.Configuration, OrderFieldNames.Configuration, fields.Configuration, values);
            FieldSettingsHelper.CreateValueIfNeeded(settings.OrderInfo, OrderFieldNames.OrderInfo, fields.OrderInfo, values);
            FieldSettingsHelper.CreateValueIfNeeded(settings.OrderInfo2, OrderFieldNames.OrderInfo2, fields.OrderInfo2, values);
        }

        private static void CreateOtherValues(
            OtherFieldSettingsOverview settings,
            OtherOverview fields,
            List<NewGridRowCellValueModel> values)
        {
            FieldSettingsHelper.CreateValueIfNeeded(settings.FileName, OtherFieldNames.FileName, fields.FileName, values);
            FieldSettingsHelper.CreateValueIfNeeded(settings.CaseNumber, OtherFieldNames.CaseNumber, fields.CaseNumber, values);
            FieldSettingsHelper.CreateValueIfNeeded(settings.Info, OtherFieldNames.Info, fields.Info, values);
            FieldSettingsHelper.CreateValueIfNeeded(settings.Status, OtherFieldNames.Status, fields.Status, values);
        }

        private static void CreateProgramValues(
            ProgramFieldSettingsOverview settings,
            ProgramOverview fields,
            List<NewGridRowCellValueModel> values)
        {
            FieldSettingsHelper.CreateValueIfNeeded(settings.Program, ProgramFieldNames.Program, fields.Programs, values);
        }

        private static void CreateReceiverValues(
            ReceiverFieldSettingsOverview settings,
            ReceiverOverview fields,
            List<NewGridRowCellValueModel> values)
        {
            FieldSettingsHelper.CreateValueIfNeeded(settings.ReceiverId, ReceiverFieldNames.ReceiverId, fields.ReceiverId, values);
            FieldSettingsHelper.CreateValueIfNeeded(settings.ReceiverName, ReceiverFieldNames.ReceiverName, fields.ReceiverName, values);
            FieldSettingsHelper.CreateValueIfNeeded(settings.ReceiverEmail, ReceiverFieldNames.ReceiverEmail, fields.ReceiverEmail, values);
            FieldSettingsHelper.CreateValueIfNeeded(settings.ReceiverPhone, ReceiverFieldNames.ReceiverPhone, fields.ReceiverPhone, values);
            FieldSettingsHelper.CreateValueIfNeeded(settings.ReceiverLocation, ReceiverFieldNames.ReceiverLocation, fields.ReceiverLocation, values);
            FieldSettingsHelper.CreateValueIfNeeded(settings.MarkOfGoods, ReceiverFieldNames.MarkOfGoods, fields.MarkOfGoods, values);
        }

        private static void CreateSupplierValues(
            SupplierFieldSettingsOverview settings,
            SupplierOverview fields,
            List<NewGridRowCellValueModel> values)
        {
            FieldSettingsHelper.CreateValueIfNeeded(settings.SupplierOrderNumber, SupplierFieldNames.SupplierOrderNumber, fields.SupplierOrderNumber, values);
            FieldSettingsHelper.CreateValueIfNeeded(settings.SupplierOrderDate, SupplierFieldNames.SupplierOrderDate, fields.SupplierOrderDate, values);
            FieldSettingsHelper.CreateValueIfNeeded(settings.SupplierOrderInfo, SupplierFieldNames.SupplierOrderInfo, fields.SupplierOrderInfo, values);
        }

        private static void CreateUserValues(
            UserFieldSettingsOverview settings,
            UserOverview fields,
            List<NewGridRowCellValueModel> values)
        {
            FieldSettingsHelper.CreateValueIfNeeded(settings.UserId, UserFieldNames.UserId, fields.UserId, values);
            FieldSettingsHelper.CreateValueIfNeeded(settings.UserFirstName, UserFieldNames.UserFirstName, fields.UserFirstName, values);
            FieldSettingsHelper.CreateValueIfNeeded(settings.UserLastName, UserFieldNames.UserLastName, fields.UserLastName, values);
        }

        #endregion
    }
}