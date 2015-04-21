namespace DH.Helpdesk.BusinessData.Models.Reports.Data.ClosedCasesDay
{
    using System;

    public sealed class ClosedCasesDayCase
    {
        public ClosedCasesDayCase(DateTime date)
        {
            this.Date = date;
        }

        public DateTime Date { get; private set; }
    }
}