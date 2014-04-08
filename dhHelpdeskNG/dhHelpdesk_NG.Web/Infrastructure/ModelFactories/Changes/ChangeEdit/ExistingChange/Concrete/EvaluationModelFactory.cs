namespace DH.Helpdesk.Web.Infrastructure.ModelFactories.Changes.ChangeEdit.ExistingChange.Concrete
{
    using System.Globalization;

    using DH.Helpdesk.BusinessData.Enums.Changes;
    using DH.Helpdesk.BusinessData.Models.Changes.Output;
    using DH.Helpdesk.BusinessData.Models.Changes.Output.Settings.ChangeEdit;
    using DH.Helpdesk.Services.Response.Changes;
    using DH.Helpdesk.Web.Infrastructure.ModelFactories.Changes.ChangeEdit.Shared;
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

        public EvaluationModel Create(
            FindChangeResponse response, ChangeEditData editData, EvaluationEditSettings settings)
        {
            var textId = response.Change.Id.ToString(CultureInfo.InvariantCulture);
            var evaluation = response.Change.Evaluation;

            var changeEvaluation = this.configurableFieldModelFactory.CreateStringField(
                settings.ChangeEvaluation, evaluation.ChangeEvaluation);

            var attachedFiles = this.configurableFieldModelFactory.CreateAttachedFiles(
                settings.AttachedFiles, textId, Subtopic.Evaluation, response.Files);

            var logs = this.configurableFieldModelFactory.CreateLogs(
                settings.Logs, response.Change.Id, Subtopic.Evaluation, response.Logs);

            var sendToDialog = this.sendToDialogModelFactory.Create(
                editData.EmailGroups, editData.WorkingGroupsWithEmails, editData.Administrators);

            var evaluationReady = this.configurableFieldModelFactory.CreateBooleanField(
                settings.ChangeEvaluation, evaluation.EvaluationReady);

            return new EvaluationModel(
                response.Change.Id, changeEvaluation, attachedFiles, logs, sendToDialog, evaluationReady);
        }

        #endregion
    }
}