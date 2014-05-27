namespace DH.Helpdesk.BusinessData.Models.Inventory
{
    using System.Collections.Generic;

    using DH.Helpdesk.Common.ValidationAttributes;

    public class ReportModelWithInventoryType
    {
        public ReportModelWithInventoryType(int inventoryId, string inventoryName, List<ReportModel> reportModel)
        {
            this.InventoryId = inventoryId;
            this.InventoryName = inventoryName;
            this.ReportModel = reportModel;
        }

        [IsId]
        public int InventoryId { get; set; }

        [NotNullAndEmpty]
        public string InventoryName { get; set; }

        [NotNull]
        public List<ReportModel> ReportModel { get; set; }
    }
}