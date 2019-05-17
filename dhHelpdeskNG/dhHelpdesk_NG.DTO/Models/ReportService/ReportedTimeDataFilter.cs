using DH.Helpdesk.BusinessData.Enums.Reports;

namespace DH.Helpdesk.BusinessData.Models.ReportService
{
    public class ReportedTimeDataFilter: CommonReportDataFilter
    {
        public ReportedTimeGroup GroupBy { get; set; }
    }
}