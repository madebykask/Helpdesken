namespace DH.Helpdesk.Services.BusinessLogic.Mappers.Licenses
{
    using System.Linq;

    using DH.Helpdesk.BusinessData.Models.Licenses.Products;
    using DH.Helpdesk.Domain;

    public static class ProductMapper
    {
        public static ProductOverview[] MapToOverviews(this IQueryable<Product> query)
        {
            var entities = query.Select(p => new 
                                                {
                                                    ProductId = p.Id,
                                                    ProductName = p.Name,
                                                    LicencesNumber = p.Licenses.Count(),
                                                    UsedLicencesNumber = p.Licenses.Where(l => l.PurshaseDate.HasValue).Count()
                                                }).ToArray();

            var overviews = entities.Select(p => new ProductOverview(
                                                    p.ProductId,
                                                    p.ProductName,
                                                    p.LicencesNumber,
                                                    p.UsedLicencesNumber)).ToArray();

            return overviews;
        }
    }
}