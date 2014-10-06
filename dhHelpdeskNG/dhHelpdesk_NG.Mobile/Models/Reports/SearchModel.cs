namespace DH.Helpdesk.Mobile.Models.Reports
{
    using System.Web.Mvc;

    using DH.Helpdesk.Common.ValidationAttributes;
    using DH.Helpdesk.Mobile.Infrastructure.Filters.Reports;

    public sealed class SearchModel : ISearchModel<ReportsFilter>
    {
        public SearchModel(SelectList reports)
        {
            this.Reports = reports;
        }

        public SearchModel()
        {
        }

        [NotNull]
        public SelectList Reports { get; private set; }

        [MinValue(0)]
        public int ReportId { get; set; }

        public ReportsFilter ExtractFilters()
        {
            var filter = new ReportsFilter(this.ReportId);
            return filter;
        }
    }
}