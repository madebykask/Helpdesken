namespace dhHelpdesk_NG.Web.Models.Changes.Output
{
    using dhHelpdesk_NG.Common.Tools;

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

        public FieldSettingModel Category { get; private set; }

        public FieldSettingModel Priority { get; private set; }

        public FieldSettingModel Responsible { get; private set; }

        public StringFieldSettingModel Solution { get; private set; }

        public FieldSettingModel Cost { get; private set; }

        public FieldSettingModel YearlyCost { get; private set; }

        public FieldSettingModel TimeEstimatesHours { get; private set; }

        public StringFieldSettingModel Risk { get; private set; }

        public FieldSettingModel StartDate { get; private set; }

        public FieldSettingModel FinishDate { get; private set; }

        public FieldSettingModel ImplementationPlan { get; private set; }

        public FieldSettingModel RecoveryPlan { get; private set; }

        public StringFieldSettingModel Recommendation { get; private set; }

        public FieldSettingModel AttachedFile { get; private set; }

        public FieldSettingModel Log { get; private set; }

        public FieldSettingModel Approval { get; private set; }
    }
}
