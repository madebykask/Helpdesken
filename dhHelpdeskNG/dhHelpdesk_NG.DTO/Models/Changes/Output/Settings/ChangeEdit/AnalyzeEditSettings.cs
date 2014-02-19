namespace DH.Helpdesk.BusinessData.Models.Changes.Output.Settings.ChangeEdit
{
    using DH.Helpdesk.Common.ValidationAttributes;

    public sealed class AnalyzeEditSettings
    {
        public AnalyzeEditSettings(
            FieldEditSetting category,
            FieldEditSetting priority,
            FieldEditSetting responsible,
            TextFieldEditSetting solution,
            FieldEditSetting cost,
            FieldEditSetting yearlyCost,
            FieldEditSetting estimatedTimeInHours,
            TextFieldEditSetting risk,
            FieldEditSetting startDate,
            FieldEditSetting finishDate,
            FieldEditSetting hasImplementationPlan,
            FieldEditSetting hasRecoveryPlan,
            FieldEditSetting attachedFiles,
            FieldEditSetting logs,
            FieldEditSetting approval,
            TextFieldEditSetting rejectExplanation)
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
            this.AttachedFiles = attachedFiles;
            this.Logs = logs;
            this.Approval = approval;
            this.RejectExplanation = rejectExplanation;
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
        public FieldEditSetting EstimatedTimeInHours { get; private set; }

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
        public FieldEditSetting AttachedFiles { get; private set; }

        [NotNull]
        public FieldEditSetting Logs { get; private set; }

        [NotNull]
        public FieldEditSetting Approval { get; private set; }

        [NotNull]
        public TextFieldEditSetting RejectExplanation { get; private set; }
    }
}
