namespace dhHelpdesk_NG.DTO.DTOs.Changes.Input
{
    using dhHelpdesk_NG.Common.Tools;

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
            ArgumentsValidator.NotNull(state, "state");
            ArgumentsValidator.NotNull(realStartDate, "realStartDate");
            ArgumentsValidator.NotNull(buildAndTextImplemented, "buildAndTextImplemented");
            ArgumentsValidator.NotNull(implementationPlanUsed, "implementationPlanUsed");
            ArgumentsValidator.NotNull(deviation, "deviation");
            ArgumentsValidator.NotNull(recoveryPlanUsed, "recoveryPlanUsed");
            ArgumentsValidator.NotNull(finishingDate, "finishingDate");
            ArgumentsValidator.NotNull(attachedFile, "attachedFile");
            ArgumentsValidator.NotNull(log, "log");
            ArgumentsValidator.NotNull(implementationReady, "implementationReady");

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

        public UpdatedFieldSettingDto State { get; private set; }

        public UpdatedFieldSettingDto RealStartDate { get; private set; }

        public UpdatedFieldSettingDto BuildAndTextImplemented { get; private set; }

        public UpdatedFieldSettingDto ImplementationPlanUsed { get; private set; }

        public UpdatedStringFieldSettingDto Deviation { get; private set; }

        public UpdatedFieldSettingDto RecoveryPlanUsed { get; private set; }

        public UpdatedFieldSettingDto FinishingDate { get; private set; }

        public UpdatedFieldSettingDto AttachedFile { get; private set; }

        public UpdatedFieldSettingDto Log { get; private set; }

        public UpdatedFieldSettingDto ImplementationReady { get; private set; }
    }
}
