namespace DH.Helpdesk.Web.Areas.Orders.Infrastructure.ModelFactories
{
    using DH.Helpdesk.BusinessData.Models.Orders.Order;
    using DH.Helpdesk.Web.Areas.Orders.Models.Order.OrderEdit;

    public interface IOrderModelFactory
    {
        FullOrderEditModel Create(FindOrderResponse response, int customerId);
    }
}