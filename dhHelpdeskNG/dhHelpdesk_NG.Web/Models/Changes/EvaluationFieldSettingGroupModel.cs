namespace dhHelpdesk_NG.Web.Models.Changes
{
    using dhHelpdesk_NG.Common.ValidationAttributes;

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
        public StringFieldSettingModel Evaluation { get; private set; }

        [NotNull]
        public FieldSettingModel AttachedFile { get; private set; }

        [NotNull]
        public FieldSettingModel Log { get; private set; }

        [NotNull]
        public FieldSettingModel EvaluationReady { get; private set; }
    }
}
