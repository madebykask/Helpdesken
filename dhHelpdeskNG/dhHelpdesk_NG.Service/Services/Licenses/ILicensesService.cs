namespace DH.Helpdesk.Services.Services.Licenses
{
    using DH.Helpdesk.BusinessData.Models.Licenses.Licenses;

    public interface ILicensesService
    {
        LicenseOverview[] GetLicenses(int customerId);

        LicenseData GetLicenseData(int? licenseId);

        LicenseModel GetById(int id);

        int AddOrUpdate(LicenseModel license);

        void Delete(int id);
    }
}