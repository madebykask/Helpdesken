namespace DH.Helpdesk.BusinessData.Models.Case.CaseSettingsOverview
{
    using DH.Helpdesk.BusinessData.Models.Shared.Output;
    using DH.Helpdesk.Common.ValidationAttributes;

    public sealed class LogSettings
    {
        public LogSettings(
            FieldOverviewSetting internalLogNote, 
            FieldOverviewSetting externalLogNote, 
            FieldOverviewSetting debiting, 
            FieldOverviewSetting attachedFile, 
            FieldOverviewSetting finishingDescription, 
            FieldOverviewSetting finishingDate, 
            FieldOverviewSetting finishingCause)
        {
            this.FinishingCause = finishingCause;
            this.FinishingDate = finishingDate;
            this.FinishingDescription = finishingDescription;
            this.AttachedFile = attachedFile;
            this.Debiting = debiting;
            this.ExternalLogNote = externalLogNote;
            this.InternalLogNote = internalLogNote;
        }

        [NotNull]
        public FieldOverviewSetting InternalLogNote { get; private set; }

        [NotNull]
        public FieldOverviewSetting ExternalLogNote { get; private set; }

        [NotNull]
        public FieldOverviewSetting Debiting { get; private set; }

        [NotNull]
        public FieldOverviewSetting AttachedFile { get; private set; }

        [NotNull]
        public FieldOverviewSetting FinishingDescription { get; private set; }

        [NotNull]
        public FieldOverviewSetting FinishingDate { get; private set; }

        [NotNull]
        public FieldOverviewSetting FinishingCause { get; private set; }
    }
}