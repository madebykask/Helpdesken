namespace DH.Helpdesk.Web.Models.Changes.ChangeEdit
{
    using System;
    using System.Collections.Generic;

    using DH.Helpdesk.BusinessData.Enums.Changes;
    using DH.Helpdesk.BusinessData.Models;
    using DH.Helpdesk.Common.Types;
    using DH.Helpdesk.Common.ValidationAttributes;
    using DH.Helpdesk.Web.Models.Common;

    public sealed class AnalyzeModel
    {
        #region Constructors and Destructors

        public AnalyzeModel()
        {
            this.RelatedChangeIds = new List<int>();
        }

        public AnalyzeModel(
            int changeId,
            ConfigurableFieldModel<string> solution,
            ConfigurableFieldModel<int> cost,
            ConfigurableFieldModel<int> yearlyCost,
            ConfigurableFieldModel<int> estimatedTimeInHours,
            ConfigurableFieldModel<string> risk,
            ConfigurableFieldModel<DateTime?> startDate,
            ConfigurableFieldModel<DateTime?> finishDate,
            ConfigurableFieldModel<bool> hasImplementationPlan,
            ConfigurableFieldModel<bool> hasRecoveryPlan,
            ConfigurableFieldModel<AttachedFilesModel> attachedFiles,
            ConfigurableFieldModel<LogsModel> logs,
            SendToDialogModel sendToDialog,
            DateTime? approvedDateAndTime,
            SendToDialogModel inviteToCabDialog,
            UserName approvedByUser,
            ConfigurableFieldModel<string> rejectExplanation)
        {
            this.ChangeId = changeId;
            this.Solution = solution;
            this.Cost = cost;
            this.YearlyCost = yearlyCost;
            this.EstimatedTimeInHours = estimatedTimeInHours;
            this.Risk = risk;
            this.StartDate = startDate;
            this.FinishDate = finishDate;
            this.HasImplementationPlan = hasImplementationPlan;
            this.HasRecoveryPlan = hasRecoveryPlan;
            this.AttachedFiles = attachedFiles;
            this.Logs = logs;
            this.SendToDialog = sendToDialog;
            this.InviteToCabDialog = inviteToCabDialog;
            this.ApprovedDateAndTime = approvedDateAndTime;
            this.ApprovedByUser = approvedByUser;
            this.RejectExplanation = rejectExplanation;
        }

        #endregion

        #region Public Properties

        public StepStatus ApprovalValue { get; set; }

        public UserName ApprovedByUser { get; private set; }

        public DateTime? ApprovedDateAndTime { get; private set; }

        [NotNull]
        public ConfigurableFieldModel<AttachedFilesModel> AttachedFiles { get; private set; }

        [IsId]
        public int? CategoryId { get; set; }

        [IsId]
        public int ChangeId { get; private set; }

        [NotNull]
        public ConfigurableFieldModel<int> Cost { get; set; }

        [IsId]
        public int? CurrencyId { get; set; }

        [NotNull]
        public ConfigurableFieldModel<int> EstimatedTimeInHours { get; set; }

        [NotNull]
        public ConfigurableFieldModel<DateTime?> FinishDate { get; set; }

        [NotNull]
        public ConfigurableFieldModel<bool> HasImplementationPlan { get; set; }

        [NotNull]
        public ConfigurableFieldModel<bool> HasRecoveryPlan { get; set; }

        public string LogText { get; set; }

        [NotNull]
        public ConfigurableFieldModel<LogsModel> Logs { get; private set; }

        [IsId]
        public int? PriorityId { get; set; }

        [NotNull]
        public ConfigurableFieldModel<string> RejectExplanation { get; set; }

        [NotNull]
        public List<int> RelatedChangeIds { get; set; }

        [IsId]
        public int? ResponsibleId { get; set; }

        [NotNull]
        public ConfigurableFieldModel<string> Risk { get; set; }

        public SendToDialogModel SendToDialog { get; set; }

        public string SendToEmails { get; set; }

        public SendToDialogModel InviteToCabDialog { get; set; }

        public string InviteToCabEmails { get; set; }

        [NotNull]
        public ConfigurableFieldModel<string> Solution { get; set; }

        [NotNull]
        public ConfigurableFieldModel<DateTime?> StartDate { get; set; }

        [NotNull]
        public ConfigurableFieldModel<int> YearlyCost { get; set; }

        #endregion
    }
}