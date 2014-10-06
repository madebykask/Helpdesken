namespace DH.Helpdesk.Mobile.Models.Inventory
{
    using DH.Helpdesk.Mobile.Enums.Inventory;

    public class ReportFilter
    {
        public ReportFilter(int reportType)
        {
            this.ReportType = reportType;
        }

        public int ReportType { get; private set; }

        public static ReportFilter GetDefault()
        {
            return new ReportFilter((int)ReportTypes.OperatingSystem);
        }
    }
}