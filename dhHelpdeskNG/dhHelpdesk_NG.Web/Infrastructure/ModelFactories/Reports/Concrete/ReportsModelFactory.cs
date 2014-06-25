namespace DH.Helpdesk.Web.Infrastructure.ModelFactories.Reports.Concrete
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
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
                                        options.WorkingGroupIds.ToArray(),
                                        options.CaseTypeIds.ToArray(),
                                        options.ProductAreaId,
                                        options.PeriodFrom,
                                        options.PeriodUntil);

            List<ReportFile> files;
            this.reportsHelper.CreateRegistratedCasesCaseTypeReport(
                                        response.Customer,
                                        response.ReportType,
                                        response.WorkingGroups,
                                        response.CaseTypes,
                                        response.ProductArea,
                                        options.PeriodFrom,
                                        options.PeriodUntil,
                                        options.ShowDetails,
                                        options.IsPrint,
                                        response.Items.ToArray(),
                                        out files);

            var instance = new RegistratedCasesCaseTypeReport(
                                    response.Customer,
                                    response.ReportType,                                  
                                    options.WorkingGroupIds != null && options.WorkingGroupIds.Any() ? response.WorkingGroups : null,
                                    options.CaseTypeIds != null && options.CaseTypeIds.Any() ? response.CaseTypes : null,
                                    response.ProductArea,
                                    options.PeriodFrom,
                                    options.PeriodUntil,
                                    files);
            return instance;
        }

        public RegistratedCasesDayOptions CreateRegistratedCasesDayOptions(OperationContext context)
        {
            var response = this.reportsService.GetRegistratedCasesDayOptionsResponse(context);
            var departments = CreateListField(response.Departments, true);
            var caseTypes = CreateMultiSelectField(response.CaseTypes);
            var workingGroups = CreateListField(response.WorkingGroups, true);
            var administrators = CreateListField(response.Administrators, true);
            var instance = new RegistratedCasesDayOptions(
                            departments,
                            caseTypes,
                            workingGroups,
                            administrators);
            var today = DateTime.Today;
            instance.Period = today;
            return instance;
        }

        public RegistratedCasesDayReport CreateRegistratedCasesDayReport(RegistratedCasesDayOptions options, OperationContext context)
        {
            var response = this.reportsService.GetRegistratedCasesDayReportResponse(
                            context,
                            options.DepartmentId,
                            options.CaseTypeIds.ToArray(),
                            options.WorkingGroupId,
                            options.AdministratorId,
                            options.Period);

            ReportFile file;
            this.reportsHelper.CreateRegistratedCasesDayReport(
                                        response.Customer,
                                        response.ReportType,
                                        response.Department,
                                        response.CaseTypes,
                                        response.WorkingGroup,
                                        response.Administrator,
                                        options.Period,
                                        options.IsPrint,
                                        response.Items.ToArray(),
                                        out file);

            var instance = new RegistratedCasesDayReport(
                                    response.Customer,
                                    response.ReportType,
                                    response.Department,
                                    options.CaseTypeIds != null && options.CaseTypeIds.Any() ? response.CaseTypes : null,
                                    response.WorkingGroup,
                                    response.Administrator,
                                    options.Period,
                                    file,
                                    response.Items);
            return instance;
        }

        public AverageSolutionTimeOptions CreateAverageSolutionTimeOptions(OperationContext context)
        {
            var response = this.reportsService.GetAverageSolutionTimeOptionsResponse(context);
            var departments = CreateListField(response.Departments, true);
            var caseTypes = CreateMultiSelectField(response.CaseTypes);
            var workingGroups = CreateListField(response.WorkingGroups, true);
            var today = DateTime.Today;
            var instance = new AverageSolutionTimeOptions(
                            departments,
                            caseTypes,
                            workingGroups,
                            today,
                            today);
            return instance;
        }

        public AverageSolutionTimeReport CreateAverageSolutionTimeReport(AverageSolutionTimeOptions options, OperationContext context)
        {
            var response = this.reportsService.GetAverageSolutionTimeReportResponse(
                            context,
                            options.DepartmentId,
                            options.CaseTypeIds.ToArray(),
                            options.WorkingGroupId,
                            options.PeriodFrom,
                            options.PeriodUntil);

            ReportFile file;
            this.reportsHelper.CreateAverageSolutionTimeReport(
                                        response.Customer,
                                        response.ReportType,
                                        response.Department,
                                        response.CaseTypes,
                                        response.WorkingGroup,
                                        options.PeriodFrom,
                                        options.PeriodUntil,
                                        options.IsPrint,
                                        out file);

            var instance = new AverageSolutionTimeReport(
                                    response.Customer,
                                    response.ReportType,
                                    response.Department,
                                    options.CaseTypeIds != null && options.CaseTypeIds.Any() ? response.CaseTypes : null,
                                    response.WorkingGroup,
                                    options.PeriodFrom,
                                    options.PeriodUntil,
                                    file);
            return instance;
        }

        private static SelectList CreateListField(
            IEnumerable<ItemOverview> items,
            int selectedId)
        {
            return new SelectList(items, "Value", "Name", selectedId);
        }

        private static SelectList CreateListField(
            IEnumerable<ItemOverview> items,
            bool needEmpty = false)
        {
            var list = new List<ItemOverview>();
            if (needEmpty)
            {
               list.Add(ItemOverview.CreateEmpty());     
            }

            list.AddRange(items);

            return new SelectList(list, "Value", "Name");
        }

        private static MultiSelectList CreateMultiSelectField(
            IEnumerable<ItemOverview> items)
        {
            return new MultiSelectList(items, "Value", "Name");
        }
    }
}