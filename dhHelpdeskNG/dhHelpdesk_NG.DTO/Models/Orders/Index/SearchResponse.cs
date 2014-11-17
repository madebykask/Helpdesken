namespace DH.Helpdesk.BusinessData.Models.Orders.Index
{
    using DH.Helpdesk.Common.ValidationAttributes;

    public sealed class SearchResponse
    {
        public SearchResponse(
                OrdersFieldSettingsOverview overviewSettings, 
                SearchResult searchResult)
        {
            this.SearchResult = searchResult;
            this.OverviewSettings = overviewSettings;
        }

        [NotNull]
        public OrdersFieldSettingsOverview OverviewSettings { get; private set; }

        [NotNull]
        public SearchResult SearchResult { get; private set; }
    }
}