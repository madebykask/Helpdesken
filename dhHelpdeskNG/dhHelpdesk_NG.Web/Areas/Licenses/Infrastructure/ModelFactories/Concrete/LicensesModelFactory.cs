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

            return new LicenseEditModel(products, departments, vendors);
        }
    }
}