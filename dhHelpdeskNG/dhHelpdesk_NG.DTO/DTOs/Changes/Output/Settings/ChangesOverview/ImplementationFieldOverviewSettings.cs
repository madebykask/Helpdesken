namespace dhHelpdesk_NG.DTO.DTOs.Changes.Output.Settings.ChangesOverview
{
    using dhHelpdesk_NG.Common.ValidationAttributes;

    public sealed class ImplementationFieldOverviewSettings
    {
        public ImplementationFieldOverviewSettings(
            FieldOverviewSetting state,
            FieldOverviewSetting realStartDate,
            FieldOverviewSetting buildAndTextImplemented,
            FieldOverviewSetting implementationPlanUsed,
            FieldOverviewSetting deviation,
            FieldOverviewSetting recoveryPlanUsed,
            FieldOverviewSetting finishingDate,
            FieldOverviewSetting implementationReady)
        {
            this.State = state;
            this.RealStartDate = realStartDate;
            this.BuildAndTextImplemented = buildAndTextImplemented;
            this.ImplementationPlanUsed = implementationPlanUsed;
            this.Deviation = deviation;
            this.RecoveryPlanUsed = recoveryPlanUsed;
            this.FinishingDate = finishingDate;
            this.ImplementationReady = implementationReady;
        }

        [NotNull]
        public FieldOverviewSetting State { get; private set; }

        [NotNull]
        public FieldOverviewSetting RealStartDate { get; private set; }

        [NotNull]
        public FieldOverviewSetting BuildAndTextImplemented { get; private set; }

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
