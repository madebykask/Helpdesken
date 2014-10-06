namespace DH.Helpdesk.Web.Areas.Licenses.Infrastructure.ModelFactories
{
    using DH.Helpdesk.BusinessData.Models.Licenses;
    using DH.Helpdesk.Web.Areas.Licenses.Models.Vendors;

    public interface IVendorsModelFactory
    {
        VendorsIndexModel GetIndexModel(VendorsFilterModel filter);

        VendorsContentModel GetContentModel(VendorOverview[] vendors);
    }
}