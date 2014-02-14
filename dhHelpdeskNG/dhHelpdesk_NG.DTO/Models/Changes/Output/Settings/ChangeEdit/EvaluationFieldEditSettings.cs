namespace DH.Helpdesk.BusinessData.Models.Changes.Output.Settings.ChangeEdit
{
    using DH.Helpdesk.Common.ValidationAttributes;

    public sealed class EvaluationFieldEditSettings
    {
        public EvaluationFieldEditSettings(
            TextFieldEditSetting changeEvaluation,
            FieldEditSetting attachedFiles,
            FieldEditSetting logs,
            FieldEditSetting evaluationReady)
        {
            this.ChangeEvaluation = changeEvaluation;
            this.AttachedFiles = attachedFiles;
            this.Logs = logs;
            this.EvaluationReady = evaluationReady;
        }

        [NotNull]
        public TextFieldEditSetting ChangeEvaluation { get; private set; }

        [NotNull]
        public FieldEditSetting AttachedFiles { get; private set; }

        [NotNull]
        public FieldEditSetting Logs { get; private set; }

        [NotNull]
        public FieldEditSetting EvaluationReady { get; private set; }
    }
}
