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
            
        }

        private static void CreateLogHeaders(LogFieldSettingsOverview settings, List<GridColumnHeaderModel> headers)
        {
            
        }

        private static void CreateOrdererHeaders(OrdererFieldSettingsOverview settings, List<GridColumnHeaderModel> headers)
        {
            
        }

        private static void CreateOrderHeaders(OrderFieldSettingsOverview settings, List<GridColumnHeaderModel> headers)
        {
            
        }

        private static void CreateOtherHeaders(OtherFieldSettingsOverview settings, List<GridColumnHeaderModel> headers)
        {
            
        }

        private static void CreateProgramHeaders(ProgramFieldSettingsOverview settings, List<GridColumnHeaderModel> headers)
        {
            
        }

        private static void CreateReceiverHeaders(ReceiverFieldSettingsOverview settings, List<GridColumnHeaderModel> headers)
        {
            
        }

        private static void CreateSupplierHeaders(SupplierFieldSettingsOverview settings, List<GridColumnHeaderModel> headers)
        {
            
        }

        private static void CreateUserHeaders(UserFieldSettingsOverview settings, List<GridColumnHeaderModel> headers)
        {
            
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