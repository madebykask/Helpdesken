namespace DH.Helpdesk.Web.Areas.Orders.Infrastructure.ModelFactories.Concrete
{
    using System.Collections.Generic;
    using System.Globalization;

    using DH.Helpdesk.BusinessData.Enums.Orders;
    using DH.Helpdesk.BusinessData.Models.Orders.Order;
    using DH.Helpdesk.BusinessData.Models.Orders.Order.OrderEditFields;
    using DH.Helpdesk.BusinessData.Models.Orders.Order.OrderEditSettings;
    using DH.Helpdesk.Web.Areas.Orders.Models.Order.FieldModels;
    using DH.Helpdesk.Web.Areas.Orders.Models.Order.OrderEdit;

    public class OrderModelFactory : IOrderModelFactory
    {
        private readonly IConfigurableFieldModelFactory configurableFieldModelFactory;

        public OrderModelFactory(IConfigurableFieldModelFactory configurableFieldModelFactory)
        {
            this.configurableFieldModelFactory = configurableFieldModelFactory;
        }

        public FullOrderEditModel Create(FindOrderResponse response, int customerId)
        {
            var id = response.EditData.Order.Id.ToString(CultureInfo.InvariantCulture);
            return new FullOrderEditModel(
                this.CreateDeliveryEditModel(response.EditSettings.Delivery, response.EditData.Order.Delivery, response.EditOptions),
                this.CreateGeneralEditModel(response.EditSettings.General, response.EditData.Order.General, response.EditOptions),
                this.CreateLogEditModel(response.EditSettings.Log, response.EditData.Order.Log, response.EditOptions),
                this.CreateOrdererEditModel(response.EditSettings.Orderer, response.EditData.Order.Orderer, response.EditOptions),
                this.CreateOrderEditModel(response.EditSettings.Order, response.EditData.Order.Order, response.EditOptions),
                this.CreateOtherEditModel(response.EditSettings.Other, response.EditData.Order.Other, response.EditOptions, id),
                this.CreateProgramEditModel(response.EditSettings.Program, response.EditData.Order.Program),
                this.CreateReceiverEditModel(response.EditSettings.Receiver, response.EditData.Order.Receiver),
                this.CreateSupplierEditModel(response.EditSettings.Supplier, response.EditData.Order.Supplier),
                this.CreateUserEditModel(response.EditSettings.User, response.EditData.Order.User),
                id,
                customerId,
                response.EditData.Order.OrderTypeId,
                false);
        }

        private DeliveryEditModel CreateDeliveryEditModel(                                
                                DeliveryEditSettings settings,
                                DeliveryEditFields fields,
                                OrderEditOptions options)
        {
            var deliveryDate = this.configurableFieldModelFactory.CreateNullableDateTimeField(settings.DeliveryDate, fields.DeliveryDate);
            var installDate = this.configurableFieldModelFactory.CreateNullableDateTimeField(settings.InstallDate, fields.InstallDate);
            var deliveryDepartment = this.configurableFieldModelFactory.CreateSelectListField(settings.DeliveryDepartment, options.DeliveryDepartment, fields.DeliveryDepartmentId);
            var deliveryOu = this.configurableFieldModelFactory.CreateStringField(settings.DeliveryOu, fields.DeliveryOu);
            var deliveryAddress = this.configurableFieldModelFactory.CreateStringField(settings.DeliveryAddress, fields.DeliveryAddress);
            var deliveryPostalCode = this.configurableFieldModelFactory.CreateStringField(settings.DeliveryPostalCode, fields.DeliveryPostalCode);
            var deliveryPostalAddress = this.configurableFieldModelFactory.CreateStringField(settings.DeliveryPostalAddress, fields.DeliveryPostalAddress);
            var deliveryLocation = this.configurableFieldModelFactory.CreateStringField(settings.DeliveryLocation, fields.DeliveryLocation);
            var deliveryInfo1 = this.configurableFieldModelFactory.CreateStringField(settings.DeliveryInfo1, fields.DeliveryInfo1);
            var deliveryInfo2 = this.configurableFieldModelFactory.CreateStringField(settings.DeliveryInfo2, fields.DeliveryInfo2);
            var deliveryInfo3 = this.configurableFieldModelFactory.CreateStringField(settings.DeliveryInfo3, fields.DeliveryInfo3);
            var deliveryOuId = this.configurableFieldModelFactory.CreateSelectListField(settings.DeliveryOuId, options.DeliveryOuId, fields.DeliveryOuIdId);

            return new DeliveryEditModel(
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

        private GeneralEditModel CreateGeneralEditModel(
                                GeneralEditSettings settings,
                                GeneralEditFields fields,
                                OrderEditOptions options)
        {
            var orderNumber = this.configurableFieldModelFactory.CreateIntegerField(settings.OrderNumber, int.Parse(fields.OrderNumber));
            var customer = this.configurableFieldModelFactory.CreateStringField(settings.Customer, fields.Customer);
            var administrator = this.configurableFieldModelFactory.CreateSelectListField(settings.Administrator, options.Administrators, fields.AdministratorId);
            var domain = this.configurableFieldModelFactory.CreateSelectListField(settings.Domain, options.Domains, fields.DomainId);
            var orderDate = this.configurableFieldModelFactory.CreateNullableDateTimeField(settings.OrderDate, fields.OrderDate);

            return new GeneralEditModel(
                            orderNumber,
                            customer,
                            administrator,
                            domain,
                            orderDate,
                            options.OrderTypeName);
        }

        private LogEditModel CreateLogEditModel(
                                LogEditSettings settings,
                                LogEditFields fields,
                                OrderEditOptions options)
        {
            var log = this.configurableFieldModelFactory.CreateLogs(
                                                    settings.Log,
                                                    options.EmailGroups,
                                                    options.WorkingGroupsWithEmails,
                                                    options.AdministratorsWithEmails);

            return new LogEditModel(log);
        }

        private OrdererEditModel CreateOrdererEditModel(
                                OrdererEditSettings settings,
                                OrdererEditFields fields,
                                OrderEditOptions options)
        {
            var ordererId = this.configurableFieldModelFactory.CreateStringField(settings.OrdererId, fields.OrdererId);
            var ordererName = this.configurableFieldModelFactory.CreateStringField(settings.OrdererName, fields.OrdererName);
            var ordererLocation = this.configurableFieldModelFactory.CreateStringField(settings.OrdererLocation, fields.OrdererLocation);
            var ordererEmail = this.configurableFieldModelFactory.CreateStringField(settings.OrdererEmail, fields.OrdererEmail);
            var ordererPhone = this.configurableFieldModelFactory.CreateStringField(settings.OrdererPhone, fields.OrdererPhone);
            var ordererCode = this.configurableFieldModelFactory.CreateStringField(settings.OrdererCode, fields.OrdererCode);
            var department = this.configurableFieldModelFactory.CreateSelectListField(settings.Department, options.Departments, fields.DepartmentId);
            var unit = this.configurableFieldModelFactory.CreateSelectListField(settings.Unit, options.Units, fields.UnitId);
            var ordererAddress = this.configurableFieldModelFactory.CreateStringField(settings.OrdererAddress, fields.OrdererAddress);
            var ordererInvoiceAddress = this.configurableFieldModelFactory.CreateStringField(settings.OrdererInvoiceAddress, fields.OrdererInvoiceAddress);
            var ordererReferenceNumber = this.configurableFieldModelFactory.CreateStringField(settings.OrdererReferenceNumber, fields.OrdererReferenceNumber);
            var accountingDimension1 = this.configurableFieldModelFactory.CreateStringField(settings.AccountingDimension1, fields.AccountingDimension1);
            var accountingDimension2 = this.configurableFieldModelFactory.CreateStringField(settings.AccountingDimension2, fields.AccountingDimension2);
            var accountingDimension3 = this.configurableFieldModelFactory.CreateStringField(settings.AccountingDimension3, fields.AccountingDimension3);
            var accountingDimension4 = this.configurableFieldModelFactory.CreateStringField(settings.AccountingDimension4, fields.AccountingDimension4);
            var accountingDimension5 = this.configurableFieldModelFactory.CreateStringField(settings.AccountingDimension5, fields.AccountingDimension5);

            return new OrdererEditModel(
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

        private OrderEditModel CreateOrderEditModel(
                                OrderEditSettings settings,
                                OrderEditFields fields,
                                OrderEditOptions options)
        {
            var property = this.configurableFieldModelFactory.CreateSelectListField(settings.Property, options.Properties, null);
            var orderRow1 = this.configurableFieldModelFactory.CreateStringField(settings.OrderRow1, null);
            var orderRow2 = this.configurableFieldModelFactory.CreateStringField(settings.OrderRow2, null);
            var orderRow3 = this.configurableFieldModelFactory.CreateStringField(settings.OrderRow3, null);
            var orderRow4 = this.configurableFieldModelFactory.CreateStringField(settings.OrderRow4, null);
            var orderRow5 = this.configurableFieldModelFactory.CreateStringField(settings.OrderRow5, null);
            var orderRow6 = this.configurableFieldModelFactory.CreateStringField(settings.OrderRow6, null);
            var orderRow7 = this.configurableFieldModelFactory.CreateStringField(settings.OrderRow7, null);
            var orderRow8 = this.configurableFieldModelFactory.CreateStringField(settings.OrderRow8, null);
            var configuration = this.configurableFieldModelFactory.CreateStringField(settings.Configuration, null);
            var orderInfo = this.configurableFieldModelFactory.CreateStringField(settings.OrderInfo, null);
            var orderInfo2 = this.configurableFieldModelFactory.CreateStringField(settings.OrderInfo2, null);

            return new OrderEditModel(
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

        private OtherEditModel CreateOtherEditModel(
                                OtherEditSettings settings,
                                OtherEditFields fields,
                                OrderEditOptions options,
                                string orderId)
        {
            var fileName = this.configurableFieldModelFactory.CreateAttachedFiles(settings.FileName, orderId, Subtopic.FileName, new List<string>(0));
            var caseNumber = this.configurableFieldModelFactory.CreateNullableDecimalField(settings.CaseNumber, null);
            var info = this.configurableFieldModelFactory.CreateStringField(settings.Info, null);
            var status = this.configurableFieldModelFactory.CreateSelectListField(settings.Status, options.Statuses, null);

            return new OtherEditModel(
                            fileName,
                            caseNumber,
                            info,
                            status);
        }

        private ProgramEditModel CreateProgramEditModel(
                                ProgramEditSettings settings,
                                ProgramEditFields fields)
        {
            var program = this.configurableFieldModelFactory.CreatePrograms(settings.Program, new List<ProgramModel>(0));

            return new ProgramEditModel(program);
        }

        private ReceiverEditModel CreateReceiverEditModel(
                                ReceiverEditSettings settings,
                                ReceiverEditFields fields)
        {
            var receiverId = this.configurableFieldModelFactory.CreateStringField(settings.ReceiverId, null);
            var receiverName = this.configurableFieldModelFactory.CreateStringField(settings.ReceiverName, null);
            var receiverEmail = this.configurableFieldModelFactory.CreateStringField(settings.ReceiverEmail, null);
            var receiverPhone = this.configurableFieldModelFactory.CreateStringField(settings.ReceiverPhone, null);
            var receiverLocation = this.configurableFieldModelFactory.CreateStringField(settings.ReceiverLocation, null);
            var markOfGoods = this.configurableFieldModelFactory.CreateStringField(settings.MarkOfGoods, null);

            return new ReceiverEditModel(
                            receiverId,
                            receiverName,
                            receiverEmail,
                            receiverPhone,
                            receiverLocation,
                            markOfGoods);
        }

        private SupplierEditModel CreateSupplierEditModel(
                                SupplierEditSettings settings,
                                SupplierEditFields fields)
        {
            var supplierOrderNumber = this.configurableFieldModelFactory.CreateStringField(settings.SupplierOrderNumber, null);
            var supplierOrderDate = this.configurableFieldModelFactory.CreateNullableDateTimeField(settings.SupplierOrderDate, null);
            var supplierOrderInfo = this.configurableFieldModelFactory.CreateStringField(settings.SupplierOrderInfo, null);

            return new SupplierEditModel(
                            supplierOrderNumber,
                            supplierOrderDate,
                            supplierOrderInfo);
        }

        private UserEditModel CreateUserEditModel(
                                UserEditSettings settings,
                                UserEditFields fields)
        {
            var userId = this.configurableFieldModelFactory.CreateStringField(settings.UserId, fields.UserId);
            var userFirstName = this.configurableFieldModelFactory.CreateStringField(settings.UserFirstName, fields.UserFirstName);
            var userLastName = this.configurableFieldModelFactory.CreateStringField(settings.UserLastName, fields.UserLastName);

            return new UserEditModel(
                            userId,
                            userFirstName,
                            userLastName);
        }
    }
}