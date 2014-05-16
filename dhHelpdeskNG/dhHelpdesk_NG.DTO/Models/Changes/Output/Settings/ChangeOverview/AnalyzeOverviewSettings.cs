namespace DH.Helpdesk.BusinessData.Models.Changes.Output.Settings.ChangeOverview
{
    using DH.Helpdesk.BusinessData.Models.Shared.Output;
    using DH.Helpdesk.Common.ValidationAttributes;

    public sealed class AnalyzeOverviewSettings
    {
        public AnalyzeOverviewSettings(
            FieldOverviewSetting category,
            FieldOverviewSetting priority,
            FieldOverviewSetting responsible,
            FieldOverviewSetting solution,
            FieldOverviewSetting cost,
            FieldOverviewSetting yearlyCost,
            FieldOverviewSetting estimatedTimeInHours,
            FieldOverviewSetting risk,
            FieldOverviewSetting startDate,
            FieldOverviewSetting finishDate,
            FieldOverviewSetting hasImplementationPlan,
            FieldOverviewSetting hasRecoveryPlan,
            FieldOverviewSetting approval,
            FieldOverviewSetting rejectRecommendation)
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
            this.HasImplementationPlan = hasImplementationPlan;
            this.HasRecoveryPlan = hasRecoveryPlan;
            this.Approval = approval;
            this.RejectRecommendation = rejectRecommendation;
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
        public FieldOverviewSetting EstimatedTimeInHours { get; private set; }

        [NotNull]
        public FieldOverviewSetting Risk { get; private set; }

        [NotNull]
        public FieldOverviewSetting StartDate { get; private set; }

        [NotNull]
        public FieldOverviewSetting FinishDate { get; private set; }

        [NotNull]
        public FieldOverviewSetting HasImplementationPlan { get; private set; }

        [NotNull]
        public FieldOverviewSetting HasRecoveryPlan { get; private set; }

        [NotNull]
        public FieldOverviewSetting Approval { get; private set; }

        [NotNull]
        public FieldOverviewSetting RejectRecommendation { get; private set; }
    }
}
