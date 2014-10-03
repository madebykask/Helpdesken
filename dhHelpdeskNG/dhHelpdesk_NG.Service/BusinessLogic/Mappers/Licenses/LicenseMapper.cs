namespace DH.Helpdesk.Services.BusinessLogic.Mappers.Licenses
{
    using System.Linq;

    using DH.Helpdesk.BusinessData.Models.Licenses.Licenses;
    using DH.Helpdesk.Domain;

    public static class LicenseMapper
    {
        public static LicenseOverview[] MapToOverviews(this IQueryable<License> query, IQueryable<Department> departments)
        {
            var entities = query.GroupJoin(
                                departments, 
                                l => l.Department_Id,
                                d => d.Id,
                                (l, d) => new { License = l, Departments = d.DefaultIfEmpty() })
                                .SelectMany(a => a.Departments.Select(b => new 
                                    {
                                        LicenseId = a.License.Id,
                                        ProductName = a.License.Product.Name,
                                        LicensesNumber = a.License.NumberOfLicenses,
                                        PurchaseDate = a.License.PurshaseDate,
                                        Department = a.License.Department != null ? a.License.Department.DepartmentName : null
                                    })).ToArray();

            var overviews = entities.Select(l => new LicenseOverview(
                                                    l.LicenseId,
                                                    l.ProductName,
                                                    l.LicensesNumber,
                                                    l.PurchaseDate,
                                                    l.Department)).ToArray();

            return overviews;
        }
    }
}