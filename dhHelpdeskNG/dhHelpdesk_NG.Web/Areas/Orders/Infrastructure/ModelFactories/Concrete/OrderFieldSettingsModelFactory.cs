namespace DH.Helpdesk.Web.Areas.Orders.Infrastructure.ModelFactories.Concrete
{
    using DH.Helpdesk.BusinessData.Models.Orders.OrderFieldSettings;
    using DH.Helpdesk.BusinessData.Models.Orders.OrderFieldSettings.FieldSettings;
    using DH.Helpdesk.Web.Areas.Orders.Models.OrderFieldSettings;
    using DH.Helpdesk.Web.Areas.Orders.Models.OrderFieldSettings.FieldSettings;
    using DH.Helpdesk.Web.Infrastructure.Tools;

    public sealed class OrderFieldSettingsModelFactory : IOrderFieldSettingsModelFactory
    {
        public OrderFieldSettingsIndexModel GetIndexModel(OrderFieldSettingsFilterData data, OrderFieldSettingsFilterModel filter)
        {
            var orderTypes = WebMvcHelper.CreateListField(data.OrderTypes, filter.OrderTypeId, true);

            return new OrderFieldSettingsIndexModel(orderTypes);
        }

        public FullFieldSettingsModel Create(GetSettingsResponse response)
        {
            return new FullFieldSettingsModel(
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
                        CreateTextFieldSettingModel(settings.OrderDate));
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
                        CreateTextFieldSettingModel(settings.Info),
                        CreateTextFieldSettingModel(settings.Status));
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
                        CreateTextFieldSettingModel(settings.UserLastName));
        }

        private static FieldSettingsModel CreateFieldSettingModel(FieldSettings settings)
        {
            return new FieldSettingsModel(
                        settings.Show,
                        settings.ShowInList,
                        settings.ShowExternal,
                        settings.Label,
                        settings.Required,
                        settings.EmailIdentifier);
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
                        settings.DefaultValue);
        }
    }
}