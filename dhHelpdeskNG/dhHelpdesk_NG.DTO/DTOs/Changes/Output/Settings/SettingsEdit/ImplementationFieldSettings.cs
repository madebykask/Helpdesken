namespace dhHelpdesk_NG.DTO.DTOs.Changes.Output.Settings.SettingsEdit
{
    using dhHelpdesk_NG.Common.ValidationAttributes;

    public sealed class ImplementationFieldSettings
    {
        public ImplementationFieldSettings(
            FieldSetting state,
            FieldSetting realStartDate,
            FieldSetting buildAndTextImplemented,
            FieldSetting implementationPlanUsed,
            StringFieldSetting deviation,
            FieldSetting recoveryPlanUsed,
            FieldSetting finishingDate,
            FieldSetting attachedFile,
            FieldSetting log,
            FieldSetting implementationReady)
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
        public FieldSetting State { get; private set; }

        [NotNull]
        public FieldSetting RealStartDate { get; private set; }

        [NotNull]
        public FieldSetting BuildAndTextImplemented { get; private set; }

        [NotNull]
        public FieldSetting ImplementationPlanUsed { get; private set; }

        [NotNull]
        public StringFieldSetting Deviation { get; private set; }

        [NotNull]
        public FieldSetting RecoveryPlanUsed { get; private set; }

        [NotNull]
        public FieldSetting FinishingDate { get; private set; }

        [NotNull]
        public FieldSetting AttachedFile { get; private set; }

        [NotNull]
        public FieldSetting Log { get; private set; }

        [NotNull]
        public FieldSetting ImplementationReady { get; private set; }
    }
}
