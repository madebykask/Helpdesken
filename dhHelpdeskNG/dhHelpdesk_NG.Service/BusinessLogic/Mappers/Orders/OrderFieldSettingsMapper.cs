using System.Collections.Generic;
using DH.Helpdesk.Domain.Orders;
using LinqLib.Operators;

namespace DH.Helpdesk.Services.BusinessLogic.Mappers.Orders
{
    using System;
    using System.Globalization;
    using System.Linq;

    using DH.Helpdesk.BusinessData.Enums.Orders.Fields;
    using DH.Helpdesk.BusinessData.Models.Orders.OrderFieldSettings;
    using DH.Helpdesk.BusinessData.Models.Orders.OrderFieldSettings.FieldSettings;
    using DH.Helpdesk.BusinessData.Models.Shared;
    using DH.Helpdesk.Common.Collections;
    using DH.Helpdesk.Common.Extensions.Boolean;
    using DH.Helpdesk.Common.Extensions.Integer;
    using DH.Helpdesk.Domain;
    using DH.Helpdesk.Services.BusinessLogic.Mappers.Orders.Data;

    using OrderFieldSettings = DH.Helpdesk.Domain.OrderFieldSettings;

    public static class OrderFieldSettingsMapper
    {
        public static string[] MapToFieldNames(this IQueryable<OrderFieldSettings> query)
        {
            return query.Select(f => f.OrderField).ToArray();
        }

        public static OrderFieldSettingsFilterData MapToFilterData(IQueryable<OrderType> query)
        {
            var entities = query.Select(t => new
                                            {
                                                t.Id,
                                                t.Name
                                            })
                                            .OrderBy(t => t.Name)
                                            .ToArray();

            return new OrderFieldSettingsFilterData(
                entities.Select(o => new ItemOverview(o.Name, o.Id.ToString(CultureInfo.InvariantCulture))).ToArray());
        }

        public static GetSettingsResponse MapToFullFieldSettings(this IQueryable<OrderFieldSettings> query,
            List<OrderFieldType> orderFieldTypes,
            List<OrderType> orderTypeSettings)
        {
            var entities = query.Select(f => new OrdersFieldSettingsMapData
                                                 {
                                                     OrderField = f.OrderField,
                                                     Show = f.Show,
                                                     ShowInList = f.ShowInList,
                                                     ShowExternal = f.ShowExternal,
                                                     Label = f.Label,
                                                     Required = f.Required,
                                                     EmailIdentifier = f.EMailIdentifier,
                                                     DefaultValue = f.DefaultValue,
                                                     FieldHelp = f.FieldHelp,
                                                     MultiValue = f.MultiValue
                                                 }).ToList();

            var fieldSettings = new NamedObjectCollection<OrdersFieldSettingsMapData>(entities);
            return new GetSettingsResponse(CreateFullFieldSettings(fieldSettings, orderFieldTypes, orderTypeSettings.FirstOrDefault()));
        }

        public static void MapToEntitiesForUpdate(
                    this IQueryable<OrderFieldSettings> query,
                    FullFieldSettings settings)
        {
            var entities = query.ToList();
            var fieldSettings = new NamedObjectCollection<OrderFieldSettings>(entities);
            MapDeliverySettings(settings.Delivery, fieldSettings, settings.ChangedDate);
            MapGeneralSettings(settings.General, fieldSettings, settings.ChangedDate);
            MapLogSettings(settings.Log, fieldSettings, settings.ChangedDate);
            MapOrdererSettings(settings.Orderer, fieldSettings, settings.ChangedDate);
            MapOrderSettings(settings.Order, fieldSettings, settings.ChangedDate);
            MapOtherSettings(settings.Other, fieldSettings, settings.ChangedDate);
            MapProgramSettings(settings.Program, fieldSettings, settings.ChangedDate);
            MapReceiverSettings(settings.Receiver, fieldSettings, settings.ChangedDate);
            MapSupplierSettings(settings.Supplier, fieldSettings, settings.ChangedDate);
            MapUserSettings(settings.User, fieldSettings, settings.ChangedDate);
            MapAccountSettings(settings.AccountInfo, fieldSettings, settings.ChangedDate);
        }

        #region Map settings for edit

        private static FullFieldSettings CreateFullFieldSettings(
            NamedObjectCollection<OrdersFieldSettingsMapData> fieldSettings,
            List<OrderFieldType> orderFieldTypes,
            OrderType orderTypeSettings)
        {
            return FullFieldSettings.CreateForEdit(
                    CreateDeliveryFieldSettings(fieldSettings, orderTypeSettings?.CaptionDeliveryInfo ?? ""),
                    CreateGeneralFieldSettings(fieldSettings, orderTypeSettings?.CaptionGeneral ?? ""),
                    CreateLogFieldSettings(fieldSettings),
                    CreateOrdererFieldSettings(fieldSettings, orderTypeSettings?.CaptionOrdererInfo ?? ""),
                    CreateOrderFieldSettings(fieldSettings, orderTypeSettings?.CaptionOrder ?? ""),
                    CreateOtherFieldSettings(fieldSettings, orderTypeSettings?.CaptionOther ?? ""),
                    CreateProgramFieldSettings(fieldSettings, orderTypeSettings?.CaptionProgram ?? ""),
                    CreateReceiverFieldSettings(fieldSettings, orderTypeSettings?.CaptionReceiverInfo ?? ""),
                    CreateSupplierFieldSettings(fieldSettings, orderTypeSettings?.CaptionOrderInfo ?? ""),
                    CreateUserFieldSettings(fieldSettings, orderTypeSettings?.CaptionUserInfo ?? ""),
                    CreateAccountInfoFieldSettings(fieldSettings, orderFieldTypes));
        }

