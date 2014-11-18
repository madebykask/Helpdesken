namespace DH.Helpdesk.Web.Areas.Orders.Infrastructure.ModelFactories.Concrete
{
    using System.Collections.Generic;

    using DH.Helpdesk.BusinessData.Enums.Orders.FieldNames;
    using DH.Helpdesk.BusinessData.Models.Orders.Index;
    using DH.Helpdesk.BusinessData.Models.Orders.Index.FieldSettingsOverview;
    using DH.Helpdesk.BusinessData.Models.Shared.Input;
    using DH.Helpdesk.BusinessData.Models.Shared.Output;
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

            return new OrdersIndexModel(orderTypes, administrators, statuses);
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

            return null;
        }
       
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
    }
}