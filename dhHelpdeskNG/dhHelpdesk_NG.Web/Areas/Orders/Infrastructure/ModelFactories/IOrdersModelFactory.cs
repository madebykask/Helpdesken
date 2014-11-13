namespace DH.Helpdesk.Web.Areas.Orders.Infrastructure.ModelFactories
{
    using DH.Helpdesk.BusinessData.Models.Orders.Index;
    using DH.Helpdesk.Web.Areas.Orders.Models.Index;

    public interface IOrdersModelFactory
    {
        OrdersIndexModel GetIndexModel(OrdersFilterData data, OrdersFilterModel filter);
    }
}