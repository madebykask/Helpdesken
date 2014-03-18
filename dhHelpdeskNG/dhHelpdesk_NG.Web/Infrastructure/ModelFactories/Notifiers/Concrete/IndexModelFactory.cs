namespace DH.Helpdesk.Web.Infrastructure.ModelFactories.Notifiers.Concrete
{
    using System.Collections.Generic;

    using DH.Helpdesk.BusinessData.Enums.Notifiers;
    using DH.Helpdesk.BusinessData.Models.Common.Output;
    using DH.Helpdesk.BusinessData.Models.Notifiers.Output;
    using DH.Helpdesk.Web.Infrastructure.Filters.Notifiers;
    using DH.Helpdesk.Web.Models.Notifiers.Output;

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
            List<ItemOverview> searchDivisions,
            NotifiersFilter predefinedFilters,
            NotifierStatus statusDefaultValue,
            int recordsOnPageDefaultValue,
            SearchResult searchResult)
        {
            var notifiersModel = this.notifiersModelFactory.Create(
                settings,
                searchDomains,
                searchRegions,
                searchDepartments,
                searchDivisions,
                predefinedFilters,
                statusDefaultValue,
                recordsOnPageDefaultValue,
                searchResult);

            return new IndexModel(notifiersModel);
        }
    }
}