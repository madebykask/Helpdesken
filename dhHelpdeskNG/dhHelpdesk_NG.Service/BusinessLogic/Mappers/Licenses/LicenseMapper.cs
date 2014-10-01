namespace DH.Helpdesk.Services.BusinessLogic.Mappers.Licenses
{
    using System.Linq;

    using DH.Helpdesk.BusinessData.Models.Licenses;
    using DH.Helpdesk.Domain;

    public static class LicenseMapper
    {
        public static LicenseOverview[] MapToOverviews(this IQueryable<License> query)
        {
            var overviews = query.Select(l => new LicenseOverview
                                                  {
                                                      LicenseId = l.Id,
                                                      ProductName = l.Product.Name,
                                                      LicensesNumber = l.NumberOfLicenses,
                                                      PurchaseDate = l.PurshaseDate,
                                                      Department = l.Department.DepartmentName
                                                  }).ToArray();

            return overviews;
        }
    }
}