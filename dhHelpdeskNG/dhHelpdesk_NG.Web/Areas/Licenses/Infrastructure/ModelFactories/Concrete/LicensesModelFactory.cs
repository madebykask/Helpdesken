namespace DH.Helpdesk.Web.Areas.Licenses.Infrastructure.ModelFactories.Concrete
{
    using DH.Helpdesk.BusinessData.Models.Licenses.Licenses;
    using DH.Helpdesk.Web.Areas.Licenses.Models.Licenses;
    using DH.Helpdesk.Web.Infrastructure.Tools;

    public sealed class LicensesModelFactory : ILicensesModelFactory
    {
        public LicensesIndexModel GetIndexModel(LicensesFilterModel filter)
        {
            return new LicensesIndexModel();
        }

        public LicensesContentModel GetContentModel(LicenseOverview[] licenses)
        {
            return new LicensesContentModel(licenses);
        }

        public LicenseEditModel GetEditModel(LicenseData data)
        {
            var products = WebMvcHelper.CreateListField(data.Products, data.License.ProductId);
            var departments = WebMvcHelper.CreateListField(data.Departments, data.License.DepartmentId);
            var vendors = WebMvcHelper.CreateListField(data.Vendors, data.License.VendorId);
            var upgradeLicenss = WebMvcHelper.CreateListField(data.UpgradeLicenses, data.License.UpgradeLicenseId);

            return new LicenseEditModel(
                                data.License.Id,
                                data.License.LicenseNumber,
                                data.License.NumberOfLicenses,
                                data.License.PurshaseDate,
                                data.License.Price,
                                data.License.PurshaseInfo,
                                data.License.ValidDate,
                                data.License.PriceYear,
                                data.License.Info,
                                products, 
                                departments, 
                                vendors,
                                upgradeLicenss);
        }

        public LicenseModel GetBusinessModel(LicenseEditModel editModel)
        {
            var model = new LicenseModel(
                                editModel.Id,
                                editModel.LicenseNumber,
                                editModel.NumberOfLicenses,
                                editModel.PurchaseDate,
                                editModel.Price,
                                editModel.PurchaseInfo,
                                editModel.PriceYear,
                                editModel.ProductId,
                                editModel.VendorId,
                                editModel.DepartmentId,
                                editModel.UpgradeLicenseId,
                                editModel.ValidDate,
                                editModel.Info,
                                null);

            return model;
        }
    }
}