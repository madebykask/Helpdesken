namespace DH.Helpdesk.Web.Areas.OrderAccounts.Models.Order
{
    using System.Collections.Generic;

    using DH.Helpdesk.Common.ValidationAttributes;
    using DH.Helpdesk.Web.Areas.Inventory.Models;
    using DH.Helpdesk.Web.Models.Shared;

    public class GridModel
    {
        public GridModel(
            List<GridColumnHeaderModel> headers,
            List<InventoryOverviewModel> orders,
            int currentActivity,
            SortFieldModel sortField)
        {
            this.Headers = headers;
            this.Orders = orders;
            this.CurrentActivity = currentActivity;
            this.SortField = sortField;
        }

        [NotNull]
        public List<GridColumnHeaderModel> Headers { get; set; }

        [NotNull]
        public List<InventoryOverviewModel> Orders { get; set; }

        public int CurrentActivity { get; set; }

        public string CurrentModeName { get; set; }

        public SortFieldModel SortField { get; set; }
    }
}