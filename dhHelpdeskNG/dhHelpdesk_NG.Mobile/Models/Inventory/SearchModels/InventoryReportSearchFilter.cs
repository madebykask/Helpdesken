namespace DH.Helpdesk.Mobile.Models.Inventory.SearchModels
{
    using DH.Helpdesk.Common.ValidationAttributes;

    public class InventoryReportSearchFilter
    {
        public InventoryReportSearchFilter()
        {
        }

        public InventoryReportSearchFilter(int? departmentId)
        {
            this.DepartmentId = departmentId;
        }

        [IsId]
        public int? DepartmentId { get; set; }

        public static InventoryReportSearchFilter CreateDefault()
        {
            return new InventoryReportSearchFilter();
        }
    }
}