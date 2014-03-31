using System;
using DH.Helpdesk.Domain;

namespace DH.Helpdesk.BusinessData.Models.DailyReport.Output
{
    public sealed class DailyReportOverview
    {
        public int Customer_Id { get; set; }
        public DateTime CreatedDate { get; set; }
        public DailyReportSubject DailyReportSubject { get; set; }
        public string DailyReportText { get; set; }
    }
}