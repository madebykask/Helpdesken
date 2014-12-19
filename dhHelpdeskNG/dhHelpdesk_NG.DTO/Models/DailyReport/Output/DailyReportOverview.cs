using System;


using System.Collections.Generic;

namespace DH.Helpdesk.BusinessData.Models.DailyReport.Output
{
    using DH.Helpdesk.Domain;
    public sealed class DailyReportOverview
    {
        public DailyReportOverview(
                int id,
                int sent,
                string userName,
                DateTime createdDate, 
                DailyReportSubject dailyReportSubject, 
                string dailyReportText,
                string firstName,
                string lastName,
                string customerName
               )
        {
            this.Id = id;
            this.Sent = Sent;
            this.UserName= userName;
            this.DailyReportText = dailyReportText;
            this.DailyReportSubject = dailyReportSubject;
            this.CreatedDate = createdDate;
            this.FirstName = firstName;
            this.LastName = lastName;
            this.CustomerName = customerName;
        }

        public int Id { get; private set; }

        public int Sent { get; private set; }

        public string UserName { get; private set; }

        public DateTime CreatedDate { get; private set; }

        public DailyReportSubject DailyReportSubject { get; private set; }

        public string DailyReportText { get; private set; }

        public string FirstName { get; private set; }

        public string LastName { get; private set; }

        public string CustomerName { get; private set; }

    }
}