        private static DeliveryFieldSettings CreateDeliveryFieldSettings(
            NamedObjectCollection<OrdersFieldSettingsMapData> fieldSettings, string headerText)
        {
            return new DeliveryFieldSettings(
                CreateTextFieldSetting(fieldSettings.FindByName(OrderFields.DeliveryDate)),
                CreateTextFieldSetting(fieldSettings.FindByName(OrderFields.DeliveryInstallDate)),
                CreateTextFieldSetting(fieldSettings.FindByName(OrderFields.DeliveryDepartment)),
                CreateTextFieldSetting(fieldSettings.FindByName(OrderFields.DeliveryOu)),
                CreateTextFieldSetting(fieldSettings.FindByName(OrderFields.DeliveryAddress)),
                CreateTextFieldSetting(fieldSettings.FindByName(OrderFields.DeliveryPostalCode)),
                CreateTextFieldSetting(fieldSettings.FindByName(OrderFields.DeliveryPostalAddress)),
                CreateTextFieldSetting(fieldSettings.FindByName(OrderFields.DeliveryLocation)),
                CreateTextFieldSetting(fieldSettings.FindByName(OrderFields.DeliveryInfo1)),
                CreateTextFieldSetting(fieldSettings.FindByName(OrderFields.DeliveryInfo2)),
                CreateTextFieldSetting(fieldSettings.FindByName(OrderFields.DeliveryInfo3)),
                CreateTextFieldSetting(fieldSettings.FindByName(OrderFields.DeliveryOuId)),
                CreateTextFieldSetting(fieldSettings.FindByName(OrderFields.DeliveryName)),
                CreateTextFieldSetting(fieldSettings.FindByName(OrderFields.DeliveryPhone)))
            {
                Header = headerText
            };
        }

        private static GeneralFieldSettings CreateGeneralFieldSettings(
            NamedObjectCollection<OrdersFieldSettingsMapData> fieldSettings, string headerText)
        {
            return new GeneralFieldSettings(
                    CreateFieldSetting(fieldSettings.FindByName(OrderFields.GeneralOrderNumber)),
                    CreateFieldSetting(fieldSettings.FindByName(OrderFields.GeneralCustomer)),
                    CreateTextFieldSetting(fieldSettings.FindByName(OrderFields.GeneralAdministrator)),
                    CreateTextFieldSetting(fieldSettings.FindByName(OrderFields.GeneralDomain)),
                    CreateTextFieldSetting(fieldSettings.FindByName(OrderFields.GeneralOrderDate)),
                    CreateTextFieldSetting(fieldSettings.FindByName(OrderFields.GeneralStatus)))
            {
                Header = headerText
            };
        }

        private static LogFieldSettings CreateLogFieldSettings(
            NamedObjectCollection<OrdersFieldSettingsMapData> fieldSettings)
        {
            return new LogFieldSettings(
                    CreateTextFieldSetting(fieldSettings.FindByName(OrderFields.Log)));
        }

        private static OrdererFieldSettings CreateOrdererFieldSettings(
            NamedObjectCollection<OrdersFieldSettingsMapData> fieldSettings, string headerText)
        {
            return new OrdererFieldSettings(
                    CreateMultiTextFieldSetting(fieldSettings.FindByName(OrderFields.OrdererId)),
                    CreateTextFieldSetting(fieldSettings.FindByName(OrderFields.OrdererName)),
                    CreateTextFieldSetting(fieldSettings.FindByName(OrderFields.OrdererLocation)),
                    CreateTextFieldSetting(fieldSettings.FindByName(OrderFields.OrdererEmail)),
                    CreateTextFieldSetting(fieldSettings.FindByName(OrderFields.OrdererPhone)),
                    CreateTextFieldSetting(fieldSettings.FindByName(OrderFields.OrdererCode)),
                    CreateTextFieldSetting(fieldSettings.FindByName(OrderFields.OrdererDepartment)),
                    CreateTextFieldSetting(fieldSettings.FindByName(OrderFields.OrdererUnit)),
                    CreateTextFieldSetting(fieldSettings.FindByName(OrderFields.OrdererAddress)),
                    CreateTextFieldSetting(fieldSettings.FindByName(OrderFields.OrdererInvoiceAddress)),
                    CreateTextFieldSetting(fieldSettings.FindByName(OrderFields.OrdererReferenceNumber)),
                    CreateTextFieldSetting(fieldSettings.FindByName(OrderFields.OrdererAccountingDimension1)),
                    CreateFieldSetting(fieldSettings.FindByName(OrderFields.OrdererAccountingDimension2)),
                    CreateTextFieldSetting(fieldSettings.FindByName(OrderFields.OrdererAccountingDimension3)),
                    CreateFieldSetting(fieldSettings.FindByName(OrderFields.OrdererAccountingDimension4)),
                    CreateTextFieldSetting(fieldSettings.FindByName(OrderFields.OrdererAccountingDimension5)))
            {
                Header = headerText
            };
        }

