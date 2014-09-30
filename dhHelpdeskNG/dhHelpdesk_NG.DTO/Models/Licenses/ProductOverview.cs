namespace DH.Helpdesk.BusinessData.Models.Licenses
{
    public sealed class ProductOverview
    {
        public int ProductId { get; set; }

        public string ProductName { get; set; }

        public int LicencesNumber { get; set; }

        public int UsedLicencesNumber { get; set; }

        public int GetLicensesDifference()
        {
            return this.LicencesNumber - this.UsedLicencesNumber;
        }
    }
}