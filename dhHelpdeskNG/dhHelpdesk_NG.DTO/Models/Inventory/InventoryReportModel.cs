namespace DH.Helpdesk.BusinessData.Models.Inventory
{
    public class InventoryReportModel
    {
        public InventoryReportModel(string inventoryName, int count)
        {
            this.InventoryName = inventoryName;
            this.Count = count;
        }

        public string InventoryName { get; set; }

        public int Count { get; set; }
    }
}