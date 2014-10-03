namespace DH.Helpdesk.Services.BusinessLogic.Mappers.Licenses
{
    using System.Linq;

    using DH.Helpdesk.BusinessData.Models.Licenses.Licenses;
    using DH.Helpdesk.Domain;

    public static class LicenseMapper
    {
        public static LicenseOverview[] MapToOverviews(this IQueryable<License> query)
        {
            var entities = query.Select(l => new 
                                                {
                                                    LicenseId = l.Id,
                                                    ProductName = l.Product.Name,
                                                    LicensesNumber = l.NumberOfLicenses,
                                                    PurchaseDate = l.PurshaseDate,
                                                    Department = l.Department.DepartmentName
                                                }).ToArray();

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