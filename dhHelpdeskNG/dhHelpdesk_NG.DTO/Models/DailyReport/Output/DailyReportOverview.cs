namespace DH.Helpdesk.BusinessData.Models.DailyReport.Output
{
    using System;

    using DH.Helpdesk.Domain;

    public sealed class DailyReportOverview
    {
        public DailyReportOverview(
                DateTime createdDate, 
                DailyReportSubject dailyReportSubject, 
                string dailyReportText)
        {
            this.DailyReportText = dailyReportText;
            this.DailyReportSubject = dailyReportSubject;
            this.CreatedDate = createdDate;
        }

        public DateTime CreatedDate { get; private set; }

        public DailyReportSubject DailyReportSubject { get; private set; }

        public string DailyReportText { get; private set; }
    }
}