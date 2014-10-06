namespace DH.Helpdesk.Mobile.Areas.Licenses.Infrastructure.ModelFactories
{
    using DH.Helpdesk.BusinessData.Models.Licenses;
    using DH.Helpdesk.BusinessData.Models.Licenses.Vendors;
    using DH.Helpdesk.Mobile.Areas.Licenses.Models.Vendors;

    public interface IVendorsModelFactory
    {
        VendorsIndexModel GetIndexModel(VendorsFilterModel filter);

        VendorsContentModel GetContentModel(VendorOverview[] vendors);
    }
}