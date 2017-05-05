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
        public static FullOrderEditSettings MapToFullOrderEditSettings(this IQueryable<OrderFieldSettings> query, OrderType orderType, bool useExtenal)
        {
            var entities = query.Select(f => new OrdersEditSettingsMapData
                                                 {
                                                     OrderField = f.OrderField,
                                                     Show = useExtenal ? f.ShowExternal : f.Show,
                                                     Label = f.Label,
                                                     Required = f.Required,
                                                     EmailIdentifier = f.EMailIdentifier,
                                                     DefaultValue = f.DefaultValue,
                                                     FieldHelp = f.FieldHelp,
                                                     MultiValue = f.MultiValue}).ToList();

            var fieldSettings = new NamedObjectCollection<OrdersEditSettingsMapData>(entities);

            return new FullOrderEditSettings(
                        CreateDeliveryEditSettings(fieldSettings, orderType?.CaptionDeliveryInfo ?? ""),
                        CreateGeneralEditSettings(fieldSettings, orderType?.CaptionGeneral ?? ""),
                        CreateLogEditSettings(fieldSettings),
                        CreateOrdererEditSettings(fieldSettings, orderType?.CaptionOrdererInfo ?? ""),
                        CreateOrderEditSettings(fieldSettings, orderType?.CaptionOrder ?? ""),
                        CreateOtherEditSettings(fieldSettings, orderType?.CaptionOther ?? ""),
                        CreateProgramEditSettings(fieldSettings, orderType?.CaptionProgram ?? ""),
                        CreateReceiverEditSettings(fieldSettings, orderType?.CaptionReceiverInfo ?? ""),
                        CreateSupplierEditSettings(fieldSettings, orderType?.CaptionOrderInfo ?? ""),
                        CreateUserEditSettings(fieldSettings, orderType?.CaptionUserInfo ?? ""),
                        CreateAccountInfoEditSettings(fieldSettings),
                        CreateContactEditSettings(fieldSettings));
        }

        private static DeliveryEditSettings CreateDeliveryEditSettings(
            NamedObjectCollection<OrdersEditSettingsMapData> editSettings, string headerText)
        {
            return new DeliveryEditSettings(
                CreateTextFieldSetting(editSettings.FindByName(OrderFields.DeliveryDate)),
                CreateTextFieldSetting(editSettings.FindByName(OrderFields.DeliveryInstallDate)),
                CreateTextFieldSetting(editSettings.FindByName(OrderFields.DeliveryDepartment)),
                CreateTextFieldSetting(editSettings.FindByName(OrderFields.DeliveryOu)),
                CreateTextFieldSetting(editSettings.FindByName(OrderFields.DeliveryAddress)),
                CreateTextFieldSetting(editSettings.FindByName(OrderFields.DeliveryPostalCode)),
                CreateTextFieldSetting(editSettings.FindByName(OrderFields.DeliveryPostalAddress)),
                CreateTextFieldSetting(editSettings.FindByName(OrderFields.DeliveryLocation)),
                CreateTextFieldSetting(editSettings.FindByName(OrderFields.DeliveryInfo1)),
                CreateTextFieldSetting(editSettings.FindByName(OrderFields.DeliveryInfo2)),
                CreateTextFieldSetting(editSettings.FindByName(OrderFields.DeliveryInfo3)),
                CreateTextFieldSetting(editSettings.FindByName(OrderFields.DeliveryOuId)),
                CreateTextFieldSetting(editSettings.FindByName(OrderFields.DeliveryName)),
                CreateTextFieldSetting(editSettings.FindByName(OrderFields.DeliveryPhone)))
            {
                Header = headerText
            };
        }

        private static GeneralEditSettings CreateGeneralEditSettings(
            NamedObjectCollection<OrdersEditSettingsMapData> editSettings, string headerText)
        {
            return new GeneralEditSettings(
                    CreateFieldSetting(editSettings.FindByName(OrderFields.GeneralOrderNumber)),
                    CreateFieldSetting(editSettings.FindByName(OrderFields.GeneralCustomer)),
                    CreateTextFieldSetting(editSettings.FindByName(OrderFields.GeneralAdministrator)),
                    CreateTextFieldSetting(editSettings.FindByName(OrderFields.GeneralDomain)),
                    CreateTextFieldSetting(editSettings.FindByName(OrderFields.GeneralOrderDate)),
                    CreateTextFieldSetting(editSettings.FindByName(OrderFields.GeneralStatus)))
            {
                Header = headerText
            };
        }

        private static LogEditSettings CreateLogEditSettings(
            NamedObjectCollection<OrdersEditSettingsMapData> editSettings)
        {
            return new LogEditSettings(
                    CreateTextFieldSetting(editSettings.FindByName(OrderFields.Log)));
        }

        private static OrdererEditSettings CreateOrdererEditSettings(
            NamedObjectCollection<OrdersEditSettingsMapData> editSettings, string headerText)
        {
            return new OrdererEditSettings(
                    CreateMultiTextFieldSetting(editSettings.FindByName(OrderFields.OrdererId)),
                    CreateTextFieldSetting(editSettings.FindByName(OrderFields.OrdererName)),
                    CreateTextFieldSetting(editSettings.FindByName(OrderFields.OrdererLocation)),
                    CreateTextFieldSetting(editSettings.FindByName(OrderFields.OrdererEmail)),
                    CreateTextFieldSetting(editSettings.FindByName(OrderFields.OrdererPhone)),
                    CreateTextFieldSetting(editSettings.FindByName(OrderFields.OrdererCode)),
                    CreateTextFieldSetting(editSettings.FindByName(OrderFields.OrdererDepartment)),
                    CreateTextFieldSetting(editSettings.FindByName(OrderFields.OrdererUnit)),
                    CreateTextFieldSetting(editSettings.FindByName(OrderFields.OrdererAddress)),
                    CreateTextFieldSetting(editSettings.FindByName(OrderFields.OrdererInvoiceAddress)),
                    CreateTextFieldSetting(editSettings.FindByName(OrderFields.OrdererReferenceNumber)),
                    CreateTextFieldSetting(editSettings.FindByName(OrderFields.OrdererAccountingDimension1)),
                    CreateFieldSetting(editSettings.FindByName(OrderFields.OrdererAccountingDimension2)),
                    CreateTextFieldSetting(editSettings.FindByName(OrderFields.OrdererAccountingDimension3)),
                    CreateFieldSetting(editSettings.FindByName(OrderFields.OrdererAccountingDimension4)),
                    CreateTextFieldSetting(editSettings.FindByName(OrderFields.OrdererAccountingDimension5)))
            {
                Header = headerText
            };
        }

        private static OrderEditSettings CreateOrderEditSettings(
            NamedObjectCollection<OrdersEditSettingsMapData> editSettings, string headerText)
        {
            return new OrderEditSettings(
                    CreateFieldSetting(editSettings.FindByName(OrderFields.OrderProperty)),
                    CreateTextFieldSetting(editSettings.FindByName(OrderFields.OrderRow1)),
                    CreateTextFieldSetting(editSettings.FindByName(OrderFields.OrderRow2)),
                    CreateTextFieldSetting(editSettings.FindByName(OrderFields.OrderRow3)),
                    CreateTextFieldSetting(editSettings.FindByName(OrderFields.OrderRow4)),
                    CreateTextFieldSetting(editSettings.FindByName(OrderFields.OrderRow5)),
                    CreateTextFieldSetting(editSettings.FindByName(OrderFields.OrderRow6)),
                    CreateTextFieldSetting(editSettings.FindByName(OrderFields.OrderRow7)),
                    CreateTextFieldSetting(editSettings.FindByName(OrderFields.OrderRow8)),
                    CreateTextFieldSetting(editSettings.FindByName(OrderFields.OrderConfiguration)),
                    CreateTextFieldSetting(editSettings.FindByName(OrderFields.OrderInfo)),
                    CreateTextFieldSetting(editSettings.FindByName(OrderFields.OrderInfo2)))
            {
                Header = headerText
            };
        }

        private static OtherEditSettings CreateOtherEditSettings(
            NamedObjectCollection<OrdersEditSettingsMapData> editSettings, string headerText)
        {
            return new OtherEditSettings(
                    CreateTextFieldSetting(editSettings.FindByName(OrderFields.OtherFileName)),
                    CreateTextFieldSetting(editSettings.FindByName(OrderFields.OtherCaseNumber)),
                    CreateTextFieldSetting(editSettings.FindByName(OrderFields.OtherInfo)))
            {
                Header = headerText
            };
        }

        private static ProgramEditSettings CreateProgramEditSettings(
            NamedObjectCollection<OrdersEditSettingsMapData> editSettings, string headerText)
        {
            return new ProgramEditSettings(
                CreateTextFieldSetting(editSettings.FindByName(OrderFields.Program)),
                CreateTextFieldSetting(editSettings.FindByName(OrderFields.ProgramInfoProduct)))
            {
                Header = headerText
            };
        }

        private static ReceiverEditSettings CreateReceiverEditSettings(
            NamedObjectCollection<OrdersEditSettingsMapData> editSettings, string headerText)
        {
            return new ReceiverEditSettings(
                    CreateTextFieldSetting(editSettings.FindByName(OrderFields.ReceiverId)),
                    CreateTextFieldSetting(editSettings.FindByName(OrderFields.ReceiverName)),
                    CreateTextFieldSetting(editSettings.FindByName(OrderFields.ReceiverEmail)),
                    CreateTextFieldSetting(editSettings.FindByName(OrderFields.ReceiverPhone)),
                    CreateTextFieldSetting(editSettings.FindByName(OrderFields.ReceiverLocation)),
                    CreateTextFieldSetting(editSettings.FindByName(OrderFields.ReceiverMarkOfGoods)))
            {
                Header = headerText
            };
        }

        private static SupplierEditSettings CreateSupplierEditSettings(
            NamedObjectCollection<OrdersEditSettingsMapData> editSettings, string headerText)
        {
            return new SupplierEditSettings(
                    CreateTextFieldSetting(editSettings.FindByName(OrderFields.SupplierOrderNumber)),
                    CreateTextFieldSetting(editSettings.FindByName(OrderFields.SupplierOrderDate)),
                    CreateTextFieldSetting(editSettings.FindByName(OrderFields.SupplierOrderInfo)))
            {
                Header = headerText
            };
        }

        private static UserEditSettings CreateUserEditSettings(
            NamedObjectCollection<OrdersEditSettingsMapData> editSettings, string headerText)
        {
            return new UserEditSettings(
                    CreateTextFieldSetting(editSettings.FindByName(OrderFields.UserId)),
                    CreateTextFieldSetting(editSettings.FindByName(OrderFields.UserFirstName)),
                    CreateTextFieldSetting(editSettings.FindByName(OrderFields.UserLastName)),
                    CreateTextFieldSetting(editSettings.FindByName(OrderFields.UserPhone)),
                    CreateTextFieldSetting(editSettings.FindByName(OrderFields.UserEMail)),
                    CreateTextFieldSetting(editSettings.FindByName(OrderFields.UserInitials)),
                    CreateTextFieldSetting(editSettings.FindByName(OrderFields.UserPersonalIdentityNumber)),
                    CreateTextFieldSetting(editSettings.FindByName(OrderFields.UserExtension)),
                    CreateTextFieldSetting(editSettings.FindByName(OrderFields.UserTitle)),
                    CreateTextFieldSetting(editSettings.FindByName(OrderFields.UserLocation)),
                    CreateTextFieldSetting(editSettings.FindByName(OrderFields.UserRoomNumber)),
                    CreateTextFieldSetting(editSettings.FindByName(OrderFields.UserPostalAddress)),
                    CreateTextFieldSetting(editSettings.FindByName(OrderFields.UserEmploymentType)),
                    CreateTextFieldSetting(editSettings.FindByName(OrderFields.UserDepartment_Id1)),
                    CreateTextFieldSetting(editSettings.FindByName(OrderFields.UserOU_Id)),
                    CreateTextFieldSetting(editSettings.FindByName(OrderFields.UserDepartment_Id2)),
                    CreateTextFieldSetting(editSettings.FindByName(OrderFields.UserInfo)),
                    CreateTextFieldSetting(editSettings.FindByName(OrderFields.UserResponsibility)),
                    CreateTextFieldSetting(editSettings.FindByName(OrderFields.UserActivity)),
                    CreateTextFieldSetting(editSettings.FindByName(OrderFields.UserManager)),
                    CreateTextFieldSetting(editSettings.FindByName(OrderFields.UserReferenceNumber)))
            {
                Header = headerText
            };
        }

        private static AccountInfoEditSettings CreateAccountInfoEditSettings(
            NamedObjectCollection<OrdersEditSettingsMapData> editSettings)
        {
            return new AccountInfoEditSettings(
                    CreateFieldSetting(editSettings.FindByName(OrderFields.AccountInfoStartedDate)),
                    CreateFieldSetting(editSettings.FindByName(OrderFields.AccountInfoFinishDate)),
                    CreateFieldSetting(editSettings.FindByName(OrderFields.AccountInfoEMailTypeId)),
                    CreateFieldSetting(editSettings.FindByName(OrderFields.AccountInfoHomeDirectory)),
                    CreateFieldSetting(editSettings.FindByName(OrderFields.AccountInfoProfile)),
                    CreateTextFieldSetting(editSettings.FindByName(OrderFields.AccountInfoInventoryNumber)),
                    CreateTextFieldSetting(editSettings.FindByName(OrderFields.AccountInfo)),
                    CreateFieldSetting(editSettings.FindByName(OrderFields.AccountInfoAccountType)),
                    CreateFieldSetting(editSettings.FindByName(OrderFields.AccountInfoAccountType2)),
                    CreateFieldSetting(editSettings.FindByName(OrderFields.AccountInfoAccountType3)),
                    CreateFieldSetting(editSettings.FindByName(OrderFields.AccountInfoAccountType4)),
                    CreateFieldSetting(editSettings.FindByName(OrderFields.AccountInfoAccountType5)));
        }

        private static ContactEditSettings CreateContactEditSettings(
            NamedObjectCollection<OrdersEditSettingsMapData> editSettings)
        {
            return new ContactEditSettings(
                    CreateTextFieldSetting(editSettings.FindByName(OrderFields.ContactId)),
                    CreateTextFieldSetting(editSettings.FindByName(OrderFields.ContactName)),
                    CreateTextFieldSetting(editSettings.FindByName(OrderFields.ContactPhone)),
                    CreateTextFieldSetting(editSettings.FindByName(OrderFields.ContactEMail))
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

        private static MultiTextFieldEditSettings CreateMultiTextFieldSetting(OrdersEditSettingsMapData data)
        {
            return new MultiTextFieldEditSettings(
                        data.Show.ToBool(),
                        data.Label,
                        data.Required.ToBool(),
                        data.EmailIdentifier,
                        data.DefaultValue,
                        data.FieldHelp,
                        data.MultiValue);
        }
    }
}