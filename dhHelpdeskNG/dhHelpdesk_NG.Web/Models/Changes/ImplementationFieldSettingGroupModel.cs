namespace dhHelpdesk_NG.Web.Models.Changes
{
    using dhHelpdesk_NG.Common.ValidationAttributes;

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
        public FieldSettingModel State { get; private set; }

        [NotNull]
        public FieldSettingModel RealStartDate { get; private set; }

        [NotNull]
        public FieldSettingModel BuildAndTextImplemented { get; private set; }

        [NotNull]
        public FieldSettingModel ImplementationPlanUsed { get; private set; }

        [NotNull]
        public StringFieldSettingModel Deviation { get; private set; }

        [NotNull]
        public FieldSettingModel RecoveryPlanUsed { get; private set; }

        [NotNull]
        public FieldSettingModel FinishingDate { get; private set; }

        [NotNull]
        public FieldSettingModel AttachedFile { get; private set; }

        [NotNull]
        public FieldSettingModel Log { get; private set; }

        [NotNull]
        public FieldSettingModel ImplementationReady { get; private set; }
    }
}
