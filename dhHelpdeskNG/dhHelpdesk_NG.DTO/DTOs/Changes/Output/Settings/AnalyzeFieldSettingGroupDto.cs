namespace dhHelpdesk_NG.DTO.DTOs.Changes.Output.Settings
{
    using dhHelpdesk_NG.Common.ValidationAttributes;

    public sealed class AnalyzeFieldSettingGroupDto
    {
        public AnalyzeFieldSettingGroupDto(
            FieldSettingDto category,
            FieldSettingDto priority,
            FieldSettingDto responsible,
            StringFieldSettingDto solution,
            FieldSettingDto cost,
            FieldSettingDto yearlyCost,
            FieldSettingDto timeEstimatesHours,
            StringFieldSettingDto risk,
            FieldSettingDto startDate,
            FieldSettingDto finishDate,
            FieldSettingDto implementationPlan,
            FieldSettingDto recoveryPlan,
            StringFieldSettingDto recommendation,
            FieldSettingDto attachedFile,
            FieldSettingDto log,
            FieldSettingDto approval)
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
        public FieldSettingDto Category { get; private set; }

        [NotNull]
        public FieldSettingDto Priority { get; private set; }

        [NotNull]
        public FieldSettingDto Responsible { get; private set; }

        [NotNull]
        public StringFieldSettingDto Solution { get; private set; }

        [NotNull]
        public FieldSettingDto Cost { get; private set; }

        [NotNull]
        public FieldSettingDto YearlyCost { get; private set; }

        [NotNull]
        public FieldSettingDto TimeEstimatesHours { get; private set; }

        [NotNull]
        public StringFieldSettingDto Risk { get; private set; }

        [NotNull]
        public FieldSettingDto StartDate { get; private set; }

        [NotNull]
        public FieldSettingDto FinishDate { get; private set; }

        [NotNull]
        public FieldSettingDto ImplementationPlan { get; private set; }

        [NotNull]
        public FieldSettingDto RecoveryPlan { get; private set; }

        [NotNull]
        public StringFieldSettingDto Recommendation { get; private set; }

        [NotNull]
        public FieldSettingDto AttachedFile { get; private set; }

        [NotNull]
        public FieldSettingDto Log { get; private set; }

        [NotNull]
        public FieldSettingDto Approval { get; private set; }
    }
}
