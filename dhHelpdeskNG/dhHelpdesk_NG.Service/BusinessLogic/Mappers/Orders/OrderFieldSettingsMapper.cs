namespace DH.Helpdesk.Services.BusinessLogic.Mappers.Orders
{
    using System.Linq;

    using DH.Helpdesk.Domain;

    public static class OrderFieldSettingsMapper
    {
        public static string[] MapToFieldNames(this IQueryable<OrderFieldSettings> query)
        {
            return query.Select(f => f.OrderField).ToArray();
        }
    }
}