namespace DH.Helpdesk.Services.BusinessLogic.Mappers.Orders
{
    using System.Globalization;
    using System.Linq;

    using DH.Helpdesk.BusinessData.Models.Orders.Index;
    using DH.Helpdesk.BusinessData.Models.Shared;
    using DH.Helpdesk.Domain;

    public static class OrderMapper
    {
        public static OrdersFilterData MapToFilterData(
                                        IQueryable<OrderType> orderTypes,
                                        IQueryable<User> administrators,
                                        IQueryable<OrderState> statuses)
        {
            var orderTypesEntities = orderTypes.Select(t => new
                                                            {
                                                                t.Id,
                                                                t.Name
                                                            })
                                                            .OrderBy(t => t.Name)
                                                            .ToArray();
            var overviews = administrators.Select(a => new { a.Id, Name = a.FirstName + " " + a.SurName, Type = "Administrator" }).Union(
                            statuses.Select(s => new { s.Id, s.Name, Type = "Status" }))
                            .OrderBy(o => o.Type)
                            .ThenBy(o => o.Name)
                            .ToArray();

            return new OrdersFilterData(
                            orderTypesEntities.Select(o => new ItemOverview(o.Name, o.Id.ToString(CultureInfo.InvariantCulture))).ToArray(),
                            overviews.Where(o => o.Type == "Administrator").Select(o => new ItemOverview(o.Name, o.Id.ToString(CultureInfo.InvariantCulture))).ToArray(),
                            overviews.Where(o => o.Type == "Status").Select(o => new ItemOverview(o.Name, o.Id.ToString(CultureInfo.InvariantCulture))).ToArray());
        }

        public static string MapToOrderNumber(this int orderId)
        {
            return string.Format("O-{0}", orderId);
        }
    }
}