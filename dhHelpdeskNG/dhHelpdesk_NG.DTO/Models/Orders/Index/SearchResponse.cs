namespace DH.Helpdesk.BusinessData.Models.Orders.Index
{
    using DH.Helpdesk.BusinessData.Models.Orders.Index.FieldSettingsOverview;
    using DH.Helpdesk.Common.ValidationAttributes;

    public sealed class SearchResponse
    {
        public SearchResponse(
                FullFieldSettingsOverview overviewSettings, 
                SearchResult searchResult)
        {
            this.SearchResult = searchResult;
            this.OverviewSettings = overviewSettings;
        }

        [NotNull]
        public FullFieldSettingsOverview OverviewSettings { get; private set; }

        [NotNull]
        public SearchResult SearchResult { get; private set; }
    }
}