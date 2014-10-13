namespace DH.Helpdesk.Mobile.Models.Inventory.SearchModels
{
    using DH.Helpdesk.Mobile.Enums.Inventory;
    using DH.Helpdesk.Mobile.Infrastructure.LocalizedAttributes;

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
    }
}