namespace DH.Helpdesk.BusinessData.Models.Licenses
{
    using System;

    public sealed class LicenseOverview
    {
        public int LicenseId { get; set; }
 
        public string ProductName { get; set; }

        public int LicensesNumber { get; set; }

        public DateTime? PurchaseDate { get; set; }

        public string Department { get; set; }
    }
}