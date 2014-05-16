namespace DH.Helpdesk.BusinessData.Models.Reports
{
    using DH.Helpdesk.BusinessData.Models.Reports.Output;
    using DH.Helpdesk.Common.ValidationAttributes;

    public sealed class SearchData
    {
        public SearchData(SearchOptions options)
        {
            this.Options = options;
        }

        [NotNull]
        public SearchOptions Options { get; private set; }
    }
}