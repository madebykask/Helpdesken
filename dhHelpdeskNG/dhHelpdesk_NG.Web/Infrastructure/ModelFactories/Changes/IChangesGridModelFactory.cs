namespace dhHelpdesk_NG.Web.Infrastructure.ModelFactories.Changes
{
    using dhHelpdesk_NG.DTO.DTOs.Changes.Output;
    using dhHelpdesk_NG.DTO.DTOs.Changes.Output.Settings;
    using dhHelpdesk_NG.DTO.DTOs.Changes.Output.Settings.ChangesOverview;
    using dhHelpdesk_NG.Web.Models.Changes;

    public interface IChangesGridModelFactory
    {
        ChangesGridModel Create(SearchResultDto searchResult, FieldOverviewSettings displaySettings);
    }
}