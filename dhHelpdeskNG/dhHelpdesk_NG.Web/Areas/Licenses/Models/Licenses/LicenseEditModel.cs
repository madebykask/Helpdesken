using DH.Helpdesk.Web.Common.Tools.Files;

namespace DH.Helpdesk.Web.Areas.Licenses.Models.Licenses
{
    using System;
    using System.Collections.Generic;
    using System.Web.Mvc;

    using DH.Helpdesk.Common.ValidationAttributes;
    using DH.Helpdesk.Web.Areas.Licenses.Models.Common;
    using DH.Helpdesk.Web.Infrastructure.LocalizedAttributes;
    using DH.Helpdesk.Web.Infrastructure.Tools;

    public sealed class LicenseEditModel : BaseEditModel
    {
        public LicenseEditModel(
                int id,
                string licenseNumber,
                int numberOfLicenses,
                DateTime? purchaseDate,
                int price,
                string purchaseInfo,
                DateTime? validDate,
                int priceYear,
                string info,
                SelectList products, 
                SelectList regions,
                SelectList departments, 
                SelectList vendors, 
                SelectList upgradeLicenses,
                AttachedFilesModel files)
        {
            this.UpgradeLicenses = upgradeLicenses;
            this.Id = id;
            this.Vendors = vendors;
            this.Regions = regions;
            this.Departments = departments;
            this.Products = products;
            this.LicenseNumber = licenseNumber;
            this.NumberOfLicenses = numberOfLicenses;
            this.PurchaseDate = purchaseDate;
            this.Price = price;
            this.PurchaseInfo = purchaseInfo;
            this.ValidDate = validDate;
            this.PriceYear = priceYear;
            this.Info = info;
            this.Files = files;

            this.NewFiles = new List<WebTemporaryFile>();
            this.DeletedFiles = new List<string>();
        }

        public LicenseEditModel()
        {
        }

        [NotNull]
        public SelectList Products { get; private set; }

        [IsId]
        [LocalizedRequired]
        [LocalizedDisplay("Produkt")]
        public int? ProductId { get; set; }

        [LocalizedRequired]
        [LocalizedStringLength(100)]
        [LocalizedDisplay("Licensnummer")]
        public string LicenseNumber { get; set; }

        [LocalizedRequired]
        [LocalizedInteger]
        [LocalizedDisplay("Antal licenser")]
        public int NumberOfLicenses { get; set; }

        [NotNull]
        public SelectList Regions { get; private set; }

        [IsId]
        [LocalizedDisplay("Område")]
        public int? RegionId { get; set; }

        [NotNull]
        public SelectList Departments { get; private set; }

        [IsId]
        [LocalizedDisplay("Avdelning")]
        public int? DepartmentId { get; set; }

        [LocalizedDisplay("Inköpsdatum")]
        public DateTime? PurchaseDate { get; set; }

        [LocalizedInteger]
        [LocalizedDisplay("Inköpspris")]
        public int Price { get; set; }

        [LocalizedStringLength(200)]
        [LocalizedDisplay("Inköpsunderlag")]
        public string PurchaseInfo { get; set; }

        [LocalizedDisplay("Giltig t.o.m")]
        public DateTime? ValidDate { get; set; }

        [NotNull]
        public SelectList Vendors { get; private set; }

        [IsId]
        [LocalizedDisplay("Försäljare")]
        public int? VendorId { get; set; }

        [LocalizedRequired]
        [LocalizedInteger]
        [LocalizedDisplay("Årlig kostnad")]
        public int PriceYear { get; set; }

        [LocalizedStringLength(1000)]
        [LocalizedDisplay("Kommentar")]
        public string Info { get; set; }

        [NotNull]
        public SelectList UpgradeLicenses { get; private set; }

        [IsId]
        [LocalizedDisplay("Uppgradering av")]
        public int? UpgradeLicenseId { get; set; }

        [NotNull]
        public AttachedFilesModel Files { get; set; }

        [NotNull]
        public List<WebTemporaryFile> NewFiles { get; set; }

        [NotNull]
        public List<string> DeletedFiles { get; set; }

        public override EntityModelType Type
        {
            get
            {
                return EntityModelType.Licenses;
            }
        }
    }
}