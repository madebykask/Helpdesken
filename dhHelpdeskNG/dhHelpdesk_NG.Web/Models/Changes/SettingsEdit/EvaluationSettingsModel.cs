namespace DH.Helpdesk.Web.Models.Changes.SettingsEdit
{
    using DH.Helpdesk.Common.ValidationAttributes;
    using DH.Helpdesk.Web.Infrastructure.LocalizedAttributes;

    public sealed class EvaluationSettingsModel
    {
        #region Constructors and Destructors

        public EvaluationSettingsModel()
        {
        }

        public EvaluationSettingsModel(
            TextFieldSettingModel changeEvaluation,
            FieldSettingModel attachedFiles,
            FieldSettingModel logs,
            FieldSettingModel evaluationReady)
        {
            this.ChangeEvaluation = changeEvaluation;
            this.AttachedFiles = attachedFiles;
            this.Logs = logs;
            this.EvaluationReady = evaluationReady;
        }

        #endregion

        #region Public Properties

        [NotNull]
        [LocalizedDisplay("Attached Files")]
        public FieldSettingModel AttachedFiles { get; set; }

        [NotNull]
        [LocalizedDisplay("Change Evaluation")]
        public TextFieldSettingModel ChangeEvaluation { get; set; }

        [NotNull]
        [LocalizedDisplay("Evaluation Ready")]
        public FieldSettingModel EvaluationReady { get; set; }

        [NotNull]
        [LocalizedDisplay("Logs")]
        public FieldSettingModel Logs { get; set; }

        #endregion
    }
}