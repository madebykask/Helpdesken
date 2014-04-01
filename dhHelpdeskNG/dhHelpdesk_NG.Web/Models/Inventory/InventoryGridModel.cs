namespace DH.Helpdesk.Web.Models.Inventory
{
    using System.Collections.Generic;

    using DH.Helpdesk.Common.ValidationAttributes;
    using DH.Helpdesk.Web.Models.Common;

    public sealed class InventoryGridModel
    {
        public InventoryGridModel(int changesFound, List<GridColumnHeaderModel> headers, List<InventoryOverviewModel> inventories)
        {
            this.InventoryFound = changesFound;
            this.Headers = headers;
            this.Inventories = inventories;
        }

        [NotNull]
        public List<GridColumnHeaderModel> Headers { get; set; }

        [NotNull]
        public List<InventoryOverviewModel> Inventories { get; set; }

        [MinValue(0)]
        public int InventoryFound { get; set; }
    }
}