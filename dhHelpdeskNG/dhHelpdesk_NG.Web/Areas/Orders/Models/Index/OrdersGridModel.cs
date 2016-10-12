namespace DH.Helpdesk.Web.Areas.Orders.Models.Index
{
    using System.Collections.Generic;

    using DH.Helpdesk.BusinessData.Models.Shared.Input;
    using DH.Helpdesk.Common.ValidationAttributes;
    using DH.Helpdesk.Web.Models.Shared;

    public sealed class OrdersGridModel
    {
        public OrdersGridModel(
                List<GridColumnHeaderModel> headers, 
                List<OrderOverviewModel> orders,
                int ordersFound, 
                SortField sortField,
                bool showType)
        {
            this.SortField = sortField;
            this.OrdersFound = ordersFound;
            this.Orders = orders;
            this.Headers = headers;
            ShowType = showType;
        }

        [NotNull]
        public List<GridColumnHeaderModel> Headers { get; private set; }

        [NotNull]
        public List<OrderOverviewModel> Orders { get; private set; }

        [MinValue(0)]
        public int OrdersFound { get; private set; }

        public SortField SortField { get; private set; }

        public bool ShowType { get; private set; }
    }
}