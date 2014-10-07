namespace DH.Helpdesk.Web.Areas.Licenses.Infrastructure.ModelFactories.Concrete
{
    using DH.Helpdesk.BusinessData.Models.Licenses.Vendors;
    using DH.Helpdesk.Web.Areas.Licenses.Models.Vendors;

    public sealed class VendorsModelFactory : IVendorsModelFactory
    {
        public VendorsIndexModel GetIndexModel(VendorsFilterModel filter)
        {
            return new VendorsIndexModel();
        }

        public VendorsContentModel GetContentModel(VendorOverview[] vendors)
        {
            return new VendorsContentModel(vendors);
        }

        public VendorEditModel GetEditModel(VendorData data)
        {
            return new VendorEditModel();
        }
    }
}