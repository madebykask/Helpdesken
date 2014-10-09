namespace DH.Helpdesk.Services.BusinessLogic.Specifications.Licenses
{
    using System.Linq;

    using DH.Helpdesk.Domain;

    public static class LicenseSpecifications
    {
        public static IQueryable<License> GetUsedLicenses(this IQueryable<License> query)
        {
            query = query.Where(l => l.PurshaseDate.HasValue);

            return query;
        }

        public static IQueryable<License> GetCustomerLicenses(this IQueryable<License> query, int customerId)
        {
            query = query.Where(l => l.Product.Customer_Id == customerId);

            return query;
        }

        public static IQueryable<License> GetUpgradeLicenses(this IQueryable<License> query, int? licenseId)
        {

            if (!licenseId.HasValue)
            {
                return query;
            }

            query = query.Where(l => l.Id != licenseId);

            return query;
        } 
    }
}