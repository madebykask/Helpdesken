namespace dhHelpdesk_NG.Web.Models.Changes
{
    using dhHelpdesk_NG.Common.ValidationAttributes;
    using dhHelpdesk_NG.Web.Infrastructure.LocalizedAttributes;

    public sealed class AnalyzeFieldSettingGroupModel
    {
        public AnalyzeFieldSettingGroupModel() 
        {
        }

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
        public FieldSettingModel Category { get; private set; }

        [NotNull]
        [LocalizedDisplay("Priority")]
        public FieldSettingModel Priority { get; private set; }

        [NotNull]
        [LocalizedDisplay("Responsible")]
        public FieldSettingModel Responsible { get; private set; }

        [NotNull]
        [LocalizedDisplay("Solution")]
        public StringFieldSettingModel Solution { get; private set; }

        [NotNull]
        [LocalizedDisplay("Cost")]
        public FieldSettingModel Cost { get; private set; }

        [NotNull]
        [LocalizedDisplay("Yearly cost")]
        public FieldSettingModel YearlyCost { get; private set; }

        [NotNull]
        [LocalizedDisplay("Time estimates hours")]
        public FieldSettingModel TimeEstimatesHours { get; private set; }

        [NotNull]
        [LocalizedDisplay("Risk")]
        public StringFieldSettingModel Risk { get; private set; }

        [NotNull]
        [LocalizedDisplay("Start date")]
        public FieldSettingModel StartDate { get; private set; }

        [NotNull]
        [LocalizedDisplay("Finish date")]
        public FieldSettingModel FinishDate { get; private set; }

        [NotNull]
        [LocalizedDisplay("Implementation plan")]
        public FieldSettingModel ImplementationPlan { get; private set; }

        [NotNull]
        [LocalizedDisplay("Recovery plan")]
        public FieldSettingModel RecoveryPlan { get; private set; }

        [NotNull]
        [LocalizedDisplay("Recommendation")]
        public StringFieldSettingModel Recommendation { get; private set; }

        [NotNull]
        [LocalizedDisplay("Attached file")]
        public FieldSettingModel AttachedFile { get; private set; }

        [NotNull]
        [LocalizedDisplay("Log")]
        public FieldSettingModel Log { get; private set; }

        [NotNull]
        [LocalizedDisplay("Approval")]
        public FieldSettingModel Approval { get; private set; }
    }
}
