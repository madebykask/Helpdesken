namespace dhHelpdesk_NG.DTO.DTOs.Changes.Input.UpdatedChangeAggregate
{
    using System;
    using System.Collections.Generic;

    using dhHelpdesk_NG.Common.ValidationAttributes;
    using dhHelpdesk_NG.DTO.Enums.Changes;

    public sealed class UpdatedAnalyzeFields
    {
        public UpdatedAnalyzeFields(
            int? categoryId,
            List<int> relatedChangeIds, 
            int? priorityId,
            int? responsibleId,
            string solution,
            int cost,
            int yearlyCost,
            int? currencyId,
            int timeEstimatesHours,
            string risk,
            DateTime? startDate,
            DateTime? endDate,
            bool hasImplementationPlan,
            bool hasRecoveryPlan,
            List<DeletedFile> deletedFiles,
            List<NewFile> newFiles,
            NewLog newLog,
            AnalyzeApproveResult approved,
            string changeRecommendation)
        {
            this.CategoryId = categoryId;
            this.RelatedChangeIds = relatedChangeIds;
            this.PriorityId = priorityId;
            this.ResponsibleId = responsibleId;
            this.Solution = solution;
            this.Cost = cost;
            this.YearlyCost = yearlyCost;
            this.CurrencyId = currencyId;
            this.TimeEstimatesHours = timeEstimatesHours;
            this.Risk = risk;
            this.StartDate = startDate;
            this.EndDate = endDate;
            this.HasImplementationPlan = hasImplementationPlan;
            this.HasRecoveryPlan = hasRecoveryPlan;
            this.DeletedFiles = deletedFiles;
            this.NewLog = newLog;
            this.NewFiles = newFiles;
            this.Approved = approved;
            this.ChangeRecommendation = changeRecommendation;
        }

        [IsId]
        public int? CategoryId { get; private set; }

        [NotNull]
        public List<int> RelatedChangeIds { get; private set; }

        [IsId]
        public int? PriorityId { get; private set; }

        [IsId]
        public int? ResponsibleId { get; private set; }

        public string Solution { get; private set; }

        public int Cost { get; private set; }

        public int YearlyCost { get; private set; }

        [IsId]
        public int? CurrencyId { get; private set; }

        public int TimeEstimatesHours { get; private set; }

        public string Risk { get; private set; }

        public DateTime? StartDate { get; private set; }

        public DateTime? EndDate { get; private set; }

        public bool HasImplementationPlan { get; private set; }

        public bool HasRecoveryPlan { get; private set; }

        [NotNull]
        public List<DeletedFile> DeletedFiles { get; private set; }

        [NotNull]
        public List<NewFile> NewFiles { get; private set; }

        public NewLog NewLog { get; private set; }

        public AnalyzeApproveResult Approved { get; private set; }

        public string ChangeRecommendation { get; private set; }
    }
}
