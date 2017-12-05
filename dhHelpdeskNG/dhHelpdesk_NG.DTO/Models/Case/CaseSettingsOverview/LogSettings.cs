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
            FieldOverviewSetting finishingCause,
            FieldOverviewSetting totalMaterial,
            FieldOverviewSetting totalOverTime,
            FieldOverviewSetting totalPrice,
            FieldOverviewSetting totalWork)
        {
            this.FinishingCause = finishingCause;
            this.FinishingDate = finishingDate;
            this.FinishingDescription = finishingDescription;
            this.AttachedFile = attachedFile;
            this.Debiting = debiting;
            this.ExternalLogNote = externalLogNote;
            this.InternalLogNote = internalLogNote;
            this.TotalMaterial = totalMaterial;
            this.TotalOverTime = totalOverTime;
            this.TotalPrice = totalPrice;
            this.TotalWork = totalWork;
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

        [NotNull]
        public FieldOverviewSetting TotalMaterial { get; private set; }
        [NotNull]
        public FieldOverviewSetting TotalOverTime { get; private set; }
        [NotNull]
        public FieldOverviewSetting TotalPrice { get; private set; }
        [NotNull]
        public FieldOverviewSetting TotalWork { get; private set; }
    }
}