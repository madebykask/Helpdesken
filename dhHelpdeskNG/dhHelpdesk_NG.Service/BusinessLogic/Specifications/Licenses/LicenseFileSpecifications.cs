namespace DH.Helpdesk.Services.BusinessLogic.Specifications.Licenses
{
    using System.Linq;

    using DH.Helpdesk.Domain;

    public static class LicenseFileSpecifications
    {
        public static IQueryable<LicenseFile> GetLicenseFiles(this IQueryable<LicenseFile> query, int licenseId)
        {
            query = query.Where(f => f.License_Id == licenseId);

            return query;
        } 
    }
}