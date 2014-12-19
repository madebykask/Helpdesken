namespace DH.Helpdesk.Services.BusinessLogic.Specifications.Orders
{
    using System.Collections.Generic;
    using System.Linq;

    using DH.Helpdesk.Domain;

    public static class OrderLogSpecifications
    {
        public static IQueryable<OrderLog> GetExcludeSpecified(
                                    this IQueryable<OrderLog> query, 
                                    int orderId, 
                                    List<int> excludeLogIds)
        {
            query = query.Where(l => l.Order_Id == orderId && !excludeLogIds.Contains(l.Id) && l.LogNote != null);

            return query;
        }
    }
}