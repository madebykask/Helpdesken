namespace DH.Helpdesk.Services.Services.Licenses
{
    using DH.Helpdesk.BusinessData.Models.Licenses.Applications;

    public interface IApplicationsService
    {
        ApplicationOverview[] GetApplications(int customerId, string name, bool onlyConnected);

        ApplicationData GetApplicationData(int customerId, int? applicationId);

        ApplicationModel GetById(int id);

        int AddOrUpdate(ApplicationModel application);

        void Delete(int id);
    }
}