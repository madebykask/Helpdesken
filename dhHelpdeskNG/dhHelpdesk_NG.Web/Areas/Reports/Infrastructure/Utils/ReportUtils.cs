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
                case ReportType.LeadtimeFinishedCases:
                    return Translation.Get("Rapport - Ledtid (avslutade ärenden)");
                case ReportType.LeadtimeActiveCases:
                    return Translation.Get("Rapport - Ledtid (aktiva ärenden)");
                case ReportType.FinishingCauseCustomer:
                    return Translation.Get("Rapport - Avslutsorsak per avdelning");
                case ReportType.FinishingCauseCategoryCustomer:
                    return Translation.Get("Rapport - Avslutskategori per avdelning");
                case ReportType.ClosedCasesDay:
                    return Translation.Get("Rapport - Avslutade ärenden per dag");
                case ReportType.RegistratedCasesDay:
                    return string.Format(
                        "{0} - {1}/{2}",
                        Translation.Get("Rapport"),
                        Translation.Get("Registrerade ärenden"),
                        Translation.Get("dag"));
                case ReportType.RegistratedCasesHour:
                    return string.Format(
                        "{0} - {1}/{2}",
                        Translation.Get("Rapport"),
                        Translation.Get("Registrerade ärenden"),
                        Translation.Get("timme"));
                case ReportType.CasesInProgressDay:
                    return string.Format(
                        "{0} - {1}/{2}",
                        Translation.Get("Rapport"),
                        Translation.Get("Pågående ärenden"),
                        Translation.Get("dag"));
                case ReportType.ServiceReport:
                    return Translation.Get("Rapport - Servicerapport");
                case ReportType.CasesWorkingGroup:
                    return string.Format(
                        "{0} - {1}/{2}",
                        Translation.Get("Rapport"),
                        Translation.Get("Ärenden"),
                        Translation.Get("driftgrupp"));
                case ReportType.RegistratedCasesCaseType:
                    return string.Format(
                        "{0} - {1}/{2}",
                        Translation.Get("Rapport"),
                        Translation.Get("Registrerade ärenden"),
                        Translation.Get("ärendetyp"));
                case ReportType.QuestionRegistration:
                    return string.Format(
                        "{0} - {1}",
                        Translation.Get("Rapport"),
                        Translation.Get("Frågeregistrering"));
                case ReportType.CaseTypeArticleNo:
                    return string.Format(
                        "{0} - {1}/{2}",
                        Translation.Get("Rapport"),
                        Translation.Get("Ärendetyp"),
                        Translation.Get("Artikelnummer"));
                case ReportType.CaseTypeSupplier:
                    return string.Format(
                        "{0} - {1}/{2}",
                        Translation.Get("Rapport"),
                        Translation.Get("Ärendetyp"),
                        Translation.Get("leverantör"));
                case ReportType.AverageSolutionTime:
                    return string.Format(
                        "{0} - {1}",
                        Translation.Get("Rapport"),
                        Translation.Get("Genomsnittlig lösningstid"));
                case ReportType.RegistrationSource:
                    return string.Format(
                        "{0} - {1}",
                        Translation.Get("Rapport"),
                        Translation.Get("Källa registrering"));
                case ReportType.ResponseTime:
                    return string.Format(
                        "{0} - {1}",
                        Translation.Get("Rapport"),
                        Translation.Get("Svarstid"));
                case ReportType.ReportGenerator:
                    return string.Format(
                        "{0} - {1}",
                        Translation.Get("Rapport"),
                        Translation.Get("Rapportgenerator"));
				case ReportType.ReportGeneratorExtendedCase:
					return string.Format(
						"{0} - {1}",
						Translation.Get("Rapport"),
						Translation.Get("Rapportgenerator") + " - " + Translation.Get("Utökat ärende"));
				case ReportType.CaseSatisfaction:
                    return string.Format(
                        "{0} - {1}",
                        Translation.Get("Rapport"),
                        Translation.Get("Case satisfaction"));
                default:
                    return string.Empty;
            }
        }
    }
}