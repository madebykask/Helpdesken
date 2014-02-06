namespace dhHelpdesk_NG.Web.Infrastructure.ModelFactories.Changes.ChangeModel
{
    using dhHelpdesk_NG.DTO.DTOs.Changes.Output;
    using dhHelpdesk_NG.DTO.DTOs.Changes.Output.ChangeAggregate;
    using dhHelpdesk_NG.DTO.DTOs.Changes.Output.Settings.ChangeEdit;
    using dhHelpdesk_NG.Web.Models.Changes.Edit;

    public interface IAnalyzeModelFactory
    {
        AnalyzeModel Create(string temporaryId, AnalyzeFieldEditSettings editSettings, ChangeEditOptions optionalData);

        AnalyzeModel Create(ChangeAggregate change, AnalyzeFieldEditSettings editSettings, ChangeEditOptions optionalData);
    }
}