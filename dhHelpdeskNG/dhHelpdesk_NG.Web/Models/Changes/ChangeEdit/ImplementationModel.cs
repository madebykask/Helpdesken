namespace DH.Helpdesk.Web.Models.Changes.ChangeEdit
{
    using System;

    using DH.Helpdesk.Common.ValidationAttributes;
    using DH.Helpdesk.Web.Models.Common;

    public sealed class ImplementationModel
    {
        #region Constructors and Destructors

        public ImplementationModel()
        {
        }

        public ImplementationModel(
            int changeId,
            ConfigurableFieldModel<DateTime?> realStartDate,
            ConfigurableFieldModel<DateTime?> finishingDate,
            ConfigurableFieldModel<bool> buildImplemented,
            ConfigurableFieldModel<bool> implementationPlanUsed,
            ConfigurableFieldModel<string> deviation,
            ConfigurableFieldModel<bool> recoveryPlanUsed,
            ConfigurableFieldModel<AttachedFilesModel> attachedFiles,
            ConfigurableFieldModel<LogsModel> logs,
            SendToDialogModel sendToDialog,
            ConfigurableFieldModel<bool> implementationReady)
        {
            this.ChangeId = changeId;
            this.RealStartDate = realStartDate;
            this.FinishingDate = finishingDate;
            this.BuildImplemented = buildImplemented;
            this.ImplementationPlanUsed = implementationPlanUsed;
            this.Deviation = deviation;
            this.RecoveryPlanUsed = recoveryPlanUsed;
            this.AttachedFiles = attachedFiles;
            this.Logs = logs;
            this.SendToDialog = sendToDialog;
            this.ImplementationReady = implementationReady;
        }

        #endregion

        #region Public Properties

        [NotNull]
        public ConfigurableFieldModel<AttachedFilesModel> AttachedFiles { get; set; }

        [NotNull]
        public ConfigurableFieldModel<bool> BuildImplemented { get; set; }

        [IsId]
        public int ChangeId { get; private set; }

        [NotNull]
        public ConfigurableFieldModel<string> Deviation { get; set; }

        [NotNull]
        public ConfigurableFieldModel<DateTime?> FinishingDate { get; set; }

        [NotNull]
        public ConfigurableFieldModel<bool> ImplementationPlanUsed { get; set; }

        [NotNull]
        public ConfigurableFieldModel<bool> ImplementationReady { get; set; }

        public string LogText { get; set; }

        [NotNull]
        public ConfigurableFieldModel<LogsModel> Logs { get; private set; }

        [NotNull]
        public ConfigurableFieldModel<DateTime?> RealStartDate { get; set; }

        [NotNull]
        public ConfigurableFieldModel<bool> RecoveryPlanUsed { get; set; }

        public SendToDialogModel SendToDialog { get; private set; }

        public string SendToEmails { get; set; }

        [IsId]
        public int? StatusId { get; set; }

        #endregion
    }
}