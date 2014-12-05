namespace DH.Helpdesk.Web.Areas.Inventory.Models.SearchModels
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