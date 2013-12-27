namespace dhHelpdesk_NG.DTO.DTOs.Changes.Output
{
    using dhHelpdesk_NG.Common.ValidationAttributes;

    public sealed class ImplementationFieldSettingGroupDto
    {
        public ImplementationFieldSettingGroupDto(
            FieldSettingDto state,
            FieldSettingDto realStartDate,
            FieldSettingDto buildAndTextImplemented,
            FieldSettingDto implementationPlanUsed,
            StringFieldSettingDto deviation,
            FieldSettingDto recoveryPlanUsed,
            FieldSettingDto finishingDate,
            FieldSettingDto attachedFile,
            FieldSettingDto log,
            FieldSettingDto implementationReady)
        {
            State = state;
            RealStartDate = realStartDate;
            BuildAndTextImplemented = buildAndTextImplemented;
            ImplementationPlanUsed = implementationPlanUsed;
            Deviation = deviation;
            RecoveryPlanUsed = recoveryPlanUsed;
            FinishingDate = finishingDate;
            AttachedFile = attachedFile;
            Log = log;
            ImplementationReady = implementationReady;
        }

        [NotNull]
        public FieldSettingDto State { get; private set; }

        [NotNull]
        public FieldSettingDto RealStartDate { get; private set; }

        [NotNull]
        public FieldSettingDto BuildAndTextImplemented { get; private set; }

        [NotNull]
        public FieldSettingDto ImplementationPlanUsed { get; private set; }

        [NotNull]
        public StringFieldSettingDto Deviation { get; private set; }

        [NotNull]
        public FieldSettingDto RecoveryPlanUsed { get; private set; }

        [NotNull]
        public FieldSettingDto FinishingDate { get; private set; }

        [NotNull]
        public FieldSettingDto AttachedFile { get; private set; }

        [NotNull]
        public FieldSettingDto Log { get; private set; }

        [NotNull]
        public FieldSettingDto ImplementationReady { get; private set; }
    }
}
