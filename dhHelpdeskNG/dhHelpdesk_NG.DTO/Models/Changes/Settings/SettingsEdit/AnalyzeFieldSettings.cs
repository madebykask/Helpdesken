namespace DH.Helpdesk.BusinessData.Models.Changes.Settings.SettingsEdit
{
    using DH.Helpdesk.Common.ValidationAttributes;

    public sealed class AnalyzeFieldSettings
    {
        public AnalyzeFieldSettings(
            FieldSetting category,
            FieldSetting priority,
            FieldSetting responsible,
            TextFieldSetting solution,
            FieldSetting cost,
            FieldSetting yearlyCost,
            FieldSetting estimatedTimeInHours,
            TextFieldSetting risk,
            FieldSetting startDate,
            FieldSetting finishDate,
            FieldSetting hasImplementationPlan,
            FieldSetting hasRecoveryPlan,
            TextFieldSetting rejectExplanation,
            FieldSetting attachedFiles,
            FieldSetting logs,
            FieldSetting approval)
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
            this.RejectExplanation = rejectExplanation;
            this.AttachedFiles = attachedFiles;
            this.Logs = logs;
            this.Approval = approval;
        }

        [NotNull]
        public FieldSetting Category { get; private set; }

        [NotNull]
        public FieldSetting Priority { get; private set; }

        [NotNull]
        public FieldSetting Responsible { get; private set; }

        [NotNull]
        public TextFieldSetting Solution { get; private set; }

        [NotNull]
        public FieldSetting Cost { get; private set; }

        [NotNull]
        public FieldSetting YearlyCost { get; private set; }

        [NotNull]
        public FieldSetting EstimatedTimeInHours { get; private set; }

        [NotNull]
        public TextFieldSetting Risk { get; private set; }

        [NotNull]
        public FieldSetting StartDate { get; private set; }

        [NotNull]
        public FieldSetting FinishDate { get; private set; }

        [NotNull]
        public FieldSetting HasImplementationPlan { get; private set; }

        [NotNull]
        public FieldSetting HasRecoveryPlan { get; private set; }

        [NotNull]
        public TextFieldSetting RejectExplanation { get; private set; }

        [NotNull]
        public FieldSetting AttachedFiles { get; private set; }

        [NotNull]
        public FieldSetting Logs { get; private set; }

        [NotNull]
        public FieldSetting Approval { get; private set; }
    }
}
