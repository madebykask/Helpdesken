namespace DH.Helpdesk.Services.BusinessLogic.Mappers.Orders
{
    using System.Collections.Generic;
    using System.Linq;

    using DH.Helpdesk.BusinessData.Models.Orders.Order.OrderEditFields;
    using DH.Helpdesk.Domain.Orders;

    public static class OrderHistoryOverviewMapper
    {
        public static List<HistoryOverview> MapToOverviews(this IQueryable<OrderHistoryEntity> query)
        {
            return new List<HistoryOverview>();
        }
    }
}