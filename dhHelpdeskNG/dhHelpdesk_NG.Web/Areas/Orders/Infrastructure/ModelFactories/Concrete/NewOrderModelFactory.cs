using System.Linq;
using System.Web.Mvc;
using DH.Helpdesk.BusinessData.Models.Shared;

namespace DH.Helpdesk.Web.Areas.Orders.Infrastructure.ModelFactories.Concrete
{
    using System.Collections.Generic;

    using DH.Helpdesk.BusinessData.Enums.Orders;
    using DH.Helpdesk.BusinessData.Models.Orders.Order;
    using DH.Helpdesk.BusinessData.Models.Orders.Order.OrderEditSettings;
    using DH.Helpdesk.Dal.Infrastructure.Context;
    using DH.Helpdesk.Web.Areas.Orders.Models.Order.FieldModels;
    using DH.Helpdesk.Web.Areas.Orders.Models.Order.OrderEdit;

    public class NewOrderModelFactory : INewOrderModelFactory
    {
        private readonly IConfigurableFieldModelFactory configurableFieldModelFactory;

        public NewOrderModelFactory(IConfigurableFieldModelFactory configurableFieldModelFactory)
        {
            this.configurableFieldModelFactory = configurableFieldModelFactory;
        }

        public FullOrderEditModel Create(string temporatyId, NewOrderEditData data, IWorkContext workContext, int? orderTypeId)
        {
            return new FullOrderEditModel(
                CreateDeliveryEditModel(data.EditSettings.Delivery, data.EditOptions),
                CreateGeneralEditModel(data.EditSettings.General, data.EditOptions),
                CreateLogEditModel(data.EditSettings.Log, data.EditOptions),
                CreateOrdererEditModel(data.EditSettings.Orderer, data.EditOptions),
                CreateOrderEditModel(data.EditSettings.Order, data.EditOptions),
                CreateOtherEditModel(data.EditSettings.Other, data.EditOptions, temporatyId),
                CreateProgramEditModel(data.EditSettings.Program),
                CreateReceiverEditModel(data.EditSettings.Receiver),
                CreateSupplierEditModel(data.EditSettings.Supplier),
                CreateUserEditModel(data.EditSettings.User, workContext),
                CreateUserInfoEditModel(data.EditSettings.User, data.EditOptions,  workContext),
                temporatyId,
                workContext.Customer.CustomerId,
                orderTypeId,
                true,
                null);
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
            var customer = this.configurableFieldModelFactory.CreateStringField(settings.Customer, null);
            var administrator = this.configurableFieldModelFactory.CreateSelectListField(settings.Administrator, options.Administrators, null);
            var domain = this.configurableFieldModelFactory.CreateSelectListField(settings.Domain, options.Domains, null);
            var orderDate = this.configurableFieldModelFactory.CreateNullableDateTimeField(settings.OrderDate, null);
            var status = this.configurableFieldModelFactory.CreateSelectListField(settings.Status, options.Statuses, null, false);

            return new GeneralEditModel(
                            orderNumber,
                            customer,
                            administrator,
                            domain,
                            orderDate,
                            options.OrderTypeName,
                            status);
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
            var orderInfo2 = this.configurableFieldModelFactory.CreateIntegerField(settings.OrderInfo2, 0);

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
                                OrderEditOptions options,
                                string orderId)
        {
            var fileName = this.configurableFieldModelFactory.CreateAttachedFiles(settings.FileName, orderId, Subtopic.FileName, new List<string>(0));
            var caseNumber = this.configurableFieldModelFactory.CreateNullableDecimalField(settings.CaseNumber, null);
            var info = this.configurableFieldModelFactory.CreateStringField(settings.Info, null);

            return new OtherEditModel(
                            fileName,
                            caseNumber,
                            info);
        }

        private ProgramEditModel CreateProgramEditModel(ProgramEditSettings settings)
        {
            var program = this.configurableFieldModelFactory.CreatePrograms(settings.Program, new List<ProgramModel>(0));

            return new ProgramEditModel(program);
        }

        private ReceiverEditModel CreateReceiverEditModel(ReceiverEditSettings settings)
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

        private SupplierEditModel CreateSupplierEditModel(SupplierEditSettings settings)
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
                        IWorkContext workContext)
        {
            var userId = configurableFieldModelFactory.CreateStringField(settings.UserId, workContext.User.Login);
            var userFirstName = configurableFieldModelFactory.CreateStringField(settings.UserFirstName, workContext.User.FirstName);
            var userLastName = configurableFieldModelFactory.CreateStringField(settings.UserLastName, workContext.User.LastName);
            var userPhone = configurableFieldModelFactory.CreateStringField(settings.UserPhone, workContext.User.Phone);
            var userEmail = configurableFieldModelFactory.CreateStringField(settings.UserEMail, workContext.User.Email);

            return new UserEditModel(
                            userId,
                            userFirstName,
                            userLastName,
                            userPhone,
                            userEmail);
        }

        private UserInfoEditModel CreateUserInfoEditModel(
                UserEditSettings settings,
                OrderEditOptions options,
                IWorkContext workContext)
        {
            var model = new UserInfoEditModel(
                            configurableFieldModelFactory.CreateStringField(settings.PersonalIdentityNumber, null),
                            configurableFieldModelFactory.CreateStringField(settings.Initials, null),
                            configurableFieldModelFactory.CreateStringField(settings.Extension, null),
                            configurableFieldModelFactory.CreateStringField(settings.Title, null),
                            configurableFieldModelFactory.CreateStringField(settings.Location, null),
                            configurableFieldModelFactory.CreateStringField(settings.RoomNumber, null),
                            configurableFieldModelFactory.CreateStringField(settings.PostalAddress, null),
                            configurableFieldModelFactory.CreateNullableIntegerField(settings.EmploymentType, null),
                            configurableFieldModelFactory.CreateNullableIntegerField(settings.DepartmentId1, null),
                            configurableFieldModelFactory.CreateNullableIntegerField(settings.UnitId, null),
                            configurableFieldModelFactory.CreateNullableIntegerField(settings.DepartmentId2, null),
                            configurableFieldModelFactory.CreateStringField(settings.Info, null),
                            configurableFieldModelFactory.CreateStringField(settings.Responsibility, null),
                            configurableFieldModelFactory.CreateStringField(settings.Activity, null),
                            configurableFieldModelFactory.CreateStringField(settings.Manager, null),
                            configurableFieldModelFactory.CreateStringField(settings.ReferenceNumber, null));
            model.EmploymentTypes = CreateSelectListField(settings.EmploymentType,
                options.EmploymentTypes, null);
            model.Departments = CreateSelectListField(settings.DepartmentId1,
                options.Departments, null);
            model.Departments2 = CreateSelectListField(settings.DepartmentId2,
                options.Departments, null);
            model.Units = CreateSelectListField(settings.UnitId,
                options.Units, null);
            model.Regions = CreateSelectListField(settings.DepartmentId1,
                options.Regions, null);

            return model;
        }

        private static SelectList CreateSelectListField(
                FieldEditSettings setting,
                ItemOverview[] items,
                string selectedValue)
       {
            if (!setting.Show)
            {
                return new SelectList(Enumerable.Empty<SelectListItem>());
            }

            var list = new SelectList(items, "Value", "Name", selectedValue);
            return list;
        }
    }
}