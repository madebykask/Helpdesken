namespace DH.Helpdesk.Web.Infrastructure.ModelFactories.Notifiers
{
    using System.Collections.Generic;

    using DH.Helpdesk.BusinessData.Enums.Notifiers;
    using DH.Helpdesk.BusinessData.Models.Common.Output;
    using DH.Helpdesk.BusinessData.Models.Notifiers.Output;
    using DH.Helpdesk.Web.Infrastructure.Filters.Notifiers;
    using DH.Helpdesk.Web.Models.Notifiers.Output;

    public interface INotifiersModelFactory
    {
        NotifiersModel Create(
            FieldSettingsDto displaySettings,
            List<ItemOverview> searchDomains,
            List<ItemOverview> searchRegions,
            List<ItemOverview> searchDepartments,
            List<ItemOverview> searchDivisions,
            NotifiersFilter predefinedFilters,
            NotifierStatus statusDefaultValue,
            int recordsOnPageDefaultValue,
            SearchResult searchResult);
    }
}