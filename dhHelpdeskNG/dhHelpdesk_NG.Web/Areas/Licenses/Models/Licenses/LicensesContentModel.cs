namespace DH.Helpdesk.Web.Areas.Licenses.Models.Licenses
{
    using DH.Helpdesk.BusinessData.Models.Licenses.Licenses;

    public sealed class LicensesContentModel
    {
        public LicensesContentModel(LicenseOverview[] licenses)
        {
            this.Licenses = licenses;
        }

        public LicenseOverview[] Licenses { get; private set; }
    }
}