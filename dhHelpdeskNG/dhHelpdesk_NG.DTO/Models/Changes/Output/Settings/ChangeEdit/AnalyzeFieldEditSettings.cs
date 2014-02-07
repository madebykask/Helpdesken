namespace DH.Helpdesk.BusinessData.Models.Changes.Output.Settings.ChangeEdit
{
    using DH.Helpdesk.Common.ValidationAttributes;

    public sealed class AnalyzeFieldEditSettings
    {
        public AnalyzeFieldEditSettings(
            FieldEditSetting category,
            FieldEditSetting priority,
            FieldEditSetting responsible,
            TextFieldEditSetting solution,
            FieldEditSetting cost,
            FieldEditSetting yearlyCost,
            FieldEditSetting timeEstimatesHours,
            TextFieldEditSetting risk,
            FieldEditSetting startDate,
            FieldEditSetting finishDate,
            FieldEditSetting hasImplementationPlan,
            FieldEditSetting hasRecoveryPlan,
            TextFieldEditSetting recommendation,
            FieldEditSetting attachedFiles,
            FieldEditSetting logs,
            FieldEditSetting approval)
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
            this.HasImplementationPlan = hasImplementationPlan;
            this.HasRecoveryPlan = hasRecoveryPlan;
            this.Recommendation = recommendation;
            this.AttachedFiles = attachedFiles;
            this.Logs = logs;
            this.Approval = approval;
        }

        [NotNull]
        public FieldEditSetting Category { get; private set; }

        [NotNull]
        public FieldEditSetting Priority { get; private set; }

        [NotNull]
        public FieldEditSetting Responsible { get; private set; }

        [NotNull]
        public TextFieldEditSetting Solution { get; private set; }

        [NotNull]
        public FieldEditSetting Cost { get; private set; }

        [NotNull]
        public FieldEditSetting YearlyCost { get; private set; }

        [NotNull]
        public FieldEditSetting TimeEstimatesHours { get; private set; }

        [NotNull]
        public TextFieldEditSetting Risk { get; private set; }

        [NotNull]
        public FieldEditSetting StartDate { get; private set; }

        [NotNull]
        public FieldEditSetting FinishDate { get; private set; }

        [NotNull]
        public FieldEditSetting HasImplementationPlan { get; private set; }

        [NotNull]
        public FieldEditSetting HasRecoveryPlan { get; private set; }

        [NotNull]
        public TextFieldEditSetting Recommendation { get; private set; }

        [NotNull]
        public FieldEditSetting AttachedFiles { get; private set; }

        [NotNull]
        public FieldEditSetting Logs { get; private set; }

        [NotNull]
        public FieldEditSetting Approval { get; private set; }
    }
}
