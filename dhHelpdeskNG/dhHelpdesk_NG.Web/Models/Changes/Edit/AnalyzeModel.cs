namespace DH.Helpdesk.Web.Models.Changes.Edit
{
    using System;
    using System.Collections.Generic;
    using System.Web.Mvc;

    using DataAnnotationsExtensions;

    using DH.Helpdesk.BusinessData.Enums.Changes;
    using DH.Helpdesk.Common.ValidationAttributes;
    using DH.Helpdesk.Web.Infrastructure.LocalizedAttributes;

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
            ConfigurableFieldModel<SelectList> currency,
            ConfigurableFieldModel<int> timeEstimatesHours,
            ConfigurableFieldModel<string> risk,
            ConfigurableFieldModel<DateTime?> startDate,
            ConfigurableFieldModel<DateTime?> endDate,
            ConfigurableFieldModel<bool> hasImplementationPlan,
            ConfigurableFieldModel<bool> hasRecoveryPlan,
            AttachedFilesContainerModel attachedFilesContainer,
            SendToDialogModel sendToDialog,
            ConfigurableFieldModel<SelectList> approval,
            ConfigurableFieldModel<string> changeRecommendation)
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
            this.TimeEstimatesHours = timeEstimatesHours;
            this.Risk = risk;
            this.StartDate = startDate;
            this.EndDate = endDate;
            this.HasImplementationPlan = hasImplementationPlan;
            this.HasRecoveryPlan = hasRecoveryPlan;
            this.AttachedFilesContainer = attachedFilesContainer;
            this.SendToDialog = sendToDialog;
            this.Approval = approval;
            this.ChangeRecommendation = changeRecommendation;
        }

        public string ChangeId { get; set; }

        [LocalizedDisplay("Category")]
        public ConfigurableFieldModel<SelectList> Category { get; set; }

        [IsId]
        public int? CategoryId { get; set; }

        [LocalizedDisplay("Related Changes")]
        public MultiSelectList RelatedChanges { get; set; }

        [NotNull]
        public List<int> RelatedChangeIds { get; set; }

        [LocalizedDisplay("Priority")]
        public ConfigurableFieldModel<SelectList> Priority { get; set; }

        [IsId]
        public int? PriorityId { get; set; }

        [LocalizedDisplay("Responsible")]
        public ConfigurableFieldModel<SelectList> Responsible { get; set; }

        [IsId]
        public int? ResponsibleId { get; set; }

        [LocalizedDisplay("Solution")]
        public ConfigurableFieldModel<string> Solution { get; set; }

        [Min(0)]
        [LocalizedDisplay("Cost")]
        public ConfigurableFieldModel<int> Cost { get; set; }

        [Min(0)]
        [LocalizedDisplay("Yearly cost")]
        public ConfigurableFieldModel<int> YearlyCost { get; set; }

        [LocalizedDisplay("Currency")]
        public ConfigurableFieldModel<SelectList> Currency { get; set; }

        [IsId]
        public int? CurrencyId { get; set; }

        [Min(0)]
        [LocalizedDisplay("Time estimates hours")]
        public ConfigurableFieldModel<int> TimeEstimatesHours { get; set; }

        [LocalizedDisplay("Risk")]
        public ConfigurableFieldModel<string> Risk { get; set; }

        [LocalizedDisplay("Start date")]
        public ConfigurableFieldModel<DateTime?> StartDate { get; set; }

        [LocalizedDisplay("End date")]
        public ConfigurableFieldModel<DateTime?> EndDate { get; set; }

        [LocalizedDisplay("Has implementation plan")]
        public ConfigurableFieldModel<bool> HasImplementationPlan { get; set; }

        [LocalizedDisplay("Has recovery plan")]
        public ConfigurableFieldModel<bool> HasRecoveryPlan { get; set; }

        [LocalizedDisplay("Attached Files")]
        public AttachedFilesContainerModel AttachedFilesContainer { get; set; }

        public NewLogModel NewLog { get; set; }

        [NotNull]
        public SendToDialogModel SendToDialog { get; set; }

        [LocalizedDisplay("Approved")]
        public ConfigurableFieldModel<SelectList> Approval { get; set; }

        public AnalyzeApproveResult ApprovedValue { get; set; }

        [LocalizedDisplay("Change recommendation")]
        public ConfigurableFieldModel<string> ChangeRecommendation { get; set; }
    }
}