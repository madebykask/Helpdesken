namespace DH.Helpdesk.Mobile.Areas.Licenses.Models.Licenses
{
    using DH.Helpdesk.Mobile.Areas.Licenses.Models.Common;

    public sealed class LicensesIndexModel : BaseIndexModel
    {
        public override IndexModelType Type
        {
            get
            {
                return IndexModelType.Licenses;
            }
        }

        public LicensesFilterModel GetFilter()
        {
            return new LicensesFilterModel();
        }
    }
}