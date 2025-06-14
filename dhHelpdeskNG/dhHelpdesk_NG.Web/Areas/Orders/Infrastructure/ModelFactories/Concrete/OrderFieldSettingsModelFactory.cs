﻿using System.Collections.Generic;
using System.Linq;
using DH.Helpdesk.Domain.Orders;
using DH.Helpdesk.Web.Infrastructure.Extensions;

namespace DH.Helpdesk.Web.Areas.Orders.Infrastructure.ModelFactories.Concrete
{
    using System;

    using DH.Helpdesk.BusinessData.Models.Orders.OrderFieldSettings;
    using DH.Helpdesk.BusinessData.Models.Orders.OrderFieldSettings.FieldSettings;
    using DH.Helpdesk.Web.Areas.Orders.Models.OrderFieldSettings;
    using DH.Helpdesk.Web.Areas.Orders.Models.OrderFieldSettings.FieldSettings;
    using DH.Helpdesk.Web.Infrastructure.Tools;
    using OrderFieldSettingsModel = DH.Helpdesk.Web.Areas.Orders.Models.OrderFieldSettings.FieldSettings.OrderFieldSettingsModel;

    public sealed class OrderFieldSettingsModelFactory : IOrderFieldSettingsModelFactory
    {
        public OrderFieldSettingsIndexModel GetIndexModel(OrderFieldSettingsFilterData data, OrderFieldSettingsFilterModel filter)
        {
            var orderTypes = WebMvcHelper.CreateListField(data.OrderTypes, filter.OrderTypeId, true);

            return new OrderFieldSettingsIndexModel(orderTypes);
        }

        public FullFieldSettingsModel Create(GetSettingsResponse response, int? orderTypeId)
        {
           return new FullFieldSettingsModel(
                        orderTypeId,
                        CreateDeliverySettings(response.Settings.Delivery),
                        CreateGeneralSettings(response.Settings.General),
                        CreateLogSettings(response.Settings.Log),
                        CreateOrdererSettings(response.Settings.Orderer),
                        CreateOrderSettings(response.Settings.Order),
                        CreateOtherSettings(response.Settings.Other),
                        CreateProgramSettings(response.Settings.Program),
                        CreateReceiverSettings(response.Settings.Receiver),
                        CreateSupplierSettings(response.Settings.Supplier),
                        CreateUserSettings(response.Settings.User),
                        CreateAccountInfoSettings(response.Settings.AccountInfo));
        }

        public FullFieldSettings CreateForUpdate(
                        FullFieldSettingsModel model, 
                        int customerId,
                        int? orderTypeId,
                        DateTime changedDate)
        {
            return FullFieldSettings.CreateUpdated(
                        customerId,
                        orderTypeId,
                        CreateDeliveryForUpdate(model.Delivery),
                        CreateGeneralForUpdate(model.General),
                        CreateLogForUpdate(model.Log),
                        CreateOrdererForUpdate(model.Orderer),
                        CreateOrderForUpdate(model.Order),
                        CreateOtherForUpdate(model.Other),
                        CreateProgramForUpdate(model.Program),
                        CreateReceiverForUpdate(model.Receiver),
                        CreateSupplierForUpdate(model.Supplier),
                        CreateUserForUpdate(model.User),
                        CreateAccountInfoForUpdate(model.AccountInfo),
                        changedDate);
        }

        #region Create model for edit

        private static DeliveryFieldSettingsModel CreateDeliverySettings(DeliveryFieldSettings settings)
        {
            return new DeliveryFieldSettingsModel(
                        CreateTextFieldSettingModel(settings.DeliveryDate),
                        CreateTextFieldSettingModel(settings.InstallDate),
                        CreateTextFieldSettingModel(settings.DeliveryDepartment),
                        CreateTextFieldSettingModel(settings.DeliveryOu),
                        CreateTextFieldSettingModel(settings.DeliveryAddress),
                        CreateTextFieldSettingModel(settings.DeliveryPostalCode),
                        CreateTextFieldSettingModel(settings.DeliveryPostalAddress),
                        CreateTextFieldSettingModel(settings.DeliveryLocation),
                        CreateTextFieldSettingModel(settings.DeliveryInfo1),
                        CreateTextFieldSettingModel(settings.DeliveryInfo2),
                        CreateTextFieldSettingModel(settings.DeliveryInfo3),
                        CreateTextFieldSettingModel(settings.DeliveryOuId),
                        CreateTextFieldSettingModel(settings.Name),
                        CreateTextFieldSettingModel(settings.Phone))
            {
                Header = settings.Header
            };
        }

