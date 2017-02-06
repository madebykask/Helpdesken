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
                                                     FieldHelp = f.FieldHelp
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
                CreateTextFieldSetting(fieldSettings.FindByName(DeliveryFields.DeliveryDate)),
                CreateTextFieldSetting(fieldSettings.FindByName(DeliveryFields.InstallDate)),
                CreateTextFieldSetting(fieldSettings.FindByName(DeliveryFields.DeliveryDepartment)),
                CreateTextFieldSetting(fieldSettings.FindByName(DeliveryFields.DeliveryOu)),
                CreateTextFieldSetting(fieldSettings.FindByName(DeliveryFields.DeliveryAddress)),
                CreateTextFieldSetting(fieldSettings.FindByName(DeliveryFields.DeliveryPostalCode)),
                CreateTextFieldSetting(fieldSettings.FindByName(DeliveryFields.DeliveryPostalAddress)),
                CreateTextFieldSetting(fieldSettings.FindByName(DeliveryFields.DeliveryLocation)),
                CreateTextFieldSetting(fieldSettings.FindByName(DeliveryFields.DeliveryInfo1)),
                CreateTextFieldSetting(fieldSettings.FindByName(DeliveryFields.DeliveryInfo2)),
                CreateTextFieldSetting(fieldSettings.FindByName(DeliveryFields.DeliveryInfo3)),
                CreateTextFieldSetting(fieldSettings.FindByName(DeliveryFields.DeliveryOuId)),
                CreateTextFieldSetting(fieldSettings.FindByName(DeliveryFields.DeliveryName)),
                CreateTextFieldSetting(fieldSettings.FindByName(DeliveryFields.DeliveryPhone)))
            {
                Header = headerText
            };
        }

        private static GeneralFieldSettings CreateGeneralFieldSettings(
            NamedObjectCollection<OrdersFieldSettingsMapData> fieldSettings, string headerText)
        {
            return new GeneralFieldSettings(
                    CreateFieldSetting(fieldSettings.FindByName(GeneralFields.OrderNumber)),
                    CreateFieldSetting(fieldSettings.FindByName(GeneralFields.Customer)),
                    CreateTextFieldSetting(fieldSettings.FindByName(GeneralFields.Administrator)),
                    CreateTextFieldSetting(fieldSettings.FindByName(GeneralFields.Domain)),
                    CreateTextFieldSetting(fieldSettings.FindByName(GeneralFields.OrderDate)),
                    CreateTextFieldSetting(fieldSettings.FindByName(GeneralFields.Status)))
            {
                Header = headerText
            };
        }

        private static LogFieldSettings CreateLogFieldSettings(
            NamedObjectCollection<OrdersFieldSettingsMapData> fieldSettings)
        {
            return new LogFieldSettings(
                    CreateTextFieldSetting(fieldSettings.FindByName(LogFields.Log)));
        }

        private static OrdererFieldSettings CreateOrdererFieldSettings(
            NamedObjectCollection<OrdersFieldSettingsMapData> fieldSettings, string headerText)
        {
            return new OrdererFieldSettings(
                    CreateTextFieldSetting(fieldSettings.FindByName(OrdererFields.OrdererId)),
                    CreateTextFieldSetting(fieldSettings.FindByName(OrdererFields.OrdererName)),
                    CreateTextFieldSetting(fieldSettings.FindByName(OrdererFields.OrdererLocation)),
                    CreateTextFieldSetting(fieldSettings.FindByName(OrdererFields.OrdererEmail)),
                    CreateTextFieldSetting(fieldSettings.FindByName(OrdererFields.OrdererPhone)),
                    CreateTextFieldSetting(fieldSettings.FindByName(OrdererFields.OrdererCode)),
                    CreateTextFieldSetting(fieldSettings.FindByName(OrdererFields.Department)),
                    CreateTextFieldSetting(fieldSettings.FindByName(OrdererFields.Unit)),
                    CreateTextFieldSetting(fieldSettings.FindByName(OrdererFields.OrdererAddress)),
                    CreateTextFieldSetting(fieldSettings.FindByName(OrdererFields.OrdererInvoiceAddress)),
                    CreateTextFieldSetting(fieldSettings.FindByName(OrdererFields.OrdererReferenceNumber)),
                    CreateTextFieldSetting(fieldSettings.FindByName(OrdererFields.AccountingDimension1)),
                    CreateFieldSetting(fieldSettings.FindByName(OrdererFields.AccountingDimension2)),
                    CreateTextFieldSetting(fieldSettings.FindByName(OrdererFields.AccountingDimension3)),
                    CreateFieldSetting(fieldSettings.FindByName(OrdererFields.AccountingDimension4)),
                    CreateTextFieldSetting(fieldSettings.FindByName(OrdererFields.AccountingDimension5)))
            {
                Header = headerText
            };
        }

        private static BusinessData.Models.Orders.OrderFieldSettings.FieldSettings.OrderFieldSettings CreateOrderFieldSettings(
            NamedObjectCollection<OrdersFieldSettingsMapData> fieldSettings, string headerText)
        {
            return new BusinessData.Models.Orders.OrderFieldSettings.FieldSettings.OrderFieldSettings(
                    CreateFieldSetting(fieldSettings.FindByName(OrderFields.Property)),
                    CreateTextFieldSetting(fieldSettings.FindByName(OrderFields.OrderRow1)),
                    CreateTextFieldSetting(fieldSettings.FindByName(OrderFields.OrderRow2)),
                    CreateTextFieldSetting(fieldSettings.FindByName(OrderFields.OrderRow3)),
                    CreateTextFieldSetting(fieldSettings.FindByName(OrderFields.OrderRow4)),
                    CreateTextFieldSetting(fieldSettings.FindByName(OrderFields.OrderRow5)),
                    CreateTextFieldSetting(fieldSettings.FindByName(OrderFields.OrderRow6)),
                    CreateTextFieldSetting(fieldSettings.FindByName(OrderFields.OrderRow7)),
                    CreateTextFieldSetting(fieldSettings.FindByName(OrderFields.OrderRow8)),
                    CreateTextFieldSetting(fieldSettings.FindByName(OrderFields.Configuration)),
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
                    CreateTextFieldSetting(fieldSettings.FindByName(OtherFields.FileName)),
                    CreateTextFieldSetting(fieldSettings.FindByName(OtherFields.CaseNumber)),
                    CreateTextFieldSetting(fieldSettings.FindByName(OtherFields.Info)))
            {
                Header = headerText
            };
        }

        private static ProgramFieldSettings CreateProgramFieldSettings(
            NamedObjectCollection<OrdersFieldSettingsMapData> fieldSettings, string headerText)
        {
            return new ProgramFieldSettings(
                CreateTextFieldSetting(fieldSettings.FindByName(ProgramFields.Program)),
                CreateTextFieldSetting(fieldSettings.FindByName(ProgramFields.InfoProduct)))
            {
                Header = headerText
            };
        }

        private static ReceiverFieldSettings CreateReceiverFieldSettings(
            NamedObjectCollection<OrdersFieldSettingsMapData> fieldSettings, string headerText)
        {
            return new ReceiverFieldSettings(
                    CreateTextFieldSetting(fieldSettings.FindByName(ReceiverFields.ReceiverId)),
                    CreateTextFieldSetting(fieldSettings.FindByName(ReceiverFields.ReceiverName)),
                    CreateTextFieldSetting(fieldSettings.FindByName(ReceiverFields.ReceiverEmail)),
                    CreateTextFieldSetting(fieldSettings.FindByName(ReceiverFields.ReceiverPhone)),
                    CreateTextFieldSetting(fieldSettings.FindByName(ReceiverFields.ReceiverLocation)),
                    CreateTextFieldSetting(fieldSettings.FindByName(ReceiverFields.MarkOfGoods)))
            {
                Header = headerText
            };
        }

        private static SupplierFieldSettings CreateSupplierFieldSettings(
            NamedObjectCollection<OrdersFieldSettingsMapData> fieldSettings, string headerText)
        {
            return new SupplierFieldSettings(
                    CreateTextFieldSetting(fieldSettings.FindByName(SupplierFields.SupplierOrderNumber)),
                    CreateTextFieldSetting(fieldSettings.FindByName(SupplierFields.SupplierOrderDate)),
                    CreateTextFieldSetting(fieldSettings.FindByName(SupplierFields.SupplierOrderInfo)))
            {
                Header = headerText
            };
        }

        private static UserFieldSettings CreateUserFieldSettings(
            NamedObjectCollection<OrdersFieldSettingsMapData> fieldSettings, string headerText)
        {
            return new UserFieldSettings(
                    CreateTextFieldSetting(fieldSettings.FindByName(UserFields.UserId)),
                    CreateTextFieldSetting(fieldSettings.FindByName(UserFields.UserFirstName)),
                    CreateTextFieldSetting(fieldSettings.FindByName(UserFields.UserLastName)),
                    CreateTextFieldSetting(fieldSettings.FindByName(UserFields.UserPhone)),
                    CreateTextFieldSetting(fieldSettings.FindByName(UserFields.UserEMail)),
                    CreateTextFieldSetting(fieldSettings.FindByName(UserFields.UserInitials)),
                    CreateTextFieldSetting(fieldSettings.FindByName(UserFields.UserPersonalIdentityNumber)),
                    CreateTextFieldSetting(fieldSettings.FindByName(UserFields.UserExtension)),
                    CreateTextFieldSetting(fieldSettings.FindByName(UserFields.UserTitle)),
                    CreateTextFieldSetting(fieldSettings.FindByName(UserFields.UserLocation)),
                    CreateTextFieldSetting(fieldSettings.FindByName(UserFields.UserRoomNumber)),
                    CreateTextFieldSetting(fieldSettings.FindByName(UserFields.UserPostalAddress)),
                    CreateTextFieldSetting(fieldSettings.FindByName(UserFields.EmploymentType)),
                    CreateTextFieldSetting(fieldSettings.FindByName(UserFields.UserDepartment_Id1)),
                    CreateTextFieldSetting(fieldSettings.FindByName(UserFields.UserOU_Id)),
                    CreateTextFieldSetting(fieldSettings.FindByName(UserFields.UserDepartment_Id2)),
                    CreateTextFieldSetting(fieldSettings.FindByName(UserFields.InfoUser)),
                    CreateTextFieldSetting(fieldSettings.FindByName(UserFields.Responsibility)),
                    CreateTextFieldSetting(fieldSettings.FindByName(UserFields.Activity)),
                    CreateTextFieldSetting(fieldSettings.FindByName(UserFields.Manager)),
                    CreateTextFieldSetting(fieldSettings.FindByName(UserFields.ReferenceNumber)))
            {
                Header = headerText
            };
        }

        private static AccountInfoFieldSettings CreateAccountInfoFieldSettings(
            NamedObjectCollection<OrdersFieldSettingsMapData> fieldSettings,
            List<OrderFieldType> orderFieldTypes)
        {
            return new AccountInfoFieldSettings(
                    CreateTextFieldSetting(fieldSettings.FindByName(AccountInfoFields.StartedDate)),
                    CreateTextFieldSetting(fieldSettings.FindByName(AccountInfoFields.FinishDate)),
                    CreateTextFieldSetting(fieldSettings.FindByName(AccountInfoFields.EMailTypeId)),
                    CreateTextFieldSetting(fieldSettings.FindByName(AccountInfoFields.HomeDirectory)),
                    CreateTextFieldSetting(fieldSettings.FindByName(AccountInfoFields.Profile)),
                    CreateTextFieldSetting(fieldSettings.FindByName(AccountInfoFields.InventoryNumber)),
                    CreateTextFieldSetting(fieldSettings.FindByName(AccountInfoFields.Info)),
                    CreateOrderFieldTypeSetting(fieldSettings.FindByName(AccountInfoFields.AccountType), OrderFieldTypes.AccountType, orderFieldTypes),
                    CreateOrderFieldTypeSetting(fieldSettings.FindByName(AccountInfoFields.AccountType2), OrderFieldTypes.AccountType2, orderFieldTypes),
                    CreateOrderFieldTypeSetting(fieldSettings.FindByName(AccountInfoFields.AccountType3), OrderFieldTypes.AccountType3, orderFieldTypes),
                    CreateOrderFieldTypeSetting(fieldSettings.FindByName(AccountInfoFields.AccountType4), OrderFieldTypes.AccountType4, orderFieldTypes),
                    CreateOrderFieldTypeSetting(fieldSettings.FindByName(AccountInfoFields.AccountType5), OrderFieldTypes.AccountType5, orderFieldTypes)
                );
        }

        private static ContactFieldSettings CreateContactFieldSettings(
            NamedObjectCollection<OrdersFieldSettingsMapData> fieldSettings)
        {
            return new ContactFieldSettings(
                    CreateTextFieldSetting(fieldSettings.FindByName(ContactFields.ContactId)),
                    CreateTextFieldSetting(fieldSettings.FindByName(ContactFields.ContactName)),
                    CreateTextFieldSetting(fieldSettings.FindByName(ContactFields.ContactPhone)),
                    CreateTextFieldSetting(fieldSettings.FindByName(ContactFields.ContactEMail))
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

        #endregion

        #region Map settings for update

        private static void MapDeliverySettings(
                DeliveryFieldSettings updatedSettings,
                NamedObjectCollection<OrderFieldSettings> existingSettings,
                DateTime changedDate)
        {
            MapTextFieldSettings(updatedSettings.DeliveryDate, existingSettings.FindByName(DeliveryFields.DeliveryDate), changedDate);
            MapTextFieldSettings(updatedSettings.InstallDate, existingSettings.FindByName(DeliveryFields.InstallDate), changedDate);
            MapTextFieldSettings(updatedSettings.DeliveryDepartment, existingSettings.FindByName(DeliveryFields.DeliveryDepartment), changedDate);
            MapTextFieldSettings(updatedSettings.DeliveryOu, existingSettings.FindByName(DeliveryFields.DeliveryOu), changedDate);
            MapTextFieldSettings(updatedSettings.DeliveryAddress, existingSettings.FindByName(DeliveryFields.DeliveryAddress), changedDate);
            MapTextFieldSettings(updatedSettings.DeliveryPostalCode, existingSettings.FindByName(DeliveryFields.DeliveryPostalCode), changedDate);
            MapTextFieldSettings(updatedSettings.DeliveryPostalAddress, existingSettings.FindByName(DeliveryFields.DeliveryPostalAddress), changedDate);
            MapTextFieldSettings(updatedSettings.DeliveryLocation, existingSettings.FindByName(DeliveryFields.DeliveryLocation), changedDate);
            MapTextFieldSettings(updatedSettings.DeliveryInfo1, existingSettings.FindByName(DeliveryFields.DeliveryInfo1), changedDate);
            MapTextFieldSettings(updatedSettings.DeliveryInfo2, existingSettings.FindByName(DeliveryFields.DeliveryInfo2), changedDate);
            MapTextFieldSettings(updatedSettings.DeliveryInfo3, existingSettings.FindByName(DeliveryFields.DeliveryInfo3), changedDate);
            MapTextFieldSettings(updatedSettings.DeliveryOuId, existingSettings.FindByName(DeliveryFields.DeliveryOuId), changedDate);
            MapTextFieldSettings(updatedSettings.Name, existingSettings.FindByName(DeliveryFields.DeliveryName), changedDate);
            MapTextFieldSettings(updatedSettings.Phone, existingSettings.FindByName(DeliveryFields.DeliveryPhone), changedDate);
        }

        private static void MapGeneralSettings(
                GeneralFieldSettings updatedSettings,
                NamedObjectCollection<OrderFieldSettings> existingSettings,
                DateTime changedDate)
        {
            MapFieldSettings(updatedSettings.OrderNumber, existingSettings.FindByName(GeneralFields.OrderNumber), changedDate);
            MapFieldSettings(updatedSettings.Customer, existingSettings.FindByName(GeneralFields.Customer), changedDate);
            MapTextFieldSettings(updatedSettings.Status, existingSettings.FindByName(GeneralFields.Status), changedDate);
            MapTextFieldSettings(updatedSettings.Administrator, existingSettings.FindByName(GeneralFields.Administrator), changedDate);
            MapTextFieldSettings(updatedSettings.Domain, existingSettings.FindByName(GeneralFields.Domain), changedDate);
            MapTextFieldSettings(updatedSettings.OrderDate, existingSettings.FindByName(GeneralFields.OrderDate), changedDate);
        }

        private static void MapLogSettings(
                LogFieldSettings updatedSettings,
                NamedObjectCollection<OrderFieldSettings> existingSettings,
                DateTime changedDate)
        {
            MapTextFieldSettings(updatedSettings.Log, existingSettings.FindByName(LogFields.Log), changedDate);
        }

        private static void MapOrdererSettings(
                OrdererFieldSettings updatedSettings,
                NamedObjectCollection<OrderFieldSettings> existingSettings,
                DateTime changedDate)
        {
            MapTextFieldSettings(updatedSettings.OrdererId, existingSettings.FindByName(OrdererFields.OrdererId), changedDate);
            MapTextFieldSettings(updatedSettings.OrdererName, existingSettings.FindByName(OrdererFields.OrdererName), changedDate);
            MapTextFieldSettings(updatedSettings.OrdererLocation, existingSettings.FindByName(OrdererFields.OrdererLocation), changedDate);
            MapTextFieldSettings(updatedSettings.OrdererEmail, existingSettings.FindByName(OrdererFields.OrdererEmail), changedDate);
            MapTextFieldSettings(updatedSettings.OrdererPhone, existingSettings.FindByName(OrdererFields.OrdererPhone), changedDate);
            MapTextFieldSettings(updatedSettings.OrdererCode, existingSettings.FindByName(OrdererFields.OrdererCode), changedDate);
            MapTextFieldSettings(updatedSettings.Department, existingSettings.FindByName(OrdererFields.Department), changedDate);
            MapTextFieldSettings(updatedSettings.Unit, existingSettings.FindByName(OrdererFields.Unit), changedDate);
            MapTextFieldSettings(updatedSettings.OrdererAddress, existingSettings.FindByName(OrdererFields.OrdererAddress), changedDate);
            MapTextFieldSettings(updatedSettings.OrdererInvoiceAddress, existingSettings.FindByName(OrdererFields.OrdererInvoiceAddress), changedDate);
            MapTextFieldSettings(updatedSettings.OrdererReferenceNumber, existingSettings.FindByName(OrdererFields.OrdererReferenceNumber), changedDate);
            MapTextFieldSettings(updatedSettings.AccountingDimension1, existingSettings.FindByName(OrdererFields.AccountingDimension1), changedDate);
            MapFieldSettings(updatedSettings.AccountingDimension2, existingSettings.FindByName(OrdererFields.AccountingDimension2), changedDate);
            MapTextFieldSettings(updatedSettings.AccountingDimension3, existingSettings.FindByName(OrdererFields.AccountingDimension3), changedDate);
            MapFieldSettings(updatedSettings.AccountingDimension4, existingSettings.FindByName(OrdererFields.AccountingDimension4), changedDate);
            MapTextFieldSettings(updatedSettings.AccountingDimension5, existingSettings.FindByName(OrdererFields.AccountingDimension5), changedDate);
        }

        private static void MapOrderSettings(
                BusinessData.Models.Orders.OrderFieldSettings.FieldSettings.OrderFieldSettings updatedSettings,
                NamedObjectCollection<OrderFieldSettings> existingSettings,
                DateTime changedDate)
        {
            MapFieldSettings(updatedSettings.Property, existingSettings.FindByName(OrderFields.Property), changedDate);
            MapTextFieldSettings(updatedSettings.OrderRow1, existingSettings.FindByName(OrderFields.OrderRow1), changedDate);
            MapTextFieldSettings(updatedSettings.OrderRow2, existingSettings.FindByName(OrderFields.OrderRow2), changedDate);
            MapTextFieldSettings(updatedSettings.OrderRow3, existingSettings.FindByName(OrderFields.OrderRow3), changedDate);
            MapTextFieldSettings(updatedSettings.OrderRow4, existingSettings.FindByName(OrderFields.OrderRow4), changedDate);
            MapTextFieldSettings(updatedSettings.OrderRow5, existingSettings.FindByName(OrderFields.OrderRow5), changedDate);
            MapTextFieldSettings(updatedSettings.OrderRow6, existingSettings.FindByName(OrderFields.OrderRow6), changedDate);
            MapTextFieldSettings(updatedSettings.OrderRow7, existingSettings.FindByName(OrderFields.OrderRow7), changedDate);
            MapTextFieldSettings(updatedSettings.OrderRow8, existingSettings.FindByName(OrderFields.OrderRow8), changedDate);
            MapTextFieldSettings(updatedSettings.Configuration, existingSettings.FindByName(OrderFields.Configuration), changedDate);
            MapTextFieldSettings(updatedSettings.OrderInfo, existingSettings.FindByName(OrderFields.OrderInfo), changedDate);
            MapTextFieldSettings(updatedSettings.OrderInfo2, existingSettings.FindByName(OrderFields.OrderInfo2), changedDate);
        }

        private static void MapOtherSettings(
                OtherFieldSettings updatedSettings,
                NamedObjectCollection<OrderFieldSettings> existingSettings,
                DateTime changedDate)
        {
            MapTextFieldSettings(updatedSettings.FileName, existingSettings.FindByName(OtherFields.FileName), changedDate);
            MapTextFieldSettings(updatedSettings.CaseNumber, existingSettings.FindByName(OtherFields.CaseNumber), changedDate);
            MapTextFieldSettings(updatedSettings.Info, existingSettings.FindByName(OtherFields.Info), changedDate);
        }

        private static void MapProgramSettings(
                ProgramFieldSettings updatedSettings,
                NamedObjectCollection<OrderFieldSettings> existingSettings,
                DateTime changedDate)
        {
            MapTextFieldSettings(updatedSettings.Program, existingSettings.FindByName(ProgramFields.Program), changedDate);
            MapTextFieldSettings(updatedSettings.InfoProduct, existingSettings.FindByName(ProgramFields.InfoProduct), changedDate);
        }

        private static void MapReceiverSettings(
                ReceiverFieldSettings updatedSettings,
                NamedObjectCollection<OrderFieldSettings> existingSettings,
                DateTime changedDate)
        {
            MapTextFieldSettings(updatedSettings.ReceiverId, existingSettings.FindByName(ReceiverFields.ReceiverId), changedDate);
            MapTextFieldSettings(updatedSettings.ReceiverName, existingSettings.FindByName(ReceiverFields.ReceiverName), changedDate);
            MapTextFieldSettings(updatedSettings.ReceiverEmail, existingSettings.FindByName(ReceiverFields.ReceiverEmail), changedDate);
            MapTextFieldSettings(updatedSettings.ReceiverPhone, existingSettings.FindByName(ReceiverFields.ReceiverPhone), changedDate);
            MapTextFieldSettings(updatedSettings.ReceiverLocation, existingSettings.FindByName(ReceiverFields.ReceiverLocation), changedDate);
            MapTextFieldSettings(updatedSettings.MarkOfGoods, existingSettings.FindByName(ReceiverFields.MarkOfGoods), changedDate);
        }

        private static void MapSupplierSettings(
                SupplierFieldSettings updatedSettings,
                NamedObjectCollection<OrderFieldSettings> existingSettings,
                DateTime changedDate)
        {
            MapTextFieldSettings(updatedSettings.SupplierOrderNumber, existingSettings.FindByName(SupplierFields.SupplierOrderNumber), changedDate);
            MapTextFieldSettings(updatedSettings.SupplierOrderDate, existingSettings.FindByName(SupplierFields.SupplierOrderDate), changedDate);
            MapTextFieldSettings(updatedSettings.SupplierOrderInfo, existingSettings.FindByName(SupplierFields.SupplierOrderInfo), changedDate);
        }

        private static void MapUserSettings(
                UserFieldSettings updatedSettings,
                NamedObjectCollection<OrderFieldSettings> existingSettings,
                DateTime changedDate)
        {
            MapTextFieldSettings(updatedSettings.UserId, existingSettings.FindByName(UserFields.UserId), changedDate);
            MapTextFieldSettings(updatedSettings.UserFirstName, existingSettings.FindByName(UserFields.UserFirstName), changedDate);
            MapTextFieldSettings(updatedSettings.UserLastName, existingSettings.FindByName(UserFields.UserLastName), changedDate);
            MapTextFieldSettings(updatedSettings.UserPhone, existingSettings.FindByName(UserFields.UserPhone), changedDate);
            MapTextFieldSettings(updatedSettings.UserEMail, existingSettings.FindByName(UserFields.UserEMail), changedDate);
            //MapTextFieldSettings(updatedSettings.Initials, existingSettings.FindByName(UserFields.UserInitials), changedDate);
            MapTextFieldSettings(updatedSettings.Info, existingSettings.FindByName(UserFields.InfoUser), changedDate);
            MapTextFieldSettings(updatedSettings.Activity, existingSettings.FindByName(UserFields.Activity), changedDate);
            //MapTextFieldSettings(updatedSettings.DepartmentId1, existingSettings.FindByName(UserFields.UserDepartment_Id1), changedDate);
            MapTextFieldSettings(updatedSettings.DepartmentId2, existingSettings.FindByName(UserFields.UserDepartment_Id2), changedDate);
            MapTextFieldSettings(updatedSettings.EmploymentType, existingSettings.FindByName(UserFields.EmploymentType), changedDate);
            MapTextFieldSettings(updatedSettings.Extension, existingSettings.FindByName(UserFields.UserExtension), changedDate);
            //MapTextFieldSettings(updatedSettings.Location, existingSettings.FindByName(UserFields.UserLocation), changedDate);
            MapTextFieldSettings(updatedSettings.Manager, existingSettings.FindByName(UserFields.Manager), changedDate);
            MapTextFieldSettings(updatedSettings.PersonalIdentityNumber, existingSettings.FindByName(UserFields.UserPersonalIdentityNumber), changedDate);
            //MapTextFieldSettings(updatedSettings.PostalAddress, existingSettings.FindByName(UserFields.UserPostalAddress), changedDate);
            //MapTextFieldSettings(updatedSettings.ReferenceNumber, existingSettings.FindByName(UserFields.ReferenceNumber), changedDate);
            //MapTextFieldSettings(updatedSettings.Responsibility, existingSettings.FindByName(UserFields.Responsibility), changedDate);
            MapTextFieldSettings(updatedSettings.RoomNumber, existingSettings.FindByName(UserFields.UserRoomNumber), changedDate);
            MapTextFieldSettings(updatedSettings.Title, existingSettings.FindByName(UserFields.UserTitle), changedDate);
            //MapTextFieldSettings(updatedSettings.UnitId, existingSettings.FindByName(UserFields.UserOU_Id), changedDate);
        }

        private static void MapAccountSettings(
            AccountInfoFieldSettings updatedSettings,
            NamedObjectCollection<OrderFieldSettings> existingSettings,
            DateTime changedDate)
        {
            MapTextFieldSettings(updatedSettings.StartedDate, existingSettings.FindByName(AccountInfoFields.StartedDate), changedDate);
            MapTextFieldSettings(updatedSettings.FinishDate, existingSettings.FindByName(AccountInfoFields.FinishDate), changedDate);
            MapTextFieldSettings(updatedSettings.EMailTypeId, existingSettings.FindByName(AccountInfoFields.EMailTypeId), changedDate);
            MapTextFieldSettings(updatedSettings.HomeDirectory, existingSettings.FindByName(AccountInfoFields.HomeDirectory), changedDate);
            MapTextFieldSettings(updatedSettings.Profile, existingSettings.FindByName(AccountInfoFields.Profile), changedDate);
            //MapTextFieldSettings(updatedSettings.InventoryNumber, existingSettings.FindByName(AccountInfoFields.InventoryNumber), changedDate);
            //MapTextFieldSettings(updatedSettings.Info, existingSettings.FindByName(AccountInfoFields.Info), changedDate);
            MapOrderFieldTypeSettings(updatedSettings.AccountType, existingSettings.FindByName(AccountInfoFields.AccountType), changedDate);
            MapOrderFieldTypeSettings(updatedSettings.AccountType2, existingSettings.FindByName(AccountInfoFields.AccountType2), changedDate);
            MapOrderFieldTypeSettings(updatedSettings.AccountType3, existingSettings.FindByName(AccountInfoFields.AccountType3), changedDate);
            MapOrderFieldTypeSettings(updatedSettings.AccountType4, existingSettings.FindByName(AccountInfoFields.AccountType4), changedDate);
            MapOrderFieldTypeSettings(updatedSettings.AccountType5, existingSettings.FindByName(AccountInfoFields.AccountType5), changedDate);
        }

        private static void MapContactSettings(
                ContactFieldSettings updatedSettings,
                NamedObjectCollection<OrderFieldSettings> existingSettings,
                DateTime changedDate)
        {
            MapTextFieldSettings(updatedSettings.Id, existingSettings.FindByName(ContactFields.ContactId), changedDate);
            MapTextFieldSettings(updatedSettings.Name, existingSettings.FindByName(ContactFields.ContactName), changedDate);
            MapTextFieldSettings(updatedSettings.Phone, existingSettings.FindByName(ContactFields.ContactPhone), changedDate);
            MapTextFieldSettings(updatedSettings.Email, existingSettings.FindByName(ContactFields.ContactEMail), changedDate);
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

        #endregion
    }
}