        private static BusinessData.Models.Orders.OrderFieldSettings.FieldSettings.OrderFieldSettings CreateOrderFieldSettings(
            NamedObjectCollection<OrdersFieldSettingsMapData> fieldSettings, string headerText)
        {
            return new BusinessData.Models.Orders.OrderFieldSettings.FieldSettings.OrderFieldSettings(
                    CreateFieldSetting(fieldSettings.FindByName(OrderFields.OrderProperty)),
                    CreateTextFieldSetting(fieldSettings.FindByName(OrderFields.OrderRow1)),
                    CreateTextFieldSetting(fieldSettings.FindByName(OrderFields.OrderRow2)),
                    CreateTextFieldSetting(fieldSettings.FindByName(OrderFields.OrderRow3)),
                    CreateTextFieldSetting(fieldSettings.FindByName(OrderFields.OrderRow4)),
                    CreateTextFieldSetting(fieldSettings.FindByName(OrderFields.OrderRow5)),
                    CreateTextFieldSetting(fieldSettings.FindByName(OrderFields.OrderRow6)),
                    CreateTextFieldSetting(fieldSettings.FindByName(OrderFields.OrderRow7)),
                    CreateTextFieldSetting(fieldSettings.FindByName(OrderFields.OrderRow8)),
                    CreateTextFieldSetting(fieldSettings.FindByName(OrderFields.OrderConfiguration)),
                    CreateTextFieldSetting(fieldSettings.FindByName(OrderFields.OrderInfo)),
                    CreateTextFieldSetting(fieldSettings.FindByName(OrderFields.OrderInfo2)))
            {
                Header = headerText
            };
        }

        private static OtherFieldSettings CreateOtherFieldSettings(
            NamedObjectCollection<OrdersFieldSettingsMapData> fieldSettings, string headerText)
        {
            return new OtherFieldSettings(
                    CreateTextFieldSetting(fieldSettings.FindByName(OrderFields.OtherFileName)),
                    CreateTextFieldSetting(fieldSettings.FindByName(OrderFields.OtherCaseNumber)),
                    CreateTextFieldSetting(fieldSettings.FindByName(OrderFields.OtherInfo)))
            {
                Header = headerText
            };
        }

        private static ProgramFieldSettings CreateProgramFieldSettings(
            NamedObjectCollection<OrdersFieldSettingsMapData> fieldSettings, string headerText)
        {
            return new ProgramFieldSettings(
                CreateTextFieldSetting(fieldSettings.FindByName(OrderFields.Program)),
                CreateTextFieldSetting(fieldSettings.FindByName(OrderFields.ProgramInfoProduct)))
            {
                Header = headerText
            };
        }

        private static ReceiverFieldSettings CreateReceiverFieldSettings(
            NamedObjectCollection<OrdersFieldSettingsMapData> fieldSettings, string headerText)
        {
            return new ReceiverFieldSettings(
                    CreateTextFieldSetting(fieldSettings.FindByName(OrderFields.ReceiverId)),
                    CreateTextFieldSetting(fieldSettings.FindByName(OrderFields.ReceiverName)),
                    CreateTextFieldSetting(fieldSettings.FindByName(OrderFields.ReceiverEmail)),
                    CreateTextFieldSetting(fieldSettings.FindByName(OrderFields.ReceiverPhone)),
                    CreateTextFieldSetting(fieldSettings.FindByName(OrderFields.ReceiverLocation)),
                    CreateTextFieldSetting(fieldSettings.FindByName(OrderFields.ReceiverMarkOfGoods)))
            {
                Header = headerText
            };
        }

        private static SupplierFieldSettings CreateSupplierFieldSettings(
            NamedObjectCollection<OrdersFieldSettingsMapData> fieldSettings, string headerText)
        {
            return new SupplierFieldSettings(
                    CreateTextFieldSetting(fieldSettings.FindByName(OrderFields.SupplierOrderNumber)),
                    CreateTextFieldSetting(fieldSettings.FindByName(OrderFields.SupplierOrderDate)),
                    CreateTextFieldSetting(fieldSettings.FindByName(OrderFields.SupplierOrderInfo)))
            {
                Header = headerText
            };
        }

