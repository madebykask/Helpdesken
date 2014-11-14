namespace DH.Helpdesk.Services.BusinessLogic.Mappers.Orders
{
    using System.Linq;

    using DH.Helpdesk.BusinessData.Models.Orders.OrderSettings;
    using DH.Helpdesk.Common.Extensions.Integer;
    using DH.Helpdesk.Domain;

    public static class OrderFieldSettingsMapper
    {
        public static OrderFieldSettingsOverview[] MapToListOverviews(this IQueryable<OrderFieldSettings> query)
        {
            var entities = query.Select(f => new
                                            {
                                               f.OrderField,
                                               f.Label,
                                               f.Show,
                                               f.ShowInList
                                            })                                            
                                            .ToArray();

            return entities.Select(f => new OrderFieldSettingsOverview(
                                                f.OrderField, 
                                                f.Label,
                                                f.Show.ToBool(),
                                                f.ShowInList.ToBool())).ToArray();
        }

        public static string[] MapToFieldNames(this IQueryable<OrderFieldSettings> query)
        {
            return query.Select(f => f.OrderField).ToArray();
        }
    }
}