namespace DH.Helpdesk.BusinessData.Models.Case.CaseOverview
{
    using System.Collections.Generic;

    using DH.Helpdesk.Common.ValidationAttributes;
    using System;

    //public sealed class LogsOverview
    //{
    //    public LogsOverview(List<LogOverview> logs)
    //    {
    //        this.Logs = logs;
    //    }

    //    [NotNull]
    //    public List<LogOverview> Logs { get; private set; }
    //}

    public sealed class LogsOverview
    {
        public LogsOverview(string finishingCause, DateTime? closingDate)
        {
            this.FinishingCause = finishingCause;
            this.ClosingDate = closingDate;
        }

        public string FinishingCause { get; private set; }

        public DateTime? ClosingDate { get; private set; }       
    }
}