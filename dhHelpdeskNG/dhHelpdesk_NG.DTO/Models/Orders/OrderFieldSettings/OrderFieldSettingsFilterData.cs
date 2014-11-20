namespace DH.Helpdesk.BusinessData.Models.Orders.OrderFieldSettings
{
    using DH.Helpdesk.BusinessData.Models.Shared;
    using DH.Helpdesk.Common.ValidationAttributes;

    public sealed class OrderFieldSettingsFilterData
    {
        public OrderFieldSettingsFilterData(ItemOverview[] orderTypes)
        {
            this.OrderTypes = orderTypes;
        }

        [NotNull]
        public ItemOverview[] OrderTypes { get; private set; }
    }
}