namespace DH.Helpdesk.Services.BusinessLogic.Mappers.Orders
{
    using System.Globalization;
    using System.Linq;

    using DH.Helpdesk.BusinessData.Models.Orders.OrderFieldSettings;
    using DH.Helpdesk.BusinessData.Models.Orders.OrderFieldSettings.FieldSettings;
    using DH.Helpdesk.BusinessData.Models.Shared;
    using DH.Helpdesk.Common.Collections;
    using DH.Helpdesk.Domain;
    using DH.Helpdesk.Services.BusinessLogic.Mappers.Orders.Data;

    using OrderFieldSettings = DH.Helpdesk.Domain.OrderFieldSettings;

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

        public static FullFieldSettings MapToFullFieldSettings(this IQueryable<OrderFieldSettings> query)
        {
            var entities = query.Select(f => new OrdersFieldSettingsMapData
                                                 {
                                                     OrderField = f.OrderField,
                                                     Show = f.Show,
                                                     ShowInList = f.ShowInList,
                                                     ShowExternal = f.ShowExternal,
                                                     Label = f.Label,
                                                     Required = f.Required,
                                                     EmailIdentifier = f.EMailIdentifier
                                                 }).ToList();

            var fieldSettings = new NamedObjectCollection<OrdersFieldSettingsMapData>(entities);
            return CreateFullFieldSettings(fieldSettings);
        }

        private static FullFieldSettings CreateFullFieldSettings(
            NamedObjectCollection<OrdersFieldSettingsMapData> fieldSettings)
        {
            return null;
        }
    }
}