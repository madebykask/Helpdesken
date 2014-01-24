namespace dhHelpdesk_NG.Web.Infrastructure.ModelFactories.Changes
{
    using dhHelpdesk_NG.DTO.DTOs.Changes.Output;
    using dhHelpdesk_NG.DTO.DTOs.Changes.Output.Settings;
    using dhHelpdesk_NG.Web.Models.Changes;

    public interface IChangesGridModelFactory
    {
        ChangesGridModel Create(SearchResultDto searchResult, FieldOverviewSettingsDto fieldSettings);
    }
}