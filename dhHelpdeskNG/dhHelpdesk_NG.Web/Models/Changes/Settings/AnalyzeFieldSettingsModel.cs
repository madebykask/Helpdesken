namespace DH.Helpdesk.Web.Models.Changes.Settings
{
    using DH.Helpdesk.Common.ValidationAttributes;
    using DH.Helpdesk.Web.Infrastructure.LocalizedAttributes;

    public sealed class AnalyzeFieldSettingsModel
    {
        public AnalyzeFieldSettingsModel() 
        {
        }

        public AnalyzeFieldSettingsModel(
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
        public StringFieldSettingModel Solution { get; set; }

        [NotNull]
        [LocalizedDisplay("Cost")]
        public FieldSettingModel Cost { get; set; }

        [NotNull]
        [LocalizedDisplay("Yearly cost")]
        public FieldSettingModel YearlyCost { get; set; }

        [NotNull]
        [LocalizedDisplay("Time estimates hours")]
        public FieldSettingModel TimeEstimatesHours { get; set; }

        [NotNull]
        [LocalizedDisplay("Risk")]
        public StringFieldSettingModel Risk { get; set; }

        [NotNull]
        [LocalizedDisplay("Start date")]
        public FieldSettingModel StartDate { get; set; }

        [NotNull]
        [LocalizedDisplay("Finish date")]
        public FieldSettingModel FinishDate { get; set; }

        [NotNull]
        [LocalizedDisplay("Implementation plan")]
        public FieldSettingModel ImplementationPlan { get; set; }

        [NotNull]
        [LocalizedDisplay("Recovery plan")]
        public FieldSettingModel RecoveryPlan { get; set; }

        [NotNull]
        [LocalizedDisplay("Recommendation")]
        public StringFieldSettingModel Recommendation { get; set; }

        [NotNull]
        [LocalizedDisplay("Attached file")]
        public FieldSettingModel AttachedFile { get; set; }

        [NotNull]
        [LocalizedDisplay("Log")]
        public FieldSettingModel Log { get; set; }

        [NotNull]
        [LocalizedDisplay("Approval")]
        public FieldSettingModel Approval { get; set; }
    }
}
