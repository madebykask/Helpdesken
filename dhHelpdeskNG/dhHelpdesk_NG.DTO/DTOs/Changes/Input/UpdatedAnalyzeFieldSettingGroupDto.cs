namespace dhHelpdesk_NG.DTO.DTOs.Changes.Input
{
    using dhHelpdesk_NG.Common.ValidationAttributes;

    public sealed class UpdatedAnalyzeFieldSettingGroupDto
    {
         public UpdatedAnalyzeFieldSettingGroupDto(
            UpdatedFieldSettingDto category,
            UpdatedFieldSettingDto priority,
            UpdatedFieldSettingDto responsible,
            UpdatedStringFieldSettingDto solution,
            UpdatedFieldSettingDto cost,
            UpdatedFieldSettingDto yearlyCost,
            UpdatedFieldSettingDto timeEstimatesHours,
            UpdatedStringFieldSettingDto risk,
            UpdatedFieldSettingDto startDate,
            UpdatedFieldSettingDto finishDate,
            UpdatedFieldSettingDto implementationPlan,
            UpdatedFieldSettingDto recoveryPlan,
            UpdatedStringFieldSettingDto recommendation,
            UpdatedFieldSettingDto attachedFile,
            UpdatedFieldSettingDto log,
            UpdatedFieldSettingDto approval)
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
        public UpdatedFieldSettingDto Category { get; private set; }

        [NotNull]
        public UpdatedFieldSettingDto Priority { get; private set; }

        [NotNull]
        public UpdatedFieldSettingDto Responsible { get; private set; }

        [NotNull]
        public UpdatedStringFieldSettingDto Solution { get; private set; }

        [NotNull]
        public UpdatedFieldSettingDto Cost { get; private set; }

        [NotNull]
        public UpdatedFieldSettingDto YearlyCost { get; private set; }

        [NotNull]
        public UpdatedFieldSettingDto TimeEstimatesHours { get; private set; }

        [NotNull]
        public UpdatedStringFieldSettingDto Risk { get; private set; }

        [NotNull]
        public UpdatedFieldSettingDto StartDate { get; private set; }

        [NotNull]
        public UpdatedFieldSettingDto FinishDate { get; private set; }

        [NotNull]
        public UpdatedFieldSettingDto ImplementationPlan { get; private set; }

        [NotNull]
        public UpdatedFieldSettingDto RecoveryPlan { get; private set; }

        [NotNull]
        public UpdatedStringFieldSettingDto Recommendation { get; private set; }

        [NotNull]
        public UpdatedFieldSettingDto AttachedFile { get; private set; }

        [NotNull]
        public UpdatedFieldSettingDto Log { get; private set; }

        [NotNull]
        public UpdatedFieldSettingDto Approval { get; private set; }
    }
}
