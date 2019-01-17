namespace DH.Helpdesk.BusinessData.Models.Inventory.Output
{
    public class InventoryTypeOverview
    {
        public bool IsStandard { get; set; }
        public bool IsActive { get; set; }
        public string Name { get; set; }
        public int Value { get; set; }
    }
}