using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DH.Helpdesk.BusinessData.Models.Shared.Input;
using DH.Helpdesk.Common.ValidationAttributes;
using DH.Helpdesk.SelfService.Models.Common;

namespace DH.Helpdesk.SelfService.Models.Orders
{
    public class OrdersGridModel
    {
        public OrdersGridModel(
                List<GridColumnHeaderModel> headers,
                List<OrderOverviewModel> orders,
                int ordersFound,
                SortField sortField,
                bool showType)
        {
            SortField = sortField;
            OrdersFound = ordersFound;
            Orders = orders;
            Headers = headers;
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