namespace DH.Helpdesk.Services.Services.Licenses
{
    using DH.Helpdesk.BusinessData.Models.Licenses.Applications;

    public interface IApplicationsService
    {
        ApplicationOverview[] GetApplications(int customerId, bool onlyConnected);

        ApplicationData GetApplicationData(int? applicationId);

        ApplicationModel GetById(int id);

        int AddOrUpdate(ApplicationModel application);

        void Delete(int id);
    }
}