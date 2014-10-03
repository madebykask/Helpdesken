namespace DH.Helpdesk.BusinessData.Models.Licenses.Vendors
{
    public sealed class VendorOverview
    {
        public VendorOverview(
                int vendorId, 
                string vendorName, 
                string vendorContact, 
                string vendorPhone, 
                string vendorEmail, 
                string vendorHomePage)
        {
            this.VendorHomePage = vendorHomePage;
            this.VendorEmail = vendorEmail;
            this.VendorPhone = vendorPhone;
            this.VendorContact = vendorContact;
            this.VendorName = vendorName;
            this.VendorId = vendorId;
        }

        public int VendorId { get; private set; }
 
        public string VendorName { get; private set; }

        public string VendorContact { get; private set; }

        public string VendorPhone { get; private set; }

        public string VendorEmail { get; private set; }

        public string VendorHomePage { get; private set; }
    }
}