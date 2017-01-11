using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DH.Helpdesk.Domain;

namespace DH.Helpdesk.Services.BusinessLogic.Specifications.Orders
{
    public static class OrderFieldTypesSpecifications
    {
        public static IQueryable<OrderFieldType> GetByType(
                                this IQueryable<OrderFieldType> query,
                                int? orderTypeId)
        {
            return query
                    .Where(f => f.OrderType_Id == orderTypeId);
        }

        public static IQueryable<OrderFieldType> ActiveOnly(this IQueryable<OrderFieldType> query)
        {
            return query
                    .Where(f => !f.Deleted);
        }
    }
}
