namespace DH.Helpdesk.Mobile.Areas.Licenses.Models.Vendors
{
    using DH.Helpdesk.Mobile.Areas.Licenses.Models.Common;

    public sealed class VendorsIndexModel : BaseIndexModel
    {
        public override IndexModelType Type
        {
            get
            {
                return IndexModelType.Vendors;
            }
        }

        public VendorsFilterModel GetFilter()
        {
            return new VendorsFilterModel();
        }
    }
}