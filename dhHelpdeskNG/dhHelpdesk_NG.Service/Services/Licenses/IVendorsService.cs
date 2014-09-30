namespace DH.Helpdesk.Services.Services.Licenses
{
    using DH.Helpdesk.BusinessData.Models.Licenses;

    public interface IVendorsService
    {
        VendorOverview[] GetVendors(int customerId);
    }
}