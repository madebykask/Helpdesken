namespace DH.Helpdesk.Services.BusinessLogic.Mappers.Orders
{
    using System.Linq;

    using DH.Helpdesk.BusinessData.Models.Orders;
    using DH.Helpdesk.Domain;

    public static class OrderFieldSettingsMapper
    {
        public static OrderFieldSettingsOverview[] MapToListOverviews(this IQueryable<OrderFieldSettings> query)
        {
            var entities = query.Select(f => new
                                            {
                                               f.OrderField,
                                               f.Label
                                            })                                            
                                            .ToArray();

            return entities.Select(f => new OrderFieldSettingsOverview(f.OrderField, f.Label))
                            .ToArray();
        }
    }
}