namespace DH.Helpdesk.BusinessData.Models.DailyReport.Output
{
    using System;

    using DH.Helpdesk.Domain;

    public sealed class DailyReportOverview
    {
        public DailyReportOverview(
                int id,
                int sent,
                string userName,
                DateTime createdDate, 
                DailyReportSubject dailyReportSubject, 
                string dailyReportText)
        {
            this.Id = id;
            this.Sent = Sent;
            this.UserName= userName;
            this.DailyReportText = dailyReportText;
            this.DailyReportSubject = dailyReportSubject;
            this.CreatedDate = createdDate;
        }

        public int Id { get; private set; }

        public int Sent { get; private set; }

        public string UserName { get; private set; }

        public DateTime CreatedDate { get; private set; }

        public DailyReportSubject DailyReportSubject { get; private set; }

        public string DailyReportText { get; private set; }
    }
}