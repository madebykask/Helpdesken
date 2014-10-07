namespace DH.Helpdesk.BusinessData.Models.Licenses.Licenses
{
    using DH.Helpdesk.Common.ValidationAttributes;

    public sealed class LicenseData
    {
        public LicenseData(LicenseModel license)
        {
            this.License = license;
        }

        [NotNull]
        public LicenseModel License { get; private set; }
    }
}