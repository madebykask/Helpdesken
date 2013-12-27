namespace dhHelpdesk_NG.Web.Models.Changes.Output
{
    using dhHelpdesk_NG.Common.ValidationAttributes;

    public sealed class AnalyzeFieldSettingGroupModel
    {
        public AnalyzeFieldSettingGroupModel(
            FieldSettingModel category,
            FieldSettingModel priority,
            FieldSettingModel responsible,
            StringFieldSettingModel solution,
            FieldSettingModel cost,
            FieldSettingModel yearlyCost,
            FieldSettingModel timeEstimatesHours,
            StringFieldSettingModel risk,
            FieldSettingModel startDate,
            FieldSettingModel finishDate,
            FieldSettingModel implementationPlan,
            FieldSettingModel recoveryPlan,
            StringFieldSettingModel recommendation,
            FieldSettingModel attachedFile,
            FieldSettingModel log,
            FieldSettingModel approval)
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
            this.AttachedFile = attachedFile;
            this.Log = log;
            this.Approval = approval;
        }

        [NotNull]
        public FieldSettingModel Category { get; private set; }

        [NotNull]
        public FieldSettingModel Priority { get; private set; }

        [NotNull]
        public FieldSettingModel Responsible { get; private set; }

        [NotNull]
        public StringFieldSettingModel Solution { get; private set; }

        [NotNull]
        public FieldSettingModel Cost { get; private set; }

        [NotNull]
        public FieldSettingModel YearlyCost { get; private set; }

        [NotNull]
        public FieldSettingModel TimeEstimatesHours { get; private set; }

        [NotNull]
        public StringFieldSettingModel Risk { get; private set; }

        [NotNull]
        public FieldSettingModel StartDate { get; private set; }

        [NotNull]
        public FieldSettingModel FinishDate { get; private set; }

        [NotNull]
        public FieldSettingModel ImplementationPlan { get; private set; }

        [NotNull]
        public FieldSettingModel RecoveryPlan { get; private set; }

        [NotNull]
        public StringFieldSettingModel Recommendation { get; private set; }

        [NotNull]
        public FieldSettingModel AttachedFile { get; private set; }

        [NotNull]
        public FieldSettingModel Log { get; private set; }

        [NotNull]
        public FieldSettingModel Approval { get; private set; }
    }
}
