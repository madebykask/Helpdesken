namespace DH.Helpdesk.BusinessData.Models.DailyReport.Input
{
    using System;

    using DH.Helpdesk.Domain;

    public sealed class DailyReportUpdate
    {
        public DailyReportUpdate(
                int customerId,
                int id,
                int sent,                                 
                int dailyReportSubjectId, 
                string dailyReportText,
                int userId,
                DateTime createdDate,
                DateTime changedDate
            )
        {
            this.CustomerId = customerId;
            this.Id = id;
            this.Sent = Sent;
            this.DailyReportText = dailyReportText;
            this.DailyReportSubjectId = dailyReportSubjectId;
            this.CreatedDate = createdDate;
            this.ChangedDate = changedDate;
            this.UserId = userId;
        }

        public int CustomerId { get; private set; }

        public int Id { get; private set; }

        public int Sent { get; private set; }

        public DateTime CreatedDate { get; private set; }

        public DateTime ChangedDate { get; private set; }

        public int DailyReportSubjectId { get; private set; }

        public string DailyReportText { get; private set; }

        public int UserId { get; private set; }
    }
}