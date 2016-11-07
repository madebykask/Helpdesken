namespace DH.Helpdesk.BusinessData.Models.Orders.Index
{
    using DH.Helpdesk.BusinessData.Models.Shared;
    using DH.Helpdesk.Common.ValidationAttributes;

    public sealed class OrdersFilterData
    {
        public OrdersFilterData(
                ItemOverview[] orderTypesSearch,
                ItemOverview[] orderTypes, 
                ItemOverview[] administrators, 
                ItemOverview[] orderStatuses)
        {
            this.OrderStatuses = orderStatuses;
            this.Administrators = administrators;
            this.OrderTypes = orderTypes;
            this.OrderTypesSearch = orderTypesSearch;
        }

        [NotNull]
        public ItemOverview[] OrderTypesSearch { get; private set; }

        [NotNull]
        public ItemOverview[] OrderTypes { get; private set; }

        [NotNull]
        public ItemOverview[] Administrators { get; private set; }

        [NotNull]
        public ItemOverview[] OrderStatuses { get; private set; }
    }
}