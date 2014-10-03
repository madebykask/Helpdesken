namespace DH.Helpdesk.Web.Areas.Licenses.Models.Vendors
{
    using DH.Helpdesk.BusinessData.Models.Licenses.Vendors;

    public sealed class VendorsContentModel
    {
        public VendorsContentModel(VendorOverview[] vendors)
        {
            this.Vendors = vendors;
        }

        public VendorOverview[] Vendors { get; private set; }
    }
}