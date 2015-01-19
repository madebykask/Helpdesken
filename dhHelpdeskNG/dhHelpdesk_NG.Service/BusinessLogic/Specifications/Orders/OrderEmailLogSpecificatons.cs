namespace DH.Helpdesk.Services.BusinessLogic.Specifications.Orders
{
    using System.Linq;

    using DH.Helpdesk.Domain;

    public static class OrderEmailLogSpecificatons
    {
        public static IQueryable<OrderEMailLog> GetByHistoryIds(this IQueryable<OrderEMailLog> query, int[] historyIds)
        {
            if (historyIds == null || !historyIds.Any())
            {
                return query;
            }

            query = query.Where(l => l.OrderHistoryId.HasValue && historyIds.Contains(l.OrderHistoryId.Value));

            return query;
        } 
    }
}