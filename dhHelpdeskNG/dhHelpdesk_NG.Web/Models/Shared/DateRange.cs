namespace DH.Helpdesk.Web.Models.Shared
{
    using System;

    public class DateRange
    {
        public DateRange()
        {
        }

        public DateRange(DateTime? dateFrom, DateTime? dateTo)
        {
            this.DateFrom = dateFrom;
            this.DateTo = dateTo;
        }

        public DateTime? DateFrom { get; set; }

        public DateTime? DateTo { get; set; }
    }
}