namespace DH.Helpdesk.BusinessData.Models.Licenses.Licenses
{
    public sealed class LicenseData
    {
        public LicenseData(LicenseModel license)
        {
            this.License = license;
        }

        public LicenseModel License { get; private set; }
    }
}