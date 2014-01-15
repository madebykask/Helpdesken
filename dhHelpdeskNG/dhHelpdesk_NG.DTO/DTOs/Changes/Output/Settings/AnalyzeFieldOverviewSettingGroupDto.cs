namespace dhHelpdesk_NG.DTO.DTOs.Changes.Output.Settings
{
    using dhHelpdesk_NG.Common.ValidationAttributes;

    public sealed class AnalyzeFieldOverviewSettingGroupDto
    {
        public AnalyzeFieldOverviewSettingGroupDto(
            FieldOverviewSettingDto category,
            FieldOverviewSettingDto priority,
            FieldOverviewSettingDto responsible,
            FieldOverviewSettingDto solution,
            FieldOverviewSettingDto cost,
            FieldOverviewSettingDto yearlyCost,
            FieldOverviewSettingDto timeEstimatesHours,
            FieldOverviewSettingDto risk,
            FieldOverviewSettingDto startDate,
            FieldOverviewSettingDto finishDate,
            FieldOverviewSettingDto implementationPlan,
            FieldOverviewSettingDto recoveryPlan,
            FieldOverviewSettingDto recommendation,
            FieldOverviewSettingDto approval)
        {
            Category = category;
            Priority = priority;
            Responsible = responsible;
            Solution = solution;
            Cost = cost;
            YearlyCost = yearlyCost;
            TimeEstimatesHours = timeEstimatesHours;
            Risk = risk;
            StartDate = startDate;
            FinishDate = finishDate;
            ImplementationPlan = implementationPlan;
            RecoveryPlan = recoveryPlan;
            Recommendation = recommendation;
            Approval = approval;
        }

        [NotNull]
        public FieldOverviewSettingDto Category { get; private set; }

        [NotNull]
        public FieldOverviewSettingDto Priority { get; private set; }

        [NotNull]
        public FieldOverviewSettingDto Responsible { get; private set; }

        [NotNull]
        public FieldOverviewSettingDto Solution { get; private set; }

        [NotNull]
        public FieldOverviewSettingDto Cost { get; private set; }

        [NotNull]
        public FieldOverviewSettingDto YearlyCost { get; private set; }

        [NotNull]
        public FieldOverviewSettingDto TimeEstimatesHours { get; private set; }

        [NotNull]
        public FieldOverviewSettingDto Risk { get; private set; }

        [NotNull]
        public FieldOverviewSettingDto StartDate { get; private set; }

        [NotNull]
        public FieldOverviewSettingDto FinishDate { get; private set; }

        [NotNull]
        public FieldOverviewSettingDto ImplementationPlan { get; private set; }

        [NotNull]
        public FieldOverviewSettingDto RecoveryPlan { get; private set; }

        [NotNull]
        public FieldOverviewSettingDto Recommendation { get; private set; }

        [NotNull]
        public FieldOverviewSettingDto Approval { get; private set; }
    }
}
