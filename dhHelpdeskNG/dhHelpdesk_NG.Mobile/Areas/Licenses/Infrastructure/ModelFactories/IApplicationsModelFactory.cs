namespace DH.Helpdesk.Mobile.Areas.Licenses.Infrastructure.ModelFactories
{
    using DH.Helpdesk.BusinessData.Models.Licenses;
    using DH.Helpdesk.BusinessData.Models.Licenses.Applications;
    using DH.Helpdesk.Mobile.Areas.Licenses.Models.Applications;

    public interface IApplicationsModelFactory
    {
        ApplicationsIndexModel GetIndexModel(ApplicationsFilterModel filter);

        ApplicationsContentModel GetContentModel(ApplicationOverview[] applications);
    }
}