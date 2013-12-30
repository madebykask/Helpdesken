namespace dhHelpdesk_NG.Web.Models.Changes
{
    using dhHelpdesk_NG.Common.ValidationAttributes;
    using dhHelpdesk_NG.Web.Infrastructure.LocalizedAttributes;

    public sealed class EvaluationFieldSettingGroupModel
    {
        public EvaluationFieldSettingGroupModel()
        {
        }

        public EvaluationFieldSettingGroupModel(
            StringFieldSettingModel evaluation,
            FieldSettingModel attachedFile,
            FieldSettingModel log,
            FieldSettingModel evaluationReady)
        {
            this.Evaluation = evaluation;
            this.AttachedFile = attachedFile;
            this.Log = log;
            this.EvaluationReady = evaluationReady;
        }

        [NotNull]
        [LocalizedDisplay("Evaluation")]
        public StringFieldSettingModel Evaluation { get; private set; }

        [NotNull]
        [LocalizedDisplay("Attached file")]
        public FieldSettingModel AttachedFile { get; private set; }

        [NotNull]
        [LocalizedDisplay("Log")]
        public FieldSettingModel Log { get; private set; }

        [NotNull]
        [LocalizedDisplay("Evaluation ready")]
        public FieldSettingModel EvaluationReady { get; private set; }
    }
}
