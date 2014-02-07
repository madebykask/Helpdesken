namespace DH.Helpdesk.BusinessData.Models.Changes.Output.Settings.SettingsEdit
{
    using DH.Helpdesk.Common.ValidationAttributes;

    public sealed class EvaluationFieldSettings
    {
        public EvaluationFieldSettings(
            StringFieldSetting evaluation,
            FieldSetting attachedFile,
            FieldSetting log,
            FieldSetting evaluationReady)
        {
            this.Evaluation = evaluation;
            this.AttachedFile = attachedFile;
            this.Log = log;
            this.EvaluationReady = evaluationReady;
        }

        [NotNull]
        public StringFieldSetting Evaluation { get; private set; }

        [NotNull]
        public FieldSetting AttachedFile { get; private set; }

        [NotNull]
        public FieldSetting Log { get; private set; }

        [NotNull]
        public FieldSetting EvaluationReady { get; private set; }
    }
}
