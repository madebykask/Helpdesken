namespace DH.Helpdesk.Web.Areas.Licenses.Models.Licenses
{
    using DH.Helpdesk.Web.Areas.Licenses.Models.Common;

    public sealed class LicensesIndexModel : BaseIndexModel
    {
        public override IndexModelType Type
        {
            get
            {
                return IndexModelType.Licenses;
            }
        }
    }
}