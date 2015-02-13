namespace DH.Helpdesk.Web.Areas.Reports.Models.Options
{
    using DH.Helpdesk.Common.ValidationAttributes;

    public sealed class ReportsFilterModel
    {
        public ReportsFilterModel(int reportId)
        {
            this.ReportId = reportId;
        }

        private ReportsFilterModel()
        {
        }

        [MinValue(0)]
        public int ReportId { get; private set; }

        public static ReportsFilterModel CreateDefault()
        {
            var instance = new ReportsFilterModel();
            return instance;
        }
    }
}