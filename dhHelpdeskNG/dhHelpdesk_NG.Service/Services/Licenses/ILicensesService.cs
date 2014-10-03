namespace DH.Helpdesk.Services.Services.Licenses
{
    using DH.Helpdesk.BusinessData.Models.Licenses.Licenses;

    public interface ILicensesService
    {
        LicenseOverview[] GetLicenses(int customerId);
    }
}