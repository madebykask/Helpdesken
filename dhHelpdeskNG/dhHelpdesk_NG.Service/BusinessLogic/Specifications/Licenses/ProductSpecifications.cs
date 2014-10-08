namespace DH.Helpdesk.Services.BusinessLogic.Specifications.Licenses
{
    using System.Linq;

    using DH.Helpdesk.Domain;

    public static class ProductSpecifications
    {
        public static IQueryable<Product> GetRegionsProducts(this IQueryable<Product> query, int[] regions)
        {
            if (regions == null || !regions.Any())
            {
                return query;
            }

            query = query.Where(p => p.Licenses.Any(l => l.Region_Id.HasValue && regions.Contains(l.Region_Id.Value)));

            return query;
        }

        public static IQueryable<Product> GetDepartmentsProducts(this IQueryable<Product> query, int[] departments)
        {
            if (departments == null || !departments.Any())
            {
                return query;
            }

            query = query.Where(p => p.Licenses.Any(l => l.Department_Id.HasValue && departments.Contains(l.Department_Id.Value)));

            return query;
        }

        public static IQueryable<Product> GetApplicationProducts(this IQueryable<Product> query, int? applicationId)
        {
            if (!applicationId.HasValue)
            {
                return query;
            }

            query = query.Where(p => p.Applications.Select(a => a.Id).Contains(applicationId.Value));

            return query;
        } 
    }
}