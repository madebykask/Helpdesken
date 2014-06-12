namespace DH.Helpdesk.Web.Models.Changes.ChangeEdit
{
    using System;
    using System.Collections.Generic;
    using System.Web.Mvc;

    using DH.Helpdesk.BusinessData.Enums.Changes;
    using DH.Helpdesk.Common.Types;
    using DH.Helpdesk.Common.ValidationAttributes;
    using DH.Helpdesk.Web.Models.Shared;

    public sealed class AnalyzeModel
    {
        #region Constructors and Destructors

        public AnalyzeModel()
        {
            this.RelatedChangeIds = new List<int>();
        }

        public AnalyzeModel(
            int changeId,
            MultiSelectList relatedChanges,
            ConfigurableFieldModel<SelectList> categories,
            ConfigurableFieldModel<SelectList> priorities,
            ConfigurableFieldModel<SelectList> responsibles,
            ConfigurableFieldModel<string> solution,
            ConfigurableFieldModel<int> cost,
            ConfigurableFieldModel<int> yearlyCost,
            ConfigurableFieldModel<SelectList> currencies,
            ConfigurableFieldModel<int> estimatedTimeInHours,
            ConfigurableFieldModel<string> risk,
            ConfigurableContainerModel<DateAndTimeModel> startDateAndTime,
            ConfigurableContainerModel<DateAndTimeModel> finishDateAndTime,
            ConfigurableFieldModel<bool> hasImplementationPlan,
            ConfigurableFieldModel<bool> hasRecoveryPlan,
            ConfigurableFieldModel<AttachedFilesModel> attachedFiles,
            ConfigurableFieldModel<LogsModel> logs,
            InviteToModel inviteToCab,
            ConfigurableFieldModel<SelectList> approvalResults,
            DateTime? approvedDateAndTime,
            UserName approvedByUser,
            ConfigurableFieldModel<string> rejectExplanation)
        {
            this.ChangeId = changeId;
            this.RelatedChanges = relatedChanges;
            this.Categories = categories;
            this.Priorities = priorities;
            this.Responsibles = responsibles;
            this.Solution = solution;
            this.Cost = cost;
            this.YearlyCost = yearlyCost;
            this.Currencies = currencies;
            this.EstimatedTimeInHours = estimatedTimeInHours;
            this.Risk = risk;
            this.StartDateAndTime = startDateAndTime;
            this.FinishDateAndTime = finishDateAndTime;
            this.HasImplementationPlan = hasImplementationPlan;
            this.HasRecoveryPlan = hasRecoveryPlan;
            this.AttachedFiles = attachedFiles;
            this.Logs = logs;
            this.InviteToCab = inviteToCab;
            this.ApprovalResults = approvalResults;
            this.ApprovedDateAndTime = approvedDateAndTime;
            this.ApprovedByUser = approvedByUser;
            this.RejectExplanation = rejectExplanation;
        }

        #endregion

        #region Public Properties

        [NotNull]
        public ConfigurableFieldModel<SelectList> ApprovalResults { get; set; }

        public StepStatus ApprovalValue { get; set; }

        public UserName ApprovedByUser { get; set; }

        public DateTime? ApprovedDateAndTime { get; set; }

        [NotNull]
        public ConfigurableFieldModel<AttachedFilesModel> AttachedFiles { get; set; }

        [NotNull]
        public ConfigurableFieldModel<SelectList> Categories { get; set; }

        [IsId]
        public int? CategoryId { get; set; }

        [IsId]
        public int ChangeId { get; set; }

        [NotNull]
        public ConfigurableFieldModel<int> Cost { get; set; }

        [NotNull]
        public ConfigurableFieldModel<SelectList> Currencies { get; set; }

        [IsId]
        public int? CurrencyId { get; set; }

        [NotNull]
        public ConfigurableFieldModel<int> EstimatedTimeInHours { get; set; }

        [NotNull]
        public ConfigurableContainerModel<DateAndTimeModel> FinishDateAndTime { get; set; }

        [NotNull]
        public ConfigurableFieldModel<bool> HasImplementationPlan { get; set; }

        [NotNull]
        public ConfigurableFieldModel<bool> HasRecoveryPlan { get; set; }

        [NotNull]
        public InviteToModel InviteToCab { get; set; }

        public ConfigurableFieldModel<LogsModel> Logs { get; set; }

        [NotNull]
        public ConfigurableFieldModel<SelectList> Priorities { get; set; }

        [IsId]
        public int? PriorityId { get; set; }

        [NotNull]
        public ConfigurableFieldModel<string> RejectExplanation { get; set; }

        [NotNull]
        public List<int> RelatedChangeIds { get; set; }

        [NotNull]
        public MultiSelectList RelatedChanges { get; set; }

        [IsId]
        public int? ResponsibleId { get; set; }

        [NotNull]
        public ConfigurableFieldModel<SelectList> Responsibles { get; set; }

        [NotNull]
        public ConfigurableFieldModel<string> Risk { get; set; }

        [NotNull]
        public ConfigurableFieldModel<string> Solution { get; set; }

        [NotNull]
        public ConfigurableContainerModel<DateAndTimeModel> StartDateAndTime { get; set; }

        [NotNull]
        public ConfigurableFieldModel<int> YearlyCost { get; set; }

        #endregion

        public bool HasShowableFields()
        {
            return this.ApprovalResults.Show ||
                this.AttachedFiles.Show ||
                this.Categories.Show ||
                this.Cost.Show ||
                this.Currencies.Show ||
                this.EstimatedTimeInHours.Show ||
                this.FinishDateAndTime.Show ||
                this.HasImplementationPlan.Show ||
                this.HasRecoveryPlan.Show ||
                this.Logs.Show ||
                this.Priorities.Show ||
                this.RejectExplanation.Show ||
                this.Responsibles.Show ||
                this.Risk.Show ||
                this.Solution.Show ||
                this.StartDateAndTime.Show ||
                this.YearlyCost.Show;
        }
    }
}