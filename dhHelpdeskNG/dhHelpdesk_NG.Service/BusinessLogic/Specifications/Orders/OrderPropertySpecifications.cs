namespace DH.Helpdesk.Services.BusinessLogic.Specifications.Orders
{
    using System.Linq;

    using DH.Helpdesk.Domain.Orders;

    public static class OrderPropertySpecifications
    {
        public static IQueryable<OrderPropertyEntity> GetByOrderType(this IQueryable<OrderPropertyEntity> query, int? orderTypeId)
        {
            query = query.Where(p => p.OrderTypeId == orderTypeId);

            return query;
        } 
    }
}