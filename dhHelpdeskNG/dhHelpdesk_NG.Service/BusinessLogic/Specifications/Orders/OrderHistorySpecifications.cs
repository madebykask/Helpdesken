namespace DH.Helpdesk.Services.BusinessLogic.Specifications.Orders
{
    using System.Linq;

    using DH.Helpdesk.Domain.Orders;

    public static class OrderHistorySpecifications
    {
        public static IQueryable<OrderHistoryEntity> GetByOrder(this IQueryable<OrderHistoryEntity> query, int orderId)
        {
            query = query.Where(h => h.OrderId == orderId);

            return query;
        } 
    }
}