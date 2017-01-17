using System.Web.Mvc;
using DH.Helpdesk.BusinessData.Enums.Accounts.Fields;
using DH.Helpdesk.BusinessData.Models.Shared;
using DH.Helpdesk.Domain.Orders;
using DH.Helpdesk.Web.Infrastructure.Extensions;

namespace DH.Helpdesk.Web.Areas.Orders.Infrastructure.ModelFactories.Concrete
{
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;

    using BusinessData.Enums.Orders;
    using BusinessData.Models.Orders.Order;
    using BusinessData.Models.Orders.Order.OrderEditFields;
    using BusinessData.Models.Orders.Order.OrderEditSettings;
    using Models.Order.FieldModels;
    using Models.Order.OrderEdit;

    public class OrderModelFactory : IOrderModelFactory
    {
        private readonly IConfigurableFieldModelFactory _configurableFieldModelFactory;

        private readonly IHistoryModelFactory _historyModelFactory;

        public OrderModelFactory(
                IConfigurableFieldModelFactory configurableFieldModelFactory, 
                IHistoryModelFactory historyModelFactory)
        {
            _configurableFieldModelFactory = configurableFieldModelFactory;
            _historyModelFactory = historyModelFactory;
        }

        public FullOrderEditModel Create(FindOrderResponse response, int customerId)
        {
            var orderId = response.EditData.Order.Id;
            var textOrderId = orderId.ToString(CultureInfo.InvariantCulture);
            var history = _historyModelFactory.Create(response);

            var model = new FullOrderEditModel(
                CreateDeliveryEditModel(response.EditSettings.Delivery, response.EditData.Order.Delivery,
                    response.EditOptions),
                CreateGeneralEditModel(response.EditSettings.General, response.EditData.Order.General,
                    response.EditOptions),
                CreateLogEditModel(response.EditSettings.Log, response.EditData.Order.Log, response.EditOptions,
                    orderId),
                CreateOrdererEditModel(response.EditSettings.Orderer, response.EditData.Order.Orderer,
                    response.EditOptions),
                CreateOrderEditModel(response.EditSettings.Order, response.EditData.Order.Order,
                    response.EditOptions),
                CreateOtherEditModel(response.EditSettings.Other, response.EditData.Order.Other,
                    response.EditOptions, textOrderId),
                CreateProgramEditModel(response.EditSettings.Program, response.EditData.Order.Program),
                CreateReceiverEditModel(response.EditSettings.Receiver, response.EditData.Order.Receiver),
                CreateSupplierEditModel(response.EditSettings.Supplier, response.EditData.Order.Supplier),
                CreateUserEditModel(response.EditSettings.User, response.EditData.Order.User),
                CreateUserInfoEditModel(response.EditSettings.User, response.EditData.Order.User, response.EditOptions),
                CreateAccountInfoEditModel(response.EditSettings.AccountInfo, response.EditData.Order.AccountInfo, response.EditOptions),
                CreateContactEditModel(response.EditSettings.Contact, response.EditData.Order.Contact, response.EditOptions),
                textOrderId,
                customerId,
                response.EditData.Order.OrderTypeId,
                false,
                history);
            model.Statuses = response.EditOptions.Statuses;

            return model;
        }

