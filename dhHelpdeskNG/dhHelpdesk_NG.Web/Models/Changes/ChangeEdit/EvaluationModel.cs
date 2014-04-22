namespace DH.Helpdesk.Web.Models.Changes.ChangeEdit
{
    using DH.Helpdesk.Common.ValidationAttributes;

    public sealed class EvaluationModel
    {
        #region Constructors and Destructors

        public EvaluationModel()
        {
        }

        public EvaluationModel(
            int changeId,
            ConfigurableFieldModel<string> changeEvaluation,
            InviteToModel inviteToPir,
            ConfigurableFieldModel<AttachedFilesModel> attachedFiles,
            ConfigurableFieldModel<LogsModel> logs,
            ConfigurableFieldModel<bool> evaluationReady)
        {
            this.ChangeId = changeId;
            this.ChangeEvaluation = changeEvaluation;
            this.InviteToPir = inviteToPir;
            this.AttachedFiles = attachedFiles;
            this.Logs = logs;
            this.EvaluationReady = evaluationReady;
        }

        #endregion

        #region Public Properties

        [NotNull]
        public ConfigurableFieldModel<AttachedFilesModel> AttachedFiles { get; set; }

        [NotNull]
        public ConfigurableFieldModel<string> ChangeEvaluation { get; set; }

        [IsId]
        public int ChangeId { get; set; }

        [NotNull]
        public ConfigurableFieldModel<bool> EvaluationReady { get; set; }

        [NotNull]
        public InviteToModel InviteToPir { get; set; }

        [NotNull]
        public ConfigurableFieldModel<LogsModel> Logs { get; set; }

        #endregion
    }
}