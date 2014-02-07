namespace DH.Helpdesk.Web.Infrastructure.ModelFactories.Changes.ChangeModel.Concrete
{
    using System.Globalization;

    using DH.Helpdesk.BusinessData.Enums.Changes;
    using DH.Helpdesk.BusinessData.Models.Changes.Output;
    using DH.Helpdesk.BusinessData.Models.Changes.Output.ChangeAggregate;
    using DH.Helpdesk.BusinessData.Models.Changes.Output.Settings.ChangeEdit;
    using DH.Helpdesk.Web.Infrastructure.ModelFactories.Common;
    using DH.Helpdesk.Web.Models.Changes;
    using DH.Helpdesk.Web.Models.Changes.Edit;

    public sealed class EvaluationModelFactory : IEvaluationModelFactory
    {
        private readonly ISendToDialogModelFactory sendToDialogModelFactory;

        private readonly IConfigurableFieldModelFactory configurableFieldModelFactory;

        public EvaluationModelFactory(
            ISendToDialogModelFactory sendToDialogModelFactory,
            IConfigurableFieldModelFactory configurableFieldModelFactory)
        {
            this.sendToDialogModelFactory = sendToDialogModelFactory;
            this.configurableFieldModelFactory = configurableFieldModelFactory;
        }

        public EvaluationModel CreateEvaluation(
            string temporaryId,
            EvaluationFieldEditSettings editSettings,
            ChangeEditOptions optionalData)
        {
            var changeEvaluation = this.CreateChangeEvaluation(null, editSettings);
            var attachedFilesContainer = new AttachedFilesContainerModel(temporaryId, Subtopic.Evaluation);

            var sendToDialog = this.sendToDialogModelFactory.Create(
                optionalData.EmailGroups,
                optionalData.WorkingGroups,
                optionalData.Administrators);

            var evaluationReady = this.CreateEvaluationReady(null, editSettings);
            
            return new EvaluationModel(
                temporaryId,
                changeEvaluation,
                attachedFilesContainer,
                sendToDialog,
                evaluationReady);
        }

        public EvaluationModel CreateEvaluation(
            ChangeAggregate change,
            EvaluationFieldEditSettings editSettings,
            ChangeEditOptions optionalData)
        {
            var changeEvaluation = this.CreateChangeEvaluation(change, editSettings);

            var attachedFilesContainer =
              new AttachedFilesContainerModel(change.Id.ToString(CultureInfo.InvariantCulture), Subtopic.Evaluation);

            var sendToDialog = this.sendToDialogModelFactory.Create(
                optionalData.EmailGroups,
                optionalData.WorkingGroups,
                optionalData.Administrators);

            var evaluationReady = this.CreateEvaluationReady(change, editSettings);

            return new EvaluationModel(
                change.Id.ToString(CultureInfo.InvariantCulture),
                changeEvaluation,
                attachedFilesContainer,
                sendToDialog,
                evaluationReady);
        }

        private ConfigurableFieldModel<string> CreateChangeEvaluation(ChangeAggregate change, EvaluationFieldEditSettings editSettings)
        {
            var value = change != null ? change.Evaluation.ChangeEvaluation : null;
            return this.configurableFieldModelFactory.CreateStringField(editSettings.Evaluation, value);
        }

        private ConfigurableFieldModel<bool> CreateEvaluationReady(ChangeAggregate change, EvaluationFieldEditSettings editSettings)
        {
            var value = change != null ? change.Evaluation.EvaluationReady : false;
            return this.configurableFieldModelFactory.CreateBooleanField(editSettings.Evaluation, value);
        } 
    }
}