        private static GeneralFieldSettingsModel CreateGeneralSettings(GeneralFieldSettings settings)
        {
            return new GeneralFieldSettingsModel(
                CreateFieldSettingModel(settings.OrderNumber),
                CreateFieldSettingModel(settings.Customer),
                CreateTextFieldSettingModel(settings.Administrator),
                CreateTextFieldSettingModel(settings.Domain),
                CreateTextFieldSettingModel(settings.OrderDate),
                CreateTextFieldSettingModel(settings.Status))
            {
                Header = settings.Header
            };
        }

        private static LogFieldSettingsModel CreateLogSettings(LogFieldSettings settings)
        {
            return new LogFieldSettingsModel(
                        CreateTextFieldSettingModel(settings.Log));
        }

        private static OrdererFieldSettingsModel CreateOrdererSettings(OrdererFieldSettings settings)
        {
            return new OrdererFieldSettingsModel(
                        CreateMultiTextFieldSettingModel(settings.OrdererId),
                        CreateTextFieldSettingModel(settings.OrdererName),
                        CreateTextFieldSettingModel(settings.OrdererLocation),
                        CreateTextFieldSettingModel(settings.OrdererEmail),
                        CreateTextFieldSettingModel(settings.OrdererPhone),
                        CreateTextFieldSettingModel(settings.OrdererCode),
                        CreateTextFieldSettingModel(settings.Department),
                        CreateTextFieldSettingModel(settings.Unit),
                        CreateTextFieldSettingModel(settings.OrdererAddress),
                        CreateTextFieldSettingModel(settings.OrdererInvoiceAddress),
                        CreateTextFieldSettingModel(settings.OrdererReferenceNumber),
                        CreateTextFieldSettingModel(settings.AccountingDimension1),
                        CreateFieldSettingModel(settings.AccountingDimension2),
                        CreateTextFieldSettingModel(settings.AccountingDimension3),
                        CreateFieldSettingModel(settings.AccountingDimension4),
                        CreateTextFieldSettingModel(settings.AccountingDimension5))
            {
                Header = settings.Header
            };
        }

        private static OrderFieldSettingsModel CreateOrderSettings(OrderFieldSettings settings)
        {
            return new OrderFieldSettingsModel(
                        CreateFieldSettingModel(settings.Property),
                        CreateTextFieldSettingModel(settings.OrderRow1),
                        CreateTextFieldSettingModel(settings.OrderRow2),
                        CreateTextFieldSettingModel(settings.OrderRow3),
                        CreateTextFieldSettingModel(settings.OrderRow4),
                        CreateTextFieldSettingModel(settings.OrderRow5),
                        CreateTextFieldSettingModel(settings.OrderRow6),
                        CreateTextFieldSettingModel(settings.OrderRow7),
                        CreateTextFieldSettingModel(settings.OrderRow8),
                        CreateTextFieldSettingModel(settings.Configuration),
                        CreateTextFieldSettingModel(settings.OrderInfo),
                        CreateTextFieldSettingModel(settings.OrderInfo2))
            {
                Header = settings.Header
            };
        }

        private static OtherFieldSettingsModel CreateOtherSettings(OtherFieldSettings settings)
        {
            return new OtherFieldSettingsModel(
                        CreateTextFieldSettingModel(settings.FileName),
                        CreateTextFieldSettingModel(settings.CaseNumber),
                        CreateTextFieldSettingModel(settings.Info))
            {
                Header = settings.Header
            };
        }

        private static ProgramFieldSettingsModel CreateProgramSettings(ProgramFieldSettings settings)
        {
            return new ProgramFieldSettingsModel(
                        CreateTextFieldSettingModel(settings.Program),
                        CreateTextFieldSettingModel(settings.InfoProduct))
            {
                Header = settings.Header
            };
        }

