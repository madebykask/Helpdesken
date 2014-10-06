namespace DH.Helpdesk.Mobile.Models.Changes.SettingsEdit
{
    using DH.Helpdesk.Common.ValidationAttributes;
    using DH.Helpdesk.Mobile.Infrastructure.LocalizedAttributes;

    public sealed class ImplementationSettingsModel
    {
        public ImplementationSettingsModel()
        {
        }

        public ImplementationSettingsModel(
            FieldSettingModel implementationStatus,
            FieldSettingModel realStartDate,
            FieldSettingModel buildImplemented,
            FieldSettingModel implementationPlanUsed,
            TextFieldSettingModel deviation,
            FieldSettingModel recoveryPlanUsed,
            FieldSettingModel finishingDate,
            FieldSettingModel attachedFiles,
            FieldSettingModel logs,
            FieldSettingModel implementationReady)
        {
            this.ImplementationStatus = implementationStatus;
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
        [LocalizedDisplay("Implementation Status")]
        public FieldSettingModel ImplementationStatus { get; set; }

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
        public TextFieldSettingModel Deviation { get; set; }

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