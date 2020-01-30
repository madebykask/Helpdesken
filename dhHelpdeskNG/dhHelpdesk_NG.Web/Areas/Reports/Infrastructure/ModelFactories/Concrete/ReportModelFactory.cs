namespace DH.Helpdesk.Web.Areas.Reports.Infrastructure.ModelFactories.Concrete
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;

    using DH.Helpdesk.BusinessData.Models;
    using DH.Helpdesk.BusinessData.Models.Reports.Data.CaseTypeArticleNo;
    using DH.Helpdesk.BusinessData.Models.Reports.Data.FinishingCauseCategoryCustomer;
    using DH.Helpdesk.BusinessData.Models.Reports.Data.FinishingCauseCustomer;
    using DH.Helpdesk.BusinessData.Models.Reports.Data.LeadtimeActiveCases;
    using DH.Helpdesk.BusinessData.Models.Reports.Data.LeadtimeFinishedCases;
    using DH.Helpdesk.BusinessData.Models.Reports.Enums;
    using DH.Helpdesk.BusinessData.Models.Reports.Options;
    using DH.Helpdesk.BusinessData.Models.Shared;
    using DH.Helpdesk.BusinessData.OldComponents;
    using DH.Helpdesk.Services.Services.Reports;
    using DH.Helpdesk.Web.Areas.Reports.Infrastructure.Utils;
    using DH.Helpdesk.Web.Areas.Reports.Models.Options;
    using DH.Helpdesk.Web.Areas.Reports.Models.Reports;
    using DH.Helpdesk.Web.Infrastructure;
    using DH.Helpdesk.Web.Infrastructure.Tools;
    using DH.Helpdesk.Common.Enums;

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

        public ReportsOptions GetReportsOptions(List<ReportType> reports)
        {
            /* Hide unfinished report options Redmine #13433 */
            //var ready = new List<ReportType>
            //                {
            //                    ReportType.RegistratedCasesDay,
            //                    ReportType.CaseTypeArticleNo,
            //                    ReportType.ReportGenerator,
            //                    ReportType.LeadtimeFinishedCases,
            //                    ReportType.LeadtimeActiveCases,
            //                    ReportType.CaseSatisfaction,
            //                    ReportType.FinishingCauseCustomer,
            //                    ReportType.FinishingCauseCategoryCustomer,
            //                    ReportType.ClosedCasesDay,
            //                    ReportType.CasesInProgressDay
            //                };

            var ready = new List<ReportType>
                            {                                
                                ReportType.ReportGenerator  ,
								ReportType.ReportGeneratorExtendedCase                           
                            };
            // It's a new report, so we need to add it to the tblReport table
            //reports.Add(ReportType.CaseSatisfaction);

            var items = reports
                        .Where(ready.Contains)
                        .Select(r => 
                            new ItemOverview(ReportUtils.GetReportName(r), ((int)r).ToString(CultureInfo.InvariantCulture)))
                            .OrderBy(r => r.Name)
                            .ToList();

            return new ReportsOptions(WebMvcHelper.CreateListField(items, null, false));
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
            var workingGroups = WebMvcHelper.CreateMultiSelectField(response.WorkingGroups);
            var caseTypes = WebMvcHelper.CreateMultiSelectField(response.CaseTypes);
            var productAreas = response.ProductAreas;
            var today = DateTime.Today;
            var instance = new CaseSatisfactionOptions(workingGroups, caseTypes, productAreas, today.AddYears(-1), today, context.CustomerId);
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

        public LeadtimeFinishedCasesOptionsModel GetLeadtimeFinishedCasesOptionsModel(LeadtimeFinishedCasesOptions options)
        {
            var departments = WebMvcHelper.CreateMultiSelectField(options.Departments);
            var caseTypes = options.CaseTypes;
            var workingGroups = WebMvcHelper.CreateMultiSelectField(options.WorkingGroups);
            var registrationSources = WebMvcHelper.CreateListField(
                                                        new[]
                                                        {
                                                            new ItemOverview(string.Empty, ((int)CaseRegistrationSource.Empty).ToString(CultureInfo.InvariantCulture)), 
                                                            new ItemOverview(Translation.Get("Handläggare"), ((int)CaseRegistrationSource.Administrator).ToString(CultureInfo.InvariantCulture)), 
                                                            new ItemOverview(Translation.Get("Självservice"), ((int)CaseRegistrationSource.SelfService).ToString(CultureInfo.InvariantCulture)), 
                                                            new ItemOverview(Translation.Get("E-post"), ((int)CaseRegistrationSource.Email).ToString(CultureInfo.InvariantCulture))
                                                        },
                                                        null,
                                                        false);
            var periodFrom = DateTime.Today.AddYears(-1);
            var periodUntil = DateTime.Today;
            var lts = new List<ItemOverview>();
            for (var i = 1; i <= 10; i++)
            {
                lts.Add(new ItemOverview(i.ToString(CultureInfo.InvariantCulture), i.ToString(CultureInfo.InvariantCulture)));
            }

            var leadTimes = WebMvcHelper.CreateListField(lts, 5, false);

            return new LeadtimeFinishedCasesOptionsModel(
                                departments, 
                                caseTypes, 
                                null, 
                                workingGroups, 
                                registrationSources,
                                periodFrom,
                                periodUntil,
                                leadTimes,
                                false);
        }

        public LeadtimeFinishedCasesModel GetLeadtimeFinishedCasesModel(LeadtimeFinishedCasesData data, bool isShowDetails)
        {
            return new LeadtimeFinishedCasesModel(data, isShowDetails);
        }

        public LeadtimeActiveCasesOptionsModel GetLeadtimeActiveCasesOptionsModel(LeadtimeActiveCasesOptions options)
        {
            var departments = WebMvcHelper.CreateMultiSelectField(options.Departments);
            var caseTypes = options.CaseTypes;

            return new LeadtimeActiveCasesOptionsModel(
                                departments,
                                caseTypes,
                                null);
        }

        public LeadtimeActiveCasesModel GetLeadtimeActiveCasesModel(
                                        LeadtimeActiveCasesData data,
                                        int highHours,
                                        int mediumDays,
                                        int lowDays)
        {
            return new LeadtimeActiveCasesModel(data, highHours, mediumDays, lowDays);
        }

        public FinishingCauseCustomerOptionsModel GetFinishingCauseCustomerOptionsModel(FinishingCauseCustomerOptions options)
        {
            var departments = WebMvcHelper.CreateMultiSelectField(options.Departments);
            var workingGroups = WebMvcHelper.CreateListField(options.WorkingGroups);
            var caseTypes = options.CaseTypes;
            var administrators = WebMvcHelper.CreateListField(options.Administrators);
            var periodFrom = DateTime.Today.AddYears(-1);
            var periodUntil = DateTime.Today;

            return new FinishingCauseCustomerOptionsModel(
                        departments,
                        workingGroups,
                        caseTypes,
                        administrators,
                        periodFrom,
                        periodUntil);
        }

        public FinishingCauseCustomerModel GetFinishingCauseCustomerModel(FinishingCauseCustomerData data, int customerId)
        {
            return new FinishingCauseCustomerModel(data, customerId);
        }

        public FinishingCauseCategoryCustomerOptionsModel GetFinishingCauseCategoryCustomerOptionsModel(FinishingCauseCategoryCustomerOptions options)
        {
            var departments = WebMvcHelper.CreateMultiSelectField(options.Departments);
            var workingGroups = WebMvcHelper.CreateMultiSelectField(options.WorkingGroups);
            var caseTypes = options.CaseTypes;
            var periodFrom = DateTime.Today.AddYears(-1);
            var periodUntil = DateTime.Today;

            return new FinishingCauseCategoryCustomerOptionsModel(
                        departments,
                        workingGroups,
                        caseTypes,
                        periodFrom,
                        periodUntil);
        }

        public FinishingCauseCategoryCustomerModel GetFinishingCauseCategoryCustomerModel(FinishingCauseCategoryCustomerData data)
        {
            return new FinishingCauseCategoryCustomerModel(data);
        }

        public ClosedCasesDayOptionsModel GetClosedCasesDayOptionsModel(ClosedCasesDayOptions options)
        {
            var departments = WebMvcHelper.CreateMultiSelectField(options.Departments);
            var workingGroups = WebMvcHelper.CreateListField(options.WorkingGroups);
            var caseTypes = options.CaseTypes;
            var administrators = WebMvcHelper.CreateListField(options.Administrators);
            var period = DateTime.Today;

            return new ClosedCasesDayOptionsModel(
                        departments,
                        workingGroups,
                        caseTypes,
                        administrators,
                        period);
        }

        public CasesInProgressDayOptionsModel GetCasesInProgressDayOptionsModel(CasesInProgressDayOptions options)
        {
            var departments = WebMvcHelper.CreateListField(options.Departments);
            var workingGroups = WebMvcHelper.CreateListField(options.WorkingGroups);
            var administrators = WebMvcHelper.CreateListField(options.Administrators);
            var period = DateTime.Today;

            return new CasesInProgressDayOptionsModel(
                        departments,
                        workingGroups,
                        administrators,
                        period);
        }
    }
}