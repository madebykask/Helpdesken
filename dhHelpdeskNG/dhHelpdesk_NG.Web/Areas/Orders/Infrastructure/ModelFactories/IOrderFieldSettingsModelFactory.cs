namespace DH.Helpdesk.Web.Areas.Orders.Infrastructure.ModelFactories
{
    using DH.Helpdesk.BusinessData.Models.Orders.OrderFieldSettings;
    using DH.Helpdesk.Web.Areas.Orders.Models.OrderFieldSettings;

    public interface IOrderFieldSettingsModelFactory
    {
        OrderFieldSettingsIndexModel GetIndexModel(OrderFieldSettingsFilterData data, OrderFieldSettingsFilterModel filter);
    }
}