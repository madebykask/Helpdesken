namespace dhHelpdesk_NG.Web.Models.Changes.Edit
{
    using dhHelpdesk_NG.Common.ValidationAttributes;
    using dhHelpdesk_NG.Web.Infrastructure.LocalizedAttributes;

    public sealed class EvaluationModel
    {
        public EvaluationModel()
        {
        }

        public EvaluationModel(
            string changeId,
            ConfigurableFieldModel<string> changeEvaluation,
            AttachedFilesContainerModel attachedFilesContainer,
            SendToDialogModel sendToDialog,
            ConfigurableFieldModel<bool> evaluationReady)
        {
            this.ChangeId = changeId;
            this.ChangeEvaluation = changeEvaluation;
            this.AttachedFilesContainer = attachedFilesContainer;
            this.SendToDialog = sendToDialog;
            this.EvaluationReady = evaluationReady;
        }

        public string ChangeId { get; set; }

        [LocalizedDisplay("Change evaluation")]
        public ConfigurableFieldModel<string> ChangeEvaluation { get; set; }

        [LocalizedDisplay("Attached Files")]
        public AttachedFilesContainerModel AttachedFilesContainer { get; set; }

        public NewLogModel NewLog { get; set; }

        [NotNull]
        public SendToDialogModel SendToDialog { get; set; }

        [LocalizedDisplay("Evaluation ready")]
        public ConfigurableFieldModel<bool> EvaluationReady { get; set; }
    }
}