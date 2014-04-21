namespace DH.Helpdesk.BusinessData.Models.Changes
{
    using DH.Helpdesk.BusinessData.Models.Changes.Output;
    using DH.Helpdesk.BusinessData.Models.Changes.Output.Settings.ChangeOverview;
    using DH.Helpdesk.Common.ValidationAttributes;

    public sealed class GetSearchDataResponse
    {
        public GetSearchDataResponse(SearchSettings overviewSettings, SearchOptions searchOptions)
        {
            this.OverviewSettings = overviewSettings;
            this.SearchOptions = searchOptions;
        }

        [NotNull]
        public SearchSettings OverviewSettings { get; private set; }
        
        [NotNull]
        public SearchOptions SearchOptions { get; private set; }
    }
}
