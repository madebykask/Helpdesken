namespace DH.Helpdesk.Web.Areas.Reports.Infrastructure.ModelFactories.Concrete
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;

    using DH.Helpdesk.BusinessData.Models.Reports;
    using DH.Helpdesk.BusinessData.Models.Reports.Options;
    using DH.Helpdesk.BusinessData.Models.Shared;
    using DH.Helpdesk.Web.Areas.Reports.Models.Options;
    using DH.Helpdesk.Web.Infrastructure;
    using DH.Helpdesk.Web.Infrastructure.Tools;

    public sealed class ReportModelFactory : IReportModelFactory
    {
        public ReportsOptions GetReportsOptions()
        {
            var reports = new List<ItemOverview>
                              {
                                  new ItemOverview(
                                      string.Format("{0} - {1}/{2}", Translation.Get("Rapport"), Translation.Get("Registrerade ärenden"), Translation.Get("dag")), 
                                      ((int)ReportType.RegistratedCasesDay).ToString(CultureInfo.InvariantCulture))
                              };

            return new ReportsOptions(WebMvcHelper.CreateListField(reports, null, false));
        }

        public RegistratedCasesDayOptionsModel GetRegistratedCasesDayOptionsModel(RegistratedCasesDayOptions options)
        {
            var departments = WebMvcHelper.CreateListField(options.Departments);
            var caseTypes = WebMvcHelper.CreateMultiSelectField(options.CaseTypes);
            var workingGroups = WebMvcHelper.CreateListField(options.WorkingGroups);
            var administrators = WebMvcHelper.CreateListField(options.Administrators);
            var period = DateTime.Today;

            return new RegistratedCasesDayOptionsModel(
                                        departments,
                                        caseTypes,
                                        workingGroups,
                                        administrators,
                                        period);
        }
    }
}