        private static UserFieldSettings CreateUserFieldSettings(
            NamedObjectCollection<OrdersFieldSettingsMapData> fieldSettings, string headerText)
        {
            return new UserFieldSettings(
                    CreateTextFieldSetting(fieldSettings.FindByName(OrderFields.UserId)),
                    CreateTextFieldSetting(fieldSettings.FindByName(OrderFields.UserFirstName)),
                    CreateTextFieldSetting(fieldSettings.FindByName(OrderFields.UserLastName)),
                    CreateTextFieldSetting(fieldSettings.FindByName(OrderFields.UserPhone)),
                    CreateTextFieldSetting(fieldSettings.FindByName(OrderFields.UserEMail)),
                    CreateTextFieldSetting(fieldSettings.FindByName(OrderFields.UserInitials)),
                    CreateTextFieldSetting(fieldSettings.FindByName(OrderFields.UserPersonalIdentityNumber)),
                    CreateTextFieldSetting(fieldSettings.FindByName(OrderFields.UserExtension)),
                    CreateTextFieldSetting(fieldSettings.FindByName(OrderFields.UserTitle)),
                    CreateTextFieldSetting(fieldSettings.FindByName(OrderFields.UserLocation)),
                    CreateTextFieldSetting(fieldSettings.FindByName(OrderFields.UserRoomNumber)),
                    CreateTextFieldSetting(fieldSettings.FindByName(OrderFields.UserPostalAddress)),
                    CreateTextFieldSetting(fieldSettings.FindByName(OrderFields.UserEmploymentType)),
                    CreateTextFieldSetting(fieldSettings.FindByName(OrderFields.UserDepartment_Id1)),
                    CreateTextFieldSetting(fieldSettings.FindByName(OrderFields.UserOU_Id)),
                    CreateTextFieldSetting(fieldSettings.FindByName(OrderFields.UserDepartment_Id2)),
                    CreateTextFieldSetting(fieldSettings.FindByName(OrderFields.UserInfo)),
                    CreateTextFieldSetting(fieldSettings.FindByName(OrderFields.UserResponsibility)),
                    CreateTextFieldSetting(fieldSettings.FindByName(OrderFields.UserActivity)),
                    CreateTextFieldSetting(fieldSettings.FindByName(OrderFields.UserManager)),
                    CreateTextFieldSetting(fieldSettings.FindByName(OrderFields.UserReferenceNumber)))
            {
                Header = headerText
            };
        }

        private static AccountInfoFieldSettings CreateAccountInfoFieldSettings(
            NamedObjectCollection<OrdersFieldSettingsMapData> fieldSettings,
            List<OrderFieldType> orderFieldTypes)
        {
            return new AccountInfoFieldSettings(
                    CreateTextFieldSetting(fieldSettings.FindByName(OrderFields.AccountInfoStartedDate)),
                    CreateTextFieldSetting(fieldSettings.FindByName(OrderFields.AccountInfoFinishDate)),
                    CreateTextFieldSetting(fieldSettings.FindByName(OrderFields.AccountInfoEMailTypeId)),
                    CreateTextFieldSetting(fieldSettings.FindByName(OrderFields.AccountInfoHomeDirectory)),
                    CreateTextFieldSetting(fieldSettings.FindByName(OrderFields.AccountInfoProfile)),
                    CreateTextFieldSetting(fieldSettings.FindByName(OrderFields.AccountInfoInventoryNumber)),
                    CreateTextFieldSetting(fieldSettings.FindByName(OrderFields.AccountInfo)),
                    CreateOrderFieldTypeSetting(fieldSettings.FindByName(OrderFields.AccountInfoAccountType), OrderFieldTypes.AccountType, orderFieldTypes),
                    CreateOrderFieldTypeSetting(fieldSettings.FindByName(OrderFields.AccountInfoAccountType2), OrderFieldTypes.AccountType2, orderFieldTypes),
                    CreateOrderFieldTypeSetting(fieldSettings.FindByName(OrderFields.AccountInfoAccountType3), OrderFieldTypes.AccountType3, orderFieldTypes),
                    CreateOrderFieldTypeSetting(fieldSettings.FindByName(OrderFields.AccountInfoAccountType4), OrderFieldTypes.AccountType4, orderFieldTypes),
                    CreateOrderFieldTypeSetting(fieldSettings.FindByName(OrderFields.AccountInfoAccountType5), OrderFieldTypes.AccountType5, orderFieldTypes)
                );
        }

        private static ContactFieldSettings CreateContactFieldSettings(
            NamedObjectCollection<OrdersFieldSettingsMapData> fieldSettings)
        {
            return new ContactFieldSettings(
                    CreateTextFieldSetting(fieldSettings.FindByName(OrderFields.ContactId)),
                    CreateTextFieldSetting(fieldSettings.FindByName(OrderFields.ContactName)),
                    CreateTextFieldSetting(fieldSettings.FindByName(OrderFields.ContactPhone)),
                    CreateTextFieldSetting(fieldSettings.FindByName(OrderFields.ContactEMail))
                );
        }

        private static OrderFieldTypeSettings CreateOrderFieldTypeSetting(OrdersFieldSettingsMapData data, OrderFieldTypes type, List<OrderFieldType> orderFieldTypes)
        {
            var values = orderFieldTypes.Where(t => t.OrderField == type)
                .Select(t => new OrderFieldTypeValueSetting(t.Id, t.Name, t.OrderField))
                .ToList();
            return OrderFieldTypeSettings.CreateForEdit(
                        data.OrderField,
                        data.Show.ToBool(),
                        data.ShowInList.ToBool(),
                        data.ShowExternal.ToBool(),
                        data.Label,
                        data.Required.ToBool(),
                        data.EmailIdentifier,
                        data.FieldHelp,
                        values);
        }

        private static FieldSettings CreateFieldSetting(OrdersFieldSettingsMapData data)
        {
            return FieldSettings.CreateForEdit(
                        data.OrderField,
                        data.Show.ToBool(),
                        data.ShowInList.ToBool(),
                        data.ShowExternal.ToBool(),
                        data.Label,
                        data.Required.ToBool(),
                        data.EmailIdentifier,
                        data.FieldHelp);
        }

