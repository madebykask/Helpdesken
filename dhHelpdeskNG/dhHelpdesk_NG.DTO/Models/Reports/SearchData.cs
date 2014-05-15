namespace DH.Helpdesk.BusinessData.Models.Reports
{
    using DH.Helpdesk.BusinessData.Models.Reports.Output;
    using DH.Helpdesk.Common.ValidationAttributes;

    public sealed class SearchData
    {
        public SearchData(
            SearchOptions options, 
            SearchSettings settings)
        {
            this.Settings = settings;
            this.Options = options;
        }

        [NotNull]
        public SearchOptions Options { get; private set; }

        [NotNull]
        public SearchSettings Settings { get; private set; }
    }
}