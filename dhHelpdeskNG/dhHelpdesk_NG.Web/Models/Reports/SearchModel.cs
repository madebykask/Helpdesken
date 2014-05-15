namespace DH.Helpdesk.Web.Models.Reports
{
    using System.Web.Mvc;

    using DH.Helpdesk.Common.ValidationAttributes;
    using DH.Helpdesk.Web.Infrastructure.Filters.Reports;
    using DH.Helpdesk.Web.Models.Shared;

    public sealed class SearchModel : ISearchModel<ReportsFilter>
    {
        public SearchModel(
            ConfigurableSearchFieldModel<SelectList> reports)
        {
            this.Reports = reports;
        }

        [NotNull]
        public ConfigurableSearchFieldModel<SelectList> Reports { get; private set; }

        [MinValue(0)]
        public int ReportId { get; set; }

        public ReportsFilter ExtractFilters()
        {
            var filter = new ReportsFilter(this.ReportId);
            return filter;
        }
    }
}