namespace DH.Helpdesk.Web.Models.Common
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

        public DateTime? DateFrom { get; private set; }

        public DateTime? DateTo { get; private set; }
    }
}