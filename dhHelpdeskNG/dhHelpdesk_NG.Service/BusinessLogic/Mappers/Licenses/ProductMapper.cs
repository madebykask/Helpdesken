namespace DH.Helpdesk.Services.BusinessLogic.Mappers.Licenses
{
    using System.Linq;

    using DH.Helpdesk.BusinessData.Models.Licenses;
    using DH.Helpdesk.Domain;
    using DH.Helpdesk.Services.BusinessLogic.Specifications.Licenses;

    public static class ProductMapper
    {
        public static ProductOverview[] MapToOverviews(this IQueryable<Product> query)
        {
            var overviews = query.Select(p => new ProductOverview
                                                  {
                                                      ProductId = p.Id,
                                                      ProductName = p.Name/*,
                                                      LicencesNumber = p.Licenses.Count(),
                                                      UsedLicencesNumber = p.Licenses.AsQueryable().GetUsedLicenses().Count()*/
                                                  }).ToArray();

            return overviews;
        }
    }
}