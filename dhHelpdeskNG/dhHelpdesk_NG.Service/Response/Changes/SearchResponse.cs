namespace DH.Helpdesk.Services.Response.Changes
{
    using DH.Helpdesk.BusinessData.Models.Changes.Output;
    using DH.Helpdesk.BusinessData.Models.Changes.Output.Settings.ChangeOverview;
    using DH.Helpdesk.Common.ValidationAttributes;

    public sealed class SearchResponse
    {
        #region Constructors and Destructors

        public SearchResponse(SearchResult result, ChangeOverviewSettings overviewSettings)
        {
            this.SearchResult = result;
            this.OverviewSettings = overviewSettings;
        }

        #endregion

        #region Public Properties

        [NotNull]
        public ChangeOverviewSettings OverviewSettings { get; private set; }

        [NotNull]
        public SearchResult SearchResult { get; private set; }

        #endregion
    }
}