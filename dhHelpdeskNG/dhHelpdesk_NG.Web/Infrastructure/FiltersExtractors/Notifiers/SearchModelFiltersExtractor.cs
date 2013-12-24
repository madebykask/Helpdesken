namespace dhHelpdesk_NG.Web.Infrastructure.FiltersExtractors.Notifiers
{
    using dhHelpdesk_NG.Web.Infrastructure.Filters.Notifiers;
    using dhHelpdesk_NG.Web.Models.Notifiers.Input;

    public static class SearchModelFiltersExtractor
    {
        public static NotifierFilters Extract(SearchInputModel model)
        {
            return new NotifierFilters
                       {
                           DomainId = model.DomainId,
                           RegionId = model.RegionId,
                           DepartmentId = model.DepartmentId,
                           DivisionId = model.DivisionId,
                           Pharse = model.Pharse,
                           Show = model.Show,
                           RecordsOnPage = model.RecordsOnPage
                       };
        }
    }
}