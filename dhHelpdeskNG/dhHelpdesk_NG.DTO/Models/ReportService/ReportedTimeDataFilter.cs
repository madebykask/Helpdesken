using System;
using DH.Helpdesk.BusinessData.Enums.Reports;

namespace DH.Helpdesk.BusinessData.Models.ReportService
{
    public class ReportedTimeDataFilter: CommonReportDataFilter
    {
        public ReportedTimeGroup GroupBy { get; set; }
        public DateTime? LogNoteFrom { get; set; }
        public DateTime? LogNoteTo { get; set; }
    }
}