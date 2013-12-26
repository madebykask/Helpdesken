namespace dhHelpdesk_NG.DTO.DTOs.Changes.Input
{
    using dhHelpdesk_NG.Common.Tools;

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
            ArgumentsValidator.NotNull(category, "category");
            ArgumentsValidator.NotNull(priority, "priority");
            ArgumentsValidator.NotNull(responsible, "responsible");
            ArgumentsValidator.NotNull(solution, "solution");
            ArgumentsValidator.NotNull(cost, "cost");
            ArgumentsValidator.NotNull(yearlyCost, "yearlyCost");
            ArgumentsValidator.NotNull(timeEstimatesHours, "timeEstimatesHours");
            ArgumentsValidator.NotNull(risk, "risk");
            ArgumentsValidator.NotNull(startDate, "startDate");
            ArgumentsValidator.NotNull(finishDate, "finishDate");
            ArgumentsValidator.NotNull(implementationPlan, "implementationPlan");
            ArgumentsValidator.NotNull(recoveryPlan, "recoveryPlan");
            ArgumentsValidator.NotNull(recommendation, "recommendation");
            ArgumentsValidator.NotNull(attachedFile, "attachedFile");
            ArgumentsValidator.NotNull(log, "log");
            ArgumentsValidator.NotNull(approval, "approval");

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

        public UpdatedFieldSettingDto Category { get; private set; }

        public UpdatedFieldSettingDto Priority { get; private set; }

        public UpdatedFieldSettingDto Responsible { get; private set; }

        public UpdatedStringFieldSettingDto Solution { get; private set; }

        public UpdatedFieldSettingDto Cost { get; private set; }

        public UpdatedFieldSettingDto YearlyCost { get; private set; }

        public UpdatedFieldSettingDto TimeEstimatesHours { get; private set; }

        public UpdatedStringFieldSettingDto Risk { get; private set; }

        public UpdatedFieldSettingDto StartDate { get; private set; }

        public UpdatedFieldSettingDto FinishDate { get; private set; }

        public UpdatedFieldSettingDto ImplementationPlan { get; private set; }

        public UpdatedFieldSettingDto RecoveryPlan { get; private set; }

        public UpdatedStringFieldSettingDto Recommendation { get; private set; }

        public UpdatedFieldSettingDto AttachedFile { get; private set; }

        public UpdatedFieldSettingDto Log { get; private set; }

        public UpdatedFieldSettingDto Approval { get; private set; }
    }
}