        private static ReceiverFieldSettingsModel CreateReceiverSettings(ReceiverFieldSettings settings)
        {
            return new ReceiverFieldSettingsModel(
                        CreateTextFieldSettingModel(settings.ReceiverId),
                        CreateTextFieldSettingModel(settings.ReceiverName),
                        CreateTextFieldSettingModel(settings.ReceiverEmail),
                        CreateTextFieldSettingModel(settings.ReceiverPhone),
                        CreateTextFieldSettingModel(settings.ReceiverLocation),
                        CreateTextFieldSettingModel(settings.MarkOfGoods))
            {
                Header = settings.Header
            };
        }

        private static SupplierFieldSettingsModel CreateSupplierSettings(SupplierFieldSettings settings)
        {
            return new SupplierFieldSettingsModel(
                        CreateTextFieldSettingModel(settings.SupplierOrderNumber),
                        CreateTextFieldSettingModel(settings.SupplierOrderDate),
                        CreateTextFieldSettingModel(settings.SupplierOrderInfo))
            {
                Header = settings.Header
            };
        }        

        private static UserFieldSettingsModel CreateUserSettings(UserFieldSettings settings)
        {
            return new UserFieldSettingsModel(
                        CreateTextFieldSettingModel(settings.UserId),
                        CreateTextFieldSettingModel(settings.UserFirstName),
                        CreateTextFieldSettingModel(settings.UserLastName),
                        CreateTextFieldSettingModel(settings.UserPhone),
                        CreateTextFieldSettingModel(settings.UserEMail),
                        CreateTextFieldSettingModel(settings.PersonalIdentityNumber),
                        CreateTextFieldSettingModel(settings.Initials),
                        CreateTextFieldSettingModel(settings.Extension),
                        CreateTextFieldSettingModel(settings.Title),
                        CreateTextFieldSettingModel(settings.Location),
                        CreateTextFieldSettingModel(settings.RoomNumber),
                        CreateTextFieldSettingModel(settings.PostalAddress),
                        CreateTextFieldSettingModel(settings.EmploymentType),
                        CreateTextFieldSettingModel(settings.DepartmentId1),
                        CreateTextFieldSettingModel(settings.UnitId),
                        CreateTextFieldSettingModel(settings.DepartmentId2),
                        CreateTextFieldSettingModel(settings.Info),
                        CreateTextFieldSettingModel(settings.Responsibility),
                        CreateTextFieldSettingModel(settings.Activity),
                        CreateTextFieldSettingModel(settings.Manager),
                        CreateTextFieldSettingModel(settings.ReferenceNumber))
            {
                Header = settings.Header
            };
        }

        private static AccountInfoFieldSettingsModel CreateAccountInfoSettings(AccountInfoFieldSettings settings)
        {
            return new AccountInfoFieldSettingsModel(
                    CreateTextFieldSettingModel(settings.StartedDate),
                    CreateTextFieldSettingModel(settings.FinishDate),
                    CreateTextFieldSettingModel(settings.EMailTypeId),
                    CreateTextFieldSettingModel(settings.HomeDirectory),
                    CreateTextFieldSettingModel(settings.Profile),
                    CreateTextFieldSettingModel(settings.InventoryNumber),
                    CreateTextFieldSettingModel(settings.Info),
                    CreateOrderFieldTypeSettingModel(settings.AccountType, OrderFieldTypes.AccountType),
                    CreateOrderFieldTypeSettingModel(settings.AccountType2, OrderFieldTypes.AccountType2),
                    CreateOrderFieldTypeSettingModel(settings.AccountType3, OrderFieldTypes.AccountType3),
                    CreateOrderFieldTypeSettingModel(settings.AccountType4, OrderFieldTypes.AccountType4),
                    CreateOrderFieldTypeSettingModel(settings.AccountType5, OrderFieldTypes.AccountType5)
                );
        }


