using System.Linq;
using System.Web.Mvc;
using DH.Helpdesk.BusinessData.Enums.Accounts.Fields;
using DH.Helpdesk.BusinessData.Models.Shared;
using DH.Helpdesk.Common.Extensions.Integer;
using DH.Helpdesk.Domain.Orders;
using DH.Helpdesk.Services.Services;
using DH.Helpdesk.Web.Infrastructure.Extensions;

namespace DH.Helpdesk.Web.Areas.Orders.Infrastructure.ModelFactories.Concrete
{
    using System.Collections.Generic;

    using BusinessData.Enums.Orders;
    using BusinessData.Models.Orders.Order;
    using BusinessData.Models.Orders.Order.OrderEditSettings;
    using Dal.Infrastructure.Context;
    using Models.Order.FieldModels;
    using Models.Order.OrderEdit;

    public class NewOrderModelFactory : INewOrderModelFactory
    {
        private readonly IConfigurableFieldModelFactory _configurableFieldModelFactory;

        public NewOrderModelFactory(IConfigurableFieldModelFactory configurableFieldModelFactory)
        {
            _configurableFieldModelFactory = configurableFieldModelFactory;
        }

        public FullOrderEditModel Create(string temporatyId, NewOrderEditData data, IWorkContext workContext, int? orderTypeId)
        {
            var model = new FullOrderEditModel(
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
                CreateAccountInfoEditModel(data.EditSettings.AccountInfo, data.EditOptions, workContext),
                CreateContactEditModel(data.EditSettings.Contact, data.EditOptions, workContext),
                temporatyId,
                workContext.Customer.CustomerId,
                orderTypeId,
                true,
                null);
            model.CreateCase = workContext.Customer.Settings.CreateCaseFromOrder;
            model.Statuses = data.EditOptions.Statuses;

            return model;
        }

        private DeliveryEditModel CreateDeliveryEditModel(
                                DeliveryEditSettings settings,
                                OrderEditOptions options)
        {
            var deliveryDate = _configurableFieldModelFactory.CreateNullableDateTimeField(settings.DeliveryDate, null);
            var installDate = _configurableFieldModelFactory.CreateNullableDateTimeField(settings.InstallDate, null);
            var deliveryDepartment = _configurableFieldModelFactory.CreateNullableIntegerField(settings.DeliveryDepartment, null);
            var deliveryOu = _configurableFieldModelFactory.CreateStringField(settings.DeliveryOu, null);
            var deliveryAddress = _configurableFieldModelFactory.CreateStringField(settings.DeliveryAddress, null);
            var deliveryPostalCode = _configurableFieldModelFactory.CreateStringField(settings.DeliveryPostalCode, null);
            var deliveryPostalAddress = _configurableFieldModelFactory.CreateStringField(settings.DeliveryPostalAddress, null);
            var deliveryLocation = _configurableFieldModelFactory.CreateStringField(settings.DeliveryLocation, null);
            var deliveryInfo1 = _configurableFieldModelFactory.CreateStringField(settings.DeliveryInfo1, null);
            var deliveryInfo2 = _configurableFieldModelFactory.CreateStringField(settings.DeliveryInfo2, null);
            var deliveryInfo3 = _configurableFieldModelFactory.CreateStringField(settings.DeliveryInfo3, null);
            var deliveryOuId = _configurableFieldModelFactory.CreateNullableIntegerField(settings.DeliveryOuId, null);
            var deliveryName = _configurableFieldModelFactory.CreateStringField(settings.DeliveryName, null);
            var deliveryPhone = _configurableFieldModelFactory.CreateStringField(settings.DeliveryPhone, null);

            var model = new DeliveryEditModel(
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
                            deliveryOuId,
                            deliveryName,
                            deliveryPhone);
            model.Departments = CreateSelectListField(settings.DeliveryDepartment, options.DeliveryDepartment, null);
            model.Units = CreateSelectListField(settings.DeliveryOuId, options.DeliveryOuId, null);

            return model;
        }

