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

            query = query.Where(p => p.Licenses.AsQueryable().HasRegions(regions));

            return query;
        }

        public static IQueryable<Product> GetDepartmentsProducts(this IQueryable<Product> query, int[] departments)
        {
            if (departments == null || !departments.Any())
            {
                return query;
            }

            query = query.Where(p => p.Licenses.AsQueryable().HasDepartments(departments));

            return query;
        } 
    }
}