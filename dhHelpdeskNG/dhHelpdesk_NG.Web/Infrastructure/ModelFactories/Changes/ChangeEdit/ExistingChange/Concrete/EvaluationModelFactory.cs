namespace DH.Helpdesk.Web.Infrastructure.ModelFactories.Changes.ChangeEdit.ExistingChange.Concrete
{
    using System.Globalization;
    using System.Linq;

    using DH.Helpdesk.BusinessData.Enums.Changes;
    using DH.Helpdesk.Services.Response.Changes;
    using DH.Helpdesk.Web.Infrastructure.ModelFactories.Common;
    using DH.Helpdesk.Web.Models.Changes.ChangeEdit;

    public sealed class EvaluationModelFactory : IEvaluationModelFactory
    {
        #region Fields

        private readonly IConfigurableFieldModelFactory configurableFieldModelFactory;

        private readonly ISendToDialogModelFactory sendToDialogModelFactory;

        #endregion

        #region Constructors and Destructors

        public EvaluationModelFactory(
            IConfigurableFieldModelFactory configurableFieldModelFactory,
            ISendToDialogModelFactory sendToDialogModelFactory)
        {
            this.configurableFieldModelFactory = configurableFieldModelFactory;
            this.sendToDialogModelFactory = sendToDialogModelFactory;
        }

        #endregion

        #region Public Methods and Operators

        public EvaluationModel Create(FindChangeResponse response)
        {
            var settings = response.EditSettings.Evaluation;
            var options = response.EditOptions;

            var textId = response.EditData.Change.Id.ToString(CultureInfo.InvariantCulture);
            var evaluation = response.EditData.Change.Evaluation;

            var changeEvaluation = this.configurableFieldModelFactory.CreateStringField(
                settings.ChangeEvaluation,
                evaluation.ChangeEvaluation);

            var attachedFiles = this.configurableFieldModelFactory.CreateAttachedFiles(
                settings.AttachedFiles,
                textId,
                Subtopic.Evaluation,
                response.EditData.Files.Where(f => f.Subtopic == Subtopic.Evaluation).Select(f => f.Name).ToList());

            var logs = this.configurableFieldModelFactory.CreateLogs(
                settings.Logs,
                response.EditData.Change.Id,
                Subtopic.Evaluation,
                response.EditData.Logs.Where(l => l.Subtopic == Subtopic.Evaluation).ToList(),
                options.EmailGroups,
                options.WorkingGroupsWithEmails,
                options.Administrators);

            var inviteToPirDialog = this.sendToDialogModelFactory.Create(
                options.EmailGroups,
                options.WorkingGroupsWithEmails,
                options.Administrators);

            var inviteToPir = new InviteToModel(inviteToPirDialog);

            var evaluationReady = this.configurableFieldModelFactory.CreateBooleanField(
                settings.ChangeEvaluation,
                evaluation.EvaluationReady);

            return new EvaluationModel(
                response.EditData.Change.Id,
                changeEvaluation,
                inviteToPir,
                attachedFiles,
                logs,
                evaluationReady);
        }

        #endregion
    }
}