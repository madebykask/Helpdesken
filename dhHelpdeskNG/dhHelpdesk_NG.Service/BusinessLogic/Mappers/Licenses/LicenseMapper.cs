namespace DH.Helpdesk.Services.BusinessLogic.Mappers.Licenses
{
    using System.Linq;

    using DH.Helpdesk.BusinessData.Models.Licenses.Licenses;
    using DH.Helpdesk.Domain;
    using DH.Helpdesk.Services.BusinessLogic.Specifications;

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

        public static LicenseModel MapToBusinessModel(this IQueryable<License> query, int id)
        {
            LicenseModel model = null;

            var entity = query.GetById(id)
                        .Select(l => new
                            {
                                l.LicenseNumber,
                                l.NumberOfLicenses,
                                l.PurshaseDate,
                                l.Price,
                                l.PurshaseInfo,
                                l.PriceYear,
                                l.Product_Id,
                                l.Vendor_Id,
                                l.Department_Id,
                                l.UpgradeLicense_Id,
                                l.ValidDate,
                                l.Info,
                                l.CreatedDate,
                                l.ChangedDate
                            }).SingleOrDefault();

            if (entity != null)
            {
                model = new LicenseModel(
                                entity.LicenseNumber,
                                entity.NumberOfLicenses,
                                entity.PurshaseDate,
                                entity.Price,
                                entity.PurshaseInfo,
                                entity.PriceYear,
                                entity.Product_Id,
                                entity.Vendor_Id,
                                entity.Department_Id,
                                entity.UpgradeLicense_Id,
                                entity.ValidDate,
                                entity.Info,
                                entity.CreatedDate,
                                entity.ChangedDate);
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
            entity.Department_Id = model.DepartmentId;
            entity.UpgradeLicense_Id = model.UpgradeLicenseId;
            entity.ValidDate = model.ValidDate;
            entity.Info = model.Info;
        }
    }
}