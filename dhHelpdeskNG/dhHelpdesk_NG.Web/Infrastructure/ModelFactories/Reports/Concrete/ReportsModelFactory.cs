namespace DH.Helpdesk.Web.Infrastructure.ModelFactories.Reports.Concrete
{
    using System;
    using System.Collections.Generic;
    using System.Web.Mvc;

    using DH.Helpdesk.BusinessData.Models;
    using DH.Helpdesk.BusinessData.Models.Reports;
    using DH.Helpdesk.BusinessData.Models.Shared;
    using DH.Helpdesk.Services.Services;
    using DH.Helpdesk.Web.Infrastructure.Filters.Reports;
    using DH.Helpdesk.Web.Infrastructure.Tools;
    using DH.Helpdesk.Web.Infrastructure.Tools.Concrete;
    using DH.Helpdesk.Web.Models.Reports;

    internal sealed class ReportsModelFactory : IReportsModelFactory
    {
        private readonly IReportsHelper reportsHelper;

        private readonly IReportsService reportsService;

        public ReportsModelFactory(
                    IReportsHelper reportsHelper, 
                    IReportsService reportsService)
        {
            this.reportsHelper = reportsHelper;
            this.reportsService = reportsService;
        }

        public IndexModel CreateIndexModel()
        {
            var instance = new IndexModel();
            return instance;
        }

        public SearchModel CreateSearchModel(
            ReportsFilter filter, 
            SearchData searchData)
        {
            var reports = CreateListField(
                        searchData.Options.Reports,
                        filter.ReportId);

            var instance = new SearchModel(reports);
            return instance;
        }

        public RegistratedCasesCaseTypeOptions CreateRegistratedCasesCaseTypeOptions(OperationContext context)
        {
            var response = this.reportsService.GetRegistratedCasesCaseTypeOptionsResponse(context);
            var workingGroups = CreateMultiSelectField(response.WorkingGroups);
            var caseTypes = CreateMultiSelectField(response.CaseTypes);
            var productAreas = response.ProductAreas;
            var instance = new RegistratedCasesCaseTypeOptions(
                            workingGroups,
                            caseTypes,
                            productAreas,
                            context.CustomerId);
            var today = DateTime.Today;
            instance.PeriodFrom = today.AddYears(-1);
            instance.PeriodUntil = today;
            return instance;
        }

        public RegistratedCasesCaseTypeReport CreateRegistratedCasesCaseTypeReport(
                                    RegistratedCasesCaseTypeOptions options,
                                    OperationContext context)
        {
            var response = this.reportsService.GetRegistratedCasesCaseTypeReportResponse(
                                        context,
                                        options.WorkingGroupIds,
                                        options.CaseTypeIds,
                                        options.ProductAreaId);

            List<ReportFile> files;
            if (!this.reportsHelper.CreateRegistratedCasesCaseTypeReport(
                                        response.Customer,
                                        response.ReportType,
                                        response.WorkingGroups,
                                        response.CaseTypes,
                                        response.ProductArea,
                                        options.PeriodFrom,
                                        options.PeriodUntil,
                                        options.ShowDetails,
                                        options.IsPrint,
                                        out files))
            {
                return null;
            }

            var instance = new RegistratedCasesCaseTypeReport(
                                    response.Customer,
                                    response.ReportType,                                  
                                    response.WorkingGroups,
                                    response.CaseTypes,
                                    response.ProductArea,
                                    options.PeriodFrom,
                                    options.PeriodUntil,
                                    files);
            return instance;
        }

        private static SelectList CreateListField(
            IEnumerable<ItemOverview> items,
            int selectedId)
        {
            return new SelectList(items, "Value", "Name", selectedId);
        }

        private static MultiSelectList CreateMultiSelectField(
            IEnumerable<ItemOverview> items)
        {
            return new MultiSelectList(items, "Value", "Name");
        }
    }
}