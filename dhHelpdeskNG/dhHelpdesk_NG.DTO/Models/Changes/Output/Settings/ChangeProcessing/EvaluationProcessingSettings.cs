namespace DH.Helpdesk.BusinessData.Models.Changes.Output.Settings.ChangeProcessing
{
    using DH.Helpdesk.Common.ValidationAttributes;

    public sealed class EvaluationProcessingSettings
    {
        public EvaluationProcessingSettings(
            FieldProcessingSetting changeEvaluation,
            FieldProcessingSetting attachedFiles,
            FieldProcessingSetting logs,
            FieldProcessingSetting evaluationReady)
        {
            this.ChangeEvaluation = changeEvaluation;
            this.AttachedFiles = attachedFiles;
            this.Logs = logs;
            this.EvaluationReady = evaluationReady;
        }

        [NotNull]
        public FieldProcessingSetting ChangeEvaluation { get; private set; }

        [NotNull]
        public FieldProcessingSetting AttachedFiles { get; private set; }

        [NotNull]
        public FieldProcessingSetting Logs { get; private set; }

        [NotNull]
        public FieldProcessingSetting EvaluationReady { get; private set; }
    }
}
