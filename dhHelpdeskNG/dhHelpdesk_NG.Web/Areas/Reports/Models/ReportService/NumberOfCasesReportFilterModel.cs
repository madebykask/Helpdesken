namespace DH.Helpdesk.Web.Areas.Reports.Models.ReportService
{
    public class NumberOfCasesReportFilterModel : CommonReportFilterModel
    {
        public int GroupBy { get; set; }
    }

    public class SolvedInTimeReportFilterModel : CommonReportFilterModel
    {
        public int GroupBy { get; set; }
    }
}