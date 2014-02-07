namespace DH.Helpdesk.Web.Infrastructure.ModelFactories.Changes
{
    using DH.Helpdesk.BusinessData.Models.Changes.Output;
    using DH.Helpdesk.BusinessData.Models.Changes.Output.Settings.ChangesOverview;
    using DH.Helpdesk.Web.Models.Changes;

    public interface IChangesGridModelFactory
    {
        ChangesGridModel Create(SearchResultDto searchResult, FieldOverviewSettings displaySettings);
    }
}