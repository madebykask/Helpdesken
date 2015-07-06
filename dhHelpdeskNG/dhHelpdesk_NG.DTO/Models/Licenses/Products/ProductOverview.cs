namespace DH.Helpdesk.BusinessData.Models.Licenses.Products
{
    using DH.Helpdesk.Common.ValidationAttributes;
    using System.Collections.Generic;
    using System.Linq;

    public sealed class ProductOverview
    {
        public ProductOverview(
                int productId, 
                string productName, 
                string[] regions, 
                string[] departments,                
                int[] licencesNumbers, 
                int usedLicencesNumber)
        {
            this.Departments = departments;
            this.Regions = regions;
            this.UsedLicencesNumber = usedLicencesNumber;
            this.LicencesNumbers = licencesNumbers;
            this.ProductName = productName;
            this.ProductId = productId;
        }

        public int ProductId { get; private set; }

        public string ProductName { get; private set; }

        [NotNull]
        public string[] Regions { get; private set; }

        [NotNull]
        public string[] Departments { get; private set; }

        public int[] LicencesNumbers { get; private set; }

        public int UsedLicencesNumber { get; private set; }

        public int GetLicensesDifference()
        {
            return this.LicencesNumbers.Sum() - this.UsedLicencesNumber;
        }
    }
}