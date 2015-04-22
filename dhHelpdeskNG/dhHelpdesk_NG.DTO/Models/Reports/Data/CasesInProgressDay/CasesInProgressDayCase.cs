namespace DH.Helpdesk.BusinessData.Models.Reports.Data.CasesInProgressDay
{
    using System;

    public sealed class CasesInProgressDayCase
    {
        public CasesInProgressDayCase(DateTime date)
        {
            this.Date = date;
        }

        public DateTime Date { get; private set; }
    }
}