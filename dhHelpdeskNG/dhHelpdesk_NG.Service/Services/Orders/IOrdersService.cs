namespace DH.Helpdesk.Services.Services.Orders
{
    using DH.Helpdesk.BusinessData.Models.Orders.Index;

    public interface IOrdersService
    {
        OrdersFilterData GetOrdersFilterData(int customerId);
    }
}