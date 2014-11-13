namespace DH.Helpdesk.Services.BusinessLogic.Specifications.Orders
{
    using System;
    using System.Linq;

    using DH.Helpdesk.Domain;

    public static class OrderSpecifications
    {
        public static IQueryable<Order> GetByTypes(this IQueryable<Order> query, int[] orderTypeIds)
        {
            if (orderTypeIds == null || !orderTypeIds.Any())
            {
                return query;
            }

            query = query.Where(o => orderTypeIds.Contains(o.OrderType_Id));

            return query;
        }

        public static IQueryable<Order> GetByAdministrators(this IQueryable<Order> query, int[] administratorIds)
        {
            if (administratorIds == null || !administratorIds.Any())
            {
                return query;
            }

            query = query.Where(o => administratorIds.Contains(o.User_Id));

            return query;
        }

        public static IQueryable<Order> GetByPeriod(
                        this IQueryable<Order> query,
                        DateTime? startDate,
                        DateTime? endDate)
        {
            if (startDate.HasValue)
            {
                query = query.Where(o => o.OrderDate >= startDate);
            }

            if (endDate.HasValue)
            {
                query = query.Where(o => o.OrderDate <= endDate);
            }

            return query;
        } 

        public static IQueryable<Order> GetByStatuses(this IQueryable<Order> query, int[] statusIds)
        {
            if (statusIds == null || !statusIds.Any())
            {
                return query;
            }

            query = query.Where(o => statusIds.Contains(o.OrderState_Id));

            return query;
        }

        public static IQueryable<Order> GetByText(this IQueryable<Order> query, string text)
        {
            if (string.IsNullOrEmpty(text))
            {
                return query;
            }

            var search = text.Trim().ToLower();

            query = query.Where(o => o.OrderRow.Trim().ToLower().Contains(search) ||
                                    o.OrderRow2.Trim().ToLower().Contains(search) ||
                                    o.OrderRow3.Trim().ToLower().Contains(search) ||
                                    o.OrderRow4.Trim().ToLower().Contains(search) ||
                                    o.OrderRow5.Trim().ToLower().Contains(search) ||
                                    o.OrderRow6.Trim().ToLower().Contains(search) ||
                                    o.OrderRow7.Trim().ToLower().Contains(search) ||
                                    o.OrderRow8.Trim().ToLower().Contains(search));

            return query;
        } 

        public static IQueryable<Order> GetForList(
                        this IQueryable<Order> query,
                        int customerId,
                        int[] orderTypeIds,
                        int[] administratorIds,
                        DateTime? startDate,
                        DateTime? endDate,
                        int[] statusIds,
                        string text)
        {
            query = query
                        .GetByCustomer(customerId)
                        .GetByTypes(orderTypeIds)
                        .GetByAdministrators(administratorIds)
                        .GetByPeriod(startDate, endDate)
                        .GetByStatuses(statusIds)
                        .GetByText(text);

            return query;
        }

        public static IQueryable<OrderType> GetOrderTypes(this IQueryable<OrderType> query, int customerId)
        {
            query = query.GetByCustomer(customerId)
                    .Where(t => t.IsActive == 1);

            return query;
        }

        public static IQueryable<OrderState> GetOrderStatuses(this IQueryable<OrderState> query, int customerId)
        {
            query = query.GetByCustomer(customerId)
                    .Where(s => s.IsActive == 1);

            return query;
        } 
    }
}