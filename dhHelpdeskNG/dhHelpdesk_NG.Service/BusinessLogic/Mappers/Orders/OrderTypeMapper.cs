using System;

namespace DH.Helpdesk.Services.BusinessLogic.Mappers.Orders
{
    using System.Linq;

    using DH.Helpdesk.Domain;

    public static class OrderTypeMapper
    {
        public static string MapToName(this IQueryable<OrderType> query)
        {
            return query.First().Name;
        }

        public static string MapToDescription(this IQueryable<OrderType> query)
        {
            return query.First().Description;
        }

        public static Tuple<int?,string> MapToDocument(this IQueryable<OrderType> query)
        {
            var doc = query.First();
            if (doc.Document != null)
            {
                return new Tuple<int?, string> (doc.Document_Id, doc.Document.Description);
            }
            return new Tuple<int?, string>(doc.Document_Id, string.Empty);
        }
    }
}