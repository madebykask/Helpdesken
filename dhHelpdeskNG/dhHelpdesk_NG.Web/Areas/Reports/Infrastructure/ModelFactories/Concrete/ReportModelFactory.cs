namespace DH.Helpdesk.Web.Areas.Reports.Infrastructure.ModelFactories.Concrete
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;

    using DH.Helpdesk.BusinessData.Models.Reports;
    using DH.Helpdesk.BusinessData.Models.Reports.Data.CaseTypeArticleNo;
    using DH.Helpdesk.BusinessData.Models.Reports.Enums;
    using DH.Helpdesk.BusinessData.Models.Reports.Options;
    using DH.Helpdesk.BusinessData.Models.Shared;
    using DH.Helpdesk.Web.Areas.Reports.Models.Options;
    using DH.Helpdesk.Web.Areas.Reports.Models.Reports;
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
                                      ((int)ReportType.RegistratedCasesDay).ToString(CultureInfo.InvariantCulture)),
                                  new ItemOverview(
                                      string.Format("{0} - {1}", Translation.Get("Rapport"), Translation.Get("Case Type/Article No")),
                                      ((int)ReportType.CaseTypeArticleNo).ToString(CultureInfo.InvariantCulture))
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

        public CaseTypeArticleNoOptionsModel GetCaseTypeArticleNoOptionsModel(CaseTypeArticleNoOptions options)
        {
            var departments = WebMvcHelper.CreateMultiSelectField(options.Departments);
            var workingGroups = WebMvcHelper.CreateMultiSelectField(options.WorkingGroups);
            var caseTypes = WebMvcHelper.CreateMultiSelectField(options.CaseTypes);
            var productAreas = options.ProductAreas;
            var periodFrom = DateTime.Today.AddYears(-1);
            var periodUntil = DateTime.Today.AddMonths(-1);
            var showCases = WebMvcHelper.CreateListField(
                                                        new[]
                                                        {
                                                            new ItemOverview(Translation.Get("Alla ärenden"), ShowCases.AllCases.ToString()), 
                                                            new ItemOverview(Translation.Get("Pågående ärenden"), ShowCases.CasesInProgress.ToString())
                                                        }, 
                                                        null, 
                                                        false);

            return new CaseTypeArticleNoOptionsModel(
                                        departments,
                                        workingGroups,
                                        caseTypes,
                                        productAreas,
                                        periodFrom,
                                        periodUntil,
                                        showCases,
                                        false,
                                        true);
        }

        public CaseTypeArticleNoModel GetCaseTypeArticleNoModel(
            CaseTypeArticleNoData data,
            bool isShowCaseTypeDetails,
            bool isShowPercents)
        {
            return new CaseTypeArticleNoModel(data, isShowCaseTypeDetails, isShowPercents);
        }
    }
}