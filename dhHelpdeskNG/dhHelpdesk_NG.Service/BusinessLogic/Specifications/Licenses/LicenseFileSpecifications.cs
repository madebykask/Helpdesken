namespace DH.Helpdesk.Services.BusinessLogic.Specifications.Licenses
{
    using System.Collections.Generic;
    using System.Linq;

    using DH.Helpdesk.Domain;

    public static class LicenseFileSpecifications
    {
        public static IQueryable<LicenseFile> GetLicenseFiles(this IQueryable<LicenseFile> query, int licenseId)
        {
            query = query.Where(f => f.License_Id == licenseId);

            return query;
        }

        public static IQueryable<LicenseFile> GetLicenseFile(
                                this IQueryable<LicenseFile> query,
                                int licenseId,
                                string fileName)
        {
            query = query.Where(f => f.License_Id == licenseId && f.FileName == fileName);

            return query;
        }  
        
        public static IQueryable<LicenseFile> GetLicenseFileExclude(
                                this IQueryable<LicenseFile> query,
                                int licenseId,
                                List<string> excludeFiles)
        {
            query = query.Where(f => f.License_Id == licenseId && !excludeFiles.Contains(f.FileName));                    

            return query;
        } 
    }
}