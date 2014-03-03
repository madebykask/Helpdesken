namespace DH.Helpdesk.BusinessData.Models.Changes.Output.Settings.ChangeProcessing
{
    using DH.Helpdesk.Common.ValidationAttributes;

    public sealed class ImplementationProcessingSettings
    {
        public ImplementationProcessingSettings(
            FieldProcessingSetting status,
            FieldProcessingSetting realStartDate,
            FieldProcessingSetting finishingDate,
            FieldProcessingSetting buildImplemented,
            FieldProcessingSetting implementationPlanUsed,
            FieldProcessingSetting deviation,
            FieldProcessingSetting recoveryPlanUsed,
            FieldProcessingSetting attachedFiles,
            FieldProcessingSetting logs,
            FieldProcessingSetting implementationReady)
        {
            this.Status = status;
            this.RealStartDate = realStartDate;
            this.FinishingDate = finishingDate;
            this.BuildImplemented = buildImplemented;
            this.ImplementationPlanUsed = implementationPlanUsed;
            this.Deviation = deviation;
            this.RecoveryPlanUsed = recoveryPlanUsed;
            this.AttachedFiles = attachedFiles;
            this.Logs = logs;
            this.ImplementationReady = implementationReady;
        }

        [NotNull]
        public FieldProcessingSetting Status { get; private set; }

        [NotNull]
        public FieldProcessingSetting RealStartDate { get; private set; }

        [NotNull]
        public FieldProcessingSetting FinishingDate { get; private set; }

        [NotNull]
        public FieldProcessingSetting BuildImplemented { get; private set; }

        [NotNull]
        public FieldProcessingSetting ImplementationPlanUsed { get; private set; }

        [NotNull]
        public FieldProcessingSetting Deviation { get; private set; }

        [NotNull]
        public FieldProcessingSetting RecoveryPlanUsed { get; private set; }

        [NotNull]
        public FieldProcessingSetting AttachedFiles { get; private set; }

        [NotNull]
        public FieldProcessingSetting Logs { get; private set; }

        [NotNull]
        public FieldProcessingSetting ImplementationReady { get; private set; }
    }
}
