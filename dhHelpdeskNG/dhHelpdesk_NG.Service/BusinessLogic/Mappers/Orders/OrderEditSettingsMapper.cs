namespace DH.Helpdesk.Services.BusinessLogic.Mappers.Orders
{
    using System.Linq;

    using DH.Helpdesk.BusinessData.Enums.Orders.Fields;
    using DH.Helpdesk.BusinessData.Models.Orders.Order.OrderEditSettings;
    using DH.Helpdesk.Common.Collections;
    using DH.Helpdesk.Common.Extensions.Integer;
    using DH.Helpdesk.Domain;
    using DH.Helpdesk.Services.BusinessLogic.Mappers.Orders.Data;

    public static class OrderEditSettingsMapper
    {
        public static FullOrderEditSettings MapToFullOrderEditSettings(this IQueryable<OrderFieldSettings> query)
        {
            var entities = query.Select(f => new OrdersEditSettingsMapData
                                                 {
                                                     OrderField = f.OrderField,
                                                     Show = f.Show,
                                                     Label = f.Label,
                                                     Required = f.Required,
                                                     EmailIdentifier = f.EMailIdentifier,
                                                     DefaultValue = f.DefaultValue,
                                                     FieldHelp = f.FieldHelp
                                                 }).ToList();

            var fieldSettings = new NamedObjectCollection<OrdersEditSettingsMapData>(entities);

            return new FullOrderEditSettings(
                        CreateDeliveryEditSettings(fieldSettings),
                        CreateGeneralEditSettings(fieldSettings),
                        CreateLogEditSettings(fieldSettings),
                        CreateOrdererEditSettings(fieldSettings),
                        CreateOrderEditSettings(fieldSettings),
                        CreateOtherEditSettings(fieldSettings),
                        CreateProgramEditSettings(fieldSettings),
                        CreateReceiverEditSettings(fieldSettings),
                        CreateSupplierEditSettings(fieldSettings),
                        CreateUserEditSettings(fieldSettings),
                        CreateAccountInfoEditSettings(fieldSettings),
                        CreateContactEditSettings(fieldSettings));
        }

        private static DeliveryEditSettings CreateDeliveryEditSettings(
            NamedObjectCollection<OrdersEditSettingsMapData> editSettings)
        {
            return new DeliveryEditSettings(
                    CreateTextFieldSetting(editSettings.FindByName(DeliveryFields.DeliveryDate)),
                    CreateTextFieldSetting(editSettings.FindByName(DeliveryFields.InstallDate)),
                    CreateTextFieldSetting(editSettings.FindByName(DeliveryFields.DeliveryDepartment)),
                    CreateTextFieldSetting(editSettings.FindByName(DeliveryFields.DeliveryOu)),
                    CreateTextFieldSetting(editSettings.FindByName(DeliveryFields.DeliveryAddress)),
                    CreateTextFieldSetting(editSettings.FindByName(DeliveryFields.DeliveryPostalCode)),
                    CreateTextFieldSetting(editSettings.FindByName(DeliveryFields.DeliveryPostalAddress)),
                    CreateTextFieldSetting(editSettings.FindByName(DeliveryFields.DeliveryLocation)),
                    CreateTextFieldSetting(editSettings.FindByName(DeliveryFields.DeliveryInfo1)),
                    CreateTextFieldSetting(editSettings.FindByName(DeliveryFields.DeliveryInfo2)),
                    CreateTextFieldSetting(editSettings.FindByName(DeliveryFields.DeliveryInfo3)),
                    CreateTextFieldSetting(editSettings.FindByName(DeliveryFields.DeliveryOuId)));
        }

        private static GeneralEditSettings CreateGeneralEditSettings(
            NamedObjectCollection<OrdersEditSettingsMapData> editSettings)
        {
            return new GeneralEditSettings(
                    CreateFieldSetting(editSettings.FindByName(GeneralFields.OrderNumber)),
                    CreateFieldSetting(editSettings.FindByName(GeneralFields.Customer)),
                    CreateTextFieldSetting(editSettings.FindByName(GeneralFields.Administrator)),
                    CreateTextFieldSetting(editSettings.FindByName(GeneralFields.Domain)),
                    CreateTextFieldSetting(editSettings.FindByName(GeneralFields.OrderDate)),
                    CreateTextFieldSetting(editSettings.FindByName(GeneralFields.Status)));
        }

        private static LogEditSettings CreateLogEditSettings(
            NamedObjectCollection<OrdersEditSettingsMapData> editSettings)
        {
            return new LogEditSettings(
                    CreateTextFieldSetting(editSettings.FindByName(LogFields.Log)));
        }

        private static OrdererEditSettings CreateOrdererEditSettings(
            NamedObjectCollection<OrdersEditSettingsMapData> editSettings)
        {
            return new OrdererEditSettings(
                    CreateTextFieldSetting(editSettings.FindByName(OrdererFields.OrdererId)),
                    CreateTextFieldSetting(editSettings.FindByName(OrdererFields.OrdererName)),
                    CreateTextFieldSetting(editSettings.FindByName(OrdererFields.OrdererLocation)),
                    CreateTextFieldSetting(editSettings.FindByName(OrdererFields.OrdererEmail)),
                    CreateTextFieldSetting(editSettings.FindByName(OrdererFields.OrdererPhone)),
                    CreateTextFieldSetting(editSettings.FindByName(OrdererFields.OrdererCode)),
                    CreateTextFieldSetting(editSettings.FindByName(OrdererFields.Department)),
                    CreateTextFieldSetting(editSettings.FindByName(OrdererFields.Unit)),
                    CreateTextFieldSetting(editSettings.FindByName(OrdererFields.OrdererAddress)),
                    CreateTextFieldSetting(editSettings.FindByName(OrdererFields.OrdererInvoiceAddress)),
                    CreateTextFieldSetting(editSettings.FindByName(OrdererFields.OrdererReferenceNumber)),
                    CreateTextFieldSetting(editSettings.FindByName(OrdererFields.AccountingDimension1)),
                    CreateFieldSetting(editSettings.FindByName(OrdererFields.AccountingDimension2)),
                    CreateTextFieldSetting(editSettings.FindByName(OrdererFields.AccountingDimension3)),
                    CreateFieldSetting(editSettings.FindByName(OrdererFields.AccountingDimension4)),
                    CreateTextFieldSetting(editSettings.FindByName(OrdererFields.AccountingDimension5)));
        }

        private static OrderEditSettings CreateOrderEditSettings(
            NamedObjectCollection<OrdersEditSettingsMapData> editSettings)
        {
            return new OrderEditSettings(
                    CreateFieldSetting(editSettings.FindByName(OrderFields.Property)),
                    CreateTextFieldSetting(editSettings.FindByName(OrderFields.OrderRow1)),
                    CreateTextFieldSetting(editSettings.FindByName(OrderFields.OrderRow2)),
                    CreateTextFieldSetting(editSettings.FindByName(OrderFields.OrderRow3)),
                    CreateTextFieldSetting(editSettings.FindByName(OrderFields.OrderRow4)),
                    CreateTextFieldSetting(editSettings.FindByName(OrderFields.OrderRow5)),
                    CreateTextFieldSetting(editSettings.FindByName(OrderFields.OrderRow6)),
                    CreateTextFieldSetting(editSettings.FindByName(OrderFields.OrderRow7)),
                    CreateTextFieldSetting(editSettings.FindByName(OrderFields.OrderRow8)),
                    CreateTextFieldSetting(editSettings.FindByName(OrderFields.Configuration)),
                    CreateTextFieldSetting(editSettings.FindByName(OrderFields.OrderInfo)),
                    CreateTextFieldSetting(editSettings.FindByName(OrderFields.OrderInfo2)));
        }

        private static OtherEditSettings CreateOtherEditSettings(
            NamedObjectCollection<OrdersEditSettingsMapData> editSettings)
        {
            return new OtherEditSettings(
                    CreateTextFieldSetting(editSettings.FindByName(OtherFields.FileName)),
                    CreateTextFieldSetting(editSettings.FindByName(OtherFields.CaseNumber)),
                    CreateTextFieldSetting(editSettings.FindByName(OtherFields.Info)));
        }

        private static ProgramEditSettings CreateProgramEditSettings(
            NamedObjectCollection<OrdersEditSettingsMapData> editSettings)
        {
            return new ProgramEditSettings(
                CreateTextFieldSetting(editSettings.FindByName(ProgramFields.Program)));
        }

        private static ReceiverEditSettings CreateReceiverEditSettings(
            NamedObjectCollection<OrdersEditSettingsMapData> editSettings)
        {
            return new ReceiverEditSettings(
                    CreateTextFieldSetting(editSettings.FindByName(ReceiverFields.ReceiverId)),
                    CreateTextFieldSetting(editSettings.FindByName(ReceiverFields.ReceiverName)),
                    CreateTextFieldSetting(editSettings.FindByName(ReceiverFields.ReceiverEmail)),
                    CreateTextFieldSetting(editSettings.FindByName(ReceiverFields.ReceiverPhone)),
                    CreateTextFieldSetting(editSettings.FindByName(ReceiverFields.ReceiverLocation)),
                    CreateTextFieldSetting(editSettings.FindByName(ReceiverFields.MarkOfGoods)));
        }

        private static SupplierEditSettings CreateSupplierEditSettings(
            NamedObjectCollection<OrdersEditSettingsMapData> editSettings)
        {
            return new SupplierEditSettings(
                    CreateTextFieldSetting(editSettings.FindByName(SupplierFields.SupplierOrderNumber)),
                    CreateTextFieldSetting(editSettings.FindByName(SupplierFields.SupplierOrderDate)),
                    CreateTextFieldSetting(editSettings.FindByName(SupplierFields.SupplierOrderInfo)));
        }

        private static UserEditSettings CreateUserEditSettings(
            NamedObjectCollection<OrdersEditSettingsMapData> editSettings)
        {
            return new UserEditSettings(
                    CreateTextFieldSetting(editSettings.FindByName(UserFields.UserId)),
                    CreateTextFieldSetting(editSettings.FindByName(UserFields.UserFirstName)),
                    CreateTextFieldSetting(editSettings.FindByName(UserFields.UserLastName)),
                    CreateTextFieldSetting(editSettings.FindByName(UserFields.UserPhone)),
                    CreateTextFieldSetting(editSettings.FindByName(UserFields.UserEMail)),
                    CreateTextFieldSetting(editSettings.FindByName(UserFields.UserInitials)),
                    CreateTextFieldSetting(editSettings.FindByName(UserFields.UserPersonalIdentityNumber)),
                    CreateTextFieldSetting(editSettings.FindByName(UserFields.UserExtension)),
                    CreateTextFieldSetting(editSettings.FindByName(UserFields.UserTitle)),
                    CreateTextFieldSetting(editSettings.FindByName(UserFields.UserLocation)),
                    CreateTextFieldSetting(editSettings.FindByName(UserFields.UserRoomNumber)),
                    CreateTextFieldSetting(editSettings.FindByName(UserFields.UserPostalAddress)),
                    CreateTextFieldSetting(editSettings.FindByName(UserFields.EmploymentType)),
                    CreateTextFieldSetting(editSettings.FindByName(UserFields.UserDepartment_Id1)),
                    CreateTextFieldSetting(editSettings.FindByName(UserFields.UserOU_Id)),
                    CreateTextFieldSetting(editSettings.FindByName(UserFields.UserDepartment_Id2)),
                    CreateTextFieldSetting(editSettings.FindByName(UserFields.InfoUser)),
                    CreateTextFieldSetting(editSettings.FindByName(UserFields.Responsibility)),
                    CreateTextFieldSetting(editSettings.FindByName(UserFields.Activity)),
                    CreateTextFieldSetting(editSettings.FindByName(UserFields.Manager)),
                    CreateTextFieldSetting(editSettings.FindByName(UserFields.ReferenceNumber)));
        }

        private static AccountInfoEditSettings CreateAccountInfoEditSettings(
            NamedObjectCollection<OrdersEditSettingsMapData> editSettings)
        {
            return new AccountInfoEditSettings(
                    CreateFieldSetting(editSettings.FindByName(AccountInfoFields.StartedDate)),
                    CreateFieldSetting(editSettings.FindByName(AccountInfoFields.FinishDate)),
                    CreateFieldSetting(editSettings.FindByName(AccountInfoFields.EMailTypeId)),
                    CreateFieldSetting(editSettings.FindByName(AccountInfoFields.HomeDirectory)),
                    CreateFieldSetting(editSettings.FindByName(AccountInfoFields.Profile)),
                    CreateTextFieldSetting(editSettings.FindByName(AccountInfoFields.InventoryNumber)),
                    CreateTextFieldSetting(editSettings.FindByName(AccountInfoFields.Info)),
                    CreateFieldSetting(editSettings.FindByName(AccountInfoFields.AccountType)),
                    CreateFieldSetting(editSettings.FindByName(AccountInfoFields.AccountType2)),
                    CreateFieldSetting(editSettings.FindByName(AccountInfoFields.AccountType3)),
                    CreateFieldSetting(editSettings.FindByName(AccountInfoFields.AccountType4)),
                    CreateFieldSetting(editSettings.FindByName(AccountInfoFields.AccountType5)));
        }

        private static ContactEditSettings CreateContactEditSettings(
            NamedObjectCollection<OrdersEditSettingsMapData> editSettings)
        {
            return new ContactEditSettings(
                    CreateTextFieldSetting(editSettings.FindByName(ContactFields.ContactId)),
                    CreateTextFieldSetting(editSettings.FindByName(ContactFields.ContactName)),
                    CreateTextFieldSetting(editSettings.FindByName(ContactFields.ContactPhone)),
                    CreateTextFieldSetting(editSettings.FindByName(ContactFields.ContactEMail))
                );
        }

        private static FieldEditSettings CreateFieldSetting(OrdersEditSettingsMapData data)
        {
            return new FieldEditSettings(
                        data.Show.ToBool(),
                        data.Label,
                        data.Required.ToBool(),
                        data.EmailIdentifier,
                        data.FieldHelp);
        }

        private static TextFieldEditSettings CreateTextFieldSetting(OrdersEditSettingsMapData data)
        {
            return new TextFieldEditSettings(
                        data.Show.ToBool(),
                        data.Label,
                        data.Required.ToBool(),
                        data.EmailIdentifier,
                        data.DefaultValue,
                        data.FieldHelp);
        }
    }
}