namespace DH.Helpdesk.Web.Infrastructure.ModelFactories.Reports.Concrete
{
    using System.Collections.Generic;
    using System.Web.Mvc;

    using DH.Helpdesk.BusinessData.Models.Reports;
    using DH.Helpdesk.BusinessData.Models.Shared;
    using DH.Helpdesk.BusinessData.Models.Shared.Output;
    using DH.Helpdesk.Web.Infrastructure.Filters.Reports;
    using DH.Helpdesk.Web.Models.Reports;
    using DH.Helpdesk.Web.Models.Shared;

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
                        searchData.Settings.Reports,
                        searchData.Options.Reports,
                        filter.ReportId);

            var instance = new SearchModel(reports);
            return instance;
        }

        private static ConfigurableSearchFieldModel<SelectList> CreateListField(
            FieldOverviewSetting setting,
            IEnumerable<ItemOverview> items,
            int selectedId)
        {
            var list = new SelectList(items, "Value", "Name", selectedId);
            return new ConfigurableSearchFieldModel<SelectList>(setting.Caption, list);
        }
    }
}