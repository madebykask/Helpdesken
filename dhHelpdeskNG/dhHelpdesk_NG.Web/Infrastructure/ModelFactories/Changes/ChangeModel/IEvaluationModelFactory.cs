namespace DH.Helpdesk.Web.Infrastructure.ModelFactories.Changes.ChangeModel
{
    using DH.Helpdesk.BusinessData.Models.Changes.Output;
    using DH.Helpdesk.BusinessData.Models.Changes.Output.Change;
    using DH.Helpdesk.BusinessData.Models.Changes.Output.Settings.ChangeEdit;
    using DH.Helpdesk.Web.Models.Changes.ChangeEdit;

    public interface IEvaluationModelFactory
    {
        #region Public Methods and Operators

        EvaluationModel CreateEvaluation(
            string temporaryId,
            EvaluationFieldEditSettings editSettings,
            ChangeEditData editData);

        EvaluationModel CreateEvaluation(
            Change change,
            EvaluationFieldEditSettings editSettings,
            ChangeEditData editData);

        #endregion
    }
}