        private static TextFieldSettings CreateTextFieldSetting(OrdersFieldSettingsMapData data)
        {
            return TextFieldSettings.CreateForEdit(
                        data.OrderField,
                        data.Show.ToBool(),
                        data.ShowInList.ToBool(),
                        data.ShowExternal.ToBool(),
                        data.Label,
                        data.Required.ToBool(),
                        data.EmailIdentifier,
                        data.DefaultValue,
                        data.FieldHelp);
        }

        private static MultiTextFieldSettings CreateMultiTextFieldSetting(OrdersFieldSettingsMapData data)
        {
            return MultiTextFieldSettings.CreateForEdit(
                        data.OrderField,
                        data.Show.ToBool(),
                        data.ShowInList.ToBool(),
                        data.ShowExternal.ToBool(),
                        data.Label,
                        data.Required.ToBool(),
                        data.EmailIdentifier,
                        data.DefaultValue,
                        data.FieldHelp,
                        data.MultiValue);
        }

        #endregion

        #region Map settings for update

        private static void MapDeliverySettings(
                DeliveryFieldSettings updatedSettings,
                NamedObjectCollection<OrderFieldSettings> existingSettings,
                DateTime changedDate)
        {
            MapTextFieldSettings(updatedSettings.DeliveryDate, existingSettings.FindByName(OrderFields.DeliveryDate), changedDate);
            MapTextFieldSettings(updatedSettings.InstallDate, existingSettings.FindByName(OrderFields.DeliveryInstallDate), changedDate);
            MapTextFieldSettings(updatedSettings.DeliveryDepartment, existingSettings.FindByName(OrderFields.DeliveryDepartment), changedDate);
            MapTextFieldSettings(updatedSettings.DeliveryOu, existingSettings.FindByName(OrderFields.DeliveryOu), changedDate);
            MapTextFieldSettings(updatedSettings.DeliveryAddress, existingSettings.FindByName(OrderFields.DeliveryAddress), changedDate);
            MapTextFieldSettings(updatedSettings.DeliveryPostalCode, existingSettings.FindByName(OrderFields.DeliveryPostalCode), changedDate);
            MapTextFieldSettings(updatedSettings.DeliveryPostalAddress, existingSettings.FindByName(OrderFields.DeliveryPostalAddress), changedDate);
            MapTextFieldSettings(updatedSettings.DeliveryLocation, existingSettings.FindByName(OrderFields.DeliveryLocation), changedDate);
            MapTextFieldSettings(updatedSettings.DeliveryInfo1, existingSettings.FindByName(OrderFields.DeliveryInfo1), changedDate);
            MapTextFieldSettings(updatedSettings.DeliveryInfo2, existingSettings.FindByName(OrderFields.DeliveryInfo2), changedDate);
            MapTextFieldSettings(updatedSettings.DeliveryInfo3, existingSettings.FindByName(OrderFields.DeliveryInfo3), changedDate);
            MapTextFieldSettings(updatedSettings.DeliveryOuId, existingSettings.FindByName(OrderFields.DeliveryOuId), changedDate);
            MapTextFieldSettings(updatedSettings.Name, existingSettings.FindByName(OrderFields.DeliveryName), changedDate);
            MapTextFieldSettings(updatedSettings.Phone, existingSettings.FindByName(OrderFields.DeliveryPhone), changedDate);
        }

        private static void MapGeneralSettings(
                GeneralFieldSettings updatedSettings,
                NamedObjectCollection<OrderFieldSettings> existingSettings,
                DateTime changedDate)
        {
            MapFieldSettings(updatedSettings.OrderNumber, existingSettings.FindByName(OrderFields.GeneralOrderNumber), changedDate);
            MapFieldSettings(updatedSettings.Customer, existingSettings.FindByName(OrderFields.GeneralCustomer), changedDate);
            MapTextFieldSettings(updatedSettings.Status, existingSettings.FindByName(OrderFields.GeneralStatus), changedDate);
            MapTextFieldSettings(updatedSettings.Administrator, existingSettings.FindByName(OrderFields.GeneralAdministrator), changedDate);
            MapTextFieldSettings(updatedSettings.Domain, existingSettings.FindByName(OrderFields.GeneralDomain), changedDate);
            MapTextFieldSettings(updatedSettings.OrderDate, existingSettings.FindByName(OrderFields.GeneralOrderDate), changedDate);
        }

        private static void MapLogSettings(
                LogFieldSettings updatedSettings,
                NamedObjectCollection<OrderFieldSettings> existingSettings,
                DateTime changedDate)
        {
            MapTextFieldSettings(updatedSettings.Log, existingSettings.FindByName(OrderFields.Log), changedDate);
        }

