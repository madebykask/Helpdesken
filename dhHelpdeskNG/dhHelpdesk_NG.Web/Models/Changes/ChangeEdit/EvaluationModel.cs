namespace DH.Helpdesk.Web.Models.Changes.ChangeEdit
{
    using DH.Helpdesk.Common.ValidationAttributes;
    using DH.Helpdesk.Web.Models.Common;

    public sealed class EvaluationModel
    {
        public EvaluationModel()
        {
        }

        public EvaluationModel(
            string changeId,
            ConfigurableFieldModel<string> changeEvaluation,
            ConfigurableFieldModel<AttachedFilesModel> attachedFiles,
            ConfigurableFieldModel<LogsModel> logs,
            SendToDialogModel sendToDialog,
            ConfigurableFieldModel<bool> evaluationReady)
        {
            this.ChangeId = changeId;
            this.ChangeEvaluation = changeEvaluation;
            this.AttachedFiles = attachedFiles;
            this.Logs = logs;
            this.SendToDialog = sendToDialog;
            this.EvaluationReady = evaluationReady;
        }

        public string ChangeId { get; private set; }

        [NotNull]
        public ConfigurableFieldModel<string> ChangeEvaluation { get; set; }

        [NotNull]
        public ConfigurableFieldModel<AttachedFilesModel> AttachedFiles { get; set; }

        [NotNull]
        public ConfigurableFieldModel<LogsModel> Logs { get; private set; }

        public SendToDialogModel SendToDialog { get; set; }

        [NotNull]
        public ConfigurableFieldModel<bool> EvaluationReady { get; set; }
    }
}