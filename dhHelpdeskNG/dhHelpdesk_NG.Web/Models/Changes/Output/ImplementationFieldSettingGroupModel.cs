namespace dhHelpdesk_NG.Web.Models.Changes.Output
{
    using dhHelpdesk_NG.Common.Tools;

    public sealed class ImplementationFieldSettingGroupModel
    {
        public ImplementationFieldSettingGroupModel(
            FieldSettingModel state,
            FieldSettingModel realStartDate,
            FieldSettingModel buildAndTextImplemented,
            FieldSettingModel implementationPlanUsed,
            StringFieldSettingModel deviation,
            FieldSettingModel recoveryPlanUsed,
            FieldSettingModel finishingDate,
            FieldSettingModel attachedFile,
            FieldSettingModel log,
            FieldSettingModel implementationReady)
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

        public FieldSettingModel State { get; private set; }

        public FieldSettingModel RealStartDate { get; private set; }

        public FieldSettingModel BuildAndTextImplemented { get; private set; }

        public FieldSettingModel ImplementationPlanUsed { get; private set; }

        public StringFieldSettingModel Deviation { get; private set; }

        public FieldSettingModel RecoveryPlanUsed { get; private set; }

        public FieldSettingModel FinishingDate { get; private set; }

        public FieldSettingModel AttachedFile { get; private set; }

        public FieldSettingModel Log { get; private set; }

        public FieldSettingModel ImplementationReady { get; private set; }
    }
}