        private static OrderFieldTypeSettingsModel CreateOrderFieldTypeSettingModel(OrderFieldTypeSettings settings, OrderFieldTypes type)
        {
            var values = settings.Values?.Select(v => new OrderFieldTypeValueSettingsModel
            {
                Id = v.Id,
                Value = v.Value
            }).ToList();
            return new OrderFieldTypeSettingsModel(
                settings.Show,
                settings.ShowInList,
                settings.ShowExternal,
                settings.Label,
                settings.Required,
                settings.EmailIdentifier,
                settings.FieldHelp,
                values,
                type);
        }

        private static FieldSettingsModel CreateFieldSettingModel(FieldSettings settings)
        {
            return new FieldSettingsModel(
                        settings.Show,
                        settings.ShowInList,
                        settings.ShowExternal,
                        settings.Label,
                        settings.Required,
                        settings.EmailIdentifier,
                        settings.FieldHelp);
        }

        private static TextFieldSettingsModel CreateTextFieldSettingModel(TextFieldSettings settings)
        {
            return new TextFieldSettingsModel(
                        settings.Show,
                        settings.ShowInList,
                        settings.ShowExternal,
                        settings.Label,
                        settings.Required,
                        settings.EmailIdentifier,
                        settings.DefaultValue,
                        settings.FieldHelp);
        }

        private static MultiTextFieldSettingsModel CreateMultiTextFieldSettingModel(MultiTextFieldSettings settings)
        {
            return new MultiTextFieldSettingsModel(
                        settings.Show,
                        settings.ShowInList,
                        settings.ShowExternal,
                        settings.Label,
                        settings.Required,
                        settings.EmailIdentifier,
                        settings.DefaultValue,
                        settings.FieldHelp,
                        settings.MultiValue);
        }

        #endregion

        #region Create model for update

        private static DeliveryFieldSettings CreateDeliveryForUpdate(DeliveryFieldSettingsModel settings)
        {
            return new DeliveryFieldSettings(
                        CreateTextFieldSettingForUpdate(settings.DeliveryDate),
                        CreateTextFieldSettingForUpdate(settings.InstallDate),
                        CreateTextFieldSettingForUpdate(settings.DeliveryDepartment),
                        CreateTextFieldSettingForUpdate(settings.DeliveryOu),
                        CreateTextFieldSettingForUpdate(settings.DeliveryAddress),
                        CreateTextFieldSettingForUpdate(settings.DeliveryPostalCode),
                        CreateTextFieldSettingForUpdate(settings.DeliveryPostalAddress),
                        CreateTextFieldSettingForUpdate(settings.DeliveryLocation),
                        CreateTextFieldSettingForUpdate(settings.DeliveryInfo1),
                        CreateTextFieldSettingForUpdate(settings.DeliveryInfo2),
                        CreateTextFieldSettingForUpdate(settings.DeliveryInfo3),
                        CreateTextFieldSettingForUpdate(settings.DeliveryOuId),
                        CreateTextFieldSettingForUpdate(settings.Name),
                        CreateTextFieldSettingForUpdate(settings.Phone))
            {
                Header = settings.Header
            };
        }

        private static GeneralFieldSettings CreateGeneralForUpdate(GeneralFieldSettingsModel settings)
        {
            return new GeneralFieldSettings(
                        CreateFieldSettingForUpdate(settings.OrderNumber),
                        CreateFieldSettingForUpdate(settings.Customer),
                        CreateTextFieldSettingForUpdate(settings.Administrator),
                        CreateTextFieldSettingForUpdate(settings.Domain),
                        CreateTextFieldSettingForUpdate(settings.OrderDate),
                        CreateTextFieldSettingForUpdate(settings.Status))
            {
                Header = settings.Header
            };
        }

        private static LogFieldSettings CreateLogForUpdate(LogFieldSettingsModel settings)
        {
            return new LogFieldSettings(
                        CreateTextFieldSettingForUpdate(settings.Log));
        }

