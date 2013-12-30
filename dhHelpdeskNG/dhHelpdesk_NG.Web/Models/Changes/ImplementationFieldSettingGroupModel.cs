namespace dhHelpdesk_NG.Web.Models.Changes
{
    using dhHelpdesk_NG.Common.ValidationAttributes;
    using dhHelpdesk_NG.Web.Infrastructure.LocalizedAttributes;

    public sealed class ImplementationFieldSettingGroupModel
    {
        public ImplementationFieldSettingGroupModel()
        {
        }

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
        public FieldSettingModel State { get; private set; }

        [NotNull]
        [LocalizedDisplay("Real start date")]
        public FieldSettingModel RealStartDate { get; private set; }

        [NotNull]
        [LocalizedDisplay("Build and text implemented")]
        public FieldSettingModel BuildAndTextImplemented { get; private set; }

        [NotNull]
        [LocalizedDisplay("Implementation plan used")]
        public FieldSettingModel ImplementationPlanUsed { get; private set; }

        [NotNull]
        [LocalizedDisplay("Deviation")]
        public StringFieldSettingModel Deviation { get; private set; }

        [NotNull]
        [LocalizedDisplay("Recovery plan used")]
        public FieldSettingModel RecoveryPlanUsed { get; private set; }

        [NotNull]
        [LocalizedDisplay("Finishing date")]
        public FieldSettingModel FinishingDate { get; private set; }

        [NotNull]
        [LocalizedDisplay("Attached file")]
        public FieldSettingModel AttachedFile { get; private set; }

        [NotNull]
        [LocalizedDisplay("Log")]
        public FieldSettingModel Log { get; private set; }

        [NotNull]
        [LocalizedDisplay("Implementation ready")]
        public FieldSettingModel ImplementationReady { get; private set; }
    }
}
