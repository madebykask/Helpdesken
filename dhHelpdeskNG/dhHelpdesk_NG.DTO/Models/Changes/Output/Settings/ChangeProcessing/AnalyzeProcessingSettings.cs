namespace DH.Helpdesk.BusinessData.Models.Changes.Output.Settings.ChangeProcessing
{
    using DH.Helpdesk.Common.ValidationAttributes;

    public sealed class AnalyzeProcessingSettings
    {
        public AnalyzeProcessingSettings(
            FieldProcessingSetting category,
            FieldProcessingSetting priority,
            FieldProcessingSetting responsible,
            FieldProcessingSetting solution,
            FieldProcessingSetting cost,
            FieldProcessingSetting estimatedTimeInHours,
            FieldProcessingSetting risk,
            FieldProcessingSetting startDate,
            FieldProcessingSetting finishDate,
            FieldProcessingSetting hasImplementationPlan,
            FieldProcessingSetting hasRecoveryPlan,
            FieldProcessingSetting attachedFiles,
            FieldProcessingSetting logs,
            FieldProcessingSetting approval,
            FieldProcessingSetting rejectExplanation)
        {
            this.Category = category;
            this.Priority = priority;
            this.Responsible = responsible;
            this.Solution = solution;
            this.Cost = cost;
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
        public FieldProcessingSetting Category { get; private set; }

        [NotNull]
        public FieldProcessingSetting Priority { get; private set; }

        [NotNull]
        public FieldProcessingSetting Responsible { get; private set; }

        [NotNull]
        public FieldProcessingSetting Solution { get; private set; }

        [NotNull]
        public FieldProcessingSetting Cost { get; private set; }

        [NotNull]
        public FieldProcessingSetting EstimatedTimeInHours { get; private set; }

        [NotNull]
        public FieldProcessingSetting Risk { get; private set; }

        [NotNull]
        public FieldProcessingSetting StartDate { get; private set; }

        [NotNull]
        public FieldProcessingSetting FinishDate { get; private set; }

        [NotNull]
        public FieldProcessingSetting HasImplementationPlan { get; private set; }

        [NotNull]
        public FieldProcessingSetting HasRecoveryPlan { get; private set; }

        [NotNull]
        public FieldProcessingSetting AttachedFiles { get; private set; }

        [NotNull]
        public FieldProcessingSetting Logs { get; private set; }

        [NotNull]
        public FieldProcessingSetting Approval { get; private set; }

        [NotNull]
        public FieldProcessingSetting RejectExplanation { get; private set; }
    }
}
