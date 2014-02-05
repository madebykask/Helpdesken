namespace dhHelpdesk_NG.DTO.DTOs.Changes.Output.Settings.ChangeEdit
{
    using dhHelpdesk_NG.Common.ValidationAttributes;

    public sealed class EvaluationFieldEditSettings
    {
        public EvaluationFieldEditSettings(
            TextFieldEditSetting evaluation,
            FieldEditSetting attachedFiles,
            FieldEditSetting logs,
            FieldEditSetting evaluationReady)
        {
            this.Evaluation = evaluation;
            this.AttachedFiles = attachedFiles;
            this.Logs = logs;
            this.EvaluationReady = evaluationReady;
        }

        [NotNull]
        public TextFieldEditSetting Evaluation { get; private set; }

        [NotNull]
        public FieldEditSetting AttachedFiles { get; private set; }

        [NotNull]
        public FieldEditSetting Logs { get; private set; }

        [NotNull]
        public FieldEditSetting EvaluationReady { get; private set; }
    }
}
