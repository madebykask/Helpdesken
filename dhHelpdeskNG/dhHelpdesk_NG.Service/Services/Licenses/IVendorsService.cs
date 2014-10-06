namespace DH.Helpdesk.Services.Services.Licenses
{
    using DH.Helpdesk.BusinessData.Models.Licenses.Vendors;

    public interface IVendorsService
    {
        VendorOverview[] GetVendors(int customerId);

        VendorData GetVendorData(int? vendorId);
    }
}