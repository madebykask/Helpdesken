namespace dhHelpdesk_NG.DTO.DTOs.Changes.Output
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
            AttachedFile = attachedFile;
            Log = log;
            Approval = approval;
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
