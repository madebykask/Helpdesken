namespace dhHelpdesk_NG.DTO.DTOs.Changes.Output.Settings.ChangeEdit
{
    using dhHelpdesk_NG.Common.ValidationAttributes;

    public sealed class ImplementationFieldEditSettings
    {
        public ImplementationFieldEditSettings(
            FieldEditSetting state,
            FieldEditSetting realStartDate,
            FieldEditSetting buildAndTextImplemented,
            FieldEditSetting implementationPlanUsed,
            TextFieldEditSetting deviation,
            FieldEditSetting recoveryPlanUsed,
            FieldEditSetting finishingDate,
            FieldEditSetting attachedFiles,
            FieldEditSetting logs,
            FieldEditSetting implementationReady)
        {
            this.State = state;
            this.RealStartDate = realStartDate;
            this.BuildAndTextImplemented = buildAndTextImplemented;
            this.ImplementationPlanUsed = implementationPlanUsed;
            this.Deviation = deviation;
            this.RecoveryPlanUsed = recoveryPlanUsed;
            this.FinishingDate = finishingDate;
            this.AttachedFiles = attachedFiles;
            this.Logs = logs;
            this.ImplementationReady = implementationReady;
        }

        [NotNull]
        public FieldEditSetting State { get; private set; }

        [NotNull]
        public FieldEditSetting RealStartDate { get; private set; }

        [NotNull]
        public FieldEditSetting BuildAndTextImplemented { get; private set; }

        [NotNull]
        public FieldEditSetting ImplementationPlanUsed { get; private set; }

        [NotNull]
        public TextFieldEditSetting Deviation { get; private set; }

        [NotNull]
        public FieldEditSetting RecoveryPlanUsed { get; private set; }

        [NotNull]
        public FieldEditSetting FinishingDate { get; private set; }

        [NotNull]
        public FieldEditSetting AttachedFiles { get; private set; }

        [NotNull]
        public FieldEditSetting Logs { get; private set; }

        [NotNull]
        public FieldEditSetting ImplementationReady { get; private set; }
    }
}
