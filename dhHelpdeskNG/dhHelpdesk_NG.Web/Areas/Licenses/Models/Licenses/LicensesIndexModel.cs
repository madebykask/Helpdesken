namespace DH.Helpdesk.Web.Areas.Licenses.Models.Licenses
{
    using DH.Helpdesk.Web.Areas.Licenses.Models.Common;

    public sealed class LicensesIndexModel : BaseIndexModel
    {
        public override EntityModelType Type
        {
            get
            {
                return EntityModelType.Licenses;
            }
        }

        public LicensesFilterModel GetFilter()
        {
            return new LicensesFilterModel();
        }
    }
}