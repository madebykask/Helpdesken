namespace dhHelpdesk_NG.Web.Models.Changes.Settings
{
    using dhHelpdesk_NG.Common.ValidationAttributes;
    using dhHelpdesk_NG.Web.Infrastructure.LocalizedAttributes;

    public sealed class ImplementationFieldSettingsModel
    {
        public ImplementationFieldSettingsModel()
        {
        }

        public ImplementationFieldSettingsModel(
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
        [LocalizedDisplay("State")]
        public FieldSettingModel State { get; set; }

        [NotNull]
        [LocalizedDisplay("Real start date")]
        public FieldSettingModel RealStartDate { get; set; }

        [NotNull]
        [LocalizedDisplay("Build and text implemented")]
        public FieldSettingModel BuildAndTextImplemented { get; set; }

        [NotNull]
        [LocalizedDisplay("Implementation plan used")]
        public FieldSettingModel ImplementationPlanUsed { get; set; }

        [NotNull]
        [LocalizedDisplay("Deviation")]
        public StringFieldSettingModel Deviation { get; set; }

        [NotNull]
        [LocalizedDisplay("Recovery plan used")]
        public FieldSettingModel RecoveryPlanUsed { get; set; }

        [NotNull]
        [LocalizedDisplay("Finishing date")]
        public FieldSettingModel FinishingDate { get; set; }

        [NotNull]
        [LocalizedDisplay("Attached file")]
        public FieldSettingModel AttachedFile { get; set; }

        [NotNull]
        [LocalizedDisplay("Log")]
        public FieldSettingModel Log { get; set; }

        [NotNull]
        [LocalizedDisplay("Implementation ready")]
        public FieldSettingModel ImplementationReady { get; set; }
    }
}
