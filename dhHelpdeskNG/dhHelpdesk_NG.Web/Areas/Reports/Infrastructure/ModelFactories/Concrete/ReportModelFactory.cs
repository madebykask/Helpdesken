namespace DH.Helpdesk.Web.Areas.Reports.Infrastructure.ModelFactories.Concrete
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Web.Mvc;

    using DH.Helpdesk.BusinessData.Models;
    using DH.Helpdesk.BusinessData.Models.Reports.Data.CaseTypeArticleNo;
    using DH.Helpdesk.BusinessData.Models.Reports.Enums;
    using DH.Helpdesk.BusinessData.Models.Reports.Options;
    using DH.Helpdesk.BusinessData.Models.Shared;
    using DH.Helpdesk.Services.Services.Reports;
    using DH.Helpdesk.Web.Areas.Reports.Infrastructure.Utils;
    using DH.Helpdesk.Web.Areas.Reports.Models.Options;
    using DH.Helpdesk.Web.Areas.Reports.Models.Reports;
    using DH.Helpdesk.Web.Infrastructure;
    using DH.Helpdesk.Web.Infrastructure.Tools;

    public sealed class ReportModelFactory : IReportModelFactory
    {
        private readonly IReportService reportsService;

        private readonly IReportsBuilder reportsBuilder;

        public ReportModelFactory(
                IReportService reportsService, 
                IReportsBuilder reportsBuilder)
        {
            this.reportsService = reportsService;
            this.reportsBuilder = reportsBuilder;
        }

        public ReportsOptions GetReportsOptions()
        {
            var reports = new List<ItemOverview>
                              {
                                  new ItemOverview(
                                      ReportUtils.GetReportName(ReportType.RegistratedCasesDay), 
                                      ((int)ReportType.RegistratedCasesDay).ToString(CultureInfo.InvariantCulture)),
                                  new ItemOverview(
                                      ReportUtils.GetReportName(ReportType.CaseTypeArticleNo),
                                      ((int)ReportType.CaseTypeArticleNo).ToString(CultureInfo.InvariantCulture)),
                                  new ItemOverview(
                                      ReportUtils.GetReportName(ReportType.CaseSatisfaction),
                                      ((int)ReportType.CaseSatisfaction).ToString(CultureInfo.InvariantCulture))
                              };

            return new ReportsOptions(WebMvcHelper.CreateListField(reports.OrderBy(r => r.Name), null, false));
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

        public CaseSatisfactionOptions CreateCaseSatisfactionOptions(OperationContext context)
        {
            var response = this.reportsService.GetRegistratedCasesCaseTypeOptionsResponse(context);
            var workingGroups = CreateMultiSelectField(response.WorkingGroups);
            var caseTypes = CreateMultiSelectField(response.CaseTypes);
            var productAreas = response.ProductAreas;
            var today = DateTime.Today;
            var instance = new CaseSatisfactionOptions(caseTypes, workingGroups, productAreas, today.AddYears(-1), today, context.CustomerId);
            return instance;
        }

        public CaseSatisfactionReport CreateCaseSatisfactionReport(CaseSatisfactionOptions options, OperationContext context)
        {
            var response = this.reportsService.GetCaseSatisfactionResponse(
                context.CustomerId,
                options.PeriodFrom,
                options.PeriodUntil,
                options.CaseTypeIds.ToArray(),
                options.ProductAreaId,
                options.WorkingGroupIds.ToArray());
            ReportFile file;
            this.reportsBuilder.CreateCaseSatisfactionReport(response.GoodVotes, response.NormalVotes, response.BadVotes, response.Count, out file);
            var instance = new CaseSatisfactionReport(response.GoodVotes, response.NormalVotes, response.BadVotes, response.Count, file);
            return instance;
        }

        private static MultiSelectList CreateMultiSelectField(
            IEnumerable<ItemOverview> items)
        {
            return new MultiSelectList(items, "Value", "Name");
        }
    }
}