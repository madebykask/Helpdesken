namespace DH.Helpdesk.BusinessData.Models.Changes.Output.Settings.SettingsEdit
{
    using DH.Helpdesk.Common.ValidationAttributes;

    public sealed class ImplementationFieldSettings
    {
        public ImplementationFieldSettings(
            FieldSetting status,
            FieldSetting realStartDate,
            FieldSetting buildImplemented,
            FieldSetting implementationPlanUsed,
            TextFieldSetting deviation,
            FieldSetting recoveryPlanUsed,
            FieldSetting finishingDate,
            FieldSetting attachedFiles,
            FieldSetting logs,
            FieldSetting implementationReady)
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
        public FieldSetting Status { get; private set; }

        [NotNull]
        public FieldSetting RealStartDate { get; private set; }

        [NotNull]
        public FieldSetting BuildImplemented { get; private set; }

        [NotNull]
        public FieldSetting ImplementationPlanUsed { get; private set; }

        [NotNull]
        public TextFieldSetting Deviation { get; private set; }

        [NotNull]
        public FieldSetting RecoveryPlanUsed { get; private set; }

        [NotNull]
        public FieldSetting FinishingDate { get; private set; }

        [NotNull]
        public FieldSetting AttachedFiles { get; private set; }

        [NotNull]
        public FieldSetting Logs { get; private set; }

        [NotNull]
        public FieldSetting ImplementationReady { get; private set; }
    }
}
