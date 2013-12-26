namespace dhHelpdesk_NG.DTO.DTOs.Changes.Output
{
    using dhHelpdesk_NG.Common.Tools;

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

        public FieldSettingDto Category { get; private set; }

        public FieldSettingDto Priority { get; private set; }

        public FieldSettingDto Responsible { get; private set; }

        public StringFieldSettingDto Solution { get; private set; }

        public FieldSettingDto Cost { get; private set; }

        public FieldSettingDto YearlyCost { get; private set; }

        public FieldSettingDto TimeEstimatesHours { get; private set; }

        public StringFieldSettingDto Risk { get; private set; }

        public FieldSettingDto StartDate { get; private set; }

        public FieldSettingDto FinishDate { get; private set; }

        public FieldSettingDto ImplementationPlan { get; private set; }

        public FieldSettingDto RecoveryPlan { get; private set; }

        public StringFieldSettingDto Recommendation { get; private set; }

        public FieldSettingDto AttachedFile { get; private set; }

        public FieldSettingDto Log { get; private set; }

        public FieldSettingDto Approval { get; private set; }
    }
}
