namespace DH.Helpdesk.BusinessData.Models.Changes.Input.UpdatedChange
{
    using System;

    using DH.Helpdesk.BusinessData.Enums.Changes.ApprovalResult;
    using DH.Helpdesk.Common.ValidationAttributes;

    public sealed class UpdatedAnalyzeFields
    {
        public UpdatedAnalyzeFields(
            int? categoryId,
            int? priorityId,
            int? responsibleUserId,
            string solution,
            int cost,
            int yearlyCost,
            int? currencyId,
            int estimatedTimeInHours,
            string risk,
            DateTime? startDate,
            DateTime? finishDate,
            bool hasImplementationPlan,
            bool hasRecoveryPlan,
            AnalyzeApprovalResult approval,
            DateTime? approvedDateAndTime,
            int? approvedByUserId,
            string rejectExplanation)
        {
            this.CategoryId = categoryId;
            this.PriorityId = priorityId;
            this.ResponsibleId = responsibleUserId;
            this.Solution = solution;
            this.Cost = cost;
            this.YearlyCost = yearlyCost;
            this.CurrencyId = currencyId;
            this.EstimatedTimeInHours = estimatedTimeInHours;
            this.Risk = risk;
            this.StartDate = startDate;
            this.FinishDate = finishDate;
            this.HasImplementationPlan = hasImplementationPlan;
            this.HasRecoveryPlan = hasRecoveryPlan;
            this.Approval = approval;
            this.ApprovedDateAndTime = approvedDateAndTime;
            this.ApprovedByUserId = approvedByUserId;
            this.RejectExplanation = rejectExplanation;
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
        public int EstimatedTimeInHours { get; private set; }

        public string Risk { get; private set; }

        public DateTime? StartDate { get; private set; }

        public DateTime? FinishDate { get; private set; }

        public bool HasImplementationPlan { get; private set; }

        public bool HasRecoveryPlan { get; private set; }

        public AnalyzeApprovalResult Approval { get; private set; }

        public DateTime? ApprovedDateAndTime { get; private set; }

        public int? ApprovedByUserId { get; private set; }

        public string RejectExplanation { get; private set; }
    }
}
