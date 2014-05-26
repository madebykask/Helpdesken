namespace DH.Helpdesk.Web.Infrastructure.ModelFactories.Reports.Concrete
{
    using System;
    using System.Collections.Generic;
    using System.Web.Mvc;

    using DH.Helpdesk.BusinessData.Models;
    using DH.Helpdesk.BusinessData.Models.Reports;
    using DH.Helpdesk.BusinessData.Models.Reports.Output;
    using DH.Helpdesk.BusinessData.Models.Shared;
    using DH.Helpdesk.Web.Infrastructure.Filters.Reports;
    using DH.Helpdesk.Web.Models.Reports;

    internal sealed class ReportsModelFactory : IReportsModelFactory
    {
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

        public RegistratedCasesCaseTypeModel CreateRegistratedCasesCaseTypeModel(
                                    RegistratedCasesCaseTypeResponse response,
                                    OperationContext context)
        {
            var workingGroups = CreateMultiSelectField(response.WorkingGroups);
            var caseTypes = CreateMultiSelectField(response.CaseTypes);
            var productAreas = response.ProductAreas;
            var instance = new RegistratedCasesCaseTypeModel(
                            workingGroups,
                            caseTypes,
                            productAreas,
                            context.CustomerId);
            var today = DateTime.Today;
            instance.PeriodFrom = today.AddYears(-1);
            instance.PeriodUntil = today;
            return instance;
        }

        public RegistratedCasesCaseTypeReportModel CreateRegistratedCasesCaseTypeReportModel(
                                    string objectId,
                                    string fileName,
                                    RegistratedCasesCaseTypeModel request,
                                    RegistratedCasesCaseTypeResponsePrint response)
        {
            var instance = new RegistratedCasesCaseTypeReportModel(                                    
                                    response.WorkingGroups,
                                    response.CaseTypes,
                                    response.ProductArea,
                                    request.PeriodFrom,
                                    request.PeriodUntil,
                                    objectId,
                                    fileName);
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