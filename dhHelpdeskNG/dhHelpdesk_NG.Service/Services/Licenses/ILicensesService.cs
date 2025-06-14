﻿namespace DH.Helpdesk.Services.Services.Licenses
{
    using System.Collections.Generic;

    using DH.Helpdesk.BusinessData.Models.Licenses.Licenses;
    using DH.Helpdesk.Domain;

    public interface ILicensesService
    {
        LicenseOverview[] GetLicenses(int customerId);

        LicenseData GetLicenseData(int customerId, int? licenseId);

        LicenseModel GetById(int id);

        int AddOrUpdate(LicenseModel license);

        void Delete(int id);

        bool FileExists(int licenseId, string fileName);

        List<string> FindFileNamesExcludeSpecified(int licenseId, List<string> excludeFiles);

        string[] GetLicenseFileNames(int licenseId);

        List<Department> GetDepartmentsFor(int customerId, int? regionId);
    }
}