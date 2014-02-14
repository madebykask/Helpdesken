namespace DH.Helpdesk.BusinessData.Models.Changes.Output.ChangeDetailedOverview
{
    using System;

    using DH.Helpdesk.BusinessData.Enums.Changes.ApprovalResult;
    using DH.Helpdesk.Common.ValidationAttributes;

    public sealed class AnalyzeFields
    {
        public AnalyzeFields(
            string category,
            string priority,
            UserName responsible,
            string solution,
            int? cost,
            int? yearlyCost,
            int? estimatedTimeInHours,
            string risk,
            DateTime? startDate,
            DateTime? finishDate,
            bool hasImplementationPlan,
            bool hasRecoveryPlan,
            AnalyzeApprovalResult approval,
            string rejectExplanation)
        {
            this.Category = category;
            this.Priority = priority;
            this.Responsible = responsible;
            this.Solution = solution;
            this.Cost = cost;
            this.YearlyCost = yearlyCost;
            this.EstimatedTimeInHours = estimatedTimeInHours;
            this.Risk = risk;
            this.StartDate = startDate;
            this.FinishDate = finishDate;
            this.HasHasImplementationPlan = hasImplementationPlan;
            this.HasHasRecoveryPlan = hasRecoveryPlan;
            this.Approval = approval;
            this.RejectExplanation = rejectExplanation;
        }

        public string Category { get; private set; }

        public string Priority { get; private set; }

        public UserName Responsible { get; private set; }

        public string Solution { get; private set; }

        [MinValue(0)]
        public int? Cost { get; private set; }

        [MinValue(0)]
        public int? YearlyCost { get; private set; }

        [MinValue(0)]
        public int? EstimatedTimeInHours { get; private set; }

        public string Risk { get; private set; }

        public DateTime? StartDate { get; private set; }

        public DateTime? FinishDate { get; private set; }

        public bool HasHasImplementationPlan { get; private set; }

        public bool HasHasRecoveryPlan { get; private set; }

        public AnalyzeApprovalResult Approval { get; private set; }

        public string RejectExplanation { get; private set; }
    }
}
