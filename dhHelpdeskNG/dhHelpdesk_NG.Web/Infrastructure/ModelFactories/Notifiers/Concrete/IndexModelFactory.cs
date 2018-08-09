namespace DH.Helpdesk.Web.Infrastructure.ModelFactories.Notifiers.Concrete
{
    using System.Collections.Generic;

    using DH.Helpdesk.BusinessData.Models.Notifiers;
    using DH.Helpdesk.BusinessData.Models.Notifiers.Settings.SettingsEdit;
    using DH.Helpdesk.BusinessData.Models.Shared;
    using DH.Helpdesk.BusinessData.Models.Shared.Output;
    using DH.Helpdesk.Web.Infrastructure.Filters.Notifiers;
    using DH.Helpdesk.Web.Models.Notifiers;

    public sealed class IndexModelFactory : IIndexModelFactory
    {
        private readonly INotifiersModelFactory notifiersModelFactory;

        public IndexModelFactory(INotifiersModelFactory notifiersModelFactory)
        {
            this.notifiersModelFactory = notifiersModelFactory;
        }

        public IndexModel Create(
            FieldSettings settings,
            List<ItemOverview> searchDomains,
            List<ItemOverview> searchRegions,
            List<ItemOverview> searchDepartments,
            List<ItemOverview> searchOrganizationUnit,
            List<ItemOverview> searchDivisions,
			List<ItemOverview> searchComputerUserCategories,
			NotifierFilters filters,
            SearchResult searchResult)
        {
            var notifiersModel = this.notifiersModelFactory.Create(
                settings,
                searchDomains,
                searchRegions,
                searchDepartments,
                searchOrganizationUnit,
                searchDivisions,
				searchComputerUserCategories,
				filters,
                searchResult);

            return new IndexModel(notifiersModel, false);
        }

        public IndexModel CreateEmpty()
        {
            var empty = new IndexModel(this.notifiersModelFactory.CreateEmpty(), true);
            return empty;
        }
    }
}