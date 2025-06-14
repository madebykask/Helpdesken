﻿namespace DH.Helpdesk.Services.BusinessLogic.Mappers.Orders
{
    using System.Globalization;
    using System.Linq;

    using DH.Helpdesk.BusinessData.Models.Orders.Index;
    using DH.Helpdesk.BusinessData.Models.Shared;
    using DH.Helpdesk.Domain;
    using System.Collections.Generic;

    public static class OrderMapper
    {
        public static OrdersFilterData MapToFilterData(
                                        IList<OrderType> orderTypesSearch,
                                        IList<OrderType> orderTypes,
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

            var orderTypesSearchEntities = orderTypesSearch.Select(t => new
                                                            {
                                                                t.Id,
                                                                t.Name
                                                            })
                                                            .OrderBy(t => t.Name)
                                                            .ToArray();
            //var overviews = administrators.Select(a => new { a.Id, Name = a.FirstName + " " + a.SurName, Type = "Administrator" }).Union(
            //                statuses.Select(s => new { s.Id, s.Name, Type = "Status" }))
            //                .OrderBy(o => o.Type)
            //                .ThenBy(o => o.Name)
            //                .ToArray();

            var overviews = administrators.Select(a => new { a.Id, Name = a.SurName +" " +a.FirstName, Type = "Administrator" })
                            .OrderBy(o => o.Type)
                            .ThenBy(o => o.Name)
                            .ToArray();

            var statusesEntities = statuses.Select(s => new { s.Id, s.Name, s.SortOrder, Type = "Status" })
                                   .OrderBy(s => s.SortOrder)
                                   .ToArray();



            return new OrdersFilterData(
                            orderTypesSearchEntities.Select(o => new ItemOverview(o.Name, o.Id.ToString(CultureInfo.InvariantCulture))).ToArray(),
                            orderTypesEntities.Select(o => new ItemOverview(o.Name, o.Id.ToString(CultureInfo.InvariantCulture))).ToArray(),
                            overviews.Where(o => o.Type == "Administrator").Select(o => new ItemOverview(o.Name, o.Id.ToString(CultureInfo.InvariantCulture))).ToArray(),
                            statusesEntities.Where(o => o.Type == "Status").Select(o => new ItemOverview(o.Name, o.Id.ToString(CultureInfo.InvariantCulture))).ToArray());
        }

        public static string MapToOrderNumber(this int orderId)
        {
            return string.Format("O-{0}", orderId);
        }
    }
}