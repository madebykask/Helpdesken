using System.Web.Mvc;
using DH.Helpdesk.BusinessData.Enums.Accounts.Fields;
using DH.Helpdesk.BusinessData.Models.Shared;
using DH.Helpdesk.Domain.Orders;
using DH.Helpdesk.Web.Infrastructure.Extensions;
using DH.Helpdesk.Web.Models;

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
	using Services.Services;

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

        public FullOrderEditModel Create(FindOrderResponse response, int customerId, IGlobalSettingService globalSettingService)
        {
            var orderId = response.EditData.Order.Id;
            var textOrderId = orderId.ToString(CultureInfo.InvariantCulture);
            var history = _historyModelFactory.Create(response);
			var fileUploadWhiteList = globalSettingService.GetFileUploadWhiteList();

            return new FullOrderEditModel(
                CreateDeliveryEditModel(response.EditSettings.Delivery, response.EditData.Order.Delivery,
                    response.EditOptions),
                CreateGeneralEditModel(response.EditSettings.General, response.EditData.Order.General,
                    response.EditOptions),
                CreateLogEditModel(response.EditSettings.Log, response.EditData.Order.Log, response.EditOptions,
                    orderId),
                CreateOrderEditModel(response.EditSettings, response.EditData.Order,
                    response.EditOptions),
                CreateOtherEditModel(response.EditSettings.Other, response.EditData.Order.Other,
                    response.EditOptions, textOrderId),
                CreateProgramEditModel(response.EditSettings.Program, response.EditData.Order.Program,
                    response.EditOptions),
                CreateReceiverEditModel(response.EditSettings.Receiver, response.EditData.Order.Receiver),
                CreateSupplierEditModel(response.EditSettings.Supplier, response.EditData.Order.Supplier),
                CreateUserEditModel(response.EditSettings, response.EditData.Order.User),
                CreateUserInfoEditModel(response.EditSettings, response.EditData.Order, response.EditOptions),
                textOrderId,
                customerId,
                response.EditData.Order.OrderTypeId,
                false,
                history,
				fileUploadWhiteList) {Statuses = response.EditOptions.Statuses};
        }

        private DeliveryEditModel CreateDeliveryEditModel(                                
                                DeliveryEditSettings settings,
                                DeliveryEditFields fields,
                                OrderEditOptions options)
        {
            var deliveryDate = _configurableFieldModelFactory.CreateNullableDateTimeField(settings.DeliveryDate, fields.DeliveryDate);
            var installDate = _configurableFieldModelFactory.CreateNullableDateTimeField(settings.InstallDate, fields.InstallDate);
            var deliveryDepartment = _configurableFieldModelFactory.CreateNullableIntegerField(settings.DeliveryDepartment, fields.DeliveryDepartmentId);
            var deliveryOu = _configurableFieldModelFactory.CreateStringField(settings.DeliveryOu, fields.DeliveryOu);
            var deliveryAddress = _configurableFieldModelFactory.CreateStringField(settings.DeliveryAddress, fields.DeliveryAddress);
            var deliveryPostalCode = _configurableFieldModelFactory.CreateStringField(settings.DeliveryPostalCode, fields.DeliveryPostalCode);
            var deliveryPostalAddress = _configurableFieldModelFactory.CreateStringField(settings.DeliveryPostalAddress, fields.DeliveryPostalAddress);
            var deliveryLocation = _configurableFieldModelFactory.CreateStringField(settings.DeliveryLocation, fields.DeliveryLocation);
            var deliveryInfo1 = _configurableFieldModelFactory.CreateStringField(settings.DeliveryInfo1, fields.DeliveryInfo1);
            var deliveryInfo2 = _configurableFieldModelFactory.CreateStringField(settings.DeliveryInfo2, fields.DeliveryInfo2);
            var deliveryInfo3 = _configurableFieldModelFactory.CreateStringField(settings.DeliveryInfo3, fields.DeliveryInfo3);
            var deliveryOuId = _configurableFieldModelFactory.CreateNullableIntegerField(settings.DeliveryOuId, fields.DeliveryOuIdId);
            var deliveryName = _configurableFieldModelFactory.CreateStringField(settings.DeliveryName, fields.DeliveryName);
            var deliveryPhone = _configurableFieldModelFactory.CreateStringField(settings.DeliveryPhone, fields.DeliveryPhone);

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
                deliveryPhone)
            {
                Departments =
                    CreateSelectListField(settings.DeliveryDepartment, options.DeliveryDepartment,
                        fields.DeliveryDepartmentId.ToString()),
                Units =
                    CreateSelectListField(settings.DeliveryOuId, options.DeliveryOuId, fields.DeliveryOuIdId.ToString()),
                Header = settings.Header
            };


            return model;
        }

        private GeneralEditModel CreateGeneralEditModel(
                                GeneralEditSettings settings,
                                GeneralEditFields fields,
                                OrderEditOptions options)
        {
            var orderNumber = _configurableFieldModelFactory.CreateIntegerField(settings.OrderNumber, fields.OrderNumber);
            var customer = _configurableFieldModelFactory.CreateStringField(settings.Customer, fields.Customer);
            var administrator = _configurableFieldModelFactory.CreateNullableIntegerField(settings.Administrator, fields.AdministratorId);
            var domain = _configurableFieldModelFactory.CreateNullableIntegerField(settings.Domain, fields.DomainId);
            var orderDate = _configurableFieldModelFactory.CreateNullableDateTimeField(settings.OrderDate, fields.OrderDate);
            var status = _configurableFieldModelFactory.CreateSelectListField(settings.Status, options.Statuses, fields.StatusId, false);

            var model = new GeneralEditModel(
                orderNumber,
                customer,
                administrator,
                domain,
                orderDate,
                options.OrderTypeName,
                status)
            {
                Administrators = CreateSelectListField(settings.Administrator, options.Administrators, fields.AdministratorId.ToString()),
                Domains = CreateSelectListField(settings.Domain, options.Domains, fields.DomainId.ToString()),
                Header = settings.Header
            };

            return model;

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


        private OrderEditModel CreateOrderEditModel(
                                FullOrderEditSettings settings,
                                FullOrderEditFields fields,
                                OrderEditOptions options)
        {
            var property = _configurableFieldModelFactory.CreateNullableIntegerField(settings.Order.Property, fields.Order.PropertyId);
            var orderRow1 = _configurableFieldModelFactory.CreateStringField(settings.Order.OrderRow1, fields.Order.OrderRow1);
            var orderRow2 = _configurableFieldModelFactory.CreateStringField(settings.Order.OrderRow2, fields.Order.OrderRow2);
            var orderRow3 = _configurableFieldModelFactory.CreateStringField(settings.Order.OrderRow3, fields.Order.OrderRow3);
            var orderRow4 = _configurableFieldModelFactory.CreateStringField(settings.Order.OrderRow4, fields.Order.OrderRow4);
            var orderRow5 = _configurableFieldModelFactory.CreateStringField(settings.Order.OrderRow5, fields.Order.OrderRow5);
            var orderRow6 = _configurableFieldModelFactory.CreateStringField(settings.Order.OrderRow6, fields.Order.OrderRow6);
            var orderRow7 = _configurableFieldModelFactory.CreateStringField(settings.Order.OrderRow7, fields.Order.OrderRow7);
            var orderRow8 = _configurableFieldModelFactory.CreateStringField(settings.Order.OrderRow8, fields.Order.OrderRow8);
            var configuration = _configurableFieldModelFactory.CreateStringField(settings.Order.Configuration, fields.Order.Configuration);
            var orderInfo = _configurableFieldModelFactory.CreateStringField(settings.Order.OrderInfo, fields.Order.OrderInfo);
            var orderInfo2 = _configurableFieldModelFactory.CreateIntegerField(settings.Order.OrderInfo2, fields.Order.OrderInfo2);
            var startedDate =_configurableFieldModelFactory.CreateNullableDateTimeField(settings.AccountInfo.StartedDate, fields.AccountInfo.StartedDate);
            var finishDate = _configurableFieldModelFactory.CreateNullableDateTimeField(settings.AccountInfo.FinishDate, fields.AccountInfo.FinishDate);
            var eMailTypeId = _configurableFieldModelFactory.CreateNullableIntegerField(settings.AccountInfo.EMailTypeId, fields.AccountInfo.EMailTypeId == 0 ? null : (int?)fields.AccountInfo.EMailTypeId);
            var homeDirectory = _configurableFieldModelFactory.CreateBooleanField(settings.AccountInfo.HomeDirectory, fields.AccountInfo.HomeDirectory);
            var profile = _configurableFieldModelFactory.CreateBooleanField(settings.AccountInfo.Profile, fields.AccountInfo.Profile);
            var accountTypeId = _configurableFieldModelFactory.CreateNullableIntegerField(settings.AccountInfo.AccountTypeId, fields.AccountInfo.AccountTypeId);
            var accountTypeId2 = _configurableFieldModelFactory.CreateCheckBoxListField(settings.AccountInfo.AccountTypeId2, fields.AccountInfo.AccountTypeId2, options.AccountTypes2);
            var accountTypeId3 = _configurableFieldModelFactory.CreateNullableIntegerField(settings.AccountInfo.AccountTypeId3, fields.AccountInfo.AccountTypeId3);
            var accountTypeId4 = _configurableFieldModelFactory.CreateNullableIntegerField(settings.AccountInfo.AccountTypeId4, fields.AccountInfo.AccountTypeId4);
            var accountTypeId5 = _configurableFieldModelFactory.CreateNullableIntegerField(settings.AccountInfo.AccountTypeId5, fields.AccountInfo.AccountTypeId5);
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
                            orderInfo2,
                            startedDate,
                            finishDate,
                            eMailTypeId,
                            homeDirectory,
                            profile,
                            accountTypeId,
                            accountTypeId2,
                            accountTypeId3,
                            accountTypeId4,
                            accountTypeId5);

            model.Properties = CreateSelectListField(settings.Order.Property,
                options.Properties, fields.Order.PropertyId.ToString());
            model.EmailTypes = new EMailTypes().ToSelectListDipslay(fields.AccountInfo.EMailTypeId != null ? ((int)fields.AccountInfo.EMailTypeId).ToString() : null);
            model.AccountTypes = CreateSelectListField(settings.AccountInfo.AccountTypeId,
                    options.AccountTypes, fields.AccountInfo.AccountTypeId.ToString());
            model.AccountTypes2 = CreateMultiSelectListField(
                    settings.AccountInfo.AccountTypeId2, options.AccountTypes2, fields.AccountInfo.AccountTypeId2);
            model.AccountTypes3 = CreateSelectListField(settings.AccountInfo.AccountTypeId3,
                    options.AccountTypes3, fields.AccountInfo.AccountTypeId3.ToString());
            model.AccountTypes4 = CreateSelectListField(settings.AccountInfo.AccountTypeId4,
                    options.AccountTypes4, fields.AccountInfo.AccountTypeId4.ToString());
            model.AccountTypes5 = CreateSelectListField(settings.AccountInfo.AccountTypeId5,
                    options.AccountTypes5, fields.AccountInfo.AccountTypeId5.ToString());
            model.Header = settings.Order.Header;

            return model;

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
                            info)
            {
                Header = settings.Header
            };
        }

        private ProgramEditModel CreateProgramEditModel(ProgramEditSettings settings, ProgramEditFields fields,OrderEditOptions options)
        {
            var infoProduct = _configurableFieldModelFactory.CreateStringField(settings.InfoProduct, fields.InfoProduct);
            var programs = _configurableFieldModelFactory.CreateCheckBoxListField(settings.Program, fields.Programs,
                options.Programs);
            var model = new ProgramEditModel(infoProduct, programs)
            {
                AllPrograms = CreateMultiSelectListField(settings.Program, options.Programs, fields.Programs),
                Header = settings.Header
            };

            return model;
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
                            markOfGoods)
            {
                Header = settings.Header
            };
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
                            supplierOrderInfo)
            {
                Header = settings.Header
            };
        }

        private UserEditModel CreateUserEditModel(
                                FullOrderEditSettings settings,
                                UserEditFields fields)
        {
            var userId = _configurableFieldModelFactory.CreateStringField(settings.User.UserId, fields.UserId);
            var userFirstName = _configurableFieldModelFactory.CreateStringField(settings.User.UserFirstName, fields.UserFirstName);
            var userLastName = _configurableFieldModelFactory.CreateStringField(settings.User.UserLastName, fields.UserLastName);
            var userPhone = _configurableFieldModelFactory.CreateStringField(settings.User.UserPhone, fields.UserPhone);
            var userEmail = _configurableFieldModelFactory.CreateStringField(settings.User.UserEMail, fields.UserEMail);

            return new UserEditModel(
                            userId,
                            userFirstName,
                            userLastName,
                            userPhone,
                            userEmail)
            {
                Header = settings.Orderer.Header
            };
        }

        private UserInfoEditModel CreateUserInfoEditModel(
                        FullOrderEditSettings settings,
                        FullOrderEditFields fields,
                        OrderEditOptions options)
        {
            var personalIdentityNumber =
                _configurableFieldModelFactory.CreateStringField(settings.User.PersonalIdentityNumber, fields.User.UserPersonalIdentityNumber);
            var extension = _configurableFieldModelFactory.CreateStringField(settings.User.Extension, fields.User.UserExtension);
            var title = _configurableFieldModelFactory.CreateStringField(settings.User.Title, fields.User.UserTitle);
            var roomNumber = _configurableFieldModelFactory.CreateStringField(settings.User.RoomNumber, fields.User.UserRoomNumber);
            var employmentType = _configurableFieldModelFactory.CreateNullableIntegerField(settings.User.EmploymentType, fields.User.EmploymentType_Id);
            var departmentId2 = _configurableFieldModelFactory.CreateNullableIntegerField(settings.User.DepartmentId2, fields.User.UserDepartment_Id2);
            var info = _configurableFieldModelFactory.CreateStringField(settings.User.Info, fields.User.InfoUser);
            var activity = _configurableFieldModelFactory.CreateStringField(settings.User.Activity, fields.User.Activity);
            var manager = _configurableFieldModelFactory.CreateStringField(settings.User.Manager, fields.User.Manager);
            var ordererId = _configurableFieldModelFactory.CreateMultiStringField(settings.Orderer.OrdererId, fields.Orderer.OrdererId);
            var ordererName = _configurableFieldModelFactory.CreateStringField(settings.Orderer.OrdererName, fields.Orderer.OrdererName);
            var ordererLocation = _configurableFieldModelFactory.CreateStringField(settings.Orderer.OrdererLocation, fields.Orderer.OrdererLocation);
            var ordererEmail = _configurableFieldModelFactory.CreateStringField(settings.Orderer.OrdererEmail, fields.Orderer.OrdererEmail);
            var ordererPhone = _configurableFieldModelFactory.CreateStringField(settings.Orderer.OrdererPhone, fields.Orderer.OrdererPhone);
            var ordererCode = _configurableFieldModelFactory.CreateStringField(settings.Orderer.OrdererCode, fields.Orderer.OrdererCode);
            var department = _configurableFieldModelFactory.CreateNullableIntegerField(settings.Orderer.Department, fields.Orderer.DepartmentId);
            var unit = _configurableFieldModelFactory.CreateNullableIntegerField(settings.Orderer.Unit, fields.Orderer.UnitId);
            var ordererAddress = _configurableFieldModelFactory.CreateStringField(settings.Orderer.OrdererAddress, fields.Orderer.OrdererAddress);
            var ordererInvoiceAddress = _configurableFieldModelFactory.CreateStringField(settings.Orderer.OrdererInvoiceAddress, fields.Orderer.OrdererInvoiceAddress);
            var ordererReferenceNumber = _configurableFieldModelFactory.CreateStringField(settings.Orderer.OrdererReferenceNumber, fields.Orderer.OrdererReferenceNumber);
            var accountingDimension1 = _configurableFieldModelFactory.CreateStringField(settings.Orderer.AccountingDimension1, fields.Orderer.AccountingDimension1);
            var accountingDimension2 = _configurableFieldModelFactory.CreateStringField(settings.Orderer.AccountingDimension2, fields.Orderer.AccountingDimension2);
            var accountingDimension3 = _configurableFieldModelFactory.CreateStringField(settings.Orderer.AccountingDimension3, fields.Orderer.AccountingDimension3);
            var accountingDimension4 = _configurableFieldModelFactory.CreateStringField(settings.Orderer.AccountingDimension4, fields.Orderer.AccountingDimension4);
            var accountingDimension5 = _configurableFieldModelFactory.CreateStringField(settings.Orderer.AccountingDimension5, fields.Orderer.AccountingDimension5);

            var model = new UserInfoEditModel(
                            personalIdentityNumber,
                            ConfigurableFieldModel<string>.CreateUnshowable(),
                            extension,
                            title,
                            ConfigurableFieldModel<string>.CreateUnshowable(),
                            roomNumber,
                            ConfigurableFieldModel<string>.CreateUnshowable(),
                            employmentType,
                            department,
                            unit,
                            departmentId2,
                            info,
                            ConfigurableFieldModel<string>.CreateUnshowable(),
                            activity,
                            manager,
                            ConfigurableFieldModel<string>.CreateUnshowable(),
                            ordererId,
                            ordererName,
                            ordererLocation,
                            ordererEmail,
                            ordererPhone,
                            ordererCode,
                            ordererAddress,
                            ordererInvoiceAddress,
                            ordererReferenceNumber,
                            accountingDimension1,
                            accountingDimension2,
                            accountingDimension3,
                            accountingDimension4,
                            accountingDimension5);
            model.EmploymentTypes = CreateSelectListField(settings.User.EmploymentType,
                options.EmploymentTypes, fields.User.EmploymentType_Id.ToString());
            model.Departments = CreateSelectListField(settings.Orderer.Department,
                options.Departments, fields.Orderer.DepartmentId.ToString());
            model.Departments2 = CreateSelectListField(settings.User.DepartmentId2,
                options.Departments, fields.User.UserDepartment_Id2.ToString());
            model.Units = CreateSelectListField(settings.Orderer.Unit,
                options.Units, fields.Orderer.UnitId.ToString());
            model.Regions = CreateSelectListField(settings.Orderer.Department,
                options.Regions, fields.User.RegionId.ToString());
            model.Header = settings.User.Header;

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