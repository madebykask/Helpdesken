namespace dhHelpdesk_NG.Web.Models.Changes.InputModel
{
    using System;
    using System.Collections.Generic;
    using System.Web.Mvc;

    using DataAnnotationsExtensions;

    using dhHelpdesk_NG.Common.ValidationAttributes;
    using dhHelpdesk_NG.DTO.Enums.Changes;
    using dhHelpdesk_NG.Web.Infrastructure.LocalizedAttributes;

    public sealed class AnalyzeModel
    {
        public AnalyzeModel()
        {
            this.RelatedChangeIds = new List<int>();
            this.SendToEmails = new List<string>();
        }

        public AnalyzeModel(
            SelectList category,
            MultiSelectList relatedChanges,
            SelectList priority,
            SelectList responsible,
            string solution,
            int cost,
            int yearlyCost,
            SelectList currency,
            int timeEstimatesHours,
            string risk,
            DateTime? startDate,
            DateTime? endDate,
            bool hasImplementationPlan,
            bool hasRecoveryPlan,
            AttachedFilesContainerModel attachedFilesContainer,
            SendToDialogModel sendToDialog,
            SelectList approved,
            string changeRecommendation)
        {
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
            this.Approved = approved;
            this.ChangeRecommendation = changeRecommendation;
        }

        [LocalizedDisplay("Category")]
        public SelectList Category { get; set; }

        [LocalizedDisplay("Related Changes")]
        public MultiSelectList RelatedChanges { get; set; }

        [NotNull]
        public List<int> RelatedChangeIds { get; set; }

        [IsId]
        public int? CategoryId { get; set; }

        [LocalizedDisplay("Priority")]
        public SelectList Priority { get; set; }

        [IsId]
        public int? PriorityId { get; set; }

        [LocalizedDisplay("Responsible")]
        public SelectList Responsible { get; set; }

        [IsId]
        public int? ResponsibleId { get; set; }

        [LocalizedDisplay("Solution")]
        public string Solution { get; set; }

        [Min(0)]
        [LocalizedDisplay("Cost")]
        public int Cost { get; set; }

        [Min(0)]
        [LocalizedDisplay("Yearly cost")]
        public int YearlyCost { get; set; }

        [LocalizedDisplay("Currency")]
        public SelectList Currency { get; set; }

        [IsId]
        public int? CurrencyId { get; set; }

        [Min(0)]
        [LocalizedDisplay("Time estimates hours")]
        public int TimeEstimatesHours { get; set; }

        [LocalizedDisplay("Risk")]
        public string Risk { get; set; }

        [LocalizedDisplay("Start date")]
        public DateTime? StartDate { get; set; }

        [LocalizedDisplay("End date")]
        public DateTime? EndDate { get; set; }

        [LocalizedDisplay("Has implementation plan")]
        public bool HasImplementationPlan { get; set; }

        [LocalizedDisplay("Has recovery plan")]
        public bool HasRecoveryPlan { get; set; }

        [LocalizedDisplay("Attached Files")]
        public AttachedFilesContainerModel AttachedFilesContainer { get; set; }

        [NotNull]
        public SendToDialogModel SendToDialog { get; set; }

        [NotNull]
        public List<string> SendToEmails { get; set; }

        [LocalizedDisplay("Approved")]
        public SelectList Approved { get; set; }

        public AnalyzeApproveResult ApprovedValue { get; set; }

        [LocalizedDisplay("Change recommendation")]
        public string ChangeRecommendation { get; set; }
    }
}