namespace dhHelpdesk_NG.Web.Infrastructure.ModelFactories.Changes.ChangeModel
{
    using dhHelpdesk_NG.DTO.DTOs.Changes.Output;
    using dhHelpdesk_NG.DTO.DTOs.Changes.Output.ChangeAggregate;
    using dhHelpdesk_NG.DTO.DTOs.Changes.Output.Settings.ChangeEdit;
    using dhHelpdesk_NG.Web.Models.Changes.Edit;

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