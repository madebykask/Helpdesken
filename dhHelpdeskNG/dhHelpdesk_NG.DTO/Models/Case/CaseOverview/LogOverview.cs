namespace DH.Helpdesk.BusinessData.Models.Case.CaseOverview
{
    using System;

    public sealed class LogOverview
    {
        public LogOverview(
            string internalLogNote, 
            string externalLogNote, 
            string debiting, 
            string attachedFile,
            string finishingDescription, 
            DateTime? finishingDate, 
            string finishingCause)
        {
            this.FinishingCause = finishingCause;
            this.FinishingDate = finishingDate;
            this.FinishingDescription = finishingDescription;
            this.AttachedFile = attachedFile;
            this.Debiting = debiting;
            this.ExternalLogNote = externalLogNote;
            this.InternalLogNote = internalLogNote;
        }

        public string InternalLogNote { get; private set; }

        public string ExternalLogNote { get; private set; }

        public string Debiting { get; private set; }

        public string AttachedFile { get; private set; }

        public string FinishingDescription { get; private set; }

        public DateTime? FinishingDate { get; private set; }

        public string FinishingCause { get; private set; }
    }
}