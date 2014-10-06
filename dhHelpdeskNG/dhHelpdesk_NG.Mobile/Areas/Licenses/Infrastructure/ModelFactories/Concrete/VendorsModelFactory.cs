namespace DH.Helpdesk.Mobile.Areas.Licenses.Infrastructure.ModelFactories.Concrete
{
    using DH.Helpdesk.BusinessData.Models.Licenses;
    using DH.Helpdesk.BusinessData.Models.Licenses.Vendors;
    using DH.Helpdesk.Mobile.Areas.Licenses.Models.Vendors;

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