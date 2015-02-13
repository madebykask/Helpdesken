namespace DH.Helpdesk.Web.Areas.Reports.Models.Options
{
    using System.Web.Mvc;

    using DH.Helpdesk.Common.ValidationAttributes;
    using DH.Helpdesk.Web.Infrastructure.LocalizedAttributes;
    using DH.Helpdesk.Web.Models;

    public sealed class ReportsOptions : ISearchModel<ReportsFilterModel>
    {
        public ReportsOptions(
            SelectList reports)
        {
            this.Reports = reports;
        }

        public ReportsOptions()
        {            
        }

        [NotNull]
        public SelectList Reports { get; private set; }

        [MinValue(0)]
        [LocalizedDisplay("Rapport")]
        public int ReportId { get; set; }

        public ReportsFilterModel ExtractFilters()
        {
            var filter = new ReportsFilterModel(this.ReportId);
            return filter;
        }
    }
}