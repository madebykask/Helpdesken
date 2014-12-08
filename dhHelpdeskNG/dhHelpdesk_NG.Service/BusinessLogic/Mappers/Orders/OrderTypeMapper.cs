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
    }
}