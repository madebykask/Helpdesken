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
                int changesFound, 
                SortField sortField)
        {
            this.SortField = sortField;
            this.ChangesFound = changesFound;
            this.Orders = orders;
            this.Headers = headers;
        }

        [NotNull]
        public List<GridColumnHeaderModel> Headers { get; private set; }

        [NotNull]
        public List<OrderOverviewModel> Orders { get; private set; }

        [MinValue(0)]
        public int ChangesFound { get; private set; }

        public SortField SortField { get; private set; }
    }
}