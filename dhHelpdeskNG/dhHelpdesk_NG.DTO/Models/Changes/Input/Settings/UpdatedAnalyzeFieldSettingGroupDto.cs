namespace DH.Helpdesk.BusinessData.Models.Changes.Input.Settings
{
    using DH.Helpdesk.Common.ValidationAttributes;

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
