namespace DH.Helpdesk.Services.Services.Licenses
{
    using System.Collections.Generic;

    using DH.Helpdesk.BusinessData.Models.Licenses.Licenses;

    public interface ILicensesService
    {
        LicenseOverview[] GetLicenses(int customerId);

        LicenseData GetLicenseData(int customerId, int? licenseId);

        LicenseModel GetById(int id);

        int AddOrUpdate(LicenseModel license);

        void Delete(int id);

        byte[] GetFileContent(int licenseId, string fileName);

        bool FileExists(int licenseId, string fileName);

        List<string> FindFileNamesExcludeSpecified(int licenseId, List<string> excludeFiles);
    }
}