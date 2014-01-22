namespace dhHelpdesk_NG.DTO.DTOs.Changes.ChangeDetailedOverview
{
    using System;

    using dhHelpdesk_NG.Common.ValidationAttributes;
    using dhHelpdesk_NG.DTO.Enums.Changes;

    public sealed class AnalyzeFieldGroupDto
    {
        public AnalyzeFieldGroupDto(
            string category,
            string priority,
            string responsible,
            string solution,
            int? cost,
            int? yearlyCost,
            int? timeEstimatesHours,
            string risk,
            DateTime? startDate,
            DateTime? finishDate,
            bool implementationPlan,
            bool recoveryPlan,
            string recommendation,
            AnalyzeResult approval)
        {
            this.Category = category;
            this.Priority = priority;
            this.Responsible = responsible;
            this.Solution = solution;
            this.Cost = cost;
            this.YearlyCost = yearlyCost;
            this.TimeEstimatesHours = timeEstimatesHours;
            this.Risk = risk;
            this.StartDate = startDate;
            this.FinishDate = finishDate;
            this.HasImplementationPlan = implementationPlan;
            this.HasRecoveryPlan = recoveryPlan;
            this.Recommendation = recommendation;
            this.Approval = approval;
        }

        public string Category { get; private set; }

        public string Priority { get; private set; }

        public string Responsible { get; private set; }

        public string Solution { get; private set; }

        [MinValue(0)]
        public int? Cost { get; private set; }

        [MinValue(0)]
        public int? YearlyCost { get; private set; }

        [MinValue(0)]
        public int? TimeEstimatesHours { get; private set; }

        public string Risk { get; private set; }

        public DateTime? StartDate { get; private set; }

        public DateTime? FinishDate { get; private set; }

        public bool HasImplementationPlan { get; private set; }

        public bool HasRecoveryPlan { get; private set; }

        public string Recommendation { get; private set; }

        public AnalyzeResult Approval { get; private set; }
    }
}
