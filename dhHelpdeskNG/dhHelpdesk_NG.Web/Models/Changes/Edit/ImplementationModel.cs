namespace dhHelpdesk_NG.Web.Models.Changes.Edit
{
    using System;
    using System.Web.Mvc;

    using dhHelpdesk_NG.Common.ValidationAttributes;
    using dhHelpdesk_NG.Web.Infrastructure.LocalizedAttributes;

    public sealed class ImplementationModel
    {
        public ImplementationModel()
        {
        }

        public ImplementationModel(
            string changeId,
            SelectList implementationStatus,
            DateTime? realStartDate,
            DateTime? finishingDate,
            bool buildImplemented,
            bool implementationPlanUsed,
            string changeDeviation,
            bool recoveryPlanUsed,
            AttachedFilesContainerModel attachedFilesContainer,
            SendToDialogModel sendToDialog,
            bool implementationReady)
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
        public SelectList ImplementationStatus { get; set; }

        [IsId]
        public int? ImplementationStatusId { get; set; }

        [LocalizedDisplay("Real start date")]
        public DateTime? RealStartDate { get; set; }

        [LocalizedDisplay("Finishing date")]
        public DateTime? FinishingDate { get; set; }

        [LocalizedDisplay("Build implemented")]
        public bool BuildImplemented { get; set; }

        [LocalizedDisplay("Implementation plan used")]
        public bool ImplementationPlanUsed { get; set; }

        [LocalizedDisplay("Change deviation")]
        public string ChangeDeviation { get; set; }

        [LocalizedDisplay("Recovery plan used")]
        public bool RecoveryPlanUsed { get; set; }

        [LocalizedDisplay("Attached Files")]
        public AttachedFilesContainerModel AttachedFilesContainer { get; set; }

        public NewLogModel NewLog { get; set; }

        [NotNull]
        public SendToDialogModel SendToDialog { get; private set; }

        [LocalizedDisplay("Implementation ready")]
        public bool ImplementationReady { get; set; }
    }
}