namespace DH.Helpdesk.BusinessData.Models.Changes.Output.Settings.SettingsEdit
{
    using DH.Helpdesk.Common.ValidationAttributes;

    public sealed class EvaluationFieldSettings
    {
        public EvaluationFieldSettings(
            TextFieldSetting evaluation,
            FieldSetting attachedFiles,
            FieldSetting logs,
            FieldSetting evaluationReady)
        {
            this.Evaluation = evaluation;
            this.AttachedFiles = attachedFiles;
            this.Logs = logs;
            this.EvaluationReady = evaluationReady;
        }

        [NotNull]
        public TextFieldSetting Evaluation { get; private set; }

        [NotNull]
        public FieldSetting AttachedFiles { get; private set; }

        [NotNull]
        public FieldSetting Logs { get; private set; }

        [NotNull]
        public FieldSetting EvaluationReady { get; private set; }
    }
}
