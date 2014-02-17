namespace DH.Helpdesk.BusinessData.Models.Changes.Input.Settings
{
    using DH.Helpdesk.Common.ValidationAttributes;

    public sealed class UpdatedAnalyzeSettings
    {
        public UpdatedAnalyzeSettings(
            UpdatedFieldSetting category,
            UpdatedFieldSetting priority,
            UpdatedFieldSetting responsible,
            UpdatedTextFieldSetting solution,
            UpdatedFieldSetting cost,
            UpdatedFieldSetting yearlyCost,
            UpdatedFieldSetting estimatedTimeInHours,
            UpdatedTextFieldSetting risk,
            UpdatedFieldSetting startDate,
            UpdatedFieldSetting finishDate,
            UpdatedFieldSetting hasImplementationPlan,
            UpdatedFieldSetting hasRecoveryPlan,
            UpdatedFieldSetting attachedFiles,
            UpdatedFieldSetting logs,
            UpdatedFieldSetting approval,
            UpdatedTextFieldSetting rejectExplanation)
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
            this.Approval = approval;
            this.AttachedFiles = attachedFiles;
            this.Logs = logs;
            this.RejectExplanation = rejectExplanation;
        }

        [NotNull]
        public UpdatedFieldSetting Category { get; private set; }

        [NotNull]
        public UpdatedFieldSetting Priority { get; private set; }

        [NotNull]
        public UpdatedFieldSetting Responsible { get; private set; }

        [NotNull]
        public UpdatedTextFieldSetting Solution { get; private set; }

        [NotNull]
        public UpdatedFieldSetting Cost { get; private set; }

        [NotNull]
        public UpdatedFieldSetting YearlyCost { get; private set; }

        [NotNull]
        public UpdatedFieldSetting EstimatedTimeInHours { get; private set; }

        [NotNull]
        public UpdatedTextFieldSetting Risk { get; private set; }

        [NotNull]
        public UpdatedFieldSetting StartDate { get; private set; }

        [NotNull]
        public UpdatedFieldSetting FinishDate { get; private set; }

        [NotNull]
        public UpdatedFieldSetting HasImplementationPlan { get; private set; }

        [NotNull]
        public UpdatedFieldSetting HasRecoveryPlan { get; private set; }

        [NotNull]
        public UpdatedFieldSetting AttachedFiles { get; private set; }

        [NotNull]
        public UpdatedFieldSetting Logs { get; private set; }

        [NotNull]
        public UpdatedFieldSetting Approval { get; private set; }

        [NotNull]
        public UpdatedTextFieldSetting RejectExplanation { get; private set; }
    }
}
