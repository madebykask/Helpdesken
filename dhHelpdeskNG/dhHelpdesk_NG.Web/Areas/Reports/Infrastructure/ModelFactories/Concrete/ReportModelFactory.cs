namespace DH.Helpdesk.Web.Areas.Reports.Infrastructure.ModelFactories.Concrete
{
    using System.Collections.Generic;

    using DH.Helpdesk.BusinessData.Models.Reports;
    using DH.Helpdesk.BusinessData.Models.Shared;
    using DH.Helpdesk.Web.Areas.Reports.Models.Options;
    using DH.Helpdesk.Web.Infrastructure;

    public sealed class ReportModelFactory : IReportModelFactory
    {
        public ReportsOptions GetReportsOptions()
        {
            var translations = new Dictionary<string, string>
                                   {
                                       { "Visa", Translation.Get("Visa") }
                                   };

            var reports = new List<ItemOverview>
                              {
                                  new ItemOverview(
                                      string.Format("{0} - {1}/{2}", Translation.Get("Rapport"), Translation.Get("Registrerade ärenden"), Translation.Get("dag")), ReportType.RegistratedCasesDay.ToString())
                              };

            return new ReportsOptions(translations, reports);
        }
    }
}