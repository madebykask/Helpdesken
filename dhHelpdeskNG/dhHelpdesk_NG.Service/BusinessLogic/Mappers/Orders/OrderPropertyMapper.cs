namespace DH.Helpdesk.Services.BusinessLogic.Mappers.Orders
{
    using System.Globalization;
    using System.Linq;

    using DH.Helpdesk.BusinessData.Models.Shared;
    using DH.Helpdesk.Domain.Orders;

    public static class OrderPropertyMapper
    {
        public static ItemOverview[] MapToItemOverviews(this IQueryable<OrderPropertyEntity> query)
        {
            var entities = query.Select(p => new
                                        {
                                            p.Id,
                                            p.OrderProperty
                                        }).ToArray();

            return entities.Select(o => new ItemOverview(o.OrderProperty, o.Id.ToString(CultureInfo.InvariantCulture))).ToArray();
        }
    }
}