        private GeneralEditModel CreateGeneralEditModel(
                                GeneralEditSettings settings,
                                OrderEditOptions options)
        {
            var orderNumber = _configurableFieldModelFactory.CreateIntegerField(settings.OrderNumber, 0);
            var customer = _configurableFieldModelFactory.CreateStringField(settings.Customer, null);
            var administrator = _configurableFieldModelFactory.CreateNullableIntegerField(settings.Administrator, null);
            var domain = _configurableFieldModelFactory.CreateNullableIntegerField(settings.Domain, null);
            var orderDate = _configurableFieldModelFactory.CreateNullableDateTimeField(settings.OrderDate, null);
            var status = _configurableFieldModelFactory.CreateSelectListField(settings.Status, options.Statuses, null, false);

            var model = new GeneralEditModel(
                            orderNumber,
                            customer,
                            administrator,
                            domain,
                            orderDate,
                            options.OrderTypeName,
                            status);

            model.Administrators = CreateSelectListField(settings.Administrator, options.Administrators, null);
            model.Domains = CreateSelectListField(settings.Domain, options.Domains, null);

            return model;

        }

        private LogEditModel CreateLogEditModel(
                                LogEditSettings settings,
                                OrderEditOptions options)
        {
            var log = _configurableFieldModelFactory.CreateLogs(
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
            var ordererId = _configurableFieldModelFactory.CreateStringField(settings.OrdererId, null);
            var ordererName = _configurableFieldModelFactory.CreateStringField(settings.OrdererName, null);
            var ordererLocation = _configurableFieldModelFactory.CreateStringField(settings.OrdererLocation, null);
            var ordererEmail = _configurableFieldModelFactory.CreateStringField(settings.OrdererEmail, null);
            var ordererPhone = _configurableFieldModelFactory.CreateStringField(settings.OrdererPhone, null);
            var ordererCode = _configurableFieldModelFactory.CreateStringField(settings.OrdererCode, null);
            var department = _configurableFieldModelFactory.CreateNullableIntegerField(settings.Department, null);
            var unit = _configurableFieldModelFactory.CreateNullableIntegerField(settings.Unit, null);
            var ordererAddress = _configurableFieldModelFactory.CreateStringField(settings.OrdererAddress, null);
            var ordererInvoiceAddress = _configurableFieldModelFactory.CreateStringField(settings.OrdererInvoiceAddress, null);
            var ordererReferenceNumber = _configurableFieldModelFactory.CreateStringField(settings.OrdererReferenceNumber, null);
            var accountingDimension1 = _configurableFieldModelFactory.CreateStringField(settings.AccountingDimension1, null);
            var accountingDimension2 = _configurableFieldModelFactory.CreateStringField(settings.AccountingDimension2, null);
            var accountingDimension3 = _configurableFieldModelFactory.CreateStringField(settings.AccountingDimension3, null);
            var accountingDimension4 = _configurableFieldModelFactory.CreateStringField(settings.AccountingDimension4, null);
            var accountingDimension5 = _configurableFieldModelFactory.CreateStringField(settings.AccountingDimension5, null);

            var model = new OrdererEditModel(
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
            model.Departments = CreateSelectListField(settings.Department, options.Departments, null);
            model.Units = CreateSelectListField(settings.Unit, options.Units, null);

            return model;

        }

        private OrderEditModel CreateOrderEditModel(
                                OrderEditSettings settings,
                                OrderEditOptions options)
        {
            var property = _configurableFieldModelFactory.CreateNullableIntegerField(settings.Property, null);
            var orderRow1 = _configurableFieldModelFactory.CreateStringField(settings.OrderRow1, null);
            var orderRow2 = _configurableFieldModelFactory.CreateStringField(settings.OrderRow2, null);
            var orderRow3 = _configurableFieldModelFactory.CreateStringField(settings.OrderRow3, null);
            var orderRow4 = _configurableFieldModelFactory.CreateStringField(settings.OrderRow4, null);
            var orderRow5 = _configurableFieldModelFactory.CreateStringField(settings.OrderRow5, null);
            var orderRow6 = _configurableFieldModelFactory.CreateStringField(settings.OrderRow6, null);
            var orderRow7 = _configurableFieldModelFactory.CreateStringField(settings.OrderRow7, null);
            var orderRow8 = _configurableFieldModelFactory.CreateStringField(settings.OrderRow8, null);
            var configuration = _configurableFieldModelFactory.CreateStringField(settings.Configuration, null);
            var orderInfo = _configurableFieldModelFactory.CreateStringField(settings.OrderInfo, null);
            var orderInfo2 = _configurableFieldModelFactory.CreateIntegerField(settings.OrderInfo2, 0);

            var model = new OrderEditModel(
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

            model.Properties = CreateSelectListField(settings.Property, options.Properties, null);

            return model;
        }

        private OtherEditModel CreateOtherEditModel(
                                OtherEditSettings settings,
                                OrderEditOptions options,
                                string orderId)
        {
            var fileName = _configurableFieldModelFactory.CreateAttachedFiles(settings.FileName, orderId, Subtopic.FileName, new List<string>(0));
            var caseNumber = _configurableFieldModelFactory.CreateNullableDecimalField(settings.CaseNumber, null);
            var caseId = _configurableFieldModelFactory.CreateNullableIntegerField(settings.CaseNumber, null);
            var info = _configurableFieldModelFactory.CreateStringField(settings.Info, null);

            return new OtherEditModel(
                            fileName,
                            caseNumber,
                            caseId,
                            info);
        }

        private ProgramEditModel CreateProgramEditModel(ProgramEditSettings settings)
        {
            var program = _configurableFieldModelFactory.CreatePrograms(settings.Program, new List<ProgramModel>(0));
            var infoProduct = _configurableFieldModelFactory.CreateStringField(settings.InfoProduct, null);
            return new ProgramEditModel(program, infoProduct);
        }

        private ReceiverEditModel CreateReceiverEditModel(ReceiverEditSettings settings)
        {
            var receiverId = _configurableFieldModelFactory.CreateStringField(settings.ReceiverId, null);
            var receiverName = _configurableFieldModelFactory.CreateStringField(settings.ReceiverName, null);
            var receiverEmail = _configurableFieldModelFactory.CreateStringField(settings.ReceiverEmail, null);
            var receiverPhone = _configurableFieldModelFactory.CreateStringField(settings.ReceiverPhone, null);
            var receiverLocation = _configurableFieldModelFactory.CreateStringField(settings.ReceiverLocation, null);
            var markOfGoods = _configurableFieldModelFactory.CreateStringField(settings.MarkOfGoods, null);

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
            var supplierOrderNumber = _configurableFieldModelFactory.CreateStringField(settings.SupplierOrderNumber, null);
            var supplierOrderDate = _configurableFieldModelFactory.CreateNullableDateTimeField(settings.SupplierOrderDate, null);
            var supplierOrderInfo = _configurableFieldModelFactory.CreateStringField(settings.SupplierOrderInfo, null);

            return new SupplierEditModel(
                            supplierOrderNumber,
                            supplierOrderDate,
                            supplierOrderInfo);
        }

        private UserEditModel CreateUserEditModel(
                        UserEditSettings settings,
                        IWorkContext workContext)
        {
            var userId = _configurableFieldModelFactory.CreateStringField(settings.UserId, workContext.User.Login);
            var userFirstName = _configurableFieldModelFactory.CreateStringField(settings.UserFirstName, workContext.User.FirstName);
            var userLastName = _configurableFieldModelFactory.CreateStringField(settings.UserLastName, workContext.User.LastName);
            var userPhone = _configurableFieldModelFactory.CreateStringField(settings.UserPhone, workContext.User.Phone);
            var userEmail = _configurableFieldModelFactory.CreateStringField(settings.UserEMail, workContext.User.Email);

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
                            _configurableFieldModelFactory.CreateStringField(settings.PersonalIdentityNumber, null),
                            _configurableFieldModelFactory.CreateStringField(settings.Initials, null),
                            _configurableFieldModelFactory.CreateStringField(settings.Extension, null),
                            _configurableFieldModelFactory.CreateStringField(settings.Title, null),
                            _configurableFieldModelFactory.CreateStringField(settings.Location, null),
                            _configurableFieldModelFactory.CreateStringField(settings.RoomNumber, null),
                            _configurableFieldModelFactory.CreateStringField(settings.PostalAddress, null),
                            _configurableFieldModelFactory.CreateNullableIntegerField(settings.EmploymentType, null),
                            _configurableFieldModelFactory.CreateNullableIntegerField(settings.DepartmentId1, null),
                            _configurableFieldModelFactory.CreateNullableIntegerField(settings.UnitId, null),
                            _configurableFieldModelFactory.CreateNullableIntegerField(settings.DepartmentId2, null),
                            _configurableFieldModelFactory.CreateStringField(settings.Info, null),
                            _configurableFieldModelFactory.CreateStringField(settings.Responsibility, null),
                            _configurableFieldModelFactory.CreateStringField(settings.Activity, null),
                            _configurableFieldModelFactory.CreateStringField(settings.Manager, null),
                            _configurableFieldModelFactory.CreateStringField(settings.ReferenceNumber, null));
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
        
        private AccountInfoEditModel CreateAccountInfoEditModel(
            AccountInfoEditSettings settings,
            OrderEditOptions options,
            IWorkContext workContext)
        {
            var model = new AccountInfoEditModel(
                    _configurableFieldModelFactory.CreateNullableDateTimeField(settings.StartedDate, null),
                    _configurableFieldModelFactory.CreateNullableDateTimeField(settings.FinishDate, null),
                    _configurableFieldModelFactory.CreateNullableIntegerField(settings.EMailTypeId, null),
                    _configurableFieldModelFactory.CreateBooleanField(settings.HomeDirectory, false),
                    _configurableFieldModelFactory.CreateBooleanField(settings.Profile, false),
                    _configurableFieldModelFactory.CreateStringField(settings.InventoryNumber, null),
                    _configurableFieldModelFactory.CreateStringField(settings.Info, null),
                    _configurableFieldModelFactory.CreateNullableIntegerField(settings.AccountTypeId, null),
                    _configurableFieldModelFactory.CreateCheckBoxListField(settings.AccountTypeId2, null, options.AccountTypes2),
                    _configurableFieldModelFactory.CreateNullableIntegerField(settings.AccountTypeId3, null),
                    _configurableFieldModelFactory.CreateNullableIntegerField(settings.AccountTypeId4, null),
                    _configurableFieldModelFactory.CreateNullableIntegerField(settings.AccountTypeId5, null)
                );

            model.EmailTypes = new EMailTypes().ToSelectListDipslay(null);
            model.AccountTypes = CreateSelectListField(settings.AccountTypeId,
                    options.AccountTypes, null);
            model.AccountTypes2 = CreateMultiSelectListField(
                    settings.AccountTypeId2, options.AccountTypes2, null);
            model.AccountTypes3 = CreateSelectListField(settings.AccountTypeId3,
                    options.AccountTypes3, null);
            model.AccountTypes4 = CreateSelectListField(settings.AccountTypeId4,
                    options.AccountTypes4, null);
            model.AccountTypes5 = CreateSelectListField(settings.AccountTypeId5,
                    options.AccountTypes5, null);

            return model;
        }

        private ContactEditModel CreateContactEditModel(
            ContactEditSettings settings,
            OrderEditOptions options,
            IWorkContext workContext)
        {
            return new ContactEditModel(
                    _configurableFieldModelFactory.CreateStringField(settings.Id, null),
                    _configurableFieldModelFactory.CreateStringField(settings.Name, null),
                    _configurableFieldModelFactory.CreateStringField(settings.Phone, null),
                    _configurableFieldModelFactory.CreateStringField(settings.EMail, null)
                );
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

        private static MultiSelectList CreateMultiSelectListField(
            FieldEditSettings setting,
            ItemOverview[] items,
            List<int> selectedValue)
        {
            if (!setting.Show)
            {
                return new MultiSelectList(Enumerable.Empty<ItemOverview>(), Enumerable.Empty<ItemOverview>());
            }


            var list = new MultiSelectList(items, "Value", "Name", selectedValue);
            return list;
        }

    }
}