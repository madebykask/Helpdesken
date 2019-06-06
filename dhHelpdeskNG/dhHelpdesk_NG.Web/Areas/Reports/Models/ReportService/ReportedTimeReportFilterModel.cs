using System;

namespace DH.Helpdesk.Web.Areas.Reports.Models.ReportService
{
    public class ReportedTimeReportFilterModel : CommonReportFilterModel
    {
        public int GroupBy { get; set; }
        public DateTime? LogNoteFrom { get; set; }
        public DateTime? LogNoteTo { get; set; }
    }
}