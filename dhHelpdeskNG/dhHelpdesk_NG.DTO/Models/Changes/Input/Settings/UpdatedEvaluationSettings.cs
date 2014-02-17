namespace DH.Helpdesk.BusinessData.Models.Changes.Input.Settings
{
    using DH.Helpdesk.Common.ValidationAttributes;

    public sealed class UpdatedEvaluationSettings
    {
        public UpdatedEvaluationSettings(
            UpdatedTextFieldSetting changeEvaluation,
            UpdatedFieldSetting attachedFiles,
            UpdatedFieldSetting logs,
            UpdatedFieldSetting evaluationReady)
        {
            this.ChangeEvaluation = changeEvaluation;
            this.AttachedFiles = attachedFiles;
            this.Logs = logs;
            this.EvaluationReady = evaluationReady;
        }

        [NotNull]
        public UpdatedTextFieldSetting ChangeEvaluation { get; private set; }

        [NotNull]
        public UpdatedFieldSetting AttachedFiles { get; private set; }

        [NotNull]
        public UpdatedFieldSetting Logs { get; private set; }

        [NotNull]
        public UpdatedFieldSetting EvaluationReady { get; private set; }
    }
}
