namespace dhHelpdesk_NG.DTO.DTOs.Changes.UpdatedChange
{
    using System;

    using dhHelpdesk_NG.Common.ValidationAttributes;
    using dhHelpdesk_NG.DTO.Enums.Changes;

    public sealed class UpdatedAnalyzeFields
    {
        public UpdatedAnalyzeFields(
            int? categoryId,
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
            AnalyzeApproveResult approved,
            DateTime? approvedDateAndTime,
            string approvedUser,
            string changeRecommendation)
        {
            this.ApprovedUser = approvedUser;
            this.ApprovedDateAndTime = approvedDateAndTime;
            this.Approved = approved;
            this.HasRecoveryPlan = hasRecoveryPlan;
            this.HasImplementationPlan = hasImplementationPlan;
            this.EndDate = endDate;
            this.StartDate = startDate;
            this.Risk = risk;
            this.TimeEstimatesHours = timeEstimatesHours;
            this.CurrencyId = currencyId;
            this.YearlyCost = yearlyCost;
            this.Cost = cost;
            this.Solution = solution;
            this.ResponsibleId = responsibleId;
            this.PriorityId = priorityId;
            this.CategoryId = categoryId;
            this.ChangeRecommendation = changeRecommendation;
        }

        [IsId]
        public int? CategoryId { get; private set; }

        [IsId]
        public int? PriorityId { get; private set; }

        [IsId]
        public int? ResponsibleId { get; private set; }

        public string Solution { get; private set; }

        [MinValue(0)]
        public int Cost { get; private set; }

        [MinValue(0)]
        public int YearlyCost { get; private set; }

        [IsId]
        public int? CurrencyId { get; private set; }

        [MinValue(0)]
        public int TimeEstimatesHours { get; private set; }

        public string Risk { get; private set; }

        public DateTime? StartDate { get; private set; }

        public DateTime? EndDate { get; private set; }

        public bool HasImplementationPlan { get; private set; }

        public bool HasRecoveryPlan { get; private set; }

        public AnalyzeApproveResult Approved { get; private set; }

        public DateTime? ApprovedDateAndTime { get; private set; }

        public string ApprovedUser { get; private set; }

        public string ChangeRecommendation { get; private set; }
    }
}
