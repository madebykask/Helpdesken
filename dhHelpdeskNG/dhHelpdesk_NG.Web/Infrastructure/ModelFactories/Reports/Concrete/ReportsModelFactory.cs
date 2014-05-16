namespace DH.Helpdesk.Web.Infrastructure.ModelFactories.Reports.Concrete
{
    using System.Collections.Generic;
    using System.Web.Mvc;

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

        public RegistratedCasesCaseTypeModel CreateRegistratedCasesCaseTypeModel(GetRegistratedCasesCaseTypeResponse response)
        {
            var workingGroups = CreateMultiSelectField(response.WorkingGroups);
            var caseTypes = CreateMultiSelectField(response.CaseTypes);
            var productAreas = response.ProductAreas;
            var instance = new RegistratedCasesCaseTypeModel(
                            workingGroups,
                            caseTypes,
                            productAreas);
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