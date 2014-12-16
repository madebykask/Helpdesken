namespace DH.Helpdesk.Web.Areas.Orders.Infrastructure.ModelFactories.Concrete
{
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;

    using DH.Helpdesk.BusinessData.Enums.Orders;
    using DH.Helpdesk.BusinessData.Models.Orders.Order;
    using DH.Helpdesk.BusinessData.Models.Orders.Order.OrderEditFields;
    using DH.Helpdesk.BusinessData.Models.Orders.Order.OrderEditSettings;
    using DH.Helpdesk.Web.Areas.Orders.Models.Order.FieldModels;
    using DH.Helpdesk.Web.Areas.Orders.Models.Order.OrderEdit;

    public class OrderModelFactory : IOrderModelFactory
    {
        private readonly IConfigurableFieldModelFactory configurableFieldModelFactory;

        private readonly IHistoryModelFactory historyModelFactory;

        public OrderModelFactory(
                IConfigurableFieldModelFactory configurableFieldModelFactory, 
                IHistoryModelFactory historyModelFactory)
        {
            this.configurableFieldModelFactory = configurableFieldModelFactory;
            this.historyModelFactory = historyModelFactory;
        }

        public FullOrderEditModel Create(FindOrderResponse response, int customerId)
        {
            var orderId = response.EditData.Order.Id;
            var textOrderId = orderId.ToString(CultureInfo.InvariantCulture);
            var history = this.historyModelFactory.Create(response);

            return new FullOrderEditModel(
                this.CreateDeliveryEditModel(response.EditSettings.Delivery, response.EditData.Order.Delivery, response.EditOptions),
                this.CreateGeneralEditModel(response.EditSettings.General, response.EditData.Order.General, response.EditOptions),
                this.CreateLogEditModel(response.EditSettings.Log, response.EditData.Order.Log, response.EditOptions, orderId),
                this.CreateOrdererEditModel(response.EditSettings.Orderer, response.EditData.Order.Orderer, response.EditOptions),
                this.CreateOrderEditModel(response.EditSettings.Order, response.EditData.Order.Order, response.EditOptions),
                this.CreateOtherEditModel(response.EditSettings.Other, response.EditData.Order.Other, response.EditOptions, textOrderId),
                this.CreateProgramEditModel(response.EditSettings.Program, response.EditData.Order.Program),
                this.CreateReceiverEditModel(response.EditSettings.Receiver, response.EditData.Order.Receiver),
                this.CreateSupplierEditModel(response.EditSettings.Supplier, response.EditData.Order.Supplier),
                this.CreateUserEditModel(response.EditSettings.User, response.EditData.Order.User),
                textOrderId,
                customerId,
                response.EditData.Order.OrderTypeId,
                false,
                history);
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
            var orderNumber = this.configurableFieldModelFactory.CreateIntegerField(settings.OrderNumber, fields.OrderNumber);
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
                                OrderEditOptions options,
                                int orderId)
        {
            var log = this.configurableFieldModelFactory.CreateLogs(
                                                    settings.Log,
                                                    orderId,
                                                    Subtopic.Log, 
                                                    fields.Logs,
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
            var property = this.configurableFieldModelFactory.CreateSelectListField(settings.Property, options.Properties, fields.PropertyId);
            var orderRow1 = this.configurableFieldModelFactory.CreateStringField(settings.OrderRow1, fields.OrderRow1);
            var orderRow2 = this.configurableFieldModelFactory.CreateStringField(settings.OrderRow2, fields.OrderRow2);
            var orderRow3 = this.configurableFieldModelFactory.CreateStringField(settings.OrderRow3, fields.OrderRow3);
            var orderRow4 = this.configurableFieldModelFactory.CreateStringField(settings.OrderRow4, fields.OrderRow4);
            var orderRow5 = this.configurableFieldModelFactory.CreateStringField(settings.OrderRow5, fields.OrderRow5);
            var orderRow6 = this.configurableFieldModelFactory.CreateStringField(settings.OrderRow6, fields.OrderRow6);
            var orderRow7 = this.configurableFieldModelFactory.CreateStringField(settings.OrderRow7, fields.OrderRow7);
            var orderRow8 = this.configurableFieldModelFactory.CreateStringField(settings.OrderRow8, fields.OrderRow8);
            var configuration = this.configurableFieldModelFactory.CreateStringField(settings.Configuration, fields.Configuration);
            var orderInfo = this.configurableFieldModelFactory.CreateStringField(settings.OrderInfo, fields.OrderInfo);
            var orderInfo2 = this.configurableFieldModelFactory.CreateIntegerField(settings.OrderInfo2, fields.OrderInfo2);

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
            var files = new List<string> { fields.FileName };

            var fileName = this.configurableFieldModelFactory.CreateAttachedFiles(settings.FileName, orderId, Subtopic.FileName, files);
            var caseNumber = this.configurableFieldModelFactory.CreateNullableDecimalField(settings.CaseNumber, fields.CaseNumber);
            var info = this.configurableFieldModelFactory.CreateStringField(settings.Info, fields.Info);
            var status = this.configurableFieldModelFactory.CreateSelectListField(settings.Status, options.Statuses, fields.StatusId);

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
            var program = this.configurableFieldModelFactory.CreatePrograms(settings.Program, fields.Programs.Select(p => new ProgramModel(p.Id, p.Name)).ToList());

            return new ProgramEditModel(program);
        }

        private ReceiverEditModel CreateReceiverEditModel(
                                ReceiverEditSettings settings,
                                ReceiverEditFields fields)
        {
            var receiverId = this.configurableFieldModelFactory.CreateStringField(settings.ReceiverId, fields.ReceiverId);
            var receiverName = this.configurableFieldModelFactory.CreateStringField(settings.ReceiverName, fields.ReceiverName);
            var receiverEmail = this.configurableFieldModelFactory.CreateStringField(settings.ReceiverEmail, fields.ReceiverEmail);
            var receiverPhone = this.configurableFieldModelFactory.CreateStringField(settings.ReceiverPhone, fields.ReceiverPhone);
            var receiverLocation = this.configurableFieldModelFactory.CreateStringField(settings.ReceiverLocation, fields.ReceiverLocation);
            var markOfGoods = this.configurableFieldModelFactory.CreateStringField(settings.MarkOfGoods, fields.MarkOfGoods);

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
            var supplierOrderNumber = this.configurableFieldModelFactory.CreateStringField(settings.SupplierOrderNumber, fields.SupplierOrderNumber);
            var supplierOrderDate = this.configurableFieldModelFactory.CreateNullableDateTimeField(settings.SupplierOrderDate, fields.SupplierOrderDate);
            var supplierOrderInfo = this.configurableFieldModelFactory.CreateStringField(settings.SupplierOrderInfo, fields.SupplierOrderInfo);

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