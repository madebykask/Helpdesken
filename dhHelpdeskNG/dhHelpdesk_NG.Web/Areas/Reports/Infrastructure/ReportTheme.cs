namespace DH.Helpdesk.Web.Areas.Reports.Infrastructure
{
    using System.Web.Helpers;

    public class ReportTheme
    {
        public ReportTheme(string theme)
        {
            this.Theme = theme;
        }

        public static ReportTheme DefaultTheme
        {
            get
            {
                return new ReportTheme(ChartTheme.Green);
            }
        }

        public string Theme { get; private set; }        
    }
}