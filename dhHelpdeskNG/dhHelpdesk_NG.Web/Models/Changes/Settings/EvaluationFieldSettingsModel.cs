namespace DH.Helpdesk.Web.Models.Changes.Settings
{
    using DH.Helpdesk.Common.ValidationAttributes;
    using DH.Helpdesk.Web.Infrastructure.LocalizedAttributes;

    public sealed class EvaluationFieldSettingsModel
    {
        public EvaluationFieldSettingsModel()
        {
        }

        public EvaluationFieldSettingsModel(
            StringFieldSettingModel changeEvaluation,
            FieldSettingModel attachedFiles,
            FieldSettingModel logs,
            FieldSettingModel evaluationReady)
        {
            this.ChangeEvaluation = changeEvaluation;
            this.AttachedFiles = attachedFiles;
            this.Logs = logs;
            this.EvaluationReady = evaluationReady;
        }

        [NotNull]
        [LocalizedDisplay("Change Evaluation")]
        public StringFieldSettingModel ChangeEvaluation { get; set; }

        [NotNull]
        [LocalizedDisplay("Attached Files")]
        public FieldSettingModel AttachedFiles { get; set; }

        [NotNull]
        [LocalizedDisplay("Logs")]
        public FieldSettingModel Logs { get; set; }

        [NotNull]
        [LocalizedDisplay("Evaluation Ready")]
        public FieldSettingModel EvaluationReady { get; set; }
    }
}
