namespace DH.Helpdesk.Web.Areas.Orders.Infrastructure.ModelFactories.Concrete
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using DH.Helpdesk.BusinessData.Enums.Orders.FieldNames;
    using DH.Helpdesk.BusinessData.Models.Orders.Index;
    using DH.Helpdesk.BusinessData.Models.Orders.Index.FieldSettingsOverview;
    using DH.Helpdesk.BusinessData.Models.Orders.Index.OrderOverview;
    using DH.Helpdesk.BusinessData.Models.Shared.Input;
    using DH.Helpdesk.BusinessData.Models.Shared.Output;
    using DH.Helpdesk.Services.DisplayValues;
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

            CreateDeliveryHeaders(response.OverviewSettings.Delivery, headers);
            CreateGeneralHeaders(response.OverviewSettings.General, headers);
            CreateLogHeaders(response.OverviewSettings.Log, headers);
            CreateOrdererHeaders(response.OverviewSettings.Orderer, headers);
            CreateOrderHeaders(response.OverviewSettings.Order, headers);
            CreateOtherHeaders(response.OverviewSettings.Other, headers);
            CreateProgramHeaders(response.OverviewSettings.Program, headers);
            CreateReceiverHeaders(response.OverviewSettings.Receiver, headers);
            CreateSupplierHeaders(response.OverviewSettings.Supplier, headers);
            CreateUserHeaders(response.OverviewSettings.User, headers);

            var orderOverviews =
                response.SearchResult.Orders.Select(o => CreateFullValues(response.OverviewSettings, o)).ToList();

            return new OrdersGridModel(headers, orderOverviews, response.SearchResult.OrdersFound, sortField);
        }
       
        #region Create headers

        private static void CreateDeliveryHeaders(DeliveryFieldSettingsOverview settings, List<GridColumnHeaderModel> headers)
        {
            CreateHeaderIfNeeded(settings.DeliveryDate, DeliveryFieldNames.DeliveryDate, headers);
            CreateHeaderIfNeeded(settings.InstallDate, DeliveryFieldNames.InstallDate, headers);
            CreateHeaderIfNeeded(settings.DeliveryDepartment, DeliveryFieldNames.DeliveryDepartment, headers);
            CreateHeaderIfNeeded(settings.DeliveryOu, DeliveryFieldNames.DeliveryOu, headers);
            CreateHeaderIfNeeded(settings.DeliveryAddress, DeliveryFieldNames.DeliveryAddress, headers);
            CreateHeaderIfNeeded(settings.DeliveryPostalCode, DeliveryFieldNames.DeliveryPostalCode, headers);
            CreateHeaderIfNeeded(settings.DeliveryPostalAddress, DeliveryFieldNames.DeliveryPostalAddress, headers);
            CreateHeaderIfNeeded(settings.DeliveryLocation, DeliveryFieldNames.DeliveryLocation, headers);
            CreateHeaderIfNeeded(settings.DeliveryInfo1, DeliveryFieldNames.DeliveryInfo1, headers);
            CreateHeaderIfNeeded(settings.DeliveryInfo2, DeliveryFieldNames.DeliveryInfo2, headers);
            CreateHeaderIfNeeded(settings.DeliveryInfo3, DeliveryFieldNames.DeliveryInfo3, headers);
            CreateHeaderIfNeeded(settings.DeliveryOuId, DeliveryFieldNames.DeliveryOuId, headers);
        }

        private static void CreateGeneralHeaders(GeneralFieldSettingsOverview settings, List<GridColumnHeaderModel> headers)
        {
            CreateHeaderIfNeeded(settings.OrderNumber, GeneralFieldNames.OrderNumber, headers);
            CreateHeaderIfNeeded(settings.Customer, GeneralFieldNames.Customer, headers);
            CreateHeaderIfNeeded(settings.Administrator, GeneralFieldNames.Administrator, headers);
            CreateHeaderIfNeeded(settings.Domain, GeneralFieldNames.Domain, headers);
            CreateHeaderIfNeeded(settings.OrderDate, GeneralFieldNames.OrderDate, headers);
        }

        private static void CreateLogHeaders(LogFieldSettingsOverview settings, List<GridColumnHeaderModel> headers)
        {
            CreateHeaderIfNeeded(settings.Log, LogFieldNames.Log, headers);
        }

        private static void CreateOrdererHeaders(OrdererFieldSettingsOverview settings, List<GridColumnHeaderModel> headers)
        {
            CreateHeaderIfNeeded(settings.OrdererId, OrdererFieldNames.OrdererId, headers);
            CreateHeaderIfNeeded(settings.OrdererName, OrdererFieldNames.OrdererName, headers);
            CreateHeaderIfNeeded(settings.OrdererLocation, OrdererFieldNames.OrdererLocation, headers);
            CreateHeaderIfNeeded(settings.OrdererEmail, OrdererFieldNames.OrdererEmail, headers);
            CreateHeaderIfNeeded(settings.OrdererPhone, OrdererFieldNames.OrdererPhone, headers);
            CreateHeaderIfNeeded(settings.OrdererCode, OrdererFieldNames.OrdererCode, headers);
            CreateHeaderIfNeeded(settings.Department, OrdererFieldNames.Department, headers);
            CreateHeaderIfNeeded(settings.Unit, OrdererFieldNames.Unit, headers);
            CreateHeaderIfNeeded(settings.OrdererAddress, OrdererFieldNames.OrdererAddress, headers);
            CreateHeaderIfNeeded(settings.OrdererInvoiceAddress, OrdererFieldNames.OrdererInvoiceAddress, headers);
            CreateHeaderIfNeeded(settings.OrdererReferenceNumber, OrdererFieldNames.OrdererReferenceNumber, headers);
            CreateHeaderIfNeeded(settings.AccountingDimension1, OrdererFieldNames.AccountingDimension1, headers);
            CreateHeaderIfNeeded(settings.AccountingDimension2, OrdererFieldNames.AccountingDimension2, headers);
            CreateHeaderIfNeeded(settings.AccountingDimension3, OrdererFieldNames.AccountingDimension3, headers);
            CreateHeaderIfNeeded(settings.AccountingDimension4, OrdererFieldNames.AccountingDimension4, headers);
            CreateHeaderIfNeeded(settings.AccountingDimension5, OrdererFieldNames.AccountingDimension5, headers);
        }

        private static void CreateOrderHeaders(OrderFieldSettingsOverview settings, List<GridColumnHeaderModel> headers)
        {
            CreateHeaderIfNeeded(settings.Property, OrderFieldNames.Property, headers);
            CreateHeaderIfNeeded(settings.OrderRow1, OrderFieldNames.OrderRow1, headers);
            CreateHeaderIfNeeded(settings.OrderRow2, OrderFieldNames.OrderRow2, headers);
            CreateHeaderIfNeeded(settings.OrderRow3, OrderFieldNames.OrderRow3, headers);
            CreateHeaderIfNeeded(settings.OrderRow4, OrderFieldNames.OrderRow4, headers);
            CreateHeaderIfNeeded(settings.OrderRow5, OrderFieldNames.OrderRow5, headers);
            CreateHeaderIfNeeded(settings.OrderRow6, OrderFieldNames.OrderRow6, headers);
            CreateHeaderIfNeeded(settings.OrderRow7, OrderFieldNames.OrderRow7, headers);
            CreateHeaderIfNeeded(settings.OrderRow8, OrderFieldNames.OrderRow8, headers);
            CreateHeaderIfNeeded(settings.Configuration, OrderFieldNames.Configuration, headers);
            CreateHeaderIfNeeded(settings.OrderInfo, OrderFieldNames.OrderInfo, headers);
            CreateHeaderIfNeeded(settings.OrderInfo2, OrderFieldNames.OrderInfo2, headers);
        }

        private static void CreateOtherHeaders(OtherFieldSettingsOverview settings, List<GridColumnHeaderModel> headers)
        {
            CreateHeaderIfNeeded(settings.FileName, OtherFieldNames.FileName, headers);
            CreateHeaderIfNeeded(settings.CaseNumber, OtherFieldNames.CaseNumber, headers);
            CreateHeaderIfNeeded(settings.Info, OtherFieldNames.Info, headers);
            CreateHeaderIfNeeded(settings.Status, OtherFieldNames.Status, headers);
        }

        private static void CreateProgramHeaders(ProgramFieldSettingsOverview settings, List<GridColumnHeaderModel> headers)
        {
            CreateHeaderIfNeeded(settings.Program, ProgramFieldNames.Program, headers);
        }

        private static void CreateReceiverHeaders(ReceiverFieldSettingsOverview settings, List<GridColumnHeaderModel> headers)
        {
            CreateHeaderIfNeeded(settings.ReceiverId, ReceiverFieldNames.ReceiverId, headers);
            CreateHeaderIfNeeded(settings.ReceiverName, ReceiverFieldNames.ReceiverName, headers);
            CreateHeaderIfNeeded(settings.ReceiverEmail, ReceiverFieldNames.ReceiverEmail, headers);
            CreateHeaderIfNeeded(settings.ReceiverPhone, ReceiverFieldNames.ReceiverPhone, headers);
            CreateHeaderIfNeeded(settings.ReceiverLocation, ReceiverFieldNames.ReceiverLocation, headers);
            CreateHeaderIfNeeded(settings.MarkOfGoods, ReceiverFieldNames.MarkOfGoods, headers);
        }

        private static void CreateSupplierHeaders(SupplierFieldSettingsOverview settings, List<GridColumnHeaderModel> headers)
        {
            CreateHeaderIfNeeded(settings.SupplierOrderNumber, SupplierFieldNames.SupplierOrderNumber, headers);
            CreateHeaderIfNeeded(settings.SupplierOrderDate, SupplierFieldNames.SupplierOrderDate, headers);
            CreateHeaderIfNeeded(settings.SupplierOrderInfo, SupplierFieldNames.SupplierOrderInfo, headers);
        }

        private static void CreateUserHeaders(UserFieldSettingsOverview settings, List<GridColumnHeaderModel> headers)
        {
            CreateHeaderIfNeeded(settings.UserId, UserFieldNames.UserId, headers);
            CreateHeaderIfNeeded(settings.UserFirstName, UserFieldNames.UserFirstName, headers);
            CreateHeaderIfNeeded(settings.UserLastName, UserFieldNames.UserLastName, headers);
        }

        private static void CreateHeaderIfNeeded(
            FieldOverviewSetting setting,
            string fieldName,
            List<GridColumnHeaderModel> headers)
        {
            if (!setting.Show)
            {
                return;
            }

            var header = new GridColumnHeaderModel(fieldName, setting.Caption);
            headers.Add(header);
        }

        #endregion

        #region Create values

        private static OrderOverviewModel CreateFullValues(FullFieldSettingsOverview settings, FullOrderOverview order)
        {
            var values = new List<NewGridRowCellValueModel>();

            CreateDeliveryValues(settings.Delivery, order.Delivery, values);
            CreateGeneralValues(settings.General, order.General, values);
            CreateLogValues(settings.Log, order.Log, values);
            CreateOrdererValues(settings.Orderer, order.Orderer, values);
            CreateOrderValues(settings.Order, order.Order, values);
            CreateOtherValues(settings.Other, order.Other, values);
            CreateProgramValues(settings.Program, order.Program, values);
            CreateReceiverValues(settings.Receiver, order.Receiver, values);
            CreateSupplierValues(settings.Supplier, order.Supplier, values);
            CreateUserValues(settings.User, order.User, values);

            return new OrderOverviewModel(order.Id, values);
        }

        private static void CreateDeliveryValues(
            DeliveryFieldSettingsOverview settings,
            DeliveryOverview fields,
            List<NewGridRowCellValueModel> values)
        {
            CreateValueIfNeeded(settings.DeliveryDate, DeliveryFieldNames.DeliveryDate, fields.DeliveryDate, values);
            CreateValueIfNeeded(settings.InstallDate, DeliveryFieldNames.InstallDate, fields.InstallDate, values);
            CreateValueIfNeeded(settings.DeliveryDepartment, DeliveryFieldNames.DeliveryDepartment, fields.DeliveryDepartment, values);
            CreateValueIfNeeded(settings.DeliveryOu, DeliveryFieldNames.DeliveryOu, fields.DeliveryOu, values);
            CreateValueIfNeeded(settings.DeliveryAddress, DeliveryFieldNames.DeliveryAddress, fields.DeliveryAddress, values);
            CreateValueIfNeeded(settings.DeliveryPostalCode, DeliveryFieldNames.DeliveryPostalCode, fields.DeliveryPostalCode, values);
            CreateValueIfNeeded(settings.DeliveryPostalAddress, DeliveryFieldNames.DeliveryPostalAddress, fields.DeliveryPostalAddress, values);
            CreateValueIfNeeded(settings.DeliveryLocation, DeliveryFieldNames.DeliveryLocation, fields.DeliveryLocation, values);
            CreateValueIfNeeded(settings.DeliveryInfo1, DeliveryFieldNames.DeliveryInfo1, fields.DeliveryInfo1, values);
            CreateValueIfNeeded(settings.DeliveryInfo2, DeliveryFieldNames.DeliveryInfo2, fields.DeliveryInfo2, values);
            CreateValueIfNeeded(settings.DeliveryInfo3, DeliveryFieldNames.DeliveryInfo3, fields.DeliveryInfo3, values);
            CreateValueIfNeeded(settings.DeliveryOuId, DeliveryFieldNames.DeliveryOuId, fields.DeliveryOuId, values);
        }

        private static void CreateGeneralValues(
            GeneralFieldSettingsOverview settings,
            GeneralOverview fields,
            List<NewGridRowCellValueModel> values)
        {
            CreateValueIfNeeded(settings.OrderNumber, GeneralFieldNames.OrderNumber, fields.OrderNumber, values);   
            CreateValueIfNeeded(settings.Customer, GeneralFieldNames.Customer, fields.Customer, values);   
            CreateValueIfNeeded(settings.Administrator, GeneralFieldNames.Administrator, fields.Administrator, values);   
            CreateValueIfNeeded(settings.Domain, GeneralFieldNames.Domain, fields.Domain, values);
            CreateValueIfNeeded(settings.OrderDate, GeneralFieldNames.OrderDate, fields.OrderDate, values);   
        }

        private static void CreateLogValues(
            LogFieldSettingsOverview settings,
            LogOverview fields,
            List<NewGridRowCellValueModel> values)
        {
            CreateValueIfNeeded(settings.Log, LogFieldNames.Log, fields.Logs, values);
        }

        private static void CreateOrdererValues(
            OrdererFieldSettingsOverview settings,
            OrdererOverview fields,
            List<NewGridRowCellValueModel> values)
        {
            CreateValueIfNeeded(settings.OrdererId, OrdererFieldNames.OrdererId, fields.OrdererId, values);
            CreateValueIfNeeded(settings.OrdererName, OrdererFieldNames.OrdererName, fields.OrdererName, values);
            CreateValueIfNeeded(settings.OrdererLocation, OrdererFieldNames.OrdererLocation, fields.OrdererLocation, values);
            CreateValueIfNeeded(settings.OrdererEmail, OrdererFieldNames.OrdererEmail, fields.OrdererEmail, values);
            CreateValueIfNeeded(settings.OrdererPhone, OrdererFieldNames.OrdererPhone, fields.OrdererPhone, values);
            CreateValueIfNeeded(settings.OrdererCode, OrdererFieldNames.OrdererCode, fields.OrdererCode, values);
            CreateValueIfNeeded(settings.Department, OrdererFieldNames.Department, fields.Department, values);
            CreateValueIfNeeded(settings.Unit, OrdererFieldNames.Unit, fields.Unit, values);
            CreateValueIfNeeded(settings.OrdererAddress, OrdererFieldNames.OrdererAddress, fields.OrdererAddress, values);
            CreateValueIfNeeded(settings.OrdererInvoiceAddress, OrdererFieldNames.OrdererInvoiceAddress, fields.OrdererInvoiceAddress, values);
            CreateValueIfNeeded(settings.OrdererReferenceNumber, OrdererFieldNames.OrdererReferenceNumber, fields.OrdererReferenceNumber, values);
            CreateValueIfNeeded(settings.AccountingDimension1, OrdererFieldNames.AccountingDimension1, fields.AccountingDimension1, values);
            CreateValueIfNeeded(settings.AccountingDimension2, OrdererFieldNames.AccountingDimension2, fields.AccountingDimension2, values);
            CreateValueIfNeeded(settings.AccountingDimension3, OrdererFieldNames.AccountingDimension3, fields.AccountingDimension3, values);
            CreateValueIfNeeded(settings.AccountingDimension4, OrdererFieldNames.AccountingDimension4, fields.AccountingDimension4, values);
            CreateValueIfNeeded(settings.AccountingDimension5, OrdererFieldNames.AccountingDimension5, fields.AccountingDimension5, values);
        }

        private static void CreateOrderValues(
            OrderFieldSettingsOverview settings,
            OrderOverview fields,
            List<NewGridRowCellValueModel> values)
        {
            CreateValueIfNeeded(settings.Property, OrderFieldNames.Property, fields.Property, values);
            CreateValueIfNeeded(settings.OrderRow1, OrderFieldNames.OrderRow1, fields.OrderRow1, values);
            CreateValueIfNeeded(settings.OrderRow2, OrderFieldNames.OrderRow2, fields.OrderRow2, values);
            CreateValueIfNeeded(settings.OrderRow3, OrderFieldNames.OrderRow3, fields.OrderRow3, values);
            CreateValueIfNeeded(settings.OrderRow4, OrderFieldNames.OrderRow4, fields.OrderRow4, values);
            CreateValueIfNeeded(settings.OrderRow5, OrderFieldNames.OrderRow5, fields.OrderRow5, values);
            CreateValueIfNeeded(settings.OrderRow6, OrderFieldNames.OrderRow6, fields.OrderRow6, values);
            CreateValueIfNeeded(settings.OrderRow7, OrderFieldNames.OrderRow7, fields.OrderRow7, values);
            CreateValueIfNeeded(settings.OrderRow8, OrderFieldNames.OrderRow8, fields.OrderRow8, values);
            CreateValueIfNeeded(settings.Configuration, OrderFieldNames.Configuration, fields.Configuration, values);
            CreateValueIfNeeded(settings.OrderInfo, OrderFieldNames.OrderInfo, fields.OrderInfo, values);
            CreateValueIfNeeded(settings.OrderInfo2, OrderFieldNames.OrderInfo2, fields.OrderInfo2, values);
        }

        private static void CreateOtherValues(
            OtherFieldSettingsOverview settings,
            OtherOverview fields,
            List<NewGridRowCellValueModel> values)
        {
            CreateValueIfNeeded(settings.FileName, OtherFieldNames.FileName, fields.FileName, values);
            CreateValueIfNeeded(settings.CaseNumber, OtherFieldNames.CaseNumber, fields.CaseNumber, values);
            CreateValueIfNeeded(settings.Info, OtherFieldNames.Info, fields.Info, values);
            CreateValueIfNeeded(settings.Status, OtherFieldNames.Status, fields.Status, values);
        }

        private static void CreateProgramValues(
            ProgramFieldSettingsOverview settings,
            ProgramOverview fields,
            List<NewGridRowCellValueModel> values)
        {
            CreateValueIfNeeded(settings.Program, ProgramFieldNames.Program, fields.Programs, values);
        }

        private static void CreateReceiverValues(
            ReceiverFieldSettingsOverview settings,
            ReceiverOverview fields,
            List<NewGridRowCellValueModel> values)
        {
            CreateValueIfNeeded(settings.ReceiverId, ReceiverFieldNames.ReceiverId, fields.ReceiverId, values);
            CreateValueIfNeeded(settings.ReceiverName, ReceiverFieldNames.ReceiverName, fields.ReceiverName, values);
            CreateValueIfNeeded(settings.ReceiverEmail, ReceiverFieldNames.ReceiverEmail, fields.ReceiverEmail, values);
            CreateValueIfNeeded(settings.ReceiverPhone, ReceiverFieldNames.ReceiverPhone, fields.ReceiverPhone, values);
            CreateValueIfNeeded(settings.ReceiverLocation, ReceiverFieldNames.ReceiverLocation, fields.ReceiverLocation, values);
            CreateValueIfNeeded(settings.MarkOfGoods, ReceiverFieldNames.MarkOfGoods, fields.MarkOfGoods, values);
        }

        private static void CreateSupplierValues(
            SupplierFieldSettingsOverview settings,
            SupplierOverview fields,
            List<NewGridRowCellValueModel> values)
        {
            CreateValueIfNeeded(settings.SupplierOrderNumber, SupplierFieldNames.SupplierOrderNumber, fields.SupplierOrderNumber, values);
            CreateValueIfNeeded(settings.SupplierOrderDate, SupplierFieldNames.SupplierOrderDate, fields.SupplierOrderDate, values);
            CreateValueIfNeeded(settings.SupplierOrderInfo, SupplierFieldNames.SupplierOrderInfo, fields.SupplierOrderInfo, values);
        }

        private static void CreateUserValues(
            UserFieldSettingsOverview settings,
            UserOverview fields,
            List<NewGridRowCellValueModel> values)
        {
            CreateValueIfNeeded(settings.UserId, UserFieldNames.UserId, fields.UserId, values);
            CreateValueIfNeeded(settings.UserFirstName, UserFieldNames.UserFirstName, fields.UserFirstName, values);
            CreateValueIfNeeded(settings.UserLastName, UserFieldNames.UserLastName, fields.UserLastName, values);
        }

        private static void CreateValueIfNeeded(
            FieldOverviewSetting setting,
            string fieldName,
            DateTime? value,
            List<NewGridRowCellValueModel> values)
        {
            var displayValue = new DateTimeDisplayValue(value);
            CreateValueIfNeeded(setting, fieldName, displayValue, values);
        }

        private static void CreateValueIfNeeded(
            FieldOverviewSetting setting,
            string fieldName,
            string value,
            List<NewGridRowCellValueModel> values)
        {
            var displayValue = new StringDisplayValue(value);
            CreateValueIfNeeded(setting, fieldName, displayValue, values);
        }
        
        private static void CreateValueIfNeeded(
            FieldOverviewSetting setting,
            string fieldName,
            decimal? value,
            List<NewGridRowCellValueModel> values)
        {
            var displayValue = new DecimalDisplayValue(value);
            CreateValueIfNeeded(setting, fieldName, displayValue, values);
        }

        private static void CreateValueIfNeeded(
            FieldOverviewSetting setting,
            string fieldName,
            string[] value,
            List<NewGridRowCellValueModel> values)
        {
            var displayValue = new HtmlStringsDisplayValue(value);
            CreateValueIfNeeded(setting, fieldName, displayValue, values);
        }

        private static void CreateValueIfNeeded(
            FieldOverviewSetting setting,
            string fieldName,
            DisplayValue value,
            List<NewGridRowCellValueModel> values)
        {
            if (!setting.Show)
            {
                return;
            }

            var fieldValue = new NewGridRowCellValueModel(fieldName, value);
            values.Add(fieldValue);
        }

        #endregion
    }
}