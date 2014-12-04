namespace DH.Helpdesk.Services.BusinessLogic.Mappers.Licenses
{
    using System.Globalization;
    using System.Linq;

    using DH.Helpdesk.BusinessData.Models.Licenses.Products;
    using DH.Helpdesk.BusinessData.Models.Shared;
    using DH.Helpdesk.Domain;
    using DH.Helpdesk.Services.BusinessLogic.Specifications;

    public static class ProductMapper
    {
        public static ProductOverview[] MapToOverviews(this IQueryable<Product> query, IQueryable<Software> software)
        {
            var entities = query.Select(p => new 
                                                {
                                                    ProductId = p.Id,
                                                    ProductName = p.Name,
                                                    Regions = p.Licenses.Select(l => l.Region.Name).Distinct(),
                                                    Departments = p.Licenses.Select(l => l.Department.DepartmentName).Distinct(),
                                                    LicencesNumber = p.Licenses.Select(l => l.NumberOfLicenses),
                                                    UsedLicencesNumber = software.Where(s => p.Applications.Select(a => a.Name).Contains(s.Name)).Count()
                                                })
                                                .OrderBy(p => p.ProductName)
                                                .ToArray();

            var overviews = entities.Select(p => new ProductOverview(
                                                    p.ProductId,
                                                    p.ProductName,
                                                    p.Regions.ToArray(),
                                                    p.Departments.ToArray(),
                                                    p.LicencesNumber.Sum(),
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
                model = new ProductModel(
                                entity.Id,
                                entity.Name,
                                entity.Manufacturer_Id,
                                entity.Customer_Id,
                                entity.CreatedDate,
                                entity.ChangedDate);
            }

            return model;
        }

        public static void MapToEntity(ProductModel model, Product entity)
        {
            entity.Customer_Id = model.CustomerId;
            entity.Manufacturer_Id = model.ManufacturerId;
            entity.Name = model.ProductName;
        }

        public static ProductsFilterData MapToFilterData(
                        IQueryable<Region> regions,
                        IQueryable<Department> departments)
        {
            var overviews = regions.Select(m => new { m.Id, m.Name, Type = "Region" }).Union(
                            departments.Select(a => new { a.Id, Name = a.DepartmentName, Type = "Department" }))
                            .OrderBy(o => o.Type)
                            .ThenBy(o => o.Name)
                            .ToArray();

            return new ProductsFilterData(
                        overviews.Where(o => o.Type == "Region").Select(o => new ItemOverview(o.Name, o.Id.ToString(CultureInfo.InvariantCulture))).ToArray(),
                        overviews.Where(o => o.Type == "Department").Select(o => new ItemOverview(o.Name, o.Id.ToString(CultureInfo.InvariantCulture))).ToArray());
        }

        public static ProductData MapToData(
                        ProductModel product,
                        IQueryable<Manufacturer> manufacturers,
                        IQueryable<Application> applications)
        {
            var overviews = manufacturers.Select(m => new { m.Id, m.Name, Type = "Manufacturer" }).Union(
                            applications.Select(a => new { a.Id, a.Name, Type = "Application" }))
                            .OrderBy(o => o.Type)
                            .ThenBy(o => o.Name)
                            .ToArray();

            return new ProductData(
                        product,
                        overviews.Where(o => o.Type == "Manufacturer").Select(o => new ItemOverview(o.Name, o.Id.ToString(CultureInfo.InvariantCulture))).ToArray(),
                        overviews.Where(o => o.Type == "Application").Select(o => new ItemOverview(o.Name, o.Id.ToString(CultureInfo.InvariantCulture))).ToArray());
        }
    }
}