        private DeliveryEditModel CreateDeliveryEditModel(                                
                                DeliveryEditSettings settings,
                                DeliveryEditFields fields,
                                OrderEditOptions options)
        {
            var deliveryDate = _configurableFieldModelFactory.CreateNullableDateTimeField(settings.DeliveryDate, fields.DeliveryDate);
            var installDate = _configurableFieldModelFactory.CreateNullableDateTimeField(settings.InstallDate, fields.InstallDate);
            var deliveryDepartment = _configurableFieldModelFactory.CreateSelectListField(settings.DeliveryDepartment, options.DeliveryDepartment, fields.DeliveryDepartmentId);
            var deliveryOu = _configurableFieldModelFactory.CreateStringField(settings.DeliveryOu, fields.DeliveryOu);
            var deliveryAddress = _configurableFieldModelFactory.CreateStringField(settings.DeliveryAddress, fields.DeliveryAddress);
            var deliveryPostalCode = _configurableFieldModelFactory.CreateStringField(settings.DeliveryPostalCode, fields.DeliveryPostalCode);
            var deliveryPostalAddress = _configurableFieldModelFactory.CreateStringField(settings.DeliveryPostalAddress, fields.DeliveryPostalAddress);
            var deliveryLocation = _configurableFieldModelFactory.CreateStringField(settings.DeliveryLocation, fields.DeliveryLocation);
            var deliveryInfo1 = _configurableFieldModelFactory.CreateStringField(settings.DeliveryInfo1, fields.DeliveryInfo1);
            var deliveryInfo2 = _configurableFieldModelFactory.CreateStringField(settings.DeliveryInfo2, fields.DeliveryInfo2);
            var deliveryInfo3 = _configurableFieldModelFactory.CreateStringField(settings.DeliveryInfo3, fields.DeliveryInfo3);
            var deliveryOuId = _configurableFieldModelFactory.CreateSelectListField(settings.DeliveryOuId, options.DeliveryOuId, fields.DeliveryOuIdId);
            var deliveryName = _configurableFieldModelFactory.CreateStringField(settings.DeliveryName, fields.DeliveryName);
            var deliveryPhone = _configurableFieldModelFactory.CreateStringField(settings.DeliveryPhone, fields.DeliveryPhone);

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
                            deliveryOuId,
                            deliveryName,
                            deliveryPhone);
        }

