namespace DH.Helpdesk.BusinessData.Models.Changes.Output.Settings.ChangeEdit
{
    using DH.Helpdesk.Common.ValidationAttributes;

    public sealed class ImplementationEditSettings
    {
        public ImplementationEditSettings(
            FieldEditSetting status,
            FieldEditSetting realStartDate,
            FieldEditSetting buildImplemented,
            FieldEditSetting implementationPlanUsed,
            TextFieldEditSetting deviation,
            FieldEditSetting recoveryPlanUsed,
            FieldEditSetting finishingDate,
            FieldEditSetting attachedFiles,
            FieldEditSetting logs,
            FieldEditSetting implementationReady)
        {
            this.Status = status;
            this.RealStartDate = realStartDate;
            this.BuildImplemented = buildImplemented;
            this.ImplementationPlanUsed = implementationPlanUsed;
            this.Deviation = deviation;
            this.RecoveryPlanUsed = recoveryPlanUsed;
            this.FinishingDate = finishingDate;
            this.AttachedFiles = attachedFiles;
            this.Logs = logs;
            this.ImplementationReady = implementationReady;
        }

        [NotNull]
        public FieldEditSetting Status { get; private set; }

        [NotNull]
        public FieldEditSetting RealStartDate { get; private set; }

        [NotNull]
        public FieldEditSetting BuildImplemented { get; private set; }

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
