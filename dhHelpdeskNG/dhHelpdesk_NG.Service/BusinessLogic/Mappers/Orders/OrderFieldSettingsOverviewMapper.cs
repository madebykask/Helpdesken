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
            var deliveryName = CreateFieldSetting(fieldSettings.FindByName(DeliveryFields.DeliveryName));
            var deliveryPhone = CreateFieldSetting(fieldSettings.FindByName(DeliveryFields.DeliveryPhone));

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
            var infoProduct = CreateFieldSetting(fieldSettings.FindByName(ProgramFields.InfoProduct));

            return new ProgramFieldSettingsOverview(program, infoProduct);
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
            var userPhone = CreateFieldSetting(fieldSettings.FindByName(UserFields.UserPhone));
            var userEMail = CreateFieldSetting(fieldSettings.FindByName(UserFields.UserEMail));
            var userInitials = CreateFieldSetting(fieldSettings.FindByName(UserFields.UserInitials));
            var userPersonalIdentityNumber = CreateFieldSetting(fieldSettings.FindByName(UserFields.UserPersonalIdentityNumber));
            var userExtension = CreateFieldSetting(fieldSettings.FindByName(UserFields.UserExtension));
            var userTitle = CreateFieldSetting(fieldSettings.FindByName(UserFields.UserTitle));
            var userLocation = CreateFieldSetting(fieldSettings.FindByName(UserFields.UserLocation));
            var userRoomNumber = CreateFieldSetting(fieldSettings.FindByName(UserFields.UserRoomNumber));
            var userPostalAddress = CreateFieldSetting(fieldSettings.FindByName(UserFields.UserPostalAddress));
            var responsibility = CreateFieldSetting(fieldSettings.FindByName(UserFields.Responsibility));
            var activity = CreateFieldSetting(fieldSettings.FindByName(UserFields.Activity));
            var manager = CreateFieldSetting(fieldSettings.FindByName(UserFields.Manager));
            var referenceNumber = CreateFieldSetting(fieldSettings.FindByName(UserFields.ReferenceNumber));
            var infoUser = CreateFieldSetting(fieldSettings.FindByName(UserFields.InfoUser));
            var userOU_Id = CreateFieldSetting(fieldSettings.FindByName(UserFields.UserOU_Id));
            var employmentType = CreateFieldSetting(fieldSettings.FindByName(UserFields.EmploymentType));
            var userDepartment_Id1 = CreateFieldSetting(fieldSettings.FindByName(UserFields.UserDepartment_Id1));
            var userDepartment_Id2 = CreateFieldSetting(fieldSettings.FindByName(UserFields.UserDepartment_Id2));

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
            var startDate = CreateFieldSetting(fieldSettings.FindByName(AccountInfoFields.StartedDate));
            var endDate = CreateFieldSetting(fieldSettings.FindByName(AccountInfoFields.FinishDate));
            var emailType = CreateFieldSetting(fieldSettings.FindByName(AccountInfoFields.EMailTypeId));
            var homeDirectory = CreateFieldSetting(fieldSettings.FindByName(AccountInfoFields.HomeDirectory));
            var profile = CreateFieldSetting(fieldSettings.FindByName(AccountInfoFields.Profile));
            var inventoryNumber = CreateFieldSetting(fieldSettings.FindByName(AccountInfoFields.InventoryNumber));
            var accountType = CreateFieldSetting(fieldSettings.FindByName(AccountInfoFields.AccountType));
            var accountType2 = CreateFieldSetting(fieldSettings.FindByName(AccountInfoFields.AccountType2));
            var accountType3 = CreateFieldSetting(fieldSettings.FindByName(AccountInfoFields.AccountType3));
            var accountType4 = CreateFieldSetting(fieldSettings.FindByName(AccountInfoFields.AccountType4));
            var accountType5 = CreateFieldSetting(fieldSettings.FindByName(AccountInfoFields.AccountType5));
            var info = CreateFieldSetting(fieldSettings.FindByName(AccountInfoFields.Info));

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
            var id = CreateFieldSetting(fieldSettings.FindByName(ContactFields.ContactId));
            var name = CreateFieldSetting(fieldSettings.FindByName(ContactFields.ContactName));
            var phone = CreateFieldSetting(fieldSettings.FindByName(ContactFields.ContactPhone));
            var email = CreateFieldSetting(fieldSettings.FindByName(ContactFields.ContactEMail));

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