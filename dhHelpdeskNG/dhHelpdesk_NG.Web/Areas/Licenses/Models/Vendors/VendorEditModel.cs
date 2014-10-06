namespace DH.Helpdesk.Web.Areas.Licenses.Models.Vendors
{
    using DH.Helpdesk.BusinessData.Models.Licenses.Vendors;

    public sealed class VendorEditModel
    {
        public VendorEditModel(VendorData data)
        {
            this.Data = data;
        }

        public VendorEditModel()
        {            
        }

        public VendorData Data { get; set; }
    }
}