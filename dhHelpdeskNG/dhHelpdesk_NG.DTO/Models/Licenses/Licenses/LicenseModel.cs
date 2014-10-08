namespace DH.Helpdesk.BusinessData.Models.Licenses.Licenses
{
    using System;

    using DH.Helpdesk.BusinessData.Models.Shared;
    using DH.Helpdesk.Common.ValidationAttributes;

    public sealed class LicenseModel : EntityBusinessModel
    {
        public LicenseModel(
                int id,
                string licenseNumber, 
                int numberOfLicenses, 
                DateTime? purshaseDate, 
                int price, 
                string purshaseInfo, 
                int priceYear, 
                int? productId, 
                int? vendorId, 
                int? departmentId, 
                int? upgradeLicenseId, 
                DateTime? validDate, 
                string info, 
                DateTime createdDate, 
                DateTime changedDate, 
                LicenseFileModel[] files)
        {
            this.Id = id;
            this.Files = files;
            this.ChangedDate = changedDate;
            this.CreatedDate = createdDate;
            this.Info = info;
            this.ValidDate = validDate;
            this.UpgradeLicenseId = upgradeLicenseId;
            this.DepartmentId = departmentId;
            this.VendorId = vendorId;
            this.ProductId = productId;
            this.PriceYear = priceYear;
            this.PurshaseInfo = purshaseInfo;
            this.Price = price;
            this.PurshaseDate = purshaseDate;
            this.NumberOfLicenses = numberOfLicenses;
            this.LicenseNumber = licenseNumber;
        }

        public LicenseModel(
                int id,
                string licenseNumber, 
                int numberOfLicenses, 
                DateTime? purshaseDate, 
                int price, 
                string purshaseInfo, 
                int priceYear, 
                int? productId, 
                int? vendorId, 
                int? departmentId, 
                int? upgradeLicenseId, 
                DateTime? validDate, 
                string info, 
                LicenseFileModel[] files)
        {
            this.Id = id;
            this.Files = files;
            this.Info = info;
            this.ValidDate = validDate;
            this.UpgradeLicenseId = upgradeLicenseId;
            this.DepartmentId = departmentId;
            this.VendorId = vendorId;
            this.ProductId = productId;
            this.PriceYear = priceYear;
            this.PurshaseInfo = purshaseInfo;
            this.Price = price;
            this.PurshaseDate = purshaseDate;
            this.NumberOfLicenses = numberOfLicenses;
            this.LicenseNumber = licenseNumber;
        }

        private LicenseModel()
        {
        }

        [NotNullAndEmpty]
        [MaxLength(100)]
        public string LicenseNumber { get; private set; }

        public int NumberOfLicenses { get; private set; }

        public DateTime? PurshaseDate { get; private set; }

        public int Price { get; private set; }

        [MaxLength(200)]
        public string PurshaseInfo { get; private set; }

        public int PriceYear { get; private set; }

        [IsId]
        public int? ProductId { get; private set; }

        [IsId]
        public int? VendorId { get; private set; }

        [IsId]
        public int? DepartmentId { get; private set; }

        [IsId]
        public int? UpgradeLicenseId { get; private set; }

        public DateTime? ValidDate { get; private set; }

        [NotNullAndEmpty]
        [MaxLength(1000)]
        public string Info { get; private set; }

        public DateTime CreatedDate { get; private set; }

        public DateTime ChangedDate { get; private set; }

        public LicenseFileModel[] Files { get; private set; }

        public static LicenseModel CreateDefault()
        {
            return new LicenseModel();
        }
    }
}