        private GeneralEditModel CreateGeneralEditModel(
                                GeneralEditSettings settings,
                                GeneralEditFields fields,
                                OrderEditOptions options)
        {
            var orderNumber = _configurableFieldModelFactory.CreateIntegerField(settings.OrderNumber, fields.OrderNumber);
            var customer = _configurableFieldModelFactory.CreateStringField(settings.Customer, fields.Customer);
            var administrator = _configurableFieldModelFactory.CreateSelectListField(settings.Administrator, options.Administrators, fields.AdministratorId);
            var domain = _configurableFieldModelFactory.CreateSelectListField(settings.Domain, options.Domains, fields.DomainId);
            var orderDate = _configurableFieldModelFactory.CreateNullableDateTimeField(settings.OrderDate, fields.OrderDate);
            var status = _configurableFieldModelFactory.CreateSelectListField(settings.Status, options.Statuses, fields.StatusId, false);

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
                                LogEditFields fields,
                                OrderEditOptions options,
                                int orderId)
        {
            var log = _configurableFieldModelFactory.CreateLogs(
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
            var ordererId = _configurableFieldModelFactory.CreateStringField(settings.OrdererId, fields.OrdererId);
            var ordererName = _configurableFieldModelFactory.CreateStringField(settings.OrdererName, fields.OrdererName);
            var ordererLocation = _configurableFieldModelFactory.CreateStringField(settings.OrdererLocation, fields.OrdererLocation);
            var ordererEmail = _configurableFieldModelFactory.CreateStringField(settings.OrdererEmail, fields.OrdererEmail);
            var ordererPhone = _configurableFieldModelFactory.CreateStringField(settings.OrdererPhone, fields.OrdererPhone);
            var ordererCode = _configurableFieldModelFactory.CreateStringField(settings.OrdererCode, fields.OrdererCode);
            var department = _configurableFieldModelFactory.CreateSelectListField(settings.Department, options.Departments, fields.DepartmentId);
            var unit = _configurableFieldModelFactory.CreateSelectListField(settings.Unit, options.Units, fields.UnitId);
            var ordererAddress = _configurableFieldModelFactory.CreateStringField(settings.OrdererAddress, fields.OrdererAddress);
            var ordererInvoiceAddress = _configurableFieldModelFactory.CreateStringField(settings.OrdererInvoiceAddress, fields.OrdererInvoiceAddress);
            var ordererReferenceNumber = _configurableFieldModelFactory.CreateStringField(settings.OrdererReferenceNumber, fields.OrdererReferenceNumber);
            var accountingDimension1 = _configurableFieldModelFactory.CreateStringField(settings.AccountingDimension1, fields.AccountingDimension1);
            var accountingDimension2 = _configurableFieldModelFactory.CreateStringField(settings.AccountingDimension2, fields.AccountingDimension2);
            var accountingDimension3 = _configurableFieldModelFactory.CreateStringField(settings.AccountingDimension3, fields.AccountingDimension3);
            var accountingDimension4 = _configurableFieldModelFactory.CreateStringField(settings.AccountingDimension4, fields.AccountingDimension4);
            var accountingDimension5 = _configurableFieldModelFactory.CreateStringField(settings.AccountingDimension5, fields.AccountingDimension5);

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
            var property = _configurableFieldModelFactory.CreateSelectListField(settings.Property, options.Properties, fields.PropertyId);
            var orderRow1 = _configurableFieldModelFactory.CreateStringField(settings.OrderRow1, fields.OrderRow1);
            var orderRow2 = _configurableFieldModelFactory.CreateStringField(settings.OrderRow2, fields.OrderRow2);
            var orderRow3 = _configurableFieldModelFactory.CreateStringField(settings.OrderRow3, fields.OrderRow3);
            var orderRow4 = _configurableFieldModelFactory.CreateStringField(settings.OrderRow4, fields.OrderRow4);
            var orderRow5 = _configurableFieldModelFactory.CreateStringField(settings.OrderRow5, fields.OrderRow5);
            var orderRow6 = _configurableFieldModelFactory.CreateStringField(settings.OrderRow6, fields.OrderRow6);
            var orderRow7 = _configurableFieldModelFactory.CreateStringField(settings.OrderRow7, fields.OrderRow7);
            var orderRow8 = _configurableFieldModelFactory.CreateStringField(settings.OrderRow8, fields.OrderRow8);
            var configuration = _configurableFieldModelFactory.CreateStringField(settings.Configuration, fields.Configuration);
            var orderInfo = _configurableFieldModelFactory.CreateStringField(settings.OrderInfo, fields.OrderInfo);
            var orderInfo2 = _configurableFieldModelFactory.CreateIntegerField(settings.OrderInfo2, fields.OrderInfo2);

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

            var fileName = _configurableFieldModelFactory.CreateAttachedFiles(settings.FileName, orderId, Subtopic.FileName, files);
            var caseNumber = _configurableFieldModelFactory.CreateNullableDecimalField(settings.CaseNumber, fields.CaseNumber);
            var caseId = _configurableFieldModelFactory.CreateNullableIntegerField(settings.CaseNumber, fields.CaseId);
            var info = _configurableFieldModelFactory.CreateStringField(settings.Info, fields.Info);

            return new OtherEditModel(
                            fileName,
                            caseNumber,
                            caseId,
                            info);
        }

        private ProgramEditModel CreateProgramEditModel(
                                ProgramEditSettings settings,
                                ProgramEditFields fields)
        {
            var program = _configurableFieldModelFactory.CreatePrograms(settings.Program, fields.Programs.Select(p => new ProgramModel(p.Id, p.Name)).ToList());
            var infoProduct = _configurableFieldModelFactory.CreateStringField(settings.InfoProduct, fields.InfoProduct);

            return new ProgramEditModel(program, infoProduct);
        }

        private ReceiverEditModel CreateReceiverEditModel(
                                ReceiverEditSettings settings,
                                ReceiverEditFields fields)
        {
            var receiverId = _configurableFieldModelFactory.CreateStringField(settings.ReceiverId, fields.ReceiverId);
            var receiverName = _configurableFieldModelFactory.CreateStringField(settings.ReceiverName, fields.ReceiverName);
            var receiverEmail = _configurableFieldModelFactory.CreateStringField(settings.ReceiverEmail, fields.ReceiverEmail);
            var receiverPhone = _configurableFieldModelFactory.CreateStringField(settings.ReceiverPhone, fields.ReceiverPhone);
            var receiverLocation = _configurableFieldModelFactory.CreateStringField(settings.ReceiverLocation, fields.ReceiverLocation);
            var markOfGoods = _configurableFieldModelFactory.CreateStringField(settings.MarkOfGoods, fields.MarkOfGoods);

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
            var supplierOrderNumber = _configurableFieldModelFactory.CreateStringField(settings.SupplierOrderNumber, fields.SupplierOrderNumber);
            var supplierOrderDate = _configurableFieldModelFactory.CreateNullableDateTimeField(settings.SupplierOrderDate, fields.SupplierOrderDate);
            var supplierOrderInfo = _configurableFieldModelFactory.CreateStringField(settings.SupplierOrderInfo, fields.SupplierOrderInfo);

            return new SupplierEditModel(
                            supplierOrderNumber,
                            supplierOrderDate,
                            supplierOrderInfo);
        }

        private UserEditModel CreateUserEditModel(
                                UserEditSettings settings,
                                UserEditFields fields)
        {
            var userId = _configurableFieldModelFactory.CreateStringField(settings.UserId, fields.UserId);
            var userFirstName = _configurableFieldModelFactory.CreateStringField(settings.UserFirstName, fields.UserFirstName);
            var userLastName = _configurableFieldModelFactory.CreateStringField(settings.UserLastName, fields.UserLastName);
            var userPhone = _configurableFieldModelFactory.CreateStringField(settings.UserPhone, fields.UserPhone);
            var userEmail = _configurableFieldModelFactory.CreateStringField(settings.UserEMail, fields.UserEMail);

            return new UserEditModel(
                            userId,
                            userFirstName,
                            userLastName,
                            userPhone,
                            userEmail);
        }

        private UserInfoEditModel CreateUserInfoEditModel(
                        UserEditSettings settings,
                        UserEditFields fields,
                        OrderEditOptions options)
        {
            var model = new UserInfoEditModel(
                            _configurableFieldModelFactory.CreateStringField(settings.PersonalIdentityNumber, fields.UserPersonalIdentityNumber),
                            _configurableFieldModelFactory.CreateStringField(settings.Initials, fields.UserInitials),
                            _configurableFieldModelFactory.CreateStringField(settings.Extension, fields.UserExtension),
                            _configurableFieldModelFactory.CreateStringField(settings.Title, fields.UserTitle),
                            _configurableFieldModelFactory.CreateStringField(settings.Location, fields.UserLocation),
                            _configurableFieldModelFactory.CreateStringField(settings.RoomNumber, fields.UserRoomNumber),
                            _configurableFieldModelFactory.CreateStringField(settings.PostalAddress, fields.UserPostalAddress),
                            _configurableFieldModelFactory.CreateNullableIntegerField(settings.EmploymentType, fields.EmploymentType_Id),
                            _configurableFieldModelFactory.CreateNullableIntegerField(settings.DepartmentId1, fields.UserDepartment_Id1),
                            _configurableFieldModelFactory.CreateNullableIntegerField(settings.UnitId, fields.UserOU_Id),
                            _configurableFieldModelFactory.CreateNullableIntegerField(settings.DepartmentId2, fields.UserDepartment_Id2),
                            _configurableFieldModelFactory.CreateStringField(settings.Info, fields.InfoUser),
                            _configurableFieldModelFactory.CreateStringField(settings.Responsibility, fields.Responsibility),
                            _configurableFieldModelFactory.CreateStringField(settings.Activity, fields.Activity),
                            _configurableFieldModelFactory.CreateStringField(settings.Manager, fields.Manager),
                            _configurableFieldModelFactory.CreateStringField(settings.ReferenceNumber, fields.ReferenceNumber));
            model.EmploymentTypes = CreateSelectListField(settings.EmploymentType,
                options.EmploymentTypes, fields.EmploymentType_Id.ToString());
            model.Departments = CreateSelectListField(settings.DepartmentId1,
                options.Departments, fields.UserDepartment_Id1.ToString());
            model.Departments2 = CreateSelectListField(settings.DepartmentId2,
                options.Departments, fields.UserDepartment_Id2.ToString());
            model.Units = CreateSelectListField(settings.UnitId,
                options.Units, fields.UserOU_Id.ToString());
            model.Regions = CreateSelectListField(settings.DepartmentId1,
                options.Regions, fields.RegionId.ToString());

            return model;
        }

        private AccountInfoEditModel CreateAccountInfoEditModel(
            AccountInfoEditSettings settings,
            AccountInfoEditFields fields,
            OrderEditOptions options)
        {
            var model = new AccountInfoEditModel(
                    _configurableFieldModelFactory.CreateNullableDateTimeField(settings.StartedDate, fields.StartedDate),
                    _configurableFieldModelFactory.CreateNullableDateTimeField(settings.FinishDate, fields.FinishDate),
                    _configurableFieldModelFactory.CreateNullableIntegerField(settings.EMailTypeId, fields.EMailTypeId == 0 ? null : (int?)fields.EMailTypeId),
                    _configurableFieldModelFactory.CreateBooleanField(settings.HomeDirectory, fields.HomeDirectory),
                    _configurableFieldModelFactory.CreateBooleanField(settings.Profile, fields.Profile),
                    _configurableFieldModelFactory.CreateStringField(settings.InventoryNumber, fields.InventoryNumber),
                    _configurableFieldModelFactory.CreateStringField(settings.Info, fields.Info),
                    _configurableFieldModelFactory.CreateNullableIntegerField(settings.AccountTypeId, fields.AccountTypeId),
                    _configurableFieldModelFactory.CreateCheckBoxListField(settings.AccountTypeId2, fields.AccountTypeId2, options.AccountTypes2),
                    _configurableFieldModelFactory.CreateNullableIntegerField(settings.AccountTypeId3, fields.AccountTypeId3),
                    _configurableFieldModelFactory.CreateNullableIntegerField(settings.AccountTypeId4, fields.AccountTypeId4),
                    _configurableFieldModelFactory.CreateNullableIntegerField(settings.AccountTypeId5, fields.AccountTypeId5)
                );

            model.EmailTypes = new EMailTypes().ToSelectListDipslay(fields.EMailTypeId != null ? ((int)fields.EMailTypeId).ToString() : null);
            model.AccountTypes = CreateSelectListField(settings.AccountTypeId,
                    options.AccountTypes, fields.AccountTypeId.ToString());
            model.AccountTypes2 = CreateMultiSelectListField(
                    settings.AccountTypeId2, options.AccountTypes2, fields.AccountTypeId2);
            model.AccountTypes3 = CreateSelectListField(settings.AccountTypeId3,
                    options.AccountTypes3, fields.AccountTypeId3.ToString());
            model.AccountTypes4 = CreateSelectListField(settings.AccountTypeId4,
                    options.AccountTypes4, fields.AccountTypeId4.ToString());
            model.AccountTypes5 = CreateSelectListField(settings.AccountTypeId5,
                    options.AccountTypes5, fields.AccountTypeId5.ToString());

            return model;
        }

        private ContactEditModel CreateContactEditModel(
            ContactEditSettings settings,
            ContactEditFields fields,
            OrderEditOptions options)
        {
            return new ContactEditModel(
                    _configurableFieldModelFactory.CreateStringField(settings.Id, fields.Id),
                    _configurableFieldModelFactory.CreateStringField(settings.Name, fields.Name),
                    _configurableFieldModelFactory.CreateStringField(settings.Phone, fields.Phone),
                    _configurableFieldModelFactory.CreateStringField(settings.EMail, fields.Email)
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
                return new MultiSelectList(Enumerable.Empty<SelectListItem>(), Enumerable.Empty<SelectListItem>());
            }


            var list = new MultiSelectList(items, "Value", "Name", selectedValue);
            return list;
        }
    }
}