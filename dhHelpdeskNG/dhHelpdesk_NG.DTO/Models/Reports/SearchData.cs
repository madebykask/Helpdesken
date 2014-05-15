namespace DH.Helpdesk.BusinessData.Models.Reports
{
    using DH.Helpdesk.BusinessData.Models.Changes.Output;
    using DH.Helpdesk.Common.ValidationAttributes;

    public sealed class SearchData
    {
        public SearchData(
            SearchOptions options)
        {
            this.Options = options;
        }

        private SearchData()
        {
        }

        [NotNull]
        public SearchOptions Options { get; private set; }
    }
}