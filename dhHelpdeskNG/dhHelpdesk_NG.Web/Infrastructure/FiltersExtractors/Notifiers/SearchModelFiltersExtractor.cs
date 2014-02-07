namespace DH.Helpdesk.Web.Infrastructure.FiltersExtractors.Notifiers
{
    using DH.Helpdesk.Web.Infrastructure.Filters.Notifiers;
    using DH.Helpdesk.Web.Models.Notifiers.Input;

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