namespace dhHelpdesk_NG.DTO.DTOs.Changes.Output.Settings
{
    using dhHelpdesk_NG.Common.ValidationAttributes;

    public sealed class ImplementationFieldOverviewSettingGroupDto
    {
        public ImplementationFieldOverviewSettingGroupDto(
            FieldOverviewSettingDto state,
            FieldOverviewSettingDto realStartDate,
            FieldOverviewSettingDto buildAndTextImplemented,
            FieldOverviewSettingDto implementationPlanUsed,
            FieldOverviewSettingDto deviation,
            FieldOverviewSettingDto recoveryPlanUsed,
            FieldOverviewSettingDto finishingDate,
            FieldOverviewSettingDto implementationReady)
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
        public FieldOverviewSettingDto State { get; private set; }

        [NotNull]
        public FieldOverviewSettingDto RealStartDate { get; private set; }

        [NotNull]
        public FieldOverviewSettingDto BuildAndTextImplemented { get; private set; }

        [NotNull]
        public FieldOverviewSettingDto ImplementationPlanUsed { get; private set; }

        [NotNull]
        public FieldOverviewSettingDto Deviation { get; private set; }

        [NotNull]
        public FieldOverviewSettingDto RecoveryPlanUsed { get; private set; }

        [NotNull]
        public FieldOverviewSettingDto FinishingDate { get; private set; }

        [NotNull]
        public FieldOverviewSettingDto ImplementationReady { get; private set; }
    }
}
