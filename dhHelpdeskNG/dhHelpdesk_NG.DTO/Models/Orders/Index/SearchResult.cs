namespace DH.Helpdesk.BusinessData.Models.Orders.Index
{
    using DH.Helpdesk.BusinessData.Models.Orders.Index.OrderOverview;
    using DH.Helpdesk.Common.ValidationAttributes;

    public sealed class SearchResult
    {
        public SearchResult(
                int ordersFound, 
                FullOrderOverview[] orders)
        {
            this.Orders = orders;
            this.OrdersFound = ordersFound;
        }

        [MinValue(0)]
        public int OrdersFound { get; private set; }

        [NotNull]
        public FullOrderOverview[] Orders { get; private set; }
    }
}