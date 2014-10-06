namespace DH.Helpdesk.Web.Areas.Licenses.Infrastructure.ModelFactories.Concrete
{
    using DH.Helpdesk.BusinessData.Models.Licenses.Applications;
    using DH.Helpdesk.Web.Areas.Licenses.Models.Applications;

    public sealed class ApplicationsModelFactory : IApplicationsModelFactory
    {
        public ApplicationsIndexModel GetIndexModel(ApplicationsFilterModel filter)
        {
            return new ApplicationsIndexModel(filter.OnlyConnected);
        }

        public ApplicationsContentModel GetContentModel(ApplicationOverview[] applications)
        {
            return new ApplicationsContentModel(applications);
        }

        public ApplicationEditModel GetEditModel(ApplicationData data)
        {
            return new ApplicationEditModel(data);
        }
    }
}