        private static OrdererFieldSettings CreateOrdererForUpdate(OrdererFieldSettingsModel settings)
        {
            return new OrdererFieldSettings(
                        CreateMultiTextFieldSettingForUpdate(settings.OrdererId),
                        CreateTextFieldSettingForUpdate(settings.OrdererName),
                        CreateTextFieldSettingForUpdate(settings.OrdererLocation),
                        CreateTextFieldSettingForUpdate(settings.OrdererEmail),
                        CreateTextFieldSettingForUpdate(settings.OrdererPhone),
                        CreateTextFieldSettingForUpdate(settings.OrdererCode),
                        CreateTextFieldSettingForUpdate(settings.Department),
                        CreateTextFieldSettingForUpdate(settings.Unit),
                        CreateTextFieldSettingForUpdate(settings.OrdererAddress),
                        CreateTextFieldSettingForUpdate(settings.OrdererInvoiceAddress),
                        CreateTextFieldSettingForUpdate(settings.OrdererReferenceNumber),
                        CreateTextFieldSettingForUpdate(settings.AccountingDimension1),
                        CreateFieldSettingForUpdate(settings.AccountingDimension2),
                        CreateTextFieldSettingForUpdate(settings.AccountingDimension3),
                        CreateFieldSettingForUpdate(settings.AccountingDimension4),
                        CreateTextFieldSettingForUpdate(settings.AccountingDimension5))
            {
                Header = settings.Header
            };
        }

        private static OrderFieldSettings CreateOrderForUpdate(OrderFieldSettingsModel settings)
        {
            return new OrderFieldSettings(
                        CreateFieldSettingForUpdate(settings.Property),
                        CreateTextFieldSettingForUpdate(settings.OrderRow1),
                        CreateTextFieldSettingForUpdate(settings.OrderRow2),
                        CreateTextFieldSettingForUpdate(settings.OrderRow3),
                        CreateTextFieldSettingForUpdate(settings.OrderRow4),
                        CreateTextFieldSettingForUpdate(settings.OrderRow5),
                        CreateTextFieldSettingForUpdate(settings.OrderRow6),
                        CreateTextFieldSettingForUpdate(settings.OrderRow7),
                        CreateTextFieldSettingForUpdate(settings.OrderRow8),
                        CreateTextFieldSettingForUpdate(settings.Configuration),
                        CreateTextFieldSettingForUpdate(settings.OrderInfo),
                        CreateTextFieldSettingForUpdate(settings.OrderInfo2))
            {
                Header = settings.Header
            };
        }

        private static OtherFieldSettings CreateOtherForUpdate(OtherFieldSettingsModel settings)
        {
            return new OtherFieldSettings(
                        CreateTextFieldSettingForUpdate(settings.FileName),
                        CreateTextFieldSettingForUpdate(settings.CaseNumber),
                        CreateTextFieldSettingForUpdate(settings.Info))
            {
                Header = settings.Header
            };
        }

        private static ProgramFieldSettings CreateProgramForUpdate(ProgramFieldSettingsModel settings)
        {
            return new ProgramFieldSettings(
                        CreateTextFieldSettingForUpdate(settings.Program),
                        CreateTextFieldSettingForUpdate(settings.InfoProduct))
            {
                Header = settings.Header
            };
        }

        private static ReceiverFieldSettings CreateReceiverForUpdate(ReceiverFieldSettingsModel settings)
        {
            return new ReceiverFieldSettings(
                        CreateTextFieldSettingForUpdate(settings.ReceiverId),
                        CreateTextFieldSettingForUpdate(settings.ReceiverName),
                        CreateTextFieldSettingForUpdate(settings.ReceiverEmail),
                        CreateTextFieldSettingForUpdate(settings.ReceiverPhone),
                        CreateTextFieldSettingForUpdate(settings.ReceiverLocation),
                        CreateTextFieldSettingForUpdate(settings.MarkOfGoods))
            {
                Header = settings.Header
            };
        }

        private static SupplierFieldSettings CreateSupplierForUpdate(SupplierFieldSettingsModel settings)
        {
            return new SupplierFieldSettings(
                        CreateTextFieldSettingForUpdate(settings.SupplierOrderNumber),
                        CreateTextFieldSettingForUpdate(settings.SupplierOrderDate),
                        CreateTextFieldSettingForUpdate(settings.SupplierOrderInfo))
            {
                Header = settings.Header
            };
        }

