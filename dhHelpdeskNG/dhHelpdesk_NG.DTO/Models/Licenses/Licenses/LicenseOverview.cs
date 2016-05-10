namespace DH.Helpdesk.BusinessData.Models.Licenses.Licenses
{
    using System;

    public sealed class LicenseOverview
    {
        public LicenseOverview(
                int licenseId, 
                string productName, 
                int licensesNumber, 
                DateTime? purchaseDate, 
                string department,
                string region
            )
        {
            this.Department = department;
            this.PurchaseDate = purchaseDate;
            this.LicensesNumber = licensesNumber;
            this.ProductName = productName;
            this.LicenseId = licenseId;
            this.Region = region;
        }

        public int LicenseId { get; private set; }
 
        public string ProductName { get; private set; }

        public int LicensesNumber { get; private set; }

        public DateTime? PurchaseDate { get; private set; }

        public string Department { get; private set; }

        public string Region { get; private set; }
    }
}