        private static void MapOrdererSettings(
                OrdererFieldSettings updatedSettings,
                NamedObjectCollection<OrderFieldSettings> existingSettings,
                DateTime changedDate)
        {
            MapMultiTextFieldSettings(updatedSettings.OrdererId, existingSettings.FindByName(OrderFields.OrdererId), changedDate);
            MapTextFieldSettings(updatedSettings.OrdererName, existingSettings.FindByName(OrderFields.OrdererName), changedDate);
            MapTextFieldSettings(updatedSettings.OrdererLocation, existingSettings.FindByName(OrderFields.OrdererLocation), changedDate);
            MapTextFieldSettings(updatedSettings.OrdererEmail, existingSettings.FindByName(OrderFields.OrdererEmail), changedDate);
            MapTextFieldSettings(updatedSettings.OrdererPhone, existingSettings.FindByName(OrderFields.OrdererPhone), changedDate);
            MapTextFieldSettings(updatedSettings.OrdererCode, existingSettings.FindByName(OrderFields.OrdererCode), changedDate);
            MapTextFieldSettings(updatedSettings.Department, existingSettings.FindByName(OrderFields.OrdererDepartment), changedDate);
            MapTextFieldSettings(updatedSettings.Unit, existingSettings.FindByName(OrderFields.OrdererUnit), changedDate);
            MapTextFieldSettings(updatedSettings.OrdererAddress, existingSettings.FindByName(OrderFields.OrdererAddress), changedDate);
            MapTextFieldSettings(updatedSettings.OrdererInvoiceAddress, existingSettings.FindByName(OrderFields.OrdererInvoiceAddress), changedDate);
            MapTextFieldSettings(updatedSettings.OrdererReferenceNumber, existingSettings.FindByName(OrderFields.OrdererReferenceNumber), changedDate);
            MapTextFieldSettings(updatedSettings.AccountingDimension1, existingSettings.FindByName(OrderFields.OrdererAccountingDimension1), changedDate);
            MapFieldSettings(updatedSettings.AccountingDimension2, existingSettings.FindByName(OrderFields.OrdererAccountingDimension2), changedDate);
            MapTextFieldSettings(updatedSettings.AccountingDimension3, existingSettings.FindByName(OrderFields.OrdererAccountingDimension3), changedDate);
            MapFieldSettings(updatedSettings.AccountingDimension4, existingSettings.FindByName(OrderFields.OrdererAccountingDimension4), changedDate);
            MapTextFieldSettings(updatedSettings.AccountingDimension5, existingSettings.FindByName(OrderFields.OrdererAccountingDimension5), changedDate);
        }

        private static void MapOrderSettings(
                BusinessData.Models.Orders.OrderFieldSettings.FieldSettings.OrderFieldSettings updatedSettings,
                NamedObjectCollection<OrderFieldSettings> existingSettings,
                DateTime changedDate)
        {
            MapFieldSettings(updatedSettings.Property, existingSettings.FindByName(OrderFields.OrderProperty), changedDate);
            MapTextFieldSettings(updatedSettings.OrderRow1, existingSettings.FindByName(OrderFields.OrderRow1), changedDate);
            MapTextFieldSettings(updatedSettings.OrderRow2, existingSettings.FindByName(OrderFields.OrderRow2), changedDate);
            MapTextFieldSettings(updatedSettings.OrderRow3, existingSettings.FindByName(OrderFields.OrderRow3), changedDate);
            MapTextFieldSettings(updatedSettings.OrderRow4, existingSettings.FindByName(OrderFields.OrderRow4), changedDate);
            MapTextFieldSettings(updatedSettings.OrderRow5, existingSettings.FindByName(OrderFields.OrderRow5), changedDate);
            MapTextFieldSettings(updatedSettings.OrderRow6, existingSettings.FindByName(OrderFields.OrderRow6), changedDate);
            MapTextFieldSettings(updatedSettings.OrderRow7, existingSettings.FindByName(OrderFields.OrderRow7), changedDate);
            MapTextFieldSettings(updatedSettings.OrderRow8, existingSettings.FindByName(OrderFields.OrderRow8), changedDate);
            MapTextFieldSettings(updatedSettings.Configuration, existingSettings.FindByName(OrderFields.OrderConfiguration), changedDate);
            MapTextFieldSettings(updatedSettings.OrderInfo, existingSettings.FindByName(OrderFields.OrderInfo), changedDate);
            MapTextFieldSettings(updatedSettings.OrderInfo2, existingSettings.FindByName(OrderFields.OrderInfo2), changedDate);
        }

        private static void MapOtherSettings(
                OtherFieldSettings updatedSettings,
                NamedObjectCollection<OrderFieldSettings> existingSettings,
                DateTime changedDate)
        {
            MapTextFieldSettings(updatedSettings.FileName, existingSettings.FindByName(OrderFields.OtherFileName), changedDate);
            MapTextFieldSettings(updatedSettings.CaseNumber, existingSettings.FindByName(OrderFields.OtherCaseNumber), changedDate);
            MapTextFieldSettings(updatedSettings.Info, existingSettings.FindByName(OrderFields.OtherInfo), changedDate);
        }

        private static void MapProgramSettings(
                ProgramFieldSettings updatedSettings,
                NamedObjectCollection<OrderFieldSettings> existingSettings,
                DateTime changedDate)
        {
            MapTextFieldSettings(updatedSettings.Program, existingSettings.FindByName(OrderFields.Program), changedDate);
            MapTextFieldSettings(updatedSettings.InfoProduct, existingSettings.FindByName(OrderFields.ProgramInfoProduct), changedDate);
        }

