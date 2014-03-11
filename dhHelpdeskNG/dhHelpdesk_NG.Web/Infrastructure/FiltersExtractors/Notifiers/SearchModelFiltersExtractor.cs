namespace DH.Helpdesk.Web.Infrastructure.FiltersExtractors.Notifiers
{
    using DH.Helpdesk.Web.Infrastructure.Filters.Notifiers;
    using DH.Helpdesk.Web.Models.Notifiers.Input;

    public static class SearchModelFiltersExtractor
    {
        public static NotifiersFilter Extract(SearchInputModel model)
        {
            return new NotifiersFilter
                       {
                           DomainId = model.DomainId,
                           RegionId = model.RegionId,
                           DepartmentId = model.DepartmentId,
                           DivisionId = model.DivisionId,
                           Pharse = model.Pharse,
                           Status = model.Status,
                           RecordsOnPage = model.RecordsOnPage
                       };
        }
    }
}