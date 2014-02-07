namespace DH.Helpdesk.Web.Infrastructure.ModelFactories.Changes.ChangeModel
{
    using DH.Helpdesk.BusinessData.Models.Changes.Output;
    using DH.Helpdesk.BusinessData.Models.Changes.Output.ChangeAggregate;
    using DH.Helpdesk.BusinessData.Models.Changes.Output.Settings.ChangeEdit;
    using DH.Helpdesk.Web.Models.Changes.Edit;

    public interface IEvaluationModelFactory
    {
        EvaluationModel CreateEvaluation(
            string temporaryId,
            EvaluationFieldEditSettings editSettings,
            ChangeEditOptions optionalData);

        EvaluationModel CreateEvaluation(
            ChangeAggregate change,
            EvaluationFieldEditSettings editSettings,
            ChangeEditOptions optionalData);
    }
}