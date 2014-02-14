namespace DH.Helpdesk.Web.Models.Changes.Settings
{
    using DH.Helpdesk.Common.ValidationAttributes;
    using DH.Helpdesk.Web.Infrastructure.LocalizedAttributes;

    public sealed class ImplementationFieldSettingsModel
    {
        public ImplementationFieldSettingsModel()
        {
        }

        public ImplementationFieldSettingsModel(
            FieldSettingModel status,
            FieldSettingModel realStartDate,
            FieldSettingModel buildImplemented,
            FieldSettingModel implementationPlanUsed,
            StringFieldSettingModel deviation,
            FieldSettingModel recoveryPlanUsed,
            FieldSettingModel finishingDate,
            FieldSettingModel attachedFiles,
            FieldSettingModel logs,
            FieldSettingModel implementationReady)
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
        [LocalizedDisplay("Status")]
        public FieldSettingModel Status { get; set; }

        [NotNull]
        [LocalizedDisplay("Real Start Date")]
        public FieldSettingModel RealStartDate { get; set; }

        [NotNull]
        [LocalizedDisplay("Build and Text Implemented")]
        public FieldSettingModel BuildImplemented { get; set; }

        [NotNull]
        [LocalizedDisplay("Implementation Plan Used")]
        public FieldSettingModel ImplementationPlanUsed { get; set; }

        [NotNull]
        [LocalizedDisplay("Deviation")]
        public StringFieldSettingModel Deviation { get; set; }

        [NotNull]
        [LocalizedDisplay("Recovery Plan Used")]
        public FieldSettingModel RecoveryPlanUsed { get; set; }

        [NotNull]
        [LocalizedDisplay("Finishing Date")]
        public FieldSettingModel FinishingDate { get; set; }

        [NotNull]
        [LocalizedDisplay("Attached Files")]
        public FieldSettingModel AttachedFiles { get; set; }

        [NotNull]
        [LocalizedDisplay("Logs")]
        public FieldSettingModel Logs { get; set; }

        [NotNull]
        [LocalizedDisplay("Implementation Ready")]
        public FieldSettingModel ImplementationReady { get; set; }
    }
}
