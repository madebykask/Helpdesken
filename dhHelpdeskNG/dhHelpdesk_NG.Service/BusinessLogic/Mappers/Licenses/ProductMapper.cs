namespace DH.Helpdesk.Services.BusinessLogic.Mappers.Licenses
{
    using System.Linq;

    using DH.Helpdesk.BusinessData.Models.Licenses.Products;
    using DH.Helpdesk.Domain;
    using DH.Helpdesk.Services.BusinessLogic.Specifications;

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

        public static ProductModel MapToBusinessModel(this IQueryable<Product> query, int id)
        {
            ProductModel model = null;

            var entity = query.GetById(id) 
                        .Select(p => new
                            {
                                p.Id,
                                p.Name,
                                p.Manufacturer_Id,
                                p.Customer_Id,
                                p.CreatedDate,
                                p.ChangedDate
                            })
                            .SingleOrDefault();

            if (entity != null)
            {
//                var applications = query.GetById(id).Single().Applications;

                model = new ProductModel(
                                entity.Id,
                                entity.Name,
                                entity.Manufacturer_Id,
                                entity.Customer_Id,
                                entity.CreatedDate,
                                entity.ChangedDate,
                                ApplicationMapper.MapToOverviews(new Application[0]));
            }

            return model;
        }

        public static void MapToEntity(ProductModel model, Product entity)
        {
            entity.Customer_Id = model.CustomerId;
            entity.Manufacturer_Id = model.ManufacturerId;
            entity.Name = model.ProductName;
        }
    }
}