namespace DH.Helpdesk.Domain
{
    using global::System;

    public class License : Entity
    {
        public int Department_Id { get; set; }
        public int NumberOfLicenses { get; set; }
        public int Price { get; set; }
        public int PriceYear { get; set; }
        public int Product_Id { get; set; }
        public int Region_Id { get; set; }
        public int UpgradeLicense_Id { get; set; }
        public int Vendor_Id { get; set; }
        public string Info { get; set; }
        public string LicenseNumber { get; set; }
        public string PurshaseInfo { get; set; }
        public DateTime ChangedDate { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime PurshaseDate { get; set; }
        public DateTime ValidDate { get; set; }

        public virtual Department Department { get; set; }
        public virtual Region Region { get; set; }
    }
}
