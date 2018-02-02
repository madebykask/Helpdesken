namespace DH.Helpdesk.Web.Infrastructure.ModelFactories.Notifiers
{
    using System.Collections.Generic;

    using DH.Helpdesk.BusinessData.Models.Notifiers;
    using DH.Helpdesk.BusinessData.Models.Notifiers.Settings.SettingsEdit;
    using DH.Helpdesk.BusinessData.Models.Shared;
    using DH.Helpdesk.BusinessData.Models.Shared.Output;
    using DH.Helpdesk.Web.Infrastructure.Filters.Notifiers;
    using DH.Helpdesk.Web.Models.Notifiers;

    public interface INotifiersModelFactory
    {
        NotifiersModel Create(
            FieldSettings settings,
            List<ItemOverview> searchDomains,
            List<ItemOverview> searchRegions,
            List<ItemOverview> searchDepartments,
            List<ItemOverview> searchOrganizationUnit,
            List<ItemOverview> searchDivisions,
			List<ItemOverview> searchComputerUserCategory,
			NotifierFilters filters,
            SearchResult searchResult);

        NotifiersModel CreateEmpty();
    }
}