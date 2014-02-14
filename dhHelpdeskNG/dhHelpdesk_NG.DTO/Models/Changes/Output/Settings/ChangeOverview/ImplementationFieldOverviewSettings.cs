namespace DH.Helpdesk.BusinessData.Models.Changes.Output.Settings.ChangeOverview
{
    using DH.Helpdesk.BusinessData.Models.Common.Output;
    using DH.Helpdesk.Common.ValidationAttributes;

    public sealed class ImplementationFieldOverviewSettings
    {
        public ImplementationFieldOverviewSettings(
            FieldOverviewSetting status,
            FieldOverviewSetting realStartDate,
            FieldOverviewSetting buildImplemented,
            FieldOverviewSetting implementationPlanUsed,
            FieldOverviewSetting deviation,
            FieldOverviewSetting recoveryPlanUsed,
            FieldOverviewSetting finishingDate,
            FieldOverviewSetting implementationReady)
        {
            this.Status = status;
            this.RealStartDate = realStartDate;
            this.BuildImplemented = buildImplemented;
            this.ImplementationPlanUsed = implementationPlanUsed;
            this.Deviation = deviation;
            this.RecoveryPlanUsed = recoveryPlanUsed;
            this.FinishingDate = finishingDate;
            this.ImplementationReady = implementationReady;
        }

        [NotNull]
        public FieldOverviewSetting Status { get; private set; }

        [NotNull]
        public FieldOverviewSetting RealStartDate { get; private set; }

        [NotNull]
        public FieldOverviewSetting BuildImplemented { get; private set; }

        [NotNull]
        public FieldOverviewSetting ImplementationPlanUsed { get; private set; }

        [NotNull]
        public FieldOverviewSetting Deviation { get; private set; }

        [NotNull]
        public FieldOverviewSetting RecoveryPlanUsed { get; private set; }

        [NotNull]
        public FieldOverviewSetting FinishingDate { get; private set; }

        [NotNull]
        public FieldOverviewSetting ImplementationReady { get; private set; }
    }
}
