namespace DH.Helpdesk.Web.Models.Changes.ChangeEdit
{
    using System;
    using System.Collections.Generic;
    using System.Web.Mvc;

    using DH.Helpdesk.BusinessData.Enums.Changes.ApprovalResult;
    using DH.Helpdesk.BusinessData.Models;
    using DH.Helpdesk.Common.ValidationAttributes;
    using DH.Helpdesk.Web.Infrastructure.LocalizedAttributes;
    using DH.Helpdesk.Web.Models.Common;

    public sealed class AnalyzeModel
    {
        public AnalyzeModel()
        {
            this.RelatedChangeIds = new List<int>();
        }

        public AnalyzeModel(
            string changeId,
            ConfigurableFieldModel<SelectList> category,
            MultiSelectList relatedChanges,
            ConfigurableFieldModel<SelectList> priority,
            ConfigurableFieldModel<SelectList> responsible,
            ConfigurableFieldModel<string> solution,
            ConfigurableFieldModel<int> cost,
            ConfigurableFieldModel<int> yearlyCost,
            SelectList currency,
            ConfigurableFieldModel<int> estimatedTimeInHours,
            ConfigurableFieldModel<string> risk,
            ConfigurableFieldModel<DateTime?> startDate,
            ConfigurableFieldModel<DateTime?> finishDate,
            ConfigurableFieldModel<bool> hasImplementationPlan,
            ConfigurableFieldModel<bool> hasRecoveryPlan,
            ConfigurableSearchFieldModel<AttachedFilesModel> attachedFiles,
            ConfigurableSearchFieldModel<LogsModel> logs,
            SendToDialogModel sendToDialog,
            ConfigurableFieldModel<SelectList> approval,
            DateTime approvedDateAndTime,
            UserName approvedByUser,
            ConfigurableFieldModel<string> rejectExplanation)
        {
            this.ChangeId = changeId;
            this.Category = category;
            this.RelatedChanges = relatedChanges;
            this.Priority = priority;
            this.Responsible = responsible;
            this.Solution = solution;
            this.Cost = cost;
            this.YearlyCost = yearlyCost;
            this.Currency = currency;
            this.EstimatedTimeInHours = estimatedTimeInHours;
            this.Risk = risk;
            this.StartDate = startDate;
            this.FinishDate = finishDate;
            this.HasImplementationPlan = hasImplementationPlan;
            this.HasRecoveryPlan = hasRecoveryPlan;
            this.AttachedFiles = attachedFiles;
            this.Logs = logs;
            this.SendToDialog = sendToDialog;
            this.Approval = approval;
            this.ApprovedDateAndTime = approvedDateAndTime;
            this.ApprovedByUser = approvedByUser;
            this.RejectExplanation = rejectExplanation;
        }

        public string ChangeId { get; private set; }

        [NotNull]
        public ConfigurableFieldModel<SelectList> Category { get; private set; }

        [IsId]
        public int? CategoryId { get; set; }

        public MultiSelectList RelatedChanges { get; private set; }

        [NotNull]
        public List<int> RelatedChangeIds { get; set; }

        [NotNull]
        public ConfigurableFieldModel<SelectList> Priority { get; private set; }

        [IsId]
        public int? PriorityId { get; set; }

        [NotNull]
        public ConfigurableFieldModel<SelectList> Responsible { get; private set; }

        [IsId]
        public int? ResponsibleId { get; set; }

        [NotNull]
        public ConfigurableFieldModel<string> Solution { get; set; }

        [NotNull]
        public ConfigurableFieldModel<int> Cost { get; set; }

        [NotNull]
        public ConfigurableFieldModel<int> YearlyCost { get; set; }

        [LocalizedDisplay("Currency")]
        public SelectList Currency { get; private set; }

        [IsId]
        public int? CurrencyId { get; set; }

        [NotNull]
        public ConfigurableFieldModel<int> EstimatedTimeInHours { get; set; }

        [NotNull]
        public ConfigurableFieldModel<string> Risk { get; set; }

        [NotNull]
        public ConfigurableFieldModel<DateTime?> StartDate { get; set; }

        [NotNull]
        public ConfigurableFieldModel<DateTime?> FinishDate { get; set; }

        [NotNull]
        public ConfigurableFieldModel<bool> HasImplementationPlan { get; set; }

        [NotNull]
        public ConfigurableFieldModel<bool> HasRecoveryPlan { get; set; }

        [NotNull]
        public ConfigurableSearchFieldModel<AttachedFilesModel> AttachedFiles { get; private set; }

        [NotNull]
        public ConfigurableSearchFieldModel<LogsModel> Logs { get; private set; }

        public SendToDialogModel SendToDialog { get; set; }

        [NotNull]
        public ConfigurableFieldModel<SelectList> Approval { get; private set; }

        public DateTime ApprovedDateAndTime { get; private set; }

        public UserName ApprovedByUser { get; private set; }

        public AnalyzeApprovalResult ApprovalValue { get; set; }

        [NotNull]
        public ConfigurableFieldModel<string> RejectExplanation { get; set; }
    }
}