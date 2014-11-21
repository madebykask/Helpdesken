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
            return null;
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