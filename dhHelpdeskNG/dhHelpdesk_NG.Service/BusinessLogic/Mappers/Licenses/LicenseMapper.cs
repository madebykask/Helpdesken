namespace DH.Helpdesk.Services.BusinessLogic.Mappers.Licenses
{
    using System;
    using System.Globalization;
    using System.Linq;

    using DH.Helpdesk.BusinessData.Models.Licenses.Licenses;
    using DH.Helpdesk.BusinessData.Models.Shared;
    using DH.Helpdesk.Domain;
    using DH.Helpdesk.Services.BusinessLogic.Specifications;
    using System.Collections.Generic;

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
                                    }))
                                    .OrderBy(l => l.ProductName)
                                    .ToArray();

            var overviews = entities.Select(l => new LicenseOverview(
                                                    l.LicenseId,
                                                    l.ProductName,
                                                    l.LicensesNumber,
                                                    l.PurchaseDate,
                                                    l.Department)).ToArray();

            return overviews;
        }

        public static LicenseModel MapToBusinessModel(
                                this IQueryable<License> query, 
                                int id)
        {
            LicenseModel model = null;

            var entity = query.GetById(id)
                        .Select(l => new
                            {
                                l.Id,
                                l.LicenseNumber,
                                l.NumberOfLicenses,
                                l.PurshaseDate,
                                l.Price,
                                l.PurshaseInfo,
                                l.PriceYear,
                                l.Product_Id,
                                l.Vendor_Id,
                                l.Region_Id,
                                l.Department_Id,
                                l.UpgradeLicense_Id,
                                l.ValidDate,
                                l.Info,
                                l.CreatedDate,
                                l.ChangedDate,
                                l.Files
                            }).SingleOrDefault();

            if (entity != null)
            {
                var licenseFiles = entity.Files
                                    .MapToBusinessModels();

                model = new LicenseModel(
                                entity.Id,
                                entity.LicenseNumber,
                                entity.NumberOfLicenses,
                                entity.PurshaseDate,
                                entity.Price,
                                entity.PurshaseInfo,
                                entity.PriceYear,
                                entity.Product_Id,
                                entity.Vendor_Id,
                                entity.Region_Id,
                                entity.Department_Id,
                                entity.UpgradeLicense_Id,
                                entity.ValidDate,
                                entity.Info,
                                entity.CreatedDate,
                                entity.ChangedDate,
                                licenseFiles);
            }

            return model;
        }

        public static void MapToEntity(LicenseModel model, License entity)
        {
            entity.LicenseNumber = model.LicenseNumber;
            entity.NumberOfLicenses = model.NumberOfLicenses;
            entity.PurshaseDate = model.PurshaseDate;
            entity.Price = model.Price;
            entity.PurshaseInfo = model.PurshaseInfo;
            entity.PriceYear = model.PriceYear;
            entity.Product_Id = model.ProductId;
            entity.Vendor_Id = model.VendorId;
            entity.Region_Id = model.RegionId;
            entity.Department_Id = model.DepartmentId;
            entity.UpgradeLicense_Id = model.UpgradeLicenseId;
            entity.ValidDate = model.ValidDate;
            entity.Info = model.Info;
        }

        public static ItemOverview[] MapToItemOverviews(this IQueryable<License> query)
        {
            var entities = query.Select(l => new
            {
                l.Id,
                l.PurshaseDate,
                ProductName = l.Product.Name
            })
            .OrderBy(l => l.ProductName)
            .ToArray();

            var overviews = entities.Select(l => new ItemOverview(
                                            l.PurshaseDate.HasValue ? 
                                                string.Format("{0}({1})", l.ProductName, l.PurshaseDate.Value.ToShortDateString()) : 
                                                l.ProductName,
                                            l.Id.ToString(CultureInfo.InvariantCulture))).ToArray();

            return overviews;
        }

        public static LicenseData MapToData(
                        LicenseModel license,
                        IQueryable<Product> products,
                        List<Region> regions,
                        List<Department> departments,
                        IQueryable<Vendor> vendors,
                        IQueryable<License> upgradeLicenses)
        {
            var separator = Guid.NewGuid().ToString();

            var overviews = products.Select(p => new { p.Id, p.Name, Type = "Product" }).Union(                            
                            vendors.Select(v => new { v.Id, v.Name, Type = "Vendor" }).Union(
                            upgradeLicenses.Select(l => new
                                                {
                                                    l.Id, 
                                                    Name = l.PurshaseDate.HasValue ? l.PurshaseDate.Value + separator + l.Product.Name : l.Product.Name, 
                                                    Type = "UpgradeLicense"                         
                                                })))
                                                .OrderBy(o => o.Type)
                                                .ThenBy(o => o.Name)
                                                .ToArray();

            

            return new LicenseData(
                            license,
                            overviews.Where(o => o.Type == "Product").Select(o => new ItemOverview(o.Name, o.Id.ToString(CultureInfo.InvariantCulture))).ToArray(),
                            regions.Select(o => new ItemOverview(o.Name, o.Id.ToString(CultureInfo.InvariantCulture))).ToArray(),
                            departments.Select(d => new ItemOverview(d.DepartmentName, d.Id.ToString(CultureInfo.InvariantCulture))).ToArray(),                            
                            overviews.Where(o => o.Type == "Vendor").Select(o => new ItemOverview(o.Name, o.Id.ToString(CultureInfo.InvariantCulture))).ToArray(),
                            overviews.Where(o => o.Type == "UpgradeLicense").Select(o => 
                                {
                                    var values = o.Name.Split(new[] { separator }, StringSplitOptions.RemoveEmptyEntries);
                                    return new ItemOverview(values.Length == 1 ? values[0] : string.Format("{0}({1})", values[1], DateTime.Parse(values[0]).ToShortDateString()), o.Id.ToString(CultureInfo.InvariantCulture));
                                }).ToArray());
        }
    }
}