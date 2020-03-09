using DH.Helpdesk.BusinessData.Enums.Reports;

namespace DH.Helpdesk.BusinessData.Models.ReportService
{
    public class SolvedInTimeDataFilter: CommonReportDataFilter
    {
        public SolvedInTimeGroup GroupBy { get; set; }
    }
}