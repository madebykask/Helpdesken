namespace DH.Helpdesk.BusinessData.Models.Reports.Data
{
    using System;

    public sealed class RegisteredCaseDay
    {
        public RegisteredCaseDay(DateTime date)
        {
            this.Date = date;
        }

        public DateTime Date { get; private set; }
    }
}