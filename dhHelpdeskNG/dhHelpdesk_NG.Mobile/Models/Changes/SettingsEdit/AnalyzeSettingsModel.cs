namespace DH.Helpdesk.Web.Models.Changes.SettingsEdit
{
    using DH.Helpdesk.Common.ValidationAttributes;
    using DH.Helpdesk.Web.Infrastructure.LocalizedAttributes;

    public sealed class AnalyzeSettingsModel
    {
        public AnalyzeSettingsModel()
        {
        }

        public AnalyzeSettingsModel(
            FieldSettingModel category,
            FieldSettingModel priority,
            FieldSettingModel responsible,
            TextFieldSettingModel solution,
            FieldSettingModel cost,
            FieldSettingModel yearlyCost,
            FieldSettingModel estimatedTimeInHours,
            TextFieldSettingModel risk,
            FieldSettingModel startDate,
            FieldSettingModel finishDate,
            FieldSettingModel hasImplementationPlan,
            FieldSettingModel hasRecoveryPlan,
            FieldSettingModel attachedFiles,
            FieldSettingModel logs,
            FieldSettingModel approval,
            TextFieldSettingModel rejectExplanation)
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
        [LocalizedDisplay("Category")]
        public FieldSettingModel Category { get; set; }

        [NotNull]
        [LocalizedDisplay("Priority")]
        public FieldSettingModel Priority { get; set; }

        [NotNull]
        [LocalizedDisplay("Responsible")]
        public FieldSettingModel Responsible { get; set; }

        [NotNull]
        [LocalizedDisplay("Solution")]
        public TextFieldSettingModel Solution { get; set; }

        [NotNull]
        [LocalizedDisplay("Cost")]
        public FieldSettingModel Cost { get; set; }

        [NotNull]
        [LocalizedDisplay("Yearly Cost")]
        public FieldSettingModel YearlyCost { get; set; }

        [NotNull]
        [LocalizedDisplay("Estimated Time in Hours")]
        public FieldSettingModel EstimatedTimeInHours { get; set; }

        [NotNull]
        [LocalizedDisplay("Risk")]
        public TextFieldSettingModel Risk { get; set; }

        [NotNull]
        [LocalizedDisplay("Start Date")]
        public FieldSettingModel StartDate { get; set; }

        [NotNull]
        [LocalizedDisplay("Finish Date")]
        public FieldSettingModel FinishDate { get; set; }

        [NotNull]
        [LocalizedDisplay("Has Implementation Plan")]
        public FieldSettingModel HasImplementationPlan { get; set; }

        [NotNull]
        [LocalizedDisplay("Has Recovery Plan")]
        public FieldSettingModel HasRecoveryPlan { get; set; }

        [NotNull]
        [LocalizedDisplay("Attached Files")]
        public FieldSettingModel AttachedFiles { get; set; }

        [NotNull]
        [LocalizedDisplay("Logs")]
        public FieldSettingModel Logs { get; set; }

        [NotNull]
        [LocalizedDisplay("Approval")]
        public FieldSettingModel Approval { get; set; }

        [NotNull]
        [LocalizedDisplay("Reject Explanation")]
        public TextFieldSettingModel RejectExplanation { get; set; }
    }
}