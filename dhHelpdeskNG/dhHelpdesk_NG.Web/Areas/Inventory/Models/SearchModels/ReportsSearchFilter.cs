namespace DH.Helpdesk.Web.Areas.Inventory.Models.SearchModels
{
    using DH.Helpdesk.Web.Enums.Inventory;
    using DH.Helpdesk.Web.Infrastructure.LocalizedAttributes;

    public class ReportsSearchFilter
    {
        public ReportsSearchFilter()
        {
        }

        public ReportsSearchFilter(
            int? departmentId,
            ReportDataTypes reportDataType,
            bool isShowParentInventory,
            string searchFor)
        {
            this.DepartmentId = departmentId;
            this.ReportDataType = reportDataType;
            this.IsShowParentInventory = isShowParentInventory;
            this.SearchFor = searchFor;
        }

        private ReportsSearchFilter(ReportDataTypes reportDataType)
        {
            this.ReportDataType = reportDataType;
        }

        public int? DepartmentId { get; set; }

        public ReportDataTypes ReportDataType { get; set; }

        public bool IsShowParentInventory { get; set; }

        [LocalizedDisplay("Sök")]
        public string SearchFor { get; set; }

        public static ReportsSearchFilter CreateDefault()
        {
            return new ReportsSearchFilter(ReportDataTypes.Workstation);
        }

        public static string CreateFilterId()
        {
            return $"{TabName.Reports}{ReportFilterMode.DefaultReport}";
        }
    }
}