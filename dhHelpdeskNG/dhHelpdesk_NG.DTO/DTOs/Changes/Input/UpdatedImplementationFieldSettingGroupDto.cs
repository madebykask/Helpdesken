namespace dhHelpdesk_NG.DTO.DTOs.Changes.Input
{
    using dhHelpdesk_NG.Common.ValidationAttributes;

    public sealed class UpdatedImplementationFieldSettingGroupDto
    {
        public UpdatedImplementationFieldSettingGroupDto(
            UpdatedFieldSettingDto state,
            UpdatedFieldSettingDto realStartDate,
            UpdatedFieldSettingDto buildAndTextImplemented,
            UpdatedFieldSettingDto implementationPlanUsed,
            UpdatedStringFieldSettingDto deviation,
            UpdatedFieldSettingDto recoveryPlanUsed,
            UpdatedFieldSettingDto finishingDate,
            UpdatedFieldSettingDto attachedFile,
            UpdatedFieldSettingDto log,
            UpdatedFieldSettingDto implementationReady)
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
        public UpdatedFieldSettingDto State { get; private set; }

        [NotNull]
        public UpdatedFieldSettingDto RealStartDate { get; private set; }

        [NotNull]
        public UpdatedFieldSettingDto BuildAndTextImplemented { get; private set; }

        [NotNull]
        public UpdatedFieldSettingDto ImplementationPlanUsed { get; private set; }

        [NotNull]
        public UpdatedStringFieldSettingDto Deviation { get; private set; }

        [NotNull]
        public UpdatedFieldSettingDto RecoveryPlanUsed { get; private set; }

        [NotNull]
        public UpdatedFieldSettingDto FinishingDate { get; private set; }

        [NotNull]
        public UpdatedFieldSettingDto AttachedFile { get; private set; }

        [NotNull]
        public UpdatedFieldSettingDto Log { get; private set; }

        [NotNull]
        public UpdatedFieldSettingDto ImplementationReady { get; private set; }
    }
}
