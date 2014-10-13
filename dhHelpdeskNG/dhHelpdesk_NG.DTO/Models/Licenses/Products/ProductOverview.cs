namespace DH.Helpdesk.BusinessData.Models.Licenses.Products
{
    public sealed class ProductOverview
    {
        public ProductOverview(
                int productId, 
                string productName, 
                int licencesNumber, 
                int usedLicencesNumber)
        {
            this.UsedLicencesNumber = usedLicencesNumber;
            this.LicencesNumber = licencesNumber;
            this.ProductName = productName;
            this.ProductId = productId;
        }

        public int ProductId { get; private set; }

        public string ProductName { get; private set; }

        public int LicencesNumber { get; private set; }

        public int UsedLicencesNumber { get; private set; }

        public int GetLicensesDifference()
        {
            return this.LicencesNumber - this.UsedLicencesNumber;
        }
    }
}