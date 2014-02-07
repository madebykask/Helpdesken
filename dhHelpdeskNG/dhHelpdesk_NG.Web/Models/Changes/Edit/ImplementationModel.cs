namespace DH.Helpdesk.Web.Models.Changes.Edit
{
    using System;
    using System.Web.Mvc;

    using DH.Helpdesk.Common.ValidationAttributes;
    using DH.Helpdesk.Web.Infrastructure.LocalizedAttributes;

    public sealed class ImplementationModel
    {
        public ImplementationModel()
        {
        }

        public ImplementationModel(
            string changeId,
            ConfigurableFieldModel<SelectList> implementationStatus,
            ConfigurableFieldModel<DateTime?> realStartDate,
            ConfigurableFieldModel<DateTime?> finishingDate,
            ConfigurableFieldModel<bool> buildImplemented,
            ConfigurableFieldModel<bool> implementationPlanUsed,
            ConfigurableFieldModel<string> changeDeviation,
            ConfigurableFieldModel<bool> recoveryPlanUsed,
            AttachedFilesContainerModel attachedFilesContainer,
            SendToDialogModel sendToDialog,
            ConfigurableFieldModel<bool> implementationReady)
        {
            this.ChangeId = changeId;
            this.ImplementationStatus = implementationStatus;
            this.RealStartDate = realStartDate;
            this.FinishingDate = finishingDate;
            this.BuildImplemented = buildImplemented;
            this.ImplementationPlanUsed = implementationPlanUsed;
            this.ChangeDeviation = changeDeviation;
            this.RecoveryPlanUsed = recoveryPlanUsed;
            this.AttachedFilesContainer = attachedFilesContainer;
            this.SendToDialog = sendToDialog;
            this.ImplementationReady = implementationReady;
        }

        public string ChangeId { get; set; }

        [LocalizedDisplay("Implementation status")]
        public ConfigurableFieldModel<SelectList> ImplementationStatus { get; set; }

        [IsId]
        public int? ImplementationStatusId { get; set; }

        [LocalizedDisplay("Real start date")]
        public ConfigurableFieldModel<DateTime?> RealStartDate { get; set; }

        [LocalizedDisplay("Finishing date")]
        public ConfigurableFieldModel<DateTime?> FinishingDate { get; set; }

        [LocalizedDisplay("Build implemented")]
        public ConfigurableFieldModel<bool> BuildImplemented { get; set; }

        [LocalizedDisplay("Implementation plan used")]
        public ConfigurableFieldModel<bool> ImplementationPlanUsed { get; set; }

        [LocalizedDisplay("Change deviation")]
        public ConfigurableFieldModel<string> ChangeDeviation { get; set; }

        [LocalizedDisplay("Recovery plan used")]
        public ConfigurableFieldModel<bool> RecoveryPlanUsed { get; set; }

        [LocalizedDisplay("Attached Files")]
        public AttachedFilesContainerModel AttachedFilesContainer { get; set; }

        public NewLogModel NewLog { get; set; }

        [NotNull]
        public SendToDialogModel SendToDialog { get; private set; }

        [LocalizedDisplay("Implementation ready")]
        public ConfigurableFieldModel<bool> ImplementationReady { get; set; }
    }
}