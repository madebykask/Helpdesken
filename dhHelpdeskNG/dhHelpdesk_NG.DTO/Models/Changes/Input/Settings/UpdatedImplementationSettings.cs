namespace DH.Helpdesk.BusinessData.Models.Changes.Input.Settings
{
    using DH.Helpdesk.Common.ValidationAttributes;

    public sealed class UpdatedImplementationSettings
    {
        public UpdatedImplementationSettings(
            UpdatedFieldSetting status,
            UpdatedFieldSetting realStartDate,
            UpdatedFieldSetting buildImplemented,
            UpdatedFieldSetting implementationPlanUsed,
            UpdatedTextFieldSetting deviation,
            UpdatedFieldSetting recoveryPlanUsed,
            UpdatedFieldSetting finishingDate,
            UpdatedFieldSetting attachedFiles,
            UpdatedFieldSetting logs,
            UpdatedFieldSetting implementationReady)
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
        public UpdatedFieldSetting Status { get; private set; }

        [NotNull]
        public UpdatedFieldSetting RealStartDate { get; private set; }

        [NotNull]
        public UpdatedFieldSetting BuildImplemented { get; private set; }

        [NotNull]
        public UpdatedFieldSetting ImplementationPlanUsed { get; private set; }

        [NotNull]
        public UpdatedTextFieldSetting Deviation { get; private set; }

        [NotNull]
        public UpdatedFieldSetting RecoveryPlanUsed { get; private set; }

        [NotNull]
        public UpdatedFieldSetting FinishingDate { get; private set; }

        [NotNull]
        public UpdatedFieldSetting AttachedFiles { get; private set; }

        [NotNull]
        public UpdatedFieldSetting Logs { get; private set; }

        [NotNull]
        public UpdatedFieldSetting ImplementationReady { get; private set; }
    }
}
