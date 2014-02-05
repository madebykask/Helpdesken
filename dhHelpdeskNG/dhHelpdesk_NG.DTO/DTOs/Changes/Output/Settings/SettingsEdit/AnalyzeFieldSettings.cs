namespace dhHelpdesk_NG.DTO.DTOs.Changes.Output.Settings.SettingsEdit
{
    using dhHelpdesk_NG.Common.ValidationAttributes;

    public sealed class AnalyzeFieldSettings
    {
        public AnalyzeFieldSettings(
            FieldSetting category,
            FieldSetting priority,
            FieldSetting responsible,
            StringFieldSetting solution,
            FieldSetting cost,
            FieldSetting yearlyCost,
            FieldSetting timeEstimatesHours,
            StringFieldSetting risk,
            FieldSetting startDate,
            FieldSetting finishDate,
            FieldSetting implementationPlan,
            FieldSetting recoveryPlan,
            StringFieldSetting recommendation,
            FieldSetting attachedFile,
            FieldSetting log,
            FieldSetting approval)
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
        public FieldSetting Category { get; private set; }

        [NotNull]
        public FieldSetting Priority { get; private set; }

        [NotNull]
        public FieldSetting Responsible { get; private set; }

        [NotNull]
        public StringFieldSetting Solution { get; private set; }

        [NotNull]
        public FieldSetting Cost { get; private set; }

        [NotNull]
        public FieldSetting YearlyCost { get; private set; }

        [NotNull]
        public FieldSetting TimeEstimatesHours { get; private set; }

        [NotNull]
        public StringFieldSetting Risk { get; private set; }

        [NotNull]
        public FieldSetting StartDate { get; private set; }

        [NotNull]
        public FieldSetting FinishDate { get; private set; }

        [NotNull]
        public FieldSetting ImplementationPlan { get; private set; }

        [NotNull]
        public FieldSetting RecoveryPlan { get; private set; }

        [NotNull]
        public StringFieldSetting Recommendation { get; private set; }

        [NotNull]
        public FieldSetting AttachedFile { get; private set; }

        [NotNull]
        public FieldSetting Log { get; private set; }

        [NotNull]
        public FieldSetting Approval { get; private set; }
    }
}
