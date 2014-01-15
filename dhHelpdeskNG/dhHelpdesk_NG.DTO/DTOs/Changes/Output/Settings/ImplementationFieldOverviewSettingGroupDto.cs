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
            State = state;
            RealStartDate = realStartDate;
            BuildAndTextImplemented = buildAndTextImplemented;
            ImplementationPlanUsed = implementationPlanUsed;
            Deviation = deviation;
            RecoveryPlanUsed = recoveryPlanUsed;
            FinishingDate = finishingDate;
            ImplementationReady = implementationReady;
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
