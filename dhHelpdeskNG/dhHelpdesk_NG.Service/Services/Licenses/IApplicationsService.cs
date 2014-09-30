namespace DH.Helpdesk.Services.Services.Licenses
{
    using DH.Helpdesk.BusinessData.Models.Licenses;

    public interface IApplicationsService
    {
        ApplicationOverview[] GetApplications(int customerId, bool onlyConnected);
    }
}