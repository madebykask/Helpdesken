namespace DH.Helpdesk.Services.Services.Orders
{
    using DH.Helpdesk.BusinessData.Models.Orders.Index;
    using DH.Helpdesk.BusinessData.Models.Orders.Order;

    public interface IOrdersService
    {
        OrdersFilterData GetOrdersFilterData(int customerId);

        SearchResponse Search(SearchParameters parameters);

        NewOrderEditData GetNewOrderEditData(int customerId, int orderTypeId);

        FindOrderResponse FindOrder(int orderId, int customerId);

        int AddOrUpdate(UpdateOrderRequest request);

        void Delete(int id);
    }
}