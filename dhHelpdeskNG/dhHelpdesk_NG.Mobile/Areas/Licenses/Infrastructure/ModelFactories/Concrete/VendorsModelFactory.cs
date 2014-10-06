namespace DH.Helpdesk.Web.Areas.Licenses.Infrastructure.ModelFactories.Concrete
{
    using DH.Helpdesk.BusinessData.Models.Licenses;
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
    }
}