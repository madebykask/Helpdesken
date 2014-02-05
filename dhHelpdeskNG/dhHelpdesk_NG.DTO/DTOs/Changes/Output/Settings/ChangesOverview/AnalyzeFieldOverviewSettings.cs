namespace dhHelpdesk_NG.DTO.DTOs.Changes.Output.Settings.ChangesOverview
{
    using dhHelpdesk_NG.Common.ValidationAttributes;

    public sealed class AnalyzeFieldOverviewSettings
    {
        public AnalyzeFieldOverviewSettings(
            FieldOverviewSetting category,
            FieldOverviewSetting priority,
            FieldOverviewSetting responsible,
            FieldOverviewSetting solution,
            FieldOverviewSetting cost,
            FieldOverviewSetting yearlyCost,
            FieldOverviewSetting timeEstimatesHours,
            FieldOverviewSetting risk,
            FieldOverviewSetting startDate,
            FieldOverviewSetting finishDate,
            FieldOverviewSetting implementationPlan,
            FieldOverviewSetting recoveryPlan,
            FieldOverviewSetting recommendation,
            FieldOverviewSetting approval)
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
            this.ImplementationPlan = implementationPlan;
            this.RecoveryPlan = recoveryPlan;
            this.Recommendation = recommendation;
            this.Approval = approval;
        }

        [NotNull]
        public FieldOverviewSetting Category { get; private set; }

        [NotNull]
        public FieldOverviewSetting Priority { get; private set; }

        [NotNull]
        public FieldOverviewSetting Responsible { get; private set; }

        [NotNull]
        public FieldOverviewSetting Solution { get; private set; }

        [NotNull]
        public FieldOverviewSetting Cost { get; private set; }

        [NotNull]
        public FieldOverviewSetting YearlyCost { get; private set; }

        [NotNull]
        public FieldOverviewSetting TimeEstimatesHours { get; private set; }

        [NotNull]
        public FieldOverviewSetting Risk { get; private set; }

        [NotNull]
        public FieldOverviewSetting StartDate { get; private set; }

        [NotNull]
        public FieldOverviewSetting FinishDate { get; private set; }

        [NotNull]
        public FieldOverviewSetting ImplementationPlan { get; private set; }

        [NotNull]
        public FieldOverviewSetting RecoveryPlan { get; private set; }

        [NotNull]
        public FieldOverviewSetting Recommendation { get; private set; }

        [NotNull]
        public FieldOverviewSetting Approval { get; private set; }
    }
}
