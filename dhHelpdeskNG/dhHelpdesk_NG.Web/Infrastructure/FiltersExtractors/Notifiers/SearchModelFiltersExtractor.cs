namespace dhHelpdesk_NG.Web.Infrastructure.FiltersExtractors.Notifiers
{
    using System.Collections.Generic;

    using dhHelpdesk_NG.Web.Infrastructure.Session;
    using dhHelpdesk_NG.Web.Models.Notifiers.Input;

    public static class SearchModelFiltersExtractor
    {
        public static PageFilters Extract(SearchInputModel model)
        {
            var filters = new List<PageFilter>();

            if (model.DomainId.HasValue)
            {
                var domainFilter = new PageFilter(FilterName.Domain, model.DomainId);
                filters.Add(domainFilter);
            }

            if (model.RegionId.HasValue)
            {
                var regionFilter = new PageFilter(FilterName.Region, model.RegionId);
                filters.Add(regionFilter);
            }

            if (model.DepartmentId.HasValue)
            {
                var departmentFilter = new PageFilter(FilterName.Department, model.DepartmentId);
                filters.Add(departmentFilter);
            }

            if (model.DivisionId.HasValue)
            {
                var divisionFilter = new PageFilter(FilterName.Division, model.DivisionId);
                filters.Add(divisionFilter);
            }

            if (!string.IsNullOrEmpty(model.Pharse))
            {
                var pharseFilter = new PageFilter(FilterName.Pharse, model.Pharse);
                filters.Add(pharseFilter);
            }

            var showFilter = new PageFilter(FilterName.Show, model.Show);
            filters.Add(showFilter);

            var recordsOnPageFilter = new PageFilter(FilterName.RecordsOnPage, model.RecordsOnPage);
            filters.Add(recordsOnPageFilter);

            return new PageFilters(Enums.PageName.Notifiers, filters);
        }
    }
}