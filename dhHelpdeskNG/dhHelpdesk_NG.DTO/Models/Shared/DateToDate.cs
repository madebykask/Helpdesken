using System;
namespace DH.Helpdesk.BusinessData.Models.Shared
{
    public class DateToDate
    {
        public DateToDate()
        {
            this.FromDate = null;
            this.ToDate = null;
        }

        public DateTime? FromDate { get; private set; }

        public DateTime? ToDate { get; private set; }

        public DateToDate(DateTime? fromDate, DateTime? toDate)
        {
            this.FromDate = fromDate;
            this.ToDate = ToDate;
        }     
    }
}