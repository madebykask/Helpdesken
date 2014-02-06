namespace dhHelpdesk_NG.Web.Infrastructure.ModelFactories.Changes.ChangeModel.Concrete
{
    using System.Globalization;

    using dhHelpdesk_NG.DTO.DTOs.Changes.Output;
    using dhHelpdesk_NG.DTO.DTOs.Changes.Output.ChangeAggregate;
    using dhHelpdesk_NG.DTO.DTOs.Changes.Output.Settings.ChangeEdit;
    using dhHelpdesk_NG.DTO.Enums.Changes;
    using dhHelpdesk_NG.Web.Infrastructure.ModelFactories.Common;
    using dhHelpdesk_NG.Web.Models.Changes;
    using dhHelpdesk_NG.Web.Models.Changes.Edit;

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