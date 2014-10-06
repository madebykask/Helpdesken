namespace DH.Helpdesk.BusinessData.Models.Licenses.Vendors
{
    public sealed class VendorData
    {
        public VendorData(VendorModel vendor)
        {
            this.Vendor = vendor;
        }

        public VendorModel Vendor { get; private set; }
    }
}