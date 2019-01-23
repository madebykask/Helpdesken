using DH.Helpdesk.Web.Enums.Inventory;

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

        public static string CreateFilterId()
        {
            return $"{TabName.Reports}{ReportFilterMode.Inventory}";
        }
    }
}