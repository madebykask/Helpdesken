namespace DH.Helpdesk.Web.Models.Inventory.SearchModels
{
    using DH.Helpdesk.Web.Enums.Inventory;
    using DH.Helpdesk.Web.Infrastructure.LocalizedAttributes;

    public class InstaledProgramsReportSearchFilter
    {
        public InstaledProgramsReportSearchFilter()
        {
        }

        public InstaledProgramsReportSearchFilter(
            int? departmentId,
            ReportDataTypes reportDataType,
            bool isShowParentInventory,
            bool isShowMissingParentInventory,
            string searchFor)
        {
            this.DepartmentId = departmentId;
            this.ReportDataType = reportDataType;
            this.IsShowParentInventory = isShowParentInventory;
            this.IsShowMissingParentInventory = isShowMissingParentInventory;
            this.SearchFor = searchFor;
        }

        private InstaledProgramsReportSearchFilter(ReportDataTypes reportDataType)
        {
            this.ReportDataType = reportDataType;
        }

        public int? DepartmentId { get; set; }

        public ReportDataTypes ReportDataType { get; set; }

        public bool IsShowParentInventory { get; set; }

        public bool IsShowMissingParentInventory { get; set; }

        [LocalizedDisplay("Sök")]
        public string SearchFor { get; set; }

        public static InstaledProgramsReportSearchFilter CreateDefault()
        {
            return new InstaledProgramsReportSearchFilter(ReportDataTypes.Workstation);
        }
    }
}