        private static UserFieldSettings CreateUserForUpdate(UserFieldSettingsModel settings)
        {
            return new UserFieldSettings(
                        CreateTextFieldSettingForUpdate(settings.UserId),
                        CreateTextFieldSettingForUpdate(settings.UserFirstName),
                        CreateTextFieldSettingForUpdate(settings.UserLastName),
                        CreateTextFieldSettingForUpdate(settings.UserPhone),
                        CreateTextFieldSettingForUpdate(settings.UserEMail),
                        null,
                        CreateTextFieldSettingForUpdate(settings.PersonalIdentityNumber),
                        CreateTextFieldSettingForUpdate(settings.Extension),
                        CreateTextFieldSettingForUpdate(settings.Title),
                        null,
                        CreateTextFieldSettingForUpdate(settings.RoomNumber),
                        null,
                        CreateTextFieldSettingForUpdate(settings.EmploymentType),
                        null,
                        null,
                        CreateTextFieldSettingForUpdate(settings.DepartmentId2),
                        CreateTextFieldSettingForUpdate(settings.Info),
                        null,
                        CreateTextFieldSettingForUpdate(settings.Activity),
                        CreateTextFieldSettingForUpdate(settings.Manager),
                        null)
            {
                Header = settings.Header
            };
        }

        private static AccountInfoFieldSettings CreateAccountInfoForUpdate(AccountInfoFieldSettingsModel settings)
        {
            return new AccountInfoFieldSettings(
                    CreateTextFieldSettingForUpdate(settings.StartedDate),
                    CreateTextFieldSettingForUpdate(settings.FinishDate),
                    CreateTextFieldSettingForUpdate(settings.EMailTypeId),
                    CreateTextFieldSettingForUpdate(settings.HomeDirectory),
                    CreateTextFieldSettingForUpdate(settings.Profile),
                    null,
                    null,
                    CreateOrderFieldTypeSetting(settings.AccountType, OrderFieldTypes.AccountType),
                    CreateOrderFieldTypeSetting(settings.AccountType2, OrderFieldTypes.AccountType2),
                    CreateOrderFieldTypeSetting(settings.AccountType3, OrderFieldTypes.AccountType3),
                    CreateOrderFieldTypeSetting(settings.AccountType4, OrderFieldTypes.AccountType4),
                    CreateOrderFieldTypeSetting(settings.AccountType5, OrderFieldTypes.AccountType5)
                );
        }

        private static OrderFieldTypeSettings CreateOrderFieldTypeSetting(OrderFieldTypeSettingsModel settings, OrderFieldTypes type)
        {
            var values = settings.Values?.Select(s => new OrderFieldTypeValueSetting(s.Id, s.Value, type)).ToList();
            return OrderFieldTypeSettings.CreateUpdated(
                        settings.Show,
                        settings.ShowInList,
                        settings.ShowExternal,
                        settings.Label,
                        settings.Required,
                        settings.EmailIdentifier,
                        settings.Help,
                        values);
        }

        private static FieldSettings CreateFieldSettingForUpdate(FieldSettingsModel settings)
        {

            return FieldSettings.CreateUpdated(
                        settings.Show,
                        settings.ShowInList,
                        settings.ShowExternal,
                        settings.Label,
                        settings.Required,
                        settings.EmailIdentifier,
                        settings.Help);
        }

        private static TextFieldSettings CreateTextFieldSettingForUpdate(TextFieldSettingsModel settings)
        {
            return TextFieldSettings.CreateUpdated(
                        settings.Show,
                        settings.ShowInList,
                        settings.ShowExternal,
                        settings.Label,
                        settings.Required,
                        settings.EmailIdentifier,
                        settings.DefaultValue,
                        settings.Help);
        }

        

        private static MultiTextFieldSettings CreateMultiTextFieldSettingForUpdate(MultiTextFieldSettingsModel settings)
        {
            return MultiTextFieldSettings.CreateUpdated(
                        settings.Show,
                        settings.ShowInList,
                        settings.ShowExternal,
                        settings.Label,
                        settings.Required,
                        settings.EmailIdentifier,
                        settings.DefaultValue,
                        settings.Help,
                        settings.IsMultiple);
        }

        #endregion
    }
}