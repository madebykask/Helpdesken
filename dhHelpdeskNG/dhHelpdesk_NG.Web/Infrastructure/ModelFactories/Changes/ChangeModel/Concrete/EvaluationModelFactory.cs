namespace DH.Helpdesk.Web.Infrastructure.ModelFactories.Changes.ChangeModel.Concrete
{
    using DH.Helpdesk.BusinessData.Models.Changes.Output;
    using DH.Helpdesk.BusinessData.Models.Changes.Output.Change;
    using DH.Helpdesk.BusinessData.Models.Changes.Output.Settings.ChangeEdit;
    using DH.Helpdesk.Web.Models.Changes.ChangeEdit;

    public sealed class EvaluationModelFactory : IEvaluationModelFactory
    {
        //        private readonly ISendToDialogModelFactory sendToDialogModelFactory;
        //
        //        private readonly IConfigurableFieldModelFactory configurableFieldModelFactory;
        //
        //        public EvaluationModelFactory(
        //            ISendToDialogModelFactory sendToDialogModelFactory,
        //            IConfigurableFieldModelFactory configurableFieldModelFactory)
        //        {
        //            this.sendToDialogModelFactory = sendToDialogModelFactory;
        //            this.configurableFieldModelFactory = configurableFieldModelFactory;
        //        }
        //
        //        public EvaluationModel CreateEvaluation(
        //            string temporaryId,
        //            EvaluationFieldEditSettings editSettings,
        //            ChangeEditData editData)
        //        {
        //            var changeEvaluation = this.CreateChangeEvaluation(null, editSettings);
        //            var attachedFilesContainer = new AttachedFilesModel(temporaryId, Subtopic.Evaluation);
        //
        //            var sendToDialog = this.sendToDialogModelFactory.Create(
        //                editData.EmailGroups,
        //                editData.WorkingGroupsWithEmails,
        //                editData.Administrators);
        //
        //            var evaluationReady = this.CreateEvaluationReady(null, editSettings);
        //            
        //            return new EvaluationModel(
        //                temporaryId,
        //                changeEvaluation,
        //                attachedFilesContainer,
        //                sendToDialog,
        //                evaluationReady);
        //        }
        //
        //        public EvaluationModel CreateEvaluation(
        //            ChangeAggregate change,
        //            EvaluationFieldEditSettings editSettings,
        //            ChangeEditData editData)
        //        {
        //            var changeEvaluation = this.CreateChangeEvaluation(change, editSettings);
        //
        //            var attachedFilesContainer =
        //              new AttachedFilesModel(change.Id.ToString(CultureInfo.InvariantCulture), Subtopic.Evaluation);
        //
        //            var sendToDialog = this.sendToDialogModelFactory.Create(
        //                editData.EmailGroups,
        //                editData.WorkingGroupsWithEmails,
        //                editData.Administrators);
        //
        //            var evaluationReady = this.CreateEvaluationReady(change, editSettings);
        //
        //            return new EvaluationModel(
        //                change.Id.ToString(CultureInfo.InvariantCulture),
        //                changeEvaluation,
        //                attachedFilesContainer,
        //                sendToDialog,
        //                evaluationReady);
        //        }
        //
        //        private ConfigurableFieldModel<string> CreateChangeEvaluation(ChangeAggregate change, EvaluationFieldEditSettings editSettings)
        //        {
        //            var value = change != null ? change.Evaluation.ChangeEvaluation : null;
        //            return this.configurableFieldModelFactory.CreateStringField(editSettings.ChangeEvaluation, value);
        //        }
        //
        //        private ConfigurableFieldModel<bool> CreateEvaluationReady(ChangeAggregate change, EvaluationFieldEditSettings editSettings)
        //        {
        //            var value = change != null ? change.Evaluation.EvaluationReady : false;
        //            return this.configurableFieldModelFactory.CreateBooleanField(editSettings.ChangeEvaluation, value);
        //        } 

        #region Public Methods and Operators

        public EvaluationModel CreateEvaluation(
            string temporaryId,
            EvaluationFieldEditSettings editSettings,
            ChangeEditData editData)
        {
            throw new System.NotImplementedException();
        }

        public EvaluationModel CreateEvaluation(
            Change change,
            EvaluationFieldEditSettings editSettings,
            ChangeEditData editData)
        {
            throw new System.NotImplementedException();
        }

        #endregion
    }
}