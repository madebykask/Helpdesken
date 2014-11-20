namespace DH.Helpdesk.Web.Areas.Orders.Infrastructure.ModelFactories.Concrete
{
    using DH.Helpdesk.BusinessData.Models.Orders.OrderFieldSettings;
    using DH.Helpdesk.Web.Areas.Orders.Models.OrderFieldSettings;
    using DH.Helpdesk.Web.Infrastructure.Tools;

    public sealed class OrderFieldSettingsModelFactory : IOrderFieldSettingsModelFactory
    {
        public OrderFieldSettingsIndexModel GetIndexModel(OrderFieldSettingsFilterData data, OrderFieldSettingsFilterModel filter)
        {
            var orderTypes = WebMvcHelper.CreateListField(data.OrderTypes, filter.OrderTypeId, true);

            return new OrderFieldSettingsIndexModel(orderTypes);
        }
    }
}