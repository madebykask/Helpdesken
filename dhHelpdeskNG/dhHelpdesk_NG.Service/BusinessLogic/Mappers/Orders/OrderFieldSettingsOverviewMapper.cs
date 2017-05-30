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
                                               ShowInList = f.ShowInList,
                                               FieldHelp = f.FieldHelp
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
            var accountInfo = CreateAccountInfoFieldSettingsOverview(fieldSettings);
            var contact = CreateContactFieldSettingsOverview(fieldSettings);

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
                                                user,
                                                accountInfo,
                                                contact);
        }

        private static DeliveryFieldSettingsOverview CreateDeliveryFieldSettingsOverview(NamedObjectCollection<OrdersFieldSettingsOverviewMapData> fieldSettings)
        {
            var deliveryDate = CreateFieldSetting(fieldSettings.FindByName(OrderFields.DeliveryDate));
            var installDate = CreateFieldSetting(fieldSettings.FindByName(OrderFields.DeliveryInstallDate));
            var deliveryDepartment = CreateFieldSetting(fieldSettings.FindByName(OrderFields.DeliveryDepartment));
            var deliveryOu = CreateFieldSetting(fieldSettings.FindByName(OrderFields.DeliveryOu));
            var deliveryAddress = CreateFieldSetting(fieldSettings.FindByName(OrderFields.DeliveryAddress));
            var deliveryPostalCode = CreateFieldSetting(fieldSettings.FindByName(OrderFields.DeliveryPostalCode));
            var deliveryPostalAddress = CreateFieldSetting(fieldSettings.FindByName(OrderFields.DeliveryPostalAddress));
            var deliveryLocation = CreateFieldSetting(fieldSettings.FindByName(OrderFields.DeliveryLocation));
            var deliveryInfo1 = CreateFieldSetting(fieldSettings.FindByName(OrderFields.DeliveryInfo1));
            var deliveryInfo2 = CreateFieldSetting(fieldSettings.FindByName(OrderFields.DeliveryInfo2));
            var deliveryInfo3 = CreateFieldSetting(fieldSettings.FindByName(OrderFields.DeliveryInfo3));
            var deliveryOuId = CreateFieldSetting(fieldSettings.FindByName(OrderFields.DeliveryOuId));
            var deliveryName = CreateFieldSetting(fieldSettings.FindByName(OrderFields.DeliveryName));
            var deliveryPhone = CreateFieldSetting(fieldSettings.FindByName(OrderFields.DeliveryPhone));

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
                                                deliveryOuId,
                                                deliveryName,
                                                deliveryPhone);
        }

        private static GeneralFieldSettingsOverview CreateGeneralFieldSettingsOverview(NamedObjectCollection<OrdersFieldSettingsOverviewMapData> fieldSettings)
        {
            var orderNumber = CreateFieldSetting(fieldSettings.FindByName(OrderFields.GeneralOrderNumber));
            var customer = CreateFieldSetting(fieldSettings.FindByName(OrderFields.GeneralCustomer));
            var administrator = CreateFieldSetting(fieldSettings.FindByName(OrderFields.GeneralAdministrator));
            var domain = CreateFieldSetting(fieldSettings.FindByName(OrderFields.GeneralDomain));
            var orderDate = CreateFieldSetting(fieldSettings.FindByName(OrderFields.GeneralOrderDate));

            return new GeneralFieldSettingsOverview(
                                                orderNumber,
                                                customer,
                                                administrator,
                                                domain,
                                                orderDate);
        }

        private static LogFieldSettingsOverview CreateLogFieldSettingsOverview(NamedObjectCollection<OrdersFieldSettingsOverviewMapData> fieldSettings)
        {
            var log = CreateFieldSetting(fieldSettings.FindByName(OrderFields.Log));

            return new LogFieldSettingsOverview(log);
        }

        private static OrdererFieldSettingsOverview CreateOrdererFieldSettingsOverview(NamedObjectCollection<OrdersFieldSettingsOverviewMapData> fieldSettings)
        {
            var ordererId = CreateFieldSetting(fieldSettings.FindByName(OrderFields.OrdererId));
            var ordererName = CreateFieldSetting(fieldSettings.FindByName(OrderFields.OrdererName));
            var ordererLocation = CreateFieldSetting(fieldSettings.FindByName(OrderFields.OrdererLocation));
            var ordererEmail = CreateFieldSetting(fieldSettings.FindByName(OrderFields.OrdererEmail));
            var ordererPhone = CreateFieldSetting(fieldSettings.FindByName(OrderFields.OrdererPhone));
            var ordererCode = CreateFieldSetting(fieldSettings.FindByName(OrderFields.OrdererCode));
            var department = CreateFieldSetting(fieldSettings.FindByName(OrderFields.OrdererDepartment));
            var unit = CreateFieldSetting(fieldSettings.FindByName(OrderFields.OrdererUnit));
            var ordererAddress = CreateFieldSetting(fieldSettings.FindByName(OrderFields.OrdererAddress));
            var ordererInvoiceAddress = CreateFieldSetting(fieldSettings.FindByName(OrderFields.OrdererInvoiceAddress));
            var ordererReferenceNumber = CreateFieldSetting(fieldSettings.FindByName(OrderFields.OrdererReferenceNumber));
            var accountingDimension1 = CreateFieldSetting(fieldSettings.FindByName(OrderFields.OrdererAccountingDimension1));
            var accountingDimension2 = CreateFieldSetting(fieldSettings.FindByName(OrderFields.OrdererAccountingDimension2));
            var accountingDimension3 = CreateFieldSetting(fieldSettings.FindByName(OrderFields.OrdererAccountingDimension3));
            var accountingDimension4 = CreateFieldSetting(fieldSettings.FindByName(OrderFields.OrdererAccountingDimension4));
            var accountingDimension5 = CreateFieldSetting(fieldSettings.FindByName(OrderFields.OrdererAccountingDimension5));

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
            var property = CreateFieldSetting(fieldSettings.FindByName(OrderFields.OrderProperty));
            var orderRow1 = CreateFieldSetting(fieldSettings.FindByName(OrderFields.OrderRow1));
            var orderRow2 = CreateFieldSetting(fieldSettings.FindByName(OrderFields.OrderRow2));
            var orderRow3 = CreateFieldSetting(fieldSettings.FindByName(OrderFields.OrderRow3));
            var orderRow4 = CreateFieldSetting(fieldSettings.FindByName(OrderFields.OrderRow4));
            var orderRow5 = CreateFieldSetting(fieldSettings.FindByName(OrderFields.OrderRow5));
            var orderRow6 = CreateFieldSetting(fieldSettings.FindByName(OrderFields.OrderRow6));
            var orderRow7 = CreateFieldSetting(fieldSettings.FindByName(OrderFields.OrderRow7));
            var orderRow8 = CreateFieldSetting(fieldSettings.FindByName(OrderFields.OrderRow8));
            var configuration = CreateFieldSetting(fieldSettings.FindByName(OrderFields.OrderConfiguration));
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
            var fileName = CreateFieldSetting(fieldSettings.FindByName(OrderFields.OtherFileName));
            var caseNumber = CreateFieldSetting(fieldSettings.FindByName(OrderFields.OtherCaseNumber));
            var info = CreateFieldSetting(fieldSettings.FindByName(OrderFields.OtherInfo));
            var status = CreateFieldSetting(fieldSettings.FindByName(OrderFields.OtherStatus));

            return new OtherFieldSettingsOverview(
                                                fileName,
                                                caseNumber,
                                                info,
                                                status);
        }

        private static ProgramFieldSettingsOverview CreateProgramFieldSettingsOverview(NamedObjectCollection<OrdersFieldSettingsOverviewMapData> fieldSettings)
        {
            var program = CreateFieldSetting(fieldSettings.FindByName(OrderFields.Program));
            var infoProduct = CreateFieldSetting(fieldSettings.FindByName(OrderFields.ProgramInfoProduct));

            return new ProgramFieldSettingsOverview(program, infoProduct);
        }

        private static ReceiverFieldSettingsOverview CreateReceiverFieldSettingsOverview(NamedObjectCollection<OrdersFieldSettingsOverviewMapData> fieldSettings)
        {
            var receiverId = CreateFieldSetting(fieldSettings.FindByName(OrderFields.ReceiverId));
            var receiverName = CreateFieldSetting(fieldSettings.FindByName(OrderFields.ReceiverName));
            var receiverEmail = CreateFieldSetting(fieldSettings.FindByName(OrderFields.ReceiverEmail));
            var receiverPhone = CreateFieldSetting(fieldSettings.FindByName(OrderFields.ReceiverPhone));
            var receiverLocation = CreateFieldSetting(fieldSettings.FindByName(OrderFields.ReceiverLocation));
            var markOfGoods = CreateFieldSetting(fieldSettings.FindByName(OrderFields.ReceiverMarkOfGoods));

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
            var supplierOrderNumber = CreateFieldSetting(fieldSettings.FindByName(OrderFields.SupplierOrderNumber));
            var supplierOrderDate = CreateFieldSetting(fieldSettings.FindByName(OrderFields.SupplierOrderDate));
            var supplierOrderInfo = CreateFieldSetting(fieldSettings.FindByName(OrderFields.SupplierOrderInfo));

            return new SupplierFieldSettingsOverview(
                                                supplierOrderNumber,
                                                supplierOrderDate,
                                                supplierOrderInfo);
        }

        private static UserFieldSettingsOverview CreateUserFieldSettingsOverview(NamedObjectCollection<OrdersFieldSettingsOverviewMapData> fieldSettings)
        {
            var userId = CreateFieldSetting(fieldSettings.FindByName(OrderFields.UserId));
            var userFirstName = CreateFieldSetting(fieldSettings.FindByName(OrderFields.UserFirstName));
            var userLastName = CreateFieldSetting(fieldSettings.FindByName(OrderFields.UserLastName));
            var userPhone = CreateFieldSetting(fieldSettings.FindByName(OrderFields.UserPhone));
            var userEMail = CreateFieldSetting(fieldSettings.FindByName(OrderFields.UserEMail));
            var userInitials = CreateFieldSetting(fieldSettings.FindByName(OrderFields.UserInitials));
            var userPersonalIdentityNumber = CreateFieldSetting(fieldSettings.FindByName(OrderFields.UserPersonalIdentityNumber));
            var userExtension = CreateFieldSetting(fieldSettings.FindByName(OrderFields.UserExtension));
            var userTitle = CreateFieldSetting(fieldSettings.FindByName(OrderFields.UserTitle));
            var userLocation = CreateFieldSetting(fieldSettings.FindByName(OrderFields.UserLocation));
            var userRoomNumber = CreateFieldSetting(fieldSettings.FindByName(OrderFields.UserRoomNumber));
            var userPostalAddress = CreateFieldSetting(fieldSettings.FindByName(OrderFields.UserPostalAddress));
            var responsibility = CreateFieldSetting(fieldSettings.FindByName(OrderFields.UserResponsibility));
            var activity = CreateFieldSetting(fieldSettings.FindByName(OrderFields.UserActivity));
            var manager = CreateFieldSetting(fieldSettings.FindByName(OrderFields.UserManager));
            var referenceNumber = CreateFieldSetting(fieldSettings.FindByName(OrderFields.UserReferenceNumber));
            var infoUser = CreateFieldSetting(fieldSettings.FindByName(OrderFields.UserInfo));
            var userOU_Id = CreateFieldSetting(fieldSettings.FindByName(OrderFields.UserOU_Id));
            var employmentType = CreateFieldSetting(fieldSettings.FindByName(OrderFields.UserEmploymentType));
            var userDepartment_Id1 = CreateFieldSetting(fieldSettings.FindByName(OrderFields.UserDepartment_Id1));
            var userDepartment_Id2 = CreateFieldSetting(fieldSettings.FindByName(OrderFields.UserDepartment_Id2));

            return new UserFieldSettingsOverview(
                                                userId,
                                                userFirstName,
                                                userLastName,
                                                userPhone,
                                                userEMail,
                                                userInitials,
                                                userPersonalIdentityNumber,
                                                userExtension,
                                                userTitle,
                                                userLocation,
                                                userRoomNumber,
                                                userPostalAddress,
                                                responsibility,
                                                activity,
                                                manager,
                                                referenceNumber,
                                                infoUser,
                                                userOU_Id,
                                                employmentType,
                                                userDepartment_Id1,
                                                userDepartment_Id2);
        }

        private static AccountInfoFieldSettingsOverview CreateAccountInfoFieldSettingsOverview(
            NamedObjectCollection<OrdersFieldSettingsOverviewMapData> fieldSettings)
        {
            var startDate = CreateFieldSetting(fieldSettings.FindByName(OrderFields.AccountInfoStartedDate));
            var endDate = CreateFieldSetting(fieldSettings.FindByName(OrderFields.AccountInfoFinishDate));
            var emailType = CreateFieldSetting(fieldSettings.FindByName(OrderFields.AccountInfoEMailTypeId));
            var homeDirectory = CreateFieldSetting(fieldSettings.FindByName(OrderFields.AccountInfoHomeDirectory));
            var profile = CreateFieldSetting(fieldSettings.FindByName(OrderFields.AccountInfoProfile));
            var inventoryNumber = CreateFieldSetting(fieldSettings.FindByName(OrderFields.AccountInfoInventoryNumber));
            var accountType = CreateFieldSetting(fieldSettings.FindByName(OrderFields.AccountInfoAccountType));
            var accountType2 = CreateFieldSetting(fieldSettings.FindByName(OrderFields.AccountInfoAccountType2));
            var accountType3 = CreateFieldSetting(fieldSettings.FindByName(OrderFields.AccountInfoAccountType3));
            var accountType4 = CreateFieldSetting(fieldSettings.FindByName(OrderFields.AccountInfoAccountType4));
            var accountType5 = CreateFieldSetting(fieldSettings.FindByName(OrderFields.AccountInfoAccountType5));
            var info = CreateFieldSetting(fieldSettings.FindByName(OrderFields.AccountInfo));

            return new AccountInfoFieldSettingsOverview(
                startDate,
                endDate,
                emailType,
                homeDirectory,
                profile,
                inventoryNumber,
                accountType,
                accountType2,
                accountType3,
                accountType4,
                accountType5,
                info);
        }

        private static ContactFieldSettingsOverview CreateContactFieldSettingsOverview(
            NamedObjectCollection<OrdersFieldSettingsOverviewMapData> fieldSettings)
        {
            var id = CreateFieldSetting(fieldSettings.FindByName(OrderFields.ContactId));
            var name = CreateFieldSetting(fieldSettings.FindByName(OrderFields.ContactName));
            var phone = CreateFieldSetting(fieldSettings.FindByName(OrderFields.ContactPhone));
            var email = CreateFieldSetting(fieldSettings.FindByName(OrderFields.ContactEMail));

            return new ContactFieldSettingsOverview(id,
                name,
                phone,
                email);
        }

        private static FieldOverviewSetting CreateFieldSetting(OrdersFieldSettingsOverviewMapData fieldSetting)
        {
            return new FieldOverviewSetting(fieldSetting.IsShowInList(), fieldSetting.GetFieldCaption());
        }

        #endregion
    }
}