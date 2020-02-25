using System.Linq;
using System.Web.Mvc;
using DH.Helpdesk.BusinessData.Enums.Accounts.Fields;
using DH.Helpdesk.BusinessData.Models.Document.Output;
using DH.Helpdesk.BusinessData.Models.Shared;
using DH.Helpdesk.Common.Extensions.Integer;
using DH.Helpdesk.Dal.NewInfrastructure;
using DH.Helpdesk.Domain;
using DH.Helpdesk.Domain.Orders;
using DH.Helpdesk.Services.BusinessLogic.Specifications;
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
		private readonly IGlobalSettingService _globalSettingService;

		public NewOrderModelFactory(IConfigurableFieldModelFactory configurableFieldModelFactory, IGlobalSettingService globalSettingService)
        {
            _configurableFieldModelFactory = configurableFieldModelFactory;
			_globalSettingService = globalSettingService;

		}

        public FullOrderEditModel Create(string temporatyId, NewOrderEditData data, IWorkContext workContext, int? orderTypeId)
        {
			var fileUploadWhiteList = _globalSettingService.GetFileUploadWhiteList();

            return new FullOrderEditModel(
                CreateDeliveryEditModel(data.EditSettings.Delivery, data.EditOptions),
                CreateGeneralEditModel(data.EditSettings.General, data.EditOptions),
                CreateLogEditModel(data.EditSettings.Log, data.EditOptions),
                CreateOrderEditModel(data.EditSettings, data.EditOptions),
                CreateOtherEditModel(data.EditSettings.Other, data.EditOptions, temporatyId),
                CreateProgramEditModel(data.EditSettings.Program, data.EditOptions),
                CreateReceiverEditModel(data.EditSettings.Receiver),
                CreateSupplierEditModel(data.EditSettings.Supplier),
                CreateUserEditModel(data.EditSettings, workContext),
                CreateUserInfoEditModel(data.EditSettings, data.EditOptions, workContext),
                temporatyId,
                workContext.Customer.CustomerId,
                orderTypeId,
                true,
                null,
				fileUploadWhiteList)
            {
                CreateCase = workContext.Customer.Settings.CreateCaseFromOrder,
                Statuses = data.EditOptions.Statuses,
                OrderTypeDescription = data.EditOptions.OrderTypeDescription,
                OrderTypeDocument = data.EditOptions.OrderTypeDocument
            };
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
                deliveryPhone)
            {
                Departments = CreateSelectListField(settings.DeliveryDepartment, options.DeliveryDepartment, null),
                Units = CreateSelectListField(settings.DeliveryOuId, options.DeliveryOuId, null),
                Header = settings.Header
            };

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
                status)
            {
                Administrators = CreateSelectListField(settings.Administrator, options.Administrators, null),
                Domains = CreateSelectListField(settings.Domain, options.Domains, null),
                Header = settings.Header
            };


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

        private OrderEditModel CreateOrderEditModel(
                                FullOrderEditSettings settings,
                                OrderEditOptions options)
        {
            var property = _configurableFieldModelFactory.CreateNullableIntegerField(settings.Order.Property, null);
            var orderRow1 = _configurableFieldModelFactory.CreateStringField(settings.Order.OrderRow1, null);
            var orderRow2 = _configurableFieldModelFactory.CreateStringField(settings.Order.OrderRow2, null);
            var orderRow3 = _configurableFieldModelFactory.CreateStringField(settings.Order.OrderRow3, null);
            var orderRow4 = _configurableFieldModelFactory.CreateStringField(settings.Order.OrderRow4, null);
            var orderRow5 = _configurableFieldModelFactory.CreateStringField(settings.Order.OrderRow5, null);
            var orderRow6 = _configurableFieldModelFactory.CreateStringField(settings.Order.OrderRow6, null);
            var orderRow7 = _configurableFieldModelFactory.CreateStringField(settings.Order.OrderRow7, null);
            var orderRow8 = _configurableFieldModelFactory.CreateStringField(settings.Order.OrderRow8, null);
            var configuration = _configurableFieldModelFactory.CreateStringField(settings.Order.Configuration, null);
            var orderInfo = _configurableFieldModelFactory.CreateStringField(settings.Order.OrderInfo, null);
            var orderInfo2 = _configurableFieldModelFactory.CreateIntegerField(settings.Order.OrderInfo2, 0);
            var startedDate =
                _configurableFieldModelFactory.CreateNullableDateTimeField(settings.AccountInfo.StartedDate, null);
            var finishDate = _configurableFieldModelFactory.CreateNullableDateTimeField(
                settings.AccountInfo.FinishDate, null);
            var eMailTypeId = _configurableFieldModelFactory.CreateNullableIntegerField(
                settings.AccountInfo.EMailTypeId, null);
            var homeDirectory = _configurableFieldModelFactory.CreateBooleanField(settings.AccountInfo.HomeDirectory,
                false);
            var profile = _configurableFieldModelFactory.CreateBooleanField(settings.AccountInfo.Profile, false);
            var accountTypeId =
                _configurableFieldModelFactory.CreateNullableIntegerField(settings.AccountInfo.AccountTypeId, null);
            var accountTypeId2 =
                _configurableFieldModelFactory.CreateCheckBoxListField(settings.AccountInfo.AccountTypeId2, null,
                    options.AccountTypes2);
            var accountTypeId3 =
                _configurableFieldModelFactory.CreateNullableIntegerField(settings.AccountInfo.AccountTypeId3, null);
            var accountTypeId4 =
                _configurableFieldModelFactory.CreateNullableIntegerField(settings.AccountInfo.AccountTypeId4, null);
            var accountTypeId5 =
                _configurableFieldModelFactory.CreateNullableIntegerField(settings.AccountInfo.AccountTypeId5, null);

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

            model.Properties = CreateSelectListField(settings.Order.Property, options.Properties, null);
            model.EmailTypes = new EMailTypes().ToSelectListDipslay(null);
            model.AccountTypes = CreateSelectListField(settings.AccountInfo.AccountTypeId,
                    options.AccountTypes, null);
            model.AccountTypes2 = CreateMultiSelectListField(
                    settings.AccountInfo.AccountTypeId2, options.AccountTypes2, null);
            model.AccountTypes3 = CreateSelectListField(settings.AccountInfo.AccountTypeId3,
                    options.AccountTypes3, null);
            model.AccountTypes4 = CreateSelectListField(settings.AccountInfo.AccountTypeId4,
                    options.AccountTypes4, null);
            model.AccountTypes5 = CreateSelectListField(settings.AccountInfo.AccountTypeId5,
                    options.AccountTypes5, null);
            model.Header = settings.Order.Header;

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
                            info)
            {
                Header = settings.Header
            };
        }

        private ProgramEditModel CreateProgramEditModel(ProgramEditSettings settings, OrderEditOptions options)
        {
            var programs = _configurableFieldModelFactory.CreateCheckBoxListField(settings.Program, null, options.Programs);
            var infoProduct = _configurableFieldModelFactory.CreateStringField(settings.InfoProduct, null);
            var model = new ProgramEditModel(infoProduct, programs)
            {
                AllPrograms = CreateMultiSelectListField(settings.Program, options.Programs, null),
                Header = settings.Header
            };
            return model;
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
                            markOfGoods)
            {
                Header = settings.Header
            };
        }

        private SupplierEditModel CreateSupplierEditModel(SupplierEditSettings settings)
        {
            var supplierOrderNumber = _configurableFieldModelFactory.CreateStringField(settings.SupplierOrderNumber, null);
            var supplierOrderDate = _configurableFieldModelFactory.CreateNullableDateTimeField(settings.SupplierOrderDate, null);
            var supplierOrderInfo = _configurableFieldModelFactory.CreateStringField(settings.SupplierOrderInfo, null);

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
                        IWorkContext workContext)
        {
            var userId = _configurableFieldModelFactory.CreateStringField(settings.User.UserId, workContext.User.Login);
            var userFirstName = _configurableFieldModelFactory.CreateStringField(settings.User.UserFirstName, workContext.User.FirstName);
            var userLastName = _configurableFieldModelFactory.CreateStringField(settings.User.UserLastName, workContext.User.LastName);
            var userPhone = _configurableFieldModelFactory.CreateStringField(settings.User.UserPhone, workContext.User.Phone);
            var userEmail = _configurableFieldModelFactory.CreateStringField(settings.User.UserEMail, workContext.User.Email);

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
                OrderEditOptions options,
                IWorkContext workContext)
        {
            var personalIdentityNumber =
                _configurableFieldModelFactory.CreateStringField(settings.User.PersonalIdentityNumber, null);
            var extension = _configurableFieldModelFactory.CreateStringField(settings.User.Extension, null);
            var title = _configurableFieldModelFactory.CreateStringField(settings.User.Title, null);
            var roomNumber = _configurableFieldModelFactory.CreateStringField(settings.User.RoomNumber, null);
            var employmentType = _configurableFieldModelFactory.CreateNullableIntegerField(settings.User.EmploymentType, null);
            var departmentId2 = _configurableFieldModelFactory.CreateNullableIntegerField(settings.User.DepartmentId2, null);
            var info = _configurableFieldModelFactory.CreateStringField(settings.User.Info, null);
            var activity = _configurableFieldModelFactory.CreateStringField(settings.User.Activity, null);
            var manager = _configurableFieldModelFactory.CreateStringField(settings.User.Manager, null);
            var ordererId = _configurableFieldModelFactory.CreateMultiStringField(settings.Orderer.OrdererId, null);
            var ordererName = _configurableFieldModelFactory.CreateStringField(settings.Orderer.OrdererName, null);
            var ordererLocation = _configurableFieldModelFactory.CreateStringField(settings.Orderer.OrdererLocation, null);
            var ordererEmail = _configurableFieldModelFactory.CreateStringField(settings.Orderer.OrdererEmail, null);
            var ordererPhone = _configurableFieldModelFactory.CreateStringField(settings.Orderer.OrdererPhone, null);
            var ordererCode = _configurableFieldModelFactory.CreateStringField(settings.Orderer.OrdererCode, null);
            var department = _configurableFieldModelFactory.CreateNullableIntegerField(settings.Orderer.Department, null);
            var unit = _configurableFieldModelFactory.CreateNullableIntegerField(settings.Orderer.Unit, null);
            var ordererAddress = _configurableFieldModelFactory.CreateStringField(settings.Orderer.OrdererAddress, null);
            var ordererInvoiceAddress = _configurableFieldModelFactory.CreateStringField(settings.Orderer.OrdererInvoiceAddress, null);
            var ordererReferenceNumber = _configurableFieldModelFactory.CreateStringField(settings.Orderer.OrdererReferenceNumber, null);
            var accountingDimension1 = _configurableFieldModelFactory.CreateStringField(settings.Orderer.AccountingDimension1, null);
            var accountingDimension2 = _configurableFieldModelFactory.CreateStringField(settings.Orderer.AccountingDimension2, null);
            var accountingDimension3 = _configurableFieldModelFactory.CreateStringField(settings.Orderer.AccountingDimension3, null);
            var accountingDimension4 = _configurableFieldModelFactory.CreateStringField(settings.Orderer.AccountingDimension4, null);
            var accountingDimension5 = _configurableFieldModelFactory.CreateStringField(settings.Orderer.AccountingDimension5, null);

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
                options.EmploymentTypes, null);
            model.Departments = CreateSelectListField(settings.Orderer.Department,
                options.Departments, null);
            model.Departments2 = CreateSelectListField(settings.User.DepartmentId2,
                options.Departments, null);
            model.Units = CreateSelectListField(settings.Orderer.Unit,
                options.Units, null);
            model.Regions = CreateSelectListField(settings.Orderer.Department,
                options.Regions, null);
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
                return new MultiSelectList(Enumerable.Empty<ItemOverview>(), Enumerable.Empty<ItemOverview>());
            }


            var list = new MultiSelectList(items, "Value", "Name", selectedValue);
            return list;
        }

    }
}