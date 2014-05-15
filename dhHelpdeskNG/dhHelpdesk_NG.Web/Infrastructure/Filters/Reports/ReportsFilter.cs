namespace DH.Helpdesk.Web.Infrastructure.Filters.Reports
{
    using DH.Helpdesk.Common.ValidationAttributes;

    public sealed class ReportsFilter
    {
        public ReportsFilter(int reportId)
        {
            this.ReportId = reportId;
        }

        private ReportsFilter()
        {
        }

        [MinValue(0)]
        public int ReportId { get; private set; }

        public static ReportsFilter CreateDefault()
        {
            var instance = new ReportsFilter();
            return instance;
        }
    }
}