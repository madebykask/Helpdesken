namespace DH.Helpdesk.BusinessData.Models.Licenses.Products
{
    using DH.Helpdesk.Common.ValidationAttributes;

    public sealed class ProductOverview
    {
        public ProductOverview(
                int productId, 
                string productName, 
                string[] regions, 
                string[] departments,
                int licencesNumber, 
                int usedLicencesNumber)
        {
            this.Departments = departments;
            this.Regions = regions;
            this.UsedLicencesNumber = usedLicencesNumber;
            this.LicencesNumber = licencesNumber;
            this.ProductName = productName;
            this.ProductId = productId;
        }

        public int ProductId { get; private set; }

        public string ProductName { get; private set; }

        [NotNull]
        public string[] Regions { get; private set; }

        [NotNull]
        public string[] Departments { get; private set; }

        public int LicencesNumber { get; private set; }

        public int UsedLicencesNumber { get; private set; }

        public int GetLicensesDifference()
        {
            return this.LicencesNumber - this.UsedLicencesNumber;
        }
    }
}