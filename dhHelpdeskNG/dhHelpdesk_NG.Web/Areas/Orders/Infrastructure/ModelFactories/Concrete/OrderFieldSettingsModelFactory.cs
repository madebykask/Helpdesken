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
                        CreateUserSettings(response.Settings.User));
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
                        CreateTextFieldSettingModel(settings.DeliveryOuId));
        }

        private static GeneralFieldSettingsModel CreateGeneralSettings(GeneralFieldSettings settings)
        {
            return new GeneralFieldSettingsModel(
                        CreateFieldSettingModel(settings.OrderNumber),
                        CreateFieldSettingModel(settings.Customer),
                        CreateTextFieldSettingModel(settings.Administrator),
                        CreateTextFieldSettingModel(settings.Domain),
                        CreateTextFieldSettingModel(settings.OrderDate),
                        CreateTextFieldSettingModel(settings.Status));
        }

        private static LogFieldSettingsModel CreateLogSettings(LogFieldSettings settings)
        {
            return new LogFieldSettingsModel(
                        CreateTextFieldSettingModel(settings.Log));
        }

        private static OrdererFieldSettingsModel CreateOrdererSettings(OrdererFieldSettings settings)
        {
            return new OrdererFieldSettingsModel(
                        CreateTextFieldSettingModel(settings.OrdererId),
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
                        CreateTextFieldSettingModel(settings.AccountingDimension5));
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
                        CreateTextFieldSettingModel(settings.OrderInfo2));
        }

        private static OtherFieldSettingsModel CreateOtherSettings(OtherFieldSettings settings)
        {
            return new OtherFieldSettingsModel(
                        CreateTextFieldSettingModel(settings.FileName),
                        CreateTextFieldSettingModel(settings.CaseNumber),
                        CreateTextFieldSettingModel(settings.Info));
        }

        private static ProgramFieldSettingsModel CreateProgramSettings(ProgramFieldSettings settings)
        {
            return new ProgramFieldSettingsModel(
                        CreateTextFieldSettingModel(settings.Program));
        }

        private static ReceiverFieldSettingsModel CreateReceiverSettings(ReceiverFieldSettings settings)
        {
            return new ReceiverFieldSettingsModel(
                        CreateTextFieldSettingModel(settings.ReceiverId),
                        CreateTextFieldSettingModel(settings.ReceiverName),
                        CreateTextFieldSettingModel(settings.ReceiverEmail),
                        CreateTextFieldSettingModel(settings.ReceiverPhone),
                        CreateTextFieldSettingModel(settings.ReceiverLocation),
                        CreateTextFieldSettingModel(settings.MarkOfGoods));
        }

        private static SupplierFieldSettingsModel CreateSupplierSettings(SupplierFieldSettings settings)
        {
            return new SupplierFieldSettingsModel(
                        CreateTextFieldSettingModel(settings.SupplierOrderNumber),
                        CreateTextFieldSettingModel(settings.SupplierOrderDate),
                        CreateTextFieldSettingModel(settings.SupplierOrderInfo));
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
                        CreateTextFieldSettingModel(settings.ReferenceNumber));
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
                        CreateTextFieldSettingForUpdate(settings.DeliveryOuId));
        }

        private static GeneralFieldSettings CreateGeneralForUpdate(GeneralFieldSettingsModel settings)
        {
            return new GeneralFieldSettings(
                        CreateFieldSettingForUpdate(settings.OrderNumber),
                        CreateFieldSettingForUpdate(settings.Customer),
                        CreateTextFieldSettingForUpdate(settings.Administrator),
                        CreateTextFieldSettingForUpdate(settings.Domain),
                        CreateTextFieldSettingForUpdate(settings.OrderDate),
                        CreateTextFieldSettingForUpdate(settings.Status));
        }

        private static LogFieldSettings CreateLogForUpdate(LogFieldSettingsModel settings)
        {
            return new LogFieldSettings(
                        CreateTextFieldSettingForUpdate(settings.Log));
        }

        private static OrdererFieldSettings CreateOrdererForUpdate(OrdererFieldSettingsModel settings)
        {
            return new OrdererFieldSettings(
                        CreateTextFieldSettingForUpdate(settings.OrdererId),
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
                        CreateTextFieldSettingForUpdate(settings.AccountingDimension5));
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
                        CreateTextFieldSettingForUpdate(settings.OrderInfo2));
        }

        private static OtherFieldSettings CreateOtherForUpdate(OtherFieldSettingsModel settings)
        {
            return new OtherFieldSettings(
                        CreateTextFieldSettingForUpdate(settings.FileName),
                        CreateTextFieldSettingForUpdate(settings.CaseNumber),
                        CreateTextFieldSettingForUpdate(settings.Info));
        }

        private static ProgramFieldSettings CreateProgramForUpdate(ProgramFieldSettingsModel settings)
        {
            return new ProgramFieldSettings(
                        CreateTextFieldSettingForUpdate(settings.Program));
        }

        private static ReceiverFieldSettings CreateReceiverForUpdate(ReceiverFieldSettingsModel settings)
        {
            return new ReceiverFieldSettings(
                        CreateTextFieldSettingForUpdate(settings.ReceiverId),
                        CreateTextFieldSettingForUpdate(settings.ReceiverName),
                        CreateTextFieldSettingForUpdate(settings.ReceiverEmail),
                        CreateTextFieldSettingForUpdate(settings.ReceiverPhone),
                        CreateTextFieldSettingForUpdate(settings.ReceiverLocation),
                        CreateTextFieldSettingForUpdate(settings.MarkOfGoods));
        }

        private static SupplierFieldSettings CreateSupplierForUpdate(SupplierFieldSettingsModel settings)
        {
            return new SupplierFieldSettings(
                        CreateTextFieldSettingForUpdate(settings.SupplierOrderNumber),
                        CreateTextFieldSettingForUpdate(settings.SupplierOrderDate),
                        CreateTextFieldSettingForUpdate(settings.SupplierOrderInfo));
        }

        private static UserFieldSettings CreateUserForUpdate(UserFieldSettingsModel settings)
        {
            return new UserFieldSettings(
                        CreateTextFieldSettingForUpdate(settings.UserId),
                        CreateTextFieldSettingForUpdate(settings.UserFirstName),
                        CreateTextFieldSettingForUpdate(settings.UserLastName),
                        CreateTextFieldSettingForUpdate(settings.UserPhone),
                        CreateTextFieldSettingForUpdate(settings.UserEMail),
                        CreateTextFieldSettingForUpdate(settings.Initials),
                        CreateTextFieldSettingForUpdate(settings.PersonalIdentityNumber),
                        CreateTextFieldSettingForUpdate(settings.Extension),
                        CreateTextFieldSettingForUpdate(settings.Title),
                        CreateTextFieldSettingForUpdate(settings.Location),
                        CreateTextFieldSettingForUpdate(settings.RoomNumber),
                        CreateTextFieldSettingForUpdate(settings.PostalAddress),
                        CreateTextFieldSettingForUpdate(settings.EmploymentType),
                        CreateTextFieldSettingForUpdate(settings.DepartmentId1),
                        CreateTextFieldSettingForUpdate(settings.UnitId),
                        CreateTextFieldSettingForUpdate(settings.DepartmentId2),
                        CreateTextFieldSettingForUpdate(settings.Info),
                        CreateTextFieldSettingForUpdate(settings.Responsibility),
                        CreateTextFieldSettingForUpdate(settings.Activity),
                        CreateTextFieldSettingForUpdate(settings.Manager),
                        CreateTextFieldSettingForUpdate(settings.ReferenceNumber));
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

        #endregion
    }
}