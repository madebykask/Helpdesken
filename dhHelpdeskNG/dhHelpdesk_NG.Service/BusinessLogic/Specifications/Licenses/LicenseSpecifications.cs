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

        public static bool HasRegions(this IQueryable<License> query, int[] regions)
        {
            if (regions == null || !regions.Any())
            {
                return true;
            }

            return query.Any(l => regions.Contains(l.Region_Id));
        }

        public static bool HasDepartments(this IQueryable<License> query, int[] departments)
        {
            if (departments == null || !departments.Any())
            {
                return true;
            }

            return query.Any(l => departments.Contains(l.Department_Id));
        }

        public static IQueryable<License> GetCustomerLicenses(this IQueryable<License> query, int customerId)
        {
            query = query.Where(l => l.Product.Customer_Id == customerId);

            return query;
        } 
    }
}