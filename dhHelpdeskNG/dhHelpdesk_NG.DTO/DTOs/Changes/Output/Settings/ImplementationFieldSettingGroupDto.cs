namespace dhHelpdesk_NG.DTO.DTOs.Changes.Output.Settings
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
            this.State = state;
            this.RealStartDate = realStartDate;
            this.BuildAndTextImplemented = buildAndTextImplemented;
            this.ImplementationPlanUsed = implementationPlanUsed;
            this.Deviation = deviation;
            this.RecoveryPlanUsed = recoveryPlanUsed;
            this.FinishingDate = finishingDate;
            this.AttachedFile = attachedFile;
            this.Log = log;
            this.ImplementationReady = implementationReady;
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
