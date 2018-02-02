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
        public LogsOverview(string finishingCause, DateTime? closingDate, string finishingDescription,
                            List<string> internalNotes, List<string> externalNotes,
                            int? totalMaterial = null, int? totalOverTime = null, decimal? totalPrice = null, int? totalWork = null)
        {
            this.FinishingCause = finishingCause;
            this.ClosingDate = closingDate;
            this.FinishingDescription = finishingDescription;
            this.IntenalLogNote = internalNotes != null? string.Join(Environment.NewLine, internalNotes) : string.Empty;
            this.ExternalLogNote = externalNotes != null? string.Join(Environment.NewLine, externalNotes) : string.Empty;
            this.TotalMaterial = totalMaterial;
            this.TotalOverTime = totalOverTime;
            this.TotalPrice = totalPrice;
            this.TotalWork = totalWork;
        }

        public string FinishingCause { get; private set; }

        public DateTime? ClosingDate { get; private set; }

        public string FinishingDescription { get; private set; }

        public string IntenalLogNote { get; private set; }

        public string ExternalLogNote { get; private set; }

        public int? TotalMaterial { get; private set; }
        public int? TotalOverTime { get; private set; }
        public decimal? TotalPrice { get; private set; }
        public int? TotalWork { get; private set; }
    }
}