namespace DH.Helpdesk.BusinessData.Models.Licenses.Vendors
{
    using DH.Helpdesk.Common.ValidationAttributes;

    public sealed class VendorData
    {
        public VendorData(VendorModel vendor)
        {
            this.Vendor = vendor;
        }

        [NotNull]
        public VendorModel Vendor { get; private set; }
    }
}