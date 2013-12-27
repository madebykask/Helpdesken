namespace dhHelpdesk_NG.Web.Models.Changes.Output
{
    using dhHelpdesk_NG.Common.Tools;

    public sealed class EvaluationFieldSettingGroupModel
    {
        public EvaluationFieldSettingGroupModel(
            StringFieldSettingModel evaluation,
            FieldSettingModel attachedFile,
            FieldSettingModel log,
            FieldSettingModel evaluationReady)
        {
            ArgumentsValidator.NotNull(evaluation, "evaluation");
            ArgumentsValidator.NotNull(attachedFile, "attachedFile");
            ArgumentsValidator.NotNull(log, "log");
            ArgumentsValidator.NotNull(evaluationReady, "evaluationReady");

            this.Evaluation = evaluation;
            this.AttachedFile = attachedFile;
            this.Log = log;
            this.EvaluationReady = evaluationReady;
        }

        public StringFieldSettingModel Evaluation { get; private set; }

        public FieldSettingModel AttachedFile { get; private set; }

        public FieldSettingModel Log { get; private set; }

        public FieldSettingModel EvaluationReady { get; private set; }
    }
}
