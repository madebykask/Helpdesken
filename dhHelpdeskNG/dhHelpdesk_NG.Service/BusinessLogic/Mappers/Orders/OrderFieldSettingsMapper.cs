namespace DH.Helpdesk.Services.BusinessLogic.Mappers.Orders
{
    using System.Globalization;
    using System.Linq;

    using DH.Helpdesk.BusinessData.Models.Orders.OrderFieldSettings;
    using DH.Helpdesk.BusinessData.Models.Shared;
    using DH.Helpdesk.Domain;

    public static class OrderFieldSettingsMapper
    {
        public static string[] MapToFieldNames(this IQueryable<OrderFieldSettings> query)
        {
            return query.Select(f => f.OrderField).ToArray();
        }

        public static OrderFieldSettingsFilterData MapToFilterData(IQueryable<OrderType> query)
        {
            var entities = query.Select(t => new
                                            {
                                                t.Id,
                                                t.Name
                                            })
                                            .OrderBy(t => t.Name)
                                            .ToArray();

            return new OrderFieldSettingsFilterData(
                entities.Select(o => new ItemOverview(o.Name, o.Id.ToString(CultureInfo.InvariantCulture))).ToArray());
        }
    }
}