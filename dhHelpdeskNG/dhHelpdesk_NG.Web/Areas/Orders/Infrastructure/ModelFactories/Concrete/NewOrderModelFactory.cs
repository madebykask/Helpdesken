namespace DH.Helpdesk.Web.Areas.Orders.Infrastructure.ModelFactories.Concrete
{
    using DH.Helpdesk.BusinessData.Models.Orders.Order;
    using DH.Helpdesk.BusinessData.Models.Orders.Order.OrderEditSettings;
    using DH.Helpdesk.Web.Areas.Orders.Models.Order.OrderEdit;

    public class NewOrderModelFactory : INewOrderModelFactory
    {
        private readonly IConfigurableFieldModelFactory configurableFieldModelFactory;

        public NewOrderModelFactory(IConfigurableFieldModelFactory configurableFieldModelFactory)
        {
            this.configurableFieldModelFactory = configurableFieldModelFactory;
        }

        public FullOrderEditModel Create(string temporatyId, NewOrderEditData data, int customerId, int? orderTypeId)
        {
            return new FullOrderEditModel(
                this.CreateDeliveryEditModel(data.EditSettings.Delivery, data.EditOptions),
                this.CreateGeneralEditModel(data.EditSettings.General, data.EditOptions),
                this.CreateLogEditModel(data.EditSettings.Log, data.EditOptions),
                this.CreateOrdererEditModel(data.EditSettings.Orderer, data.EditOptions),
                this.CreateOrderEditModel(data.EditSettings.Order, data.EditOptions),
                this.CreateOtherEditModel(data.EditSettings.Other, data.EditOptions),
                this.CreateProgramEditModel(data.EditSettings.Program, data.EditOptions),
                this.CreateReceiverEditModel(data.EditSettings.Receiver, data.EditOptions),
                this.CreateSupplierEditModel(data.EditSettings.Supplier, data.EditOptions),
                this.CreateUserEditModel(data.EditSettings.User, data.EditOptions),
                temporatyId,
                customerId,
                orderTypeId);
        }

        private DeliveryEditModel CreateDeliveryEditModel(
                                DeliveryEditSettings settings,
                                OrderEditOptions options)
        {
            var deliveryDate = this.configurableFieldModelFactory.CreateNullableDateTimeField(settings.DeliveryDate, null);
            var installDate = this.configurableFieldModelFactory.CreateNullableDateTimeField(settings.InstallDate, null);
            var deliveryDepartment = this.configurableFieldModelFactory.CreateSelectListField(settings.DeliveryDepartment, options.DeliveryDepartment, null);
            var deliveryOu = this.configurableFieldModelFactory.CreateStringField(settings.DeliveryOu, null);
            var deliveryAddress = this.configurableFieldModelFactory.CreateStringField(settings.DeliveryAddress, null);
            var deliveryPostalCode = this.configurableFieldModelFactory.CreateStringField(settings.DeliveryPostalCode, null);
            var deliveryPostalAddress = this.configurableFieldModelFactory.CreateStringField(settings.DeliveryPostalAddress, null);
            var deliveryLocation = this.configurableFieldModelFactory.CreateStringField(settings.DeliveryLocation, null);
            var deliveryInfo1 = this.configurableFieldModelFactory.CreateStringField(settings.DeliveryInfo1, null);
            var deliveryInfo2 = this.configurableFieldModelFactory.CreateStringField(settings.DeliveryInfo2, null);
            var deliveryInfo3 = this.configurableFieldModelFactory.CreateStringField(settings.DeliveryInfo3, null);
            var deliveryOuId = this.configurableFieldModelFactory.CreateSelectListField(settings.DeliveryOuId, options.DeliveryOuId, null);

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
                                OrderEditOptions options)
        {
            var orderNumber = this.configurableFieldModelFactory.CreateIntegerField(settings.OrderNumber, 0);
            var customer = this.configurableFieldModelFactory.CreateIntegerField(settings.Customer, 0);
            var administrator = this.configurableFieldModelFactory.CreateSelectListField(settings.Administrator, options.Administrators, null);
            var domain = this.configurableFieldModelFactory.CreateSelectListField(settings.Domain, options.Domains, null);
            var orderDate = this.configurableFieldModelFactory.CreateNullableDateTimeField(settings.OrderDate, null);

            return new GeneralEditModel(
                            orderNumber,
                            customer,
                            administrator,
                            domain,
                            orderDate);
        }

        private LogEditModel CreateLogEditModel(
                                LogEditSettings settings,
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
                                OrderEditOptions options)
        {
            var ordererId = this.configurableFieldModelFactory.CreateStringField(settings.OrdererId, null);
            var ordererName = this.configurableFieldModelFactory.CreateStringField(settings.OrdererName, null);
            var ordererLocation = this.configurableFieldModelFactory.CreateStringField(settings.OrdererLocation, null);
            var ordererEmail = this.configurableFieldModelFactory.CreateStringField(settings.OrdererEmail, null);
            var ordererPhone = this.configurableFieldModelFactory.CreateStringField(settings.OrdererPhone, null);
            var ordererCode = this.configurableFieldModelFactory.CreateStringField(settings.OrdererCode, null);
            var department = this.configurableFieldModelFactory.CreateSelectListField(settings.Department, options.Departments, null);
            var unit = this.configurableFieldModelFactory.CreateSelectListField(settings.Unit, options.Units, null);
            var ordererAddress = this.configurableFieldModelFactory.CreateStringField(settings.OrdererAddress, null);
            var ordererInvoiceAddress = this.configurableFieldModelFactory.CreateStringField(settings.OrdererInvoiceAddress, null);
            var ordererReferenceNumber = this.configurableFieldModelFactory.CreateStringField(settings.OrdererReferenceNumber, null);
            var accountingDimension1 = this.configurableFieldModelFactory.CreateStringField(settings.AccountingDimension1, null);
            var accountingDimension2 = this.configurableFieldModelFactory.CreateStringField(settings.AccountingDimension2, null);
            var accountingDimension3 = this.configurableFieldModelFactory.CreateStringField(settings.AccountingDimension3, null);
            var accountingDimension4 = this.configurableFieldModelFactory.CreateStringField(settings.AccountingDimension4, null);
            var accountingDimension5 = this.configurableFieldModelFactory.CreateStringField(settings.AccountingDimension5, null);

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
                                OrderEditOptions options)
        {
            return null;
        }

        private OtherEditModel CreateOtherEditModel(
                                OtherEditSettings settings,
                                OrderEditOptions options)
        {
            return null;
        }

        private ProgramEditModel CreateProgramEditModel(
                                ProgramEditSettings settings,
                                OrderEditOptions options)
        {
            return null;
        }

        private ReceiverEditModel CreateReceiverEditModel(
                                ReceiverEditSettings settings,
                                OrderEditOptions options)
        {
            return null;
        }

        private SupplierEditModel CreateSupplierEditModel(
                                SupplierEditSettings settings,
                                OrderEditOptions options)
        {
            return null;
        }

        private UserEditModel CreateUserEditModel(
                                UserEditSettings settings,
                                OrderEditOptions options)
        {
            return null;
        }
    }
}