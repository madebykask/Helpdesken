namespace dhHelpdesk_NG.DTO.DTOs.Changes.Output
{
    using dhHelpdesk_NG.Common.Tools;

    public sealed class ImplementationFieldSettingGroupDto
    {
        public ImplementationFieldSettingGroupDto(
            FieldSettingDto state,
            FieldSettingDto realStartDate,
            FieldSettingDto buildAndTextImplemented,
            FieldSettingDto implementationPlanUsed,
            FieldSettingDto deviation,
            FieldSettingDto recoveryPlanUsed,
            FieldSettingDto finishingDate,
            FieldSettingDto attachedFile,
            FieldSettingDto log,
            FieldSettingDto implementationReady)
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

        public FieldSettingDto State { get; private set; }

        public FieldSettingDto RealStartDate { get; private set; }

        public FieldSettingDto BuildAndTextImplemented { get; private set; }

        public FieldSettingDto ImplementationPlanUsed { get; private set; }

        public FieldSettingDto Deviation { get; private set; }

        public FieldSettingDto RecoveryPlanUsed { get; private set; }

        public FieldSettingDto FinishingDate { get; private set; }

        public FieldSettingDto AttachedFile { get; private set; }

        public FieldSettingDto Log { get; private set; }

        public FieldSettingDto ImplementationReady { get; private set; }
    }
}
