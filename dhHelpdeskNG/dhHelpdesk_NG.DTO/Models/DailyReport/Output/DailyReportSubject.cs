using System;
using System.Collections.Generic;

namespace DH.Helpdesk.BusinessData.Models.DailyReport.Output
{
    
    public sealed class DailyReportSubjectBM
    {
        public DailyReportSubjectBM(
                int customerId, 
                int id,
                int isActive,
                int showOnStartPage,
                string subject,
                DateTime changedDate,
                DateTime createdDate
               )
        {
            this.CustomerId = customerId;
            this.Id = id;
            this.IsActive = isActive;
            this.ShowOnStartPage = showOnStartPage;
            this.Subject = subject;
            this.ChangedDate = changedDate;
            this.CreatedDate = createdDate;
        }

        public int CustomerId { get;private set; }

        public int Id { get; private set; }

        public int IsActive { get; private set; }

        public int ShowOnStartPage { get; private set; }

        public string Subject { get; private set; }

        public DateTime ChangedDate { get; private set; }

        public DateTime CreatedDate { get; private set; }

    }
}