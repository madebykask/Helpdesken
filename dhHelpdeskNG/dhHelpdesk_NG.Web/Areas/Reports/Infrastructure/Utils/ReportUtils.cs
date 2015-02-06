namespace DH.Helpdesk.Web.Areas.Reports.Infrastructure.Utils
{
    using DH.Helpdesk.BusinessData.Models.Reports.Enums;
    using DH.Helpdesk.Web.Infrastructure;

    public static class ReportUtils
    {
        public static string GetReportName(ReportType report)
        {
            switch (report)
            {
                case ReportType.RegistratedCasesDay:
                    return string.Format(
                        "{0} - {1}/{2}",
                        Translation.Get("Rapport"),
                        Translation.Get("Registrerade ärenden"),
                        Translation.Get("dag"));
                case ReportType.CaseTypeArticleNo:
                    return string.Format(
                        "{0} - {1}",
                        Translation.Get("Rapport"),
                        Translation.Get("Case Type/Article No"));
                case ReportType.CaseSatisfaction:
                    return string.Format(
                        "{0} - {1}",
                        Translation.Get("Rapport"),
                        Translation.Get("Kundnöjdhet - ärende"));
                default:
                    return string.Empty;
            }
        }
    }
}