        private static void MapReceiverSettings(
                ReceiverFieldSettings updatedSettings,
                NamedObjectCollection<OrderFieldSettings> existingSettings,
                DateTime changedDate)
        {
            MapTextFieldSettings(updatedSettings.ReceiverId, existingSettings.FindByName(OrderFields.ReceiverId), changedDate);
            MapTextFieldSettings(updatedSettings.ReceiverName, existingSettings.FindByName(OrderFields.ReceiverName), changedDate);
            MapTextFieldSettings(updatedSettings.ReceiverEmail, existingSettings.FindByName(OrderFields.ReceiverEmail), changedDate);
            MapTextFieldSettings(updatedSettings.ReceiverPhone, existingSettings.FindByName(OrderFields.ReceiverPhone), changedDate);
            MapTextFieldSettings(updatedSettings.ReceiverLocation, existingSettings.FindByName(OrderFields.ReceiverLocation), changedDate);
            MapTextFieldSettings(updatedSettings.MarkOfGoods, existingSettings.FindByName(OrderFields.ReceiverMarkOfGoods), changedDate);
        }

        private static void MapSupplierSettings(
                SupplierFieldSettings updatedSettings,
                NamedObjectCollection<OrderFieldSettings> existingSettings,
                DateTime changedDate)
        {
            MapTextFieldSettings(updatedSettings.SupplierOrderNumber, existingSettings.FindByName(OrderFields.SupplierOrderNumber), changedDate);
            MapTextFieldSettings(updatedSettings.SupplierOrderDate, existingSettings.FindByName(OrderFields.SupplierOrderDate), changedDate);
            MapTextFieldSettings(updatedSettings.SupplierOrderInfo, existingSettings.FindByName(OrderFields.SupplierOrderInfo), changedDate);
        }

        private static void MapUserSettings(
                UserFieldSettings updatedSettings,
                NamedObjectCollection<OrderFieldSettings> existingSettings,
                DateTime changedDate)
        {
            MapTextFieldSettings(updatedSettings.UserId, existingSettings.FindByName(OrderFields.UserId), changedDate);
            MapTextFieldSettings(updatedSettings.UserFirstName, existingSettings.FindByName(OrderFields.UserFirstName), changedDate);
            MapTextFieldSettings(updatedSettings.UserLastName, existingSettings.FindByName(OrderFields.UserLastName), changedDate);
            MapTextFieldSettings(updatedSettings.UserPhone, existingSettings.FindByName(OrderFields.UserPhone), changedDate);
            MapTextFieldSettings(updatedSettings.UserEMail, existingSettings.FindByName(OrderFields.UserEMail), changedDate);
            //MapTextFieldSettings(updatedSettings.Initials, existingSettings.FindByName(UserFields.UserInitials), changedDate);
            MapTextFieldSettings(updatedSettings.Info, existingSettings.FindByName(OrderFields.UserInfo), changedDate);
            MapTextFieldSettings(updatedSettings.Activity, existingSettings.FindByName(OrderFields.UserActivity), changedDate);
            //MapTextFieldSettings(updatedSettings.DepartmentId1, existingSettings.FindByName(UserFields.UserDepartment_Id1), changedDate);
            MapTextFieldSettings(updatedSettings.DepartmentId2, existingSettings.FindByName(OrderFields.UserDepartment_Id2), changedDate);
            MapTextFieldSettings(updatedSettings.EmploymentType, existingSettings.FindByName(OrderFields.UserEmploymentType), changedDate);
            MapTextFieldSettings(updatedSettings.Extension, existingSettings.FindByName(OrderFields.UserExtension), changedDate);
            //MapTextFieldSettings(updatedSettings.Location, existingSettings.FindByName(UserFields.UserLocation), changedDate);
            MapTextFieldSettings(updatedSettings.Manager, existingSettings.FindByName(OrderFields.UserManager), changedDate);
            MapTextFieldSettings(updatedSettings.PersonalIdentityNumber, existingSettings.FindByName(OrderFields.UserPersonalIdentityNumber), changedDate);
            //MapTextFieldSettings(updatedSettings.PostalAddress, existingSettings.FindByName(UserFields.UserPostalAddress), changedDate);
            //MapTextFieldSettings(updatedSettings.ReferenceNumber, existingSettings.FindByName(UserFields.ReferenceNumber), changedDate);
            //MapTextFieldSettings(updatedSettings.Responsibility, existingSettings.FindByName(UserFields.Responsibility), changedDate);
            MapTextFieldSettings(updatedSettings.RoomNumber, existingSettings.FindByName(OrderFields.UserRoomNumber), changedDate);
            MapTextFieldSettings(updatedSettings.Title, existingSettings.FindByName(OrderFields.UserTitle), changedDate);
            //MapTextFieldSettings(updatedSettings.UnitId, existingSettings.FindByName(UserFields.UserOU_Id), changedDate);
        }

