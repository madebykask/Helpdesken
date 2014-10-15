﻿namespace DH.Helpdesk.Web.Areas.Licenses.Models.Licenses
{
    using System;
    using System.Web.Mvc;

    using DH.Helpdesk.Common.ValidationAttributes;
    using DH.Helpdesk.Web.Areas.Licenses.Models.Common;
    using DH.Helpdesk.Web.Infrastructure.LocalizedAttributes;

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
                SelectList departments, 
                SelectList vendors, 
                SelectList upgradeLicenses)
        {
            this.UpgradeLicenses = upgradeLicenses;
            this.Id = id;
            this.Vendors = vendors;
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
        }

        public LicenseEditModel()
        {
        }

        [NotNull]
        public SelectList Products { get; private set; }

        [IsId]
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

        public override EntityModelType Type
        {
            get
            {
                return EntityModelType.Licenses;
            }
        }
    }
}