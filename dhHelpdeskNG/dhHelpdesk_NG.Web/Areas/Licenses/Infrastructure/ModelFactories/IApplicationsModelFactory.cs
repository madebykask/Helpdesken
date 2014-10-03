namespace DH.Helpdesk.Web.Areas.Licenses.Infrastructure.ModelFactories
{
    using DH.Helpdesk.BusinessData.Models.Licenses.Applications;
    using DH.Helpdesk.Web.Areas.Licenses.Models.Applications;

    public interface IApplicationsModelFactory
    {
        ApplicationsIndexModel GetIndexModel(ApplicationsFilterModel filter);

        ApplicationsContentModel GetContentModel(ApplicationOverview[] applications);
    }
}