namespace DH.Helpdesk.BusinessData.Models.Licenses.Licenses
{
    using DH.Helpdesk.BusinessData.Models.Shared;
    using DH.Helpdesk.Common.ValidationAttributes;

    public sealed class LicenseData
    {
        public LicenseData(
                LicenseModel license, 
                ItemOverview[] products, 
                ItemOverview[] regions,
                ItemOverview[] departments, 
                ItemOverview[] vendors, 
                ItemOverview[] upgradeLicenses)
        {
            this.UpgradeLicenses = upgradeLicenses;
            this.Vendors = vendors;
            this.Regions = regions;
            this.Departments = departments;
            this.Products = products;
            this.License = license;
        }

        [NotNull]
        public LicenseModel License { get; private set; }

        [NotNull]
        public ItemOverview[] Products { get; private set; }

        [NotNull]
        public ItemOverview[] Regions { get; private set; }

        [NotNull]
        public ItemOverview[] Departments { get; private set; }

        [NotNull]
        public ItemOverview[] Vendors { get; private set; }

        [NotNull]
        public ItemOverview[] UpgradeLicenses { get; private set; }
    }
}