        private static void MapAccountSettings(
            AccountInfoFieldSettings updatedSettings,
            NamedObjectCollection<OrderFieldSettings> existingSettings,
            DateTime changedDate)
        {
            MapTextFieldSettings(updatedSettings.StartedDate, existingSettings.FindByName(OrderFields.AccountInfoStartedDate), changedDate);
            MapTextFieldSettings(updatedSettings.FinishDate, existingSettings.FindByName(OrderFields.AccountInfoFinishDate), changedDate);
            MapTextFieldSettings(updatedSettings.EMailTypeId, existingSettings.FindByName(OrderFields.AccountInfoEMailTypeId), changedDate);
            MapTextFieldSettings(updatedSettings.HomeDirectory, existingSettings.FindByName(OrderFields.AccountInfoHomeDirectory), changedDate);
            MapTextFieldSettings(updatedSettings.Profile, existingSettings.FindByName(OrderFields.AccountInfoProfile), changedDate);
            //MapTextFieldSettings(updatedSettings.InventoryNumber, existingSettings.FindByName(AccountInfoFields.InventoryNumber), changedDate);
            //MapTextFieldSettings(updatedSettings.Info, existingSettings.FindByName(AccountInfoFields.Info), changedDate);
            MapOrderFieldTypeSettings(updatedSettings.AccountType, existingSettings.FindByName(OrderFields.AccountInfoAccountType), changedDate);
            MapOrderFieldTypeSettings(updatedSettings.AccountType2, existingSettings.FindByName(OrderFields.AccountInfoAccountType2), changedDate);
            MapOrderFieldTypeSettings(updatedSettings.AccountType3, existingSettings.FindByName(OrderFields.AccountInfoAccountType3), changedDate);
            MapOrderFieldTypeSettings(updatedSettings.AccountType4, existingSettings.FindByName(OrderFields.AccountInfoAccountType4), changedDate);
            MapOrderFieldTypeSettings(updatedSettings.AccountType5, existingSettings.FindByName(OrderFields.AccountInfoAccountType5), changedDate);
        }

        private static void MapContactSettings(
                ContactFieldSettings updatedSettings,
                NamedObjectCollection<OrderFieldSettings> existingSettings,
                DateTime changedDate)
        {
            MapTextFieldSettings(updatedSettings.Id, existingSettings.FindByName(OrderFields.ContactId), changedDate);
            MapTextFieldSettings(updatedSettings.Name, existingSettings.FindByName(OrderFields.ContactName), changedDate);
            MapTextFieldSettings(updatedSettings.Phone, existingSettings.FindByName(OrderFields.ContactPhone), changedDate);
            MapTextFieldSettings(updatedSettings.Email, existingSettings.FindByName(OrderFields.ContactEMail), changedDate);
        }

        private static void MapOrderFieldTypeSettings(OrderFieldTypeSettings updatedSettings,
            OrderFieldSettings fieldSettings,
            DateTime changedDate)
        {
            MapFieldSettings(updatedSettings, fieldSettings, changedDate);
        }

        private static void MapFieldSettings(
                FieldSettings updatedSettings,
                OrderFieldSettings fieldSettings,
                DateTime changedDate)
        {
            fieldSettings.Required = updatedSettings.Required.ToInt();
            fieldSettings.Show = updatedSettings.Show.ToInt();
            fieldSettings.ShowExternal = updatedSettings.ShowExternal.ToInt();
            fieldSettings.ShowInList = updatedSettings.ShowInList.ToInt();
            fieldSettings.EMailIdentifier = updatedSettings.EmailIdentifier;
            fieldSettings.Label = updatedSettings.Label;
            fieldSettings.ChangedDate = changedDate;
            fieldSettings.FieldHelp = updatedSettings.FieldHelp;
        }

        private static void MapTextFieldSettings(
                TextFieldSettings updatedSettings,
                OrderFieldSettings fieldSettings,
                DateTime changedDate)
        {
            fieldSettings.Required = updatedSettings.Required.ToInt();
            fieldSettings.Show = updatedSettings.Show.ToInt();
            fieldSettings.ShowExternal = updatedSettings.ShowExternal.ToInt();
            fieldSettings.ShowInList = updatedSettings.ShowInList.ToInt();
            fieldSettings.EMailIdentifier = updatedSettings.EmailIdentifier;
            fieldSettings.Label = updatedSettings.Label;
            fieldSettings.ChangedDate = changedDate;
            fieldSettings.DefaultValue = updatedSettings.DefaultValue;
            fieldSettings.FieldHelp = updatedSettings.FieldHelp;
        }

        private static void MapMultiTextFieldSettings(
        MultiTextFieldSettings updatedSettings,
        OrderFieldSettings fieldSettings,
        DateTime changedDate)
        {
            fieldSettings.Required = updatedSettings.Required.ToInt();
            fieldSettings.Show = updatedSettings.Show.ToInt();
            fieldSettings.ShowExternal = updatedSettings.ShowExternal.ToInt();
            fieldSettings.ShowInList = updatedSettings.ShowInList.ToInt();
            fieldSettings.EMailIdentifier = updatedSettings.EmailIdentifier;
            fieldSettings.Label = updatedSettings.Label;
            fieldSettings.ChangedDate = changedDate;
            fieldSettings.DefaultValue = updatedSettings.DefaultValue;
            fieldSettings.FieldHelp = updatedSettings.FieldHelp;
            fieldSettings.MultiValue = updatedSettings.MultiValue;
        }

        #endregion
    }
}