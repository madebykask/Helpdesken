using DH.Helpdesk.BusinessData.Enums.Reports;

namespace DH.Helpdesk.BusinessData.Models.ReportService
{
    public class NumberOfCasesDataFilter: CommonReportDataFilter
    {
        public NumberOfCasesGroup GroupBy { get; set; }
    }
}