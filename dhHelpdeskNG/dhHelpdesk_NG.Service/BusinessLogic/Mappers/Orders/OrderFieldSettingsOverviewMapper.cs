namespace DH.Helpdesk.Services.BusinessLogic.Mappers.Orders
{
    using System.Linq;

    using DH.Helpdesk.BusinessData.Enums.Orders.Fields;
    using DH.Helpdesk.BusinessData.Models.Orders.Index.FieldSettingsOverview;
    using DH.Helpdesk.BusinessData.Models.Shared.Output;
    using DH.Helpdesk.Common.Collections;
    using DH.Helpdesk.Domain;
    using DH.Helpdesk.Services.BusinessLogic.Mappers.Orders.Data;

    public static class OrderFieldSettingsOverviewMapper
    {
        public static FullFieldSettingsOverview MapToOrdersFieldSettingsOverview(this IQueryable<OrderFieldSettings> query)
        {
            var entities = query.Select(f => new OrdersFieldSettingsOverviewMapData
                                            {
                                               FieldName = f.OrderField,
                                               Caption = f.Label,
                                               Show = f.Show,
                                               ShowInList = f.ShowInList
                                            })                                            
                                            .ToList();

            var fieldSettings = new NamedObjectCollection<OrdersFieldSettingsOverviewMapData>(entities);
            return CreateFieldSettingsOverview(fieldSettings);
        }

        #region create order field settings overview

        private static FullFieldSettingsOverview CreateFieldSettingsOverview(NamedObjectCollection<OrdersFieldSettingsOverviewMapData> fieldSettings)
        {
            var delivery = CreateDeliveryFieldSettingsOverview(fieldSettings);
            var general = CreateGeneralFieldSettingsOverview(fieldSettings);
            var log = CreateLogFieldSettingsOverview(fieldSettings);
            var orderer = CreateOrdererFieldSettingsOverview(fieldSettings);
            var order = CreateOrderFieldSettingsOverview(fieldSettings);
            var other = CreateOtherFieldSettingsOverview(fieldSettings);
            var program = CreateProgramFieldSettingsOverview(fieldSettings);
            var receiver = CreateReceiverFieldSettingsOverview(fieldSettings);
            var supplier = CreateSupplierFieldSettingsOverview(fieldSettings);
            var user = CreateUserFieldSettingsOverview(fieldSettings);

            return new FullFieldSettingsOverview(
                                                delivery,
                                                general,
                                                log,
                                                orderer,
                                                order,
                                                other,
                                                program,
                                                receiver,
                                                supplier,
                                                user);
        }

        private static DeliveryFieldSettingsOverview CreateDeliveryFieldSettingsOverview(NamedObjectCollection<OrdersFieldSettingsOverviewMapData> fieldSettings)
        {
            var deliveryDate = CreateFieldSetting(fieldSettings.FindByName(DeliveryFields.DeliveryDate));
            var installDate = CreateFieldSetting(fieldSettings.FindByName(DeliveryFields.InstallDate));
            var deliveryDepartment = CreateFieldSetting(fieldSettings.FindByName(DeliveryFields.DeliveryDepartment));
            var deliveryOu = CreateFieldSetting(fieldSettings.FindByName(DeliveryFields.DeliveryOu));
            var deliveryAddress = CreateFieldSetting(fieldSettings.FindByName(DeliveryFields.DeliveryAddress));
            var deliveryPostalCode = CreateFieldSetting(fieldSettings.FindByName(DeliveryFields.DeliveryPostalCode));
            var deliveryPostalAddress = CreateFieldSetting(fieldSettings.FindByName(DeliveryFields.DeliveryPostalAddress));
            var deliveryLocation = CreateFieldSetting(fieldSettings.FindByName(DeliveryFields.DeliveryLocation));
            var deliveryInfo1 = CreateFieldSetting(fieldSettings.FindByName(DeliveryFields.DeliveryInfo1));
            var deliveryInfo2 = CreateFieldSetting(fieldSettings.FindByName(DeliveryFields.DeliveryInfo2));
            var deliveryInfo3 = CreateFieldSetting(fieldSettings.FindByName(DeliveryFields.DeliveryInfo3));
            var deliveryOuId = CreateFieldSetting(fieldSettings.FindByName(DeliveryFields.DeliveryOuId));

            return new DeliveryFieldSettingsOverview(
                                                deliveryDate,
                                                installDate,
                                                deliveryDepartment,
                                                deliveryOu,
                                                deliveryAddress,
                                                deliveryPostalCode,
                                                deliveryPostalAddress,
                                                deliveryLocation,
                                                deliveryInfo1,
                                                deliveryInfo2,
                                                deliveryInfo3,
                                                deliveryOuId);
        }

        private static GeneralFieldSettingsOverview CreateGeneralFieldSettingsOverview(NamedObjectCollection<OrdersFieldSettingsOverviewMapData> fieldSettings)
        {
            var orderNumber = CreateFieldSetting(fieldSettings.FindByName(GeneralFields.OrderNumber));
            var customer = CreateFieldSetting(fieldSettings.FindByName(GeneralFields.Customer));
            var administrator = CreateFieldSetting(fieldSettings.FindByName(GeneralFields.Administrator));
            var domain = CreateFieldSetting(fieldSettings.FindByName(GeneralFields.Domain));
            var orderDate = CreateFieldSetting(fieldSettings.FindByName(GeneralFields.OrderDate));

            return new GeneralFieldSettingsOverview(
                                                orderNumber,
                                                customer,
                                                administrator,
                                                domain,
                                                orderDate);
        }

        private static LogFieldSettingsOverview CreateLogFieldSettingsOverview(NamedObjectCollection<OrdersFieldSettingsOverviewMapData> fieldSettings)
        {
            var log = CreateFieldSetting(fieldSettings.FindByName(LogFields.Log));

            return new LogFieldSettingsOverview(log);
        }

        private static OrdererFieldSettingsOverview CreateOrdererFieldSettingsOverview(NamedObjectCollection<OrdersFieldSettingsOverviewMapData> fieldSettings)
        {
            var ordererId = CreateFieldSetting(fieldSettings.FindByName(OrdererFields.OrdererId));
            var ordererName = CreateFieldSetting(fieldSettings.FindByName(OrdererFields.OrdererName));
            var ordererLocation = CreateFieldSetting(fieldSettings.FindByName(OrdererFields.OrdererLocation));
            var ordererEmail = CreateFieldSetting(fieldSettings.FindByName(OrdererFields.OrdererEmail));
            var ordererPhone = CreateFieldSetting(fieldSettings.FindByName(OrdererFields.OrdererPhone));
            var ordererCode = CreateFieldSetting(fieldSettings.FindByName(OrdererFields.OrdererCode));
            var department = CreateFieldSetting(fieldSettings.FindByName(OrdererFields.Department));
            var unit = CreateFieldSetting(fieldSettings.FindByName(OrdererFields.Unit));
            var ordererAddress = CreateFieldSetting(fieldSettings.FindByName(OrdererFields.OrdererAddress));
            var ordererInvoiceAddress = CreateFieldSetting(fieldSettings.FindByName(OrdererFields.OrdererInvoiceAddress));
            var ordererReferenceNumber = CreateFieldSetting(fieldSettings.FindByName(OrdererFields.OrdererReferenceNumber));
            var accountingDimension1 = CreateFieldSetting(fieldSettings.FindByName(OrdererFields.AccountingDimension1));
            var accountingDimension2 = CreateFieldSetting(fieldSettings.FindByName(OrdererFields.AccountingDimension2));
            var accountingDimension3 = CreateFieldSetting(fieldSettings.FindByName(OrdererFields.AccountingDimension3));
            var accountingDimension4 = CreateFieldSetting(fieldSettings.FindByName(OrdererFields.AccountingDimension4));
            var accountingDimension5 = CreateFieldSetting(fieldSettings.FindByName(OrdererFields.AccountingDimension5));

            return new OrdererFieldSettingsOverview(
                                                ordererId,
                                                ordererName,
                                                ordererLocation,
                                                ordererEmail,
                                                ordererPhone,
                                                ordererCode,
                                                department,
                                                unit,
                                                ordererAddress,
                                                ordererInvoiceAddress,
                                                ordererReferenceNumber,
                                                accountingDimension1,
                                                accountingDimension2,
                                                accountingDimension3,
                                                accountingDimension4,
                                                accountingDimension5);
        }

        private static OrderFieldSettingsOverview CreateOrderFieldSettingsOverview(NamedObjectCollection<OrdersFieldSettingsOverviewMapData> fieldSettings)
        {
            var property = CreateFieldSetting(fieldSettings.FindByName(OrderFields.Property));
            var orderRow1 = CreateFieldSetting(fieldSettings.FindByName(OrderFields.OrderRow1));
            var orderRow2 = CreateFieldSetting(fieldSettings.FindByName(OrderFields.OrderRow2));
            var orderRow3 = CreateFieldSetting(fieldSettings.FindByName(OrderFields.OrderRow3));
            var orderRow4 = CreateFieldSetting(fieldSettings.FindByName(OrderFields.OrderRow4));
            var orderRow5 = CreateFieldSetting(fieldSettings.FindByName(OrderFields.OrderRow5));
            var orderRow6 = CreateFieldSetting(fieldSettings.FindByName(OrderFields.OrderRow6));
            var orderRow7 = CreateFieldSetting(fieldSettings.FindByName(OrderFields.OrderRow7));
            var orderRow8 = CreateFieldSetting(fieldSettings.FindByName(OrderFields.OrderRow8));
            var configuration = CreateFieldSetting(fieldSettings.FindByName(OrderFields.Configuration));
            var orderInfo = CreateFieldSetting(fieldSettings.FindByName(OrderFields.OrderInfo));
            var orderInfo2 = CreateFieldSetting(fieldSettings.FindByName(OrderFields.OrderInfo2));

            return new OrderFieldSettingsOverview(
                                                property,
                                                orderRow1,
                                                orderRow2,
                                                orderRow3,
                                                orderRow4,
                                                orderRow5,
                                                orderRow6,
                                                orderRow7,
                                                orderRow8,
                                                configuration,
                                                orderInfo,
                                                orderInfo2);
        }

        private static OtherFieldSettingsOverview CreateOtherFieldSettingsOverview(NamedObjectCollection<OrdersFieldSettingsOverviewMapData> fieldSettings)
        {
            var fileName = CreateFieldSetting(fieldSettings.FindByName(OtherFields.FileName));
            var caseNumber = CreateFieldSetting(fieldSettings.FindByName(OtherFields.CaseNumber));
            var info = CreateFieldSetting(fieldSettings.FindByName(OtherFields.Info));
            var status = CreateFieldSetting(fieldSettings.FindByName(OtherFields.Status));

            return new OtherFieldSettingsOverview(
                                                fileName,
                                                caseNumber,
                                                info,
                                                status);
        }

        private static ProgramFieldSettingsOverview CreateProgramFieldSettingsOverview(NamedObjectCollection<OrdersFieldSettingsOverviewMapData> fieldSettings)
        {
            var program = CreateFieldSetting(fieldSettings.FindByName(ProgramFields.Program));

            return new ProgramFieldSettingsOverview(program);
        }

        private static ReceiverFieldSettingsOverview CreateReceiverFieldSettingsOverview(NamedObjectCollection<OrdersFieldSettingsOverviewMapData> fieldSettings)
        {
            var receiverId = CreateFieldSetting(fieldSettings.FindByName(ReceiverFields.ReceiverId));
            var receiverName = CreateFieldSetting(fieldSettings.FindByName(ReceiverFields.ReceiverName));
            var receiverEmail = CreateFieldSetting(fieldSettings.FindByName(ReceiverFields.ReceiverEmail));
            var receiverPhone = CreateFieldSetting(fieldSettings.FindByName(ReceiverFields.ReceiverPhone));
            var receiverLocation = CreateFieldSetting(fieldSettings.FindByName(ReceiverFields.ReceiverLocation));
            var markOfGoods = CreateFieldSetting(fieldSettings.FindByName(ReceiverFields.MarkOfGoods));

            return new ReceiverFieldSettingsOverview(
                                                receiverId,
                                                receiverName,
                                                receiverEmail,
                                                receiverPhone,
                                                receiverLocation,
                                                markOfGoods);
        }

        private static SupplierFieldSettingsOverview CreateSupplierFieldSettingsOverview(NamedObjectCollection<OrdersFieldSettingsOverviewMapData> fieldSettings)
        {
            var supplierOrderNumber = CreateFieldSetting(fieldSettings.FindByName(SupplierFields.SupplierOrderNumber));
            var supplierOrderDate = CreateFieldSetting(fieldSettings.FindByName(SupplierFields.SupplierOrderDate));
            var supplierOrderInfo = CreateFieldSetting(fieldSettings.FindByName(SupplierFields.SupplierOrderInfo));

            return new SupplierFieldSettingsOverview(
                                                supplierOrderNumber,
                                                supplierOrderDate,
                                                supplierOrderInfo);
        }

        private static UserFieldSettingsOverview CreateUserFieldSettingsOverview(NamedObjectCollection<OrdersFieldSettingsOverviewMapData> fieldSettings)
        {
            var userId = CreateFieldSetting(fieldSettings.FindByName(UserFields.UserId));
            var userFirstName = CreateFieldSetting(fieldSettings.FindByName(UserFields.UserFirstName));
            var userLastName = CreateFieldSetting(fieldSettings.FindByName(UserFields.UserLastName));

            return new UserFieldSettingsOverview(
                                                userId,
                                                userFirstName,
                                                userLastName);
        }

        private static FieldOverviewSetting CreateFieldSetting(OrdersFieldSettingsOverviewMapData fieldSetting)
        {
            return new FieldOverviewSetting(fieldSetting.IsShowInList(), fieldSetting.GetFieldCaption());
